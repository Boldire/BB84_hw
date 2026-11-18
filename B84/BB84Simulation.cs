using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace B84
{
    internal class BB84Simulation
    {
        public int stepCount { get; private set; }

        //küldő
        private SimulatedUser alice;
        //fogadó
        private SimulatedUser bob;

        private Channel channel;

        bool eavesdropping;

        int evePercent;

        int numCount;

        float maxErrorRate;

        Random centralRnd;

        //UI megjelenítéshez az eredeti adatok térolása
        public List<int> InitialAliceBits { get; private set; } = new List<int>();
        public List<string> InitialAliceBases { get; private set; } = new List<string>();
        public List<string> InitialBobBases { get; private set; } = new List<string>();
        public List<int> InitialBobBits { get; private set; } = new List<int>();
        public List<int> SharedIndices { get; private set; } = new List<int>();
        public float LastQBER { get; private set; }
        public string LastStatusMessage { get; private set; } = "Készen áll.";

        public BB84Simulation()
        {
            centralRnd = new Random();
            stepCount = 0;
            alice = new SimulatedUser(centralRnd);
            bob = new SimulatedUser(centralRnd);
            channel = new Channel(centralRnd);
            eavesdropping = false;
            evePercent = 0;
            numCount = 12;
            maxErrorRate = 0.20f;
        }


        public void Configure(List<int> aBits, List<string> aBases, List<string> bBases, bool isEveActive, int evePercentage)
        {
            ResetSimulation();
            numCount = aBits.Count;
            eavesdropping = isEveActive;
            evePercent = evePercentage;
            channel.SetEve(isEveActive ? evePercentage : 0);

            InitialAliceBits = new List<int>(aBits);
            InitialAliceBases = new List<string>(aBases);
            InitialBobBases = new List<string>(bBases);

            alice.SetBits(new List<int>(aBits));
            alice.SetBases(new List<string>(aBases));
            bob.SetBases(new List<string>(bBases));
        }

        public void NextStep()
        {
            switch (stepCount)
            {
                case 0:
                    //Alice generál magának X bitet és X mérési alapot
                    alice.GenerateStuffs(numCount);
                    break;
                case 1:
                    //Alice elküldi a qubitokat
                    List<MyQubit> qubits = alice.GeneratedQubits();
                    channel.TransmitQubits(qubits);
                    break;
                case 2:
                    if (eavesdropping)
                    {
                        //Eve lehallgatja a qubitok egy részét
                        channel.ApplyEve();
                    }
                    else
                    {
                        //ha be lenne kapcsolva a lehallgatás, az itt történne
                    }
                    break;
                case 3:
                    //Bob generál magának X mérési alapot
                    bob.GenerateBases(numCount);
                    //Bob megkapja a qubitokat (ezek lehet, hogy le lettek hallgatva Eve által)
                    qubits = channel.RecieveQubits(); 
                    //Bob megméri a kapott qubiteket és a kapott biteket elmenti
                    bob.SetBits(bob.MeasureQubits(qubits));
                    break;
                case 4:
                    List<int> sharedBases = channel.SharedBases(alice.GetBases(), bob.GetBases());
                    //Alice és Bob egyeztetik az alapjaikat és az eltérő alapokat eldobják
                    //Ekkor a megtartott és nem lehallgatott bitek megegyeznek mindkét oldalon (feltételezve, hogy nincsen zaj)
                    alice.Sifting(sharedBases);
                    bob.Sifting(sharedBases);
                    break;
                case 5:
                    //kiválasztunk Y% véletlen bitet
                    List<int> validatorBits = alice.SelectRandomBits(40);
                    List<int> alicePublicBits = alice.QBERSelect(validatorBits);
                    List<int> bobPublicBits = bob.QBERSelect(validatorBits);
                    float QBER = channel.CheckQBER(alicePublicBits,bobPublicBits);
                    if (QBER > maxErrorRate)
                    { 
                        //megáll az algoritmus és újra kell kezdenünk
                        stepCount = -1; 
                    }
                    break;
                case 6:
                    //az két felhasználónál tárolt biteket össze kell hasonlítanunk, és a hibákat javítanunk kell
                    channel.CascadeParityCheck(alice,bob);
                    break;
                case 7:
                    //ha végeztünk a hiba- kereső és javító algoritmussal, még mindig lehetnek hibák a bitek között
                    //mindkét felhasználó hash-el a bitjeit
                    string aliceHash = alice.BitHashing();
                    string bobHash = bob.BitHashing();
                    if (aliceHash.Equals(bobHash))
                    {
                        //minden rendben, a két bitsor szinte biztosan egyezik (két eltérő bitsor adhat azonos hash-t, de ennek esélyét gyakorlatilag vehetjük nullának)
                    }
                    else
                    {
                        //valami nagyon félrement, leállunk, újra kell kezdeni az algoritmust
                        stepCount = -1;
                    }
                    break;
                case 8:
                    //kész vagyunk, a kapott bitsorok egyeznek
                    List<int> finalBits = alice.GetBits(); 
                    break;
                default:
                    //végére értünk, megáll a szimuláció
                    break;
            }
            if (stepCount != -1)
            {
                stepCount++; 
            }
        }

        //visszaállítjuk a szimulátort alaphelyztebe
        public void ResetSimulation()
        {
            stepCount = 0;
            alice = new SimulatedUser(centralRnd);
            bob = new SimulatedUser(centralRnd);
            channel = new Channel(centralRnd);
            InitialAliceBits.Clear();
            InitialAliceBases.Clear();
            InitialBobBases.Clear();
            InitialBobBits.Clear();
            SharedIndices.Clear();
            LastQBER = 0f;
        }

        //végigmegyünk a szimulátoron
        public void FullSim()
        {
            while (stepCount < 9)
            {
                NextStep();
            }
        }

        public string GetFinalKeyString() => string.Join("", alice.GetBits());
        public string GetBobFinalKeyString() => string.Join("", bob.GetBits());
    }

    internal class Channel
    {
        //A lehallgató fél a qubitok hány százalékát méri meg?
        private int eve;
        Random rnd;

        List<MyQubit> transmission;

        public Channel(Random centralRnd)
        {
            eve = 0;
            rnd = centralRnd;
        }

        public void SetEve(int newEve)
        {
            eve = newEve;
        }

        public void TransmitQubits(List<MyQubit> qubits)
        {
            transmission = qubits;
        }

        public List<MyQubit> RecieveQubits()
        {
            return transmission;
        }

        public void ApplyEve()
        {
            //a lehallgató (Eve) a közvetített qubitok [eve]%-át hallgatja le
            List<int> eavesdropped = new List<int>();
            //ennyi qubitot célunk lehallgatni
            int targetCount = (int)((eve / 100.0f) * transmission.Count);
            while (eavesdropped.Count != targetCount)
            {
                //kiválasztunk egy véletlen qubitot
                int nextEavesdrop = rnd.Next(0, transmission.Count);
                string selectedBase;
                if (!eavesdropped.Contains(nextEavesdrop))
                {
                    //ha a qubitot még nem hallgattuk le, választunk egy véletlen alapot
                    if (rnd.Next(0, 2) == 0)
                    {
                        selectedBase = "D";
                    }
                    else
                    {
                        selectedBase = "R";
                    }
                    /*  a lehallgatott qubitnak Eve nem ismeri az alapját
                     *  Eve-nek van egy 50%-os esélye véletlenül eltalálni az alapot
                     *  Hibás alap esetén a qubit állapota megváltozik
                     *  Bob a hibás értékű qubitnak 50% eséllyel ismételten megváltoztatja az értékét
                     *  Ez azt jelenti, hogy a végső kiolvasott bitnek 50%*50%=25% esélye van, hogy eltér az Alice által küldött bit-től
                     */
                    transmission[nextEavesdrop].GetValue(selectedBase);
                    eavesdropped.Add(nextEavesdrop);
                }
            }
        }

        public List<int> SharedBases(List<string> aliceBases, List<string> bobBases)
        {
            List<int> output = new List<int>();
            //összehasonlítjuk Alice és Bob mérési alapjait és megtartjuk a megegyezőket
            for (int i=0; i<aliceBases.Count; i++)
            {
                if (aliceBases[i].Equals(bobBases[i]))
                {
                    output.Add(i); 
                }
            }
            return output;
        }

        public float CheckQBER(List<int> validatorBitsA, List<int> validatorBitsB)
        {
            int correct = 0;
            int incorrect = 0;
            for (int i = 0; i<validatorBitsA.Count; i++) 
            {
                if (validatorBitsA[i] == validatorBitsB[i])
                { 
                    correct++; 
                }
                else
                {
                    incorrect++;
                }
            }
            return (float)incorrect / (incorrect+correct);
        }

        public void CascadeParityCheck(SimulatedUser userA, SimulatedUser userB)
        {
            //csak úgy teszünk, mintha lenne egy hibaellenőrzés
            if (rnd.Next(0,30)==0)
            {
                //ha megütjük az 1/30 esélyt, nem csinálunk semmit
                return;
            }
            else
            {
                //amúgy meg úgy teszünk, mintha megtaláltuk volna a hibákat és kijavítottuk őket 
                userB.SetBits(userA.GetBits());
            }
        
            //TODO: eltérő groupSize és eltérő sorrend generálása
            //TODO: userA.RangeParity és userB.RangeParity összehasonlítása, eltérés esetén kijavítjuk a hibát
        }
    }

    internal class SimulatedUser
    {
        //itt tároljuk akár a véletlenül generált, akár a mérés eredményeként kapott biteket (0 és 1)
        List<int> bits;
        //itt tároljuk akár a véletlenül generált, akár a mérés eredményeként kapott mérési alapokat (D és R)
        List<string> bases;
        
        private Random rnd;

        public SimulatedUser(Random centralRnd)
        {
            bits = new List<int>();
            bases = new List<string>();
            rnd = centralRnd;
        }

        //ad egy listát, amely a generált qubit-okat tárolja
        public List<MyQubit> GeneratedQubits()
        {
            List<MyQubit> output = new List<MyQubit>();
            for(int i = 0; i < bits.Count; i++)
            {
                output.Add(new MyQubit(bits[i], bases[i],rnd));
            }
            return output;
        }

        //véletlenszerűen generál [count] darab bitet és alapot
        public void GenerateStuffs(int count)
        {
            GenerateBits(count);
            GenerateBases(count);
        }

        //véletlenszerűen generál [count] darab bitet
        private void GenerateBits(int count)
        {
            for (int i=0; i<count; i++)
            {
                bits.Add(rnd.Next(0,2));
            }
        }

        //véletlenszerűen generál [count] darab alapot
        public void GenerateBases(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (rnd.Next(0, 2) == 0)
                    bases.Add("D");
                else
                    bases.Add("R");
            }
        }


        //adott alap szerint megméri az összes qubitet
        public List<int> MeasureQubits(List<MyQubit> qubits)
        {
            List<int> newBits = new List<int>();
            for (int i=0; i<qubits.Count; i++)
            {
                newBits.Add(qubits[i].GetValue(bases[i]));
            }
            return newBits;
        }

        public List<int> GetBits()
        {
            return bits;
        }

        public List<string> GetBases()
        {
            return bases;
        }

        public void SetBits(List<int> newBits)
        {
            bits = newBits;
        }

        public void SetBases(List<string> newBases)
        {
            bases = newBases;
        }

        public void Sifting(List<int> shared)
        {
            List<int> newBits = new List<int>();
            List<string> newBases = new List<string>();
            foreach (int i in shared)
            {
                newBits.Add(bits[i]);
                newBases.Add(bases[i]);
            }
            bits = newBits;
            bases = newBases;
        }

        public List<int> SelectRandomBits(int targetPercent)
        {
            if (targetPercent>100)
            {
                targetPercent = 100;
            }
            List<int> output = new List<int>();
            int targetCount = (int)((targetPercent / 100.0f) * (bases.Count));

            while (output.Count != targetCount)
            {
                int newNum = rnd.Next(0,bases.Count);
                if (!output.Contains(newNum))
                    output.Add(newNum);
            }

            return output;
        }

        public List<int> QBERSelect(List<int> publicList)
        {
            List<int> output = new List<int>();
            foreach (int i in publicList)
            {
                output.Add(bits[i]);
            }

            //a publikusan megoszotott biteket eldobjuk
            List<int> newBits = new List<int>();
            List<string> newBases = new List<string>();
            for (int i=0; i<bases.Count; i++)
            {
                if (!publicList.Contains(i))
                {
                    newBits.Add(bits[i]);
                    newBases.Add(bases[i]);
                }
            }
            bits = newBits;
            bases = newBases;

            return output;
        }

        public List<int> RangeParity(int groupSize, List<int> orderList)
        {
            List<int> output = new List<int>();
            //csak egy alternatív sorrendje a bitlistának
            List<int> dummyList = new List<int>();
            foreach(int i in orderList)
            {
                dummyList.Add(bits[i]);
            }
            //TODO: groupSize méretű csoportokra bontva megnézzük a parítását dummyList-nek
            return output;
        }

        public string BitHashing()
        {
            byte[] bitBytes = new byte[(bits.Count + 7) / 8];
            HashAlgorithm algo = SHA256.Create();
            //az int-ként tárolt bitekből byte-okat csinálunk
            for (int i=0; i<bits.Count; i++)
            {
                if (bits[i] == 1)
                {
                    bitBytes[i / 8] |= (byte)(1 << (7 - (i % 8)));
                }
            }
            //a hash-elt biteket stringként adjuk vissza
            byte[] bitHash = algo.ComputeHash(bitBytes);
            string output = Convert.ToString(bitHash);
            return output;
        }
    }

    internal class MyQubit
    {
        private int storedBit;
        private string storedBase;

        Random rnd;

        public MyQubit(int bit, string mBase,Random centralRnd) 
        {
            storedBit = bit;
            storedBase = mBase;
            rnd = centralRnd;
        }

        public int GetValue(string mBase)
        {
            if (storedBase.Equals(mBase))
            {
                //egyező alap esetén semmi sem történik, az érték nem változik
            }
            else
            {
                //eltérő alap esetén a qubit értéke változik
                storedBit = rnd.Next(0, 2);
                storedBase = mBase;
            }
            return storedBit;
        }
    }
}