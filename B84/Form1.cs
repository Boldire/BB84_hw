using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace B84
{
    public partial class Form1 : Form
    {
        private BB84Simulation sim = new BB84Simulation();
        private Random rnd = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) 
        {
            // Alapértelmezett beállítások induláskor: van lehallgató, random bemenetek
            numEavesDropper.Enabled = chkEavesActive.Checked;
            GenerateRandomInputs();
        }

        private void Véletlengenerálás_Click(object sender, EventArgs e)
        {
            GenerateRandomInputs();
        }

        // Random bemeneet generál a véletlen generálás gombhoz, beállítja a ui-t
        private void GenerateRandomInputs() 
        {
            int count = (int)numBitCount.Value;
            char[] aBits = new char[count];
            char[] aBases = new char[count];
            char[] bBases = new char[count];

            for (int i = 0; i < count; i++)
            {
                aBits[i] = rnd.Next(0, 2) == 0 ? '0' : '1';
                aBases[i] = rnd.Next(0, 2) == 0 ? 'R' : 'D';
                bBases[i] = rnd.Next(0, 2) == 0 ? 'R' : 'D';
            }

            txtSenderBits.Text = new string(aBits);
            txtSenderBases.Text = new string(aBases);
            txtRecieverBases.Text = new string(bBases);

            sim.ResetSimulation();
            dgvSimulation.Rows.Clear();
            dgvSimulation.Columns.Clear();
            lblStepDescription.Text = "Állapot: Új véletlen adatok legenerálva. Kattints a 'Következő lépés' vagy 'Teljes futtatás' gombra!";
            lblResult.Text = "Eredmény: Még nincs lefuttatott szimuláció.";
            lblResult.ForeColor = Color.Black;


        }

        // Bemenetek kiolvasása UI-ból, ellenőrzés és átadás a logikának
        private bool PrepareSimulationFromUI()
        {
            string aBitsStr = txtSenderBits.Text.Trim();
            string aBasesStr = txtSenderBases.Text.Trim().ToUpper().Replace('+', 'R').Replace('X', 'D');
            string bBasesStr = txtRecieverBases.Text.Trim().ToUpper().Replace('+', 'R').Replace('X', 'D');

            if (string.IsNullOrEmpty(aBitsStr))
            {
                MessageBox.Show("Adj meg legalább 1 bitet a Küldőnek!", "Hiányzó adat", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            int targetLen = aBitsStr.Length;

            // Ha kevesebb bázist adott meg, mint ahány bit van
            if (aBasesStr.Length < targetLen || bBasesStr.Length < targetLen)
            {
                MessageBox.Show("Kevesebb bázist adtál meg, mint ahány bitet! Pótold a hiányzó karaktereket.", "Hiányos bemenet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Ha több bázist adott meg, levágjuk a felesleget a végéről
            if (aBasesStr.Length > targetLen || bBasesStr.Length > targetLen)
            {
                if (aBasesStr.Length > targetLen)
                {
                    aBasesStr = aBasesStr.Substring(0, targetLen);
                    txtSenderBases.Text = aBasesStr;
                }
                if (bBasesStr.Length > targetLen)
                {
                    bBasesStr = bBasesStr.Substring(0, targetLen);
                    txtRecieverBases.Text = bBasesStr;
                }

                MessageBox.Show($"Több bázist adtál meg, mint ahány bitet ({targetLen} db). A felesleges bázisok a végéről levágásra kerültek!",
                    "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Karakterek érvényességének ellenőrzése és listává alakítása
            List<int> aBits = new List<int>();
            List<string> aBases = new List<string>();
            List<string> bBases = new List<string>();

            for (int i = 0; i < targetLen; i++)
            {
                if (aBitsStr[i] != '0' && aBitsStr[i] != '1')
                {
                    MessageBox.Show("A küldő bitjei csak '0' és '1' karaktereket tartalmazhatnak!", "Hibás formátum", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if ((aBasesStr[i] != 'R' && aBasesStr[i] != 'D') || (bBasesStr[i] != 'R' && bBasesStr[i] != 'D'))
                {
                    MessageBox.Show("A bázisok csak 'R' (vagy '+') és 'D' (vagy 'x') karaktereket tartalmazhatnak!", "Hibás formátum", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                aBits.Add(aBitsStr[i] - '0');
                aBases.Add(aBasesStr[i].ToString());
                bBases.Add(bBasesStr[i].ToString());
            }

            numBitCount.Value = Math.Min(numBitCount.Maximum, Math.Max(numBitCount.Minimum, targetLen));
            sim.Configure(aBits, aBases, bBases, chkEavesActive.Checked, (int)numEavesDropper.Value);
            return true;
        }


        // Következő lépés gomb
        private void btnNextStep_Click(object sender, EventArgs e)
        {
            if (sim.stepCount == 0)
            {
                if (!PrepareSimulationFromUI()) return;
            }

            sim.NextStep();
            UpdateUI();
        }

        // Teljes futtatás gomb
        private void btnRunAll_Click(object sender, EventArgs e)
        {
            if (sim.stepCount == 0 || sim.stepCount == -1 || sim.stepCount > 8)
            {
                if (!PrepareSimulationFromUI()) return;
            }

            while (sim.stepCount != -1 && sim.stepCount <= 8)
            {
                sim.NextStep();
            }
            UpdateUI();
        }

        // Lehallgató checkbox változása
        private void chkEavesActive_CheckedChanged(object sender, EventArgs e)
        {
            numEavesDropper.Enabled = chkEavesActive.Checked;
        }

        // Táblázat és feliratok frissítése a lépések közben
        private void UpdateUI()
        {
            lblStepDescription.Text = sim.LastStatusMessage;
            int n = sim.InitialAliceBits.Count;
            if (n == 0) return;

            dgvSimulation.Rows.Clear();
            dgvSimulation.Columns.Clear();

            dgvSimulation.Columns.Add("Phase", "Protokoll lépés");
            dgvSimulation.Columns[0].Width = 170;

            for (int i = 0; i < n; i++)
            {
                dgvSimulation.Columns.Add($"Bit{i}", $"{i + 1}.");
                dgvSimulation.Columns[i + 1].Width = 35;
            }

            // 1. lépés után: Küldő adatai és polarizáció
            if (sim.stepCount >= 1 || sim.stepCount == -1)
            {
                AddRow("Küldő bitjei", sim.InitialAliceBits.Select(b => b.ToString()).ToArray());
                AddRow("Küldő bázisai", sim.InitialAliceBases.ToArray());

                string[] arrows = new string[n];
                for (int i = 0; i < n; i++)
                    arrows[i] = GetPolarizationArrow(sim.InitialAliceBits[i], sim.InitialAliceBases[i]);
                AddRow("Küldött polarizáció", arrows);
            }

            // 4. lépés után: Vevő adatai
            if (sim.stepCount >= 4 || sim.stepCount == -1)
            {
                AddRow("Vevő bázisai", sim.InitialBobBases.ToArray());
                AddRow("Vevő mért bitjei", sim.InitialBobBits.Select(b => b.ToString()).ToArray());
            }

            // 5. lépés után: Bázisegyeztetés
            if (sim.stepCount >= 5 || sim.stepCount == -1)
            {
                string[] matchRow = new string[n];
                for (int i = 0; i < n; i++)
                    matchRow[i] = sim.SharedIndices.Contains(i) ? "OK" : "-";

                int rowIndex = AddRow("Bázisegyeztetés", matchRow);
                for (int i = 0; i < n; i++)
                {
                    if (sim.SharedIndices.Contains(i))
                        dgvSimulation.Rows[rowIndex].Cells[i + 1].Style.BackColor = Color.LightGreen;
                }
            }

            // Végeredmény kiírása
            if (sim.stepCount >= 6 || sim.stepCount == -1)
            {
                if (sim.stepCount == -1)
                {
                    lblResult.Text = $"RIASZTÁS: Lehallgatás észlelve! QBER hibaarány: {sim.LastQBER * 100:0.0}% (megengedett max: 20%).\nA kulcscsere megszakítva!";
                    lblResult.ForeColor = Color.DarkRed;
                }
                else
                {
                    lblResult.Text = $"Hibaarány (QBER): {sim.LastQBER * 100:0.0}%\n" +
                                     $"Küldő végső kulcsa: {sim.GetFinalKeyString()}\n" +
                                     $"Vevő végső kulcsa:   {sim.GetRecieverFinalKeyString()}";
                    lblResult.ForeColor = Color.DarkGreen;
                }
            }
        }

        // Sorok hozzáadása a táblázathoz
        private int AddRow(string title, string[] values) 
        {
            object[] rowData = new object[values.Length + 1];
            rowData[0] = title;
            for (int i = 0; i < values.Length; i++)
            {
                rowData[i + 1] = values[i];
            }

            return dgvSimulation.Rows.Add(rowData);

        }

        // Polarizáció megállapítása
        private string GetPolarizationArrow(int bit, string basis)
        { 
            if (basis == "R") return bit == 0 ? "↔" : "↕";
            return bit == 0 ? "↗" : "↖";
        }
    }
}
