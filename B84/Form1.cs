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
            //alapértelmezett beállítások induláskor: van lehallgató, random bemenetek
            numEavesDropper.Enabled = chkEavesActive.Checked;
            GenerateRandomInputs();
        }

        private void Véletlengenerálás_Click(object sender, EventArgs e)
        {
            GenerateRandomInputs();
        }

        //random bemeneet generál a véletlen generálás gombhoz
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

        //lehallgató checkbox változása
        private void chkEavesActive_CheckedChanged(object sender, EventArgs e)
        {
            numEavesDropper.Enabled = chkEavesActive.Checked;
        }





        private void trckEavesDropper_Scroll(object sender, EventArgs e)
        {

        }
    }
}
