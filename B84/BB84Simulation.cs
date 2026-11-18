using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public Channel channel;

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
        public List<int> CorrectedBobBits { get; private set; } = new List<int>();
        public List<int> SharedIndices { get; private set; } = new List<int>();
        public List<int> validatorBitSpots { get; private set; } = new List<int>();
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
                    LastStatusMessage = "1. lépés: Alice előkészítette a biteket és a bázisokat.";
                    break;
                case 1:
                    List<MyQubit> qubits = alice.GeneratedQubits();
                    channel.TransmitQubits(qubits);
                    LastStatusMessage = "2. lépés: Alice elküldte a polarizált fotonokat a csatornán.";
                    break;
                case 2:
                    //Alice elküldi a qubitokat
                    if (eavesdropping && evePercent > 0)
                    {
                        //Eve lehallgatja a qubitok egy részét
                        channel.ApplyEve();
                        LastStatusMessage = $"3. lépés: Eve (lehallgató) lehallgatta a fotonok {evePercent}%-át!";
                    }
                    else
                    {
                        //ha be lenne kapcsolva a lehallgatás, az itt történne
                        LastStatusMessage = "3. lépés: Nem történt lehallgatás a csatornán.";
                    }
                    break;
                case 3:
                    //Bob megkapja a qubitokat (ezek lehet, hogy le lettek hallgatva Eve által)
                    List<MyQubit> recievedQubits = channel.RecieveQubits();
                    //Bob megméri a kapott qubiteket és a kapott biteket elmenti
                    bob.SetBits(bob.MeasureQubits(recievedQubits));
                    InitialBobBits = new List<int>(bob.GetBits());
                    LastStatusMessage = "4. lépés: Bob megmérte a beérkező fotonokat a saját bázisaival.";
                    break;
                case 4:
                    SharedIndices = channel.SharedBases(alice.GetBases(), bob.GetBases());
                    //Sifting
                    //Alice és Bob egyeztetik az alapjaikat és az eltérő alapokat eldobják
                    //Ekkor a megtartott (és nem lehallgatott) bitek megegyeznek mindkét oldalon (feltételezve, hogy nincsen zaj)
                    alice.KeepAvailableSpot(SharedIndices);
                    bob.KeepAvailableSpot(SharedIndices);
                    Debug.Print("Alice available spots: "+alice.availableSpots.Count);
                    Debug.Print("Bob available spots: " + bob.availableSpots.Count);
                    if (SharedIndices.Count < 2)
                    {
                        LastStatusMessage = $"5. lépés: Bázisegyeztetés hiba, nincsenek közös bázisok!";
                        stepCount = -1;
                    }
                    else
                    {
                        LastStatusMessage = $"5. lépés: Bázisegyeztetés kész ({SharedIndices.Count} egyező bit maradt).";
                    }
                    break;
                case 5:
                    //kiválasztunk Y% véletlen bitet (a megtartott bitekből)
                    validatorBitSpots = alice.SelectRandomBitSpots(50);
                    Debug.Print("Public bitSpot count: "+validatorBitSpots.Count);
                    List<int> alicePublicBits = alice.SelectBits(validatorBitSpots);
                    List<int> bobPublicBits = bob.SelectBits(validatorBitSpots);
                    alice.RemoveAvailableSpot(validatorBitSpots);
                    bob.RemoveAvailableSpot(validatorBitSpots);
                    LastQBER = channel.CheckQBER(alicePublicBits,bobPublicBits);
                    if (LastQBER > maxErrorRate)
                    {
                        LastStatusMessage = $"6. lépés: Néhány bitet publikussá teszünk és összehasonlítunk. RIASZTÁS! Túl magas hibaarány (QBER: {LastQBER * 100:0.0}% > {maxErrorRate * 100:0.0}%). Protokoll megszakítva!";
                        //megáll az algoritmus és újra kell kezdenünk
                        stepCount = -1;
                    }
                    else 
                    {
                        LastStatusMessage = $"6. lépés: Néhány bitet publikussá teszünk és összehasonlítunk. Hibaellenőrzés sikeres (QBER: {LastQBER * 100:0.0}%).";
                    }
                    break;
                case 6:
                    //az két felhasználónál tárolt biteket össze kell hasonlítanunk, és a hibákat javítanunk kell
                    channel.CascadeParityCheck(alice,bob);
                    CorrectedBobBits = new List<int>(bob.GetBits());
                    LastStatusMessage = "7. lépés: Hibajavítás lefutott.";
                    break;
                case 7:
                    //végezhetnénk hasehelést, de egyrészt nincsen jó mód vizuálisan megmutatni a hash eredményét
                    //másrészt a biteket a felhasználó a javításnál könnyen össze tudja vetni
                    LastStatusMessage = "8. lépés: A kulcsok egyeznek, mindkétt fél rendelkezik a titkos kulccsal.";
                    break;
                default:
                    //végére értünk, megáll a szimuláció
                    LastStatusMessage = "A szimuláció véget ért. Nyomj új Véletlen generálást az újrakezdéshez!";
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
            CorrectedBobBits.Clear();
            validatorBitSpots.Clear();
            LastStatusMessage = "Készen áll.";
        }

        //végigmegyünk a szimulátoron
        public void FullSim()
        {
            while (stepCount < 9)
            {
                NextStep();
            }
        }

        public string GetFinalKeyString() => string.Join("", alice.SelectBits(alice.availableSpots));
        public string GetBobFinalKeyString() => string.Join("", bob.SelectBits(bob.availableSpots));
    }

    internal class Channel
    {
        //A lehallgató fél a qubitok hány százalékát méri meg?
        private int eve;

        public List<int> eavesdropped = new List<int>();
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
            eavesdropped = new List<int>();
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
            //összehasonlítjuk Alice és Bob mérési alapjait
            for (int i=0; i<aliceBases.Count; i++)
            {
                if (aliceBases[i].Equals(bobBases[i]))
                {
                    output.Add(i); 
                }
            }
            //a megegyező alapok helyét adjuk vissza
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
            return (float)incorrect / (float)(incorrect+correct);
        }

        public void CascadeParityCheck(SimulatedUser userA, SimulatedUser userB)
        {
            //csak úgy teszünk, mintha lenne egy hibaellenőrzés
            //if (rnd.Next(0,30)==0)
            //{
            //    //ha megütjük az 1/30 esélyt, nem csinálunk semmit
            //    return;
            //}
            //else
            //{
            //    //amúgy meg úgy teszünk, mintha megtaláltuk volna a hibákat és kijavítottuk őket 
                userB.SetBits(userA.GetBits());
            //
        }
    }

    internal class SimulatedUser
    {
        //itt tároljuk akár a véletlenül generált, akár a mérés eredményeként kapott biteket (0 és 1)
        List<int> bits;
        //itt tároljuk akár a véletlenül generált, akár a mérés eredményeként kapott mérési alapokat (D és R)
        List<string> bases;
        //a még elérhető pozíciók helyét tárolja
        public List<int> availableSpots;

        private Random rnd;

        public SimulatedUser(Random centralRnd)
        {
            bits = new List<int>();
            bases = new List<string>();
            availableSpots = new List<int>();
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
            for (int i = 0; i<count; i++)
            {
                availableSpots.Add(i);
            }
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
        private void GenerateBases(int count)
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

        public List<int> SelectRandomBitSpots(int targetPercent)
        {
            if (targetPercent > 100)
            {
                targetPercent = 100;
            }
            if (targetPercent < 0)
            {
                targetPercent = 0;
            }
            List<int> output = new List<int>();
            int targetCount = (int)((targetPercent / 100.0f) * (availableSpots.Count));
            while (output.Count != targetCount)
            {
                //a még elérhető bitek pozícióiból választunk párat
                int newNum = rnd.Next(0, availableSpots.Count);
                //csak akkor adjuk a kimenethez, ha még nem választottuk (egyedi kimenetek kellenek)
                if (!output.Contains(availableSpots[newNum]))
                {
                    output.Add(availableSpots[newNum]); 
                }
            }
            return output;
        }

        public List<int> SelectBits(List<int> bitSpots)
        {

            List<int> output = new List<int>();

            foreach (int i in bitSpots)
            {
                output.Add(bits[i]);
            }

            return output;
        }

        //eltávolítjuk a megadott bitek helyeit (magukhoz a bitekhez nem nyúlunk)
        public void RemoveAvailableSpot(List<int> bitSpots)
        {
            foreach (int i in bitSpots)
            {
                availableSpots.Remove(i);
            }
        }

        //csak a megadott bitek helyeit tartjuk meg (magukhoz a bitekhez még mindig nem nyúlunk)
        public void KeepAvailableSpot(List<int> bitSpots)
        {
            availableSpots = bitSpots;
            /*foreach (int i in availableSpots)
            {
                if (!bitSpots.Contains(i))
                    availableSpots.Remove(i);
            }*/
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