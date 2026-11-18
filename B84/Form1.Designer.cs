namespace B84
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkEavesActive = new System.Windows.Forms.CheckBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnNextStep = new System.Windows.Forms.Button();
            this.btnRunAll = new System.Windows.Forms.Button();
            this.txtSenderBits = new System.Windows.Forms.TextBox();
            this.labelSenderBits = new System.Windows.Forms.Label();
            this.txtSenderBases = new System.Windows.Forms.TextBox();
            this.labelSenderBases = new System.Windows.Forms.Label();
            this.txtRecieverBases = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblStepDescription = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.dgvSimulation = new System.Windows.Forms.DataGridView();
            this.numEavesDropper = new System.Windows.Forms.NumericUpDown();
            this.numBitCount = new System.Windows.Forms.NumericUpDown();
            this.labelNumBits = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSimulation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEavesDropper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBitCount)).BeginInit();
            this.SuspendLayout();
            // 
            // chkEavesActive
            // 
            this.chkEavesActive.AutoSize = true;
            this.chkEavesActive.Location = new System.Drawing.Point(165, 21);
            this.chkEavesActive.Name = "chkEavesActive";
            this.chkEavesActive.Size = new System.Drawing.Size(75, 17);
            this.chkEavesActive.TabIndex = 0;
            this.chkEavesActive.Text = "Lehallgató";
            this.chkEavesActive.UseVisualStyleBackColor = true;
            this.chkEavesActive.CheckedChanged += new System.EventHandler(this.chkEavesActive_CheckedChanged);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(485, 120);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(179, 23);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "Véletlen generálás";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.Véletlengenerálás_Click);
            // 
            // btnNextStep
            // 
            this.btnNextStep.Location = new System.Drawing.Point(705, 120);
            this.btnNextStep.Name = "btnNextStep";
            this.btnNextStep.Size = new System.Drawing.Size(165, 23);
            this.btnNextStep.TabIndex = 2;
            this.btnNextStep.Text = "Következő lépés";
            this.btnNextStep.UseVisualStyleBackColor = true;
            this.btnNextStep.Click += new System.EventHandler(this.btnNextStep_Click);
            // 
            // btnRunAll
            // 
            this.btnRunAll.Location = new System.Drawing.Point(906, 120);
            this.btnRunAll.Name = "btnRunAll";
            this.btnRunAll.Size = new System.Drawing.Size(109, 23);
            this.btnRunAll.TabIndex = 3;
            this.btnRunAll.Text = "Teljes futtatás";
            this.btnRunAll.UseVisualStyleBackColor = true;
            this.btnRunAll.Click += new System.EventHandler(this.btnRunAll_Click);
            // 
            // txtSenderBits
            // 
            this.txtSenderBits.Location = new System.Drawing.Point(24, 122);
            this.txtSenderBits.Name = "txtSenderBits";
            this.txtSenderBits.Size = new System.Drawing.Size(118, 20);
            this.txtSenderBits.TabIndex = 4;
            // 
            // labelSenderBits
            // 
            this.labelSenderBits.AutoSize = true;
            this.labelSenderBits.Location = new System.Drawing.Point(21, 98);
            this.labelSenderBits.Name = "labelSenderBits";
            this.labelSenderBits.Size = new System.Drawing.Size(54, 13);
            this.labelSenderBits.TabIndex = 5;
            this.labelSenderBits.Text = "Alice bitjei";
            
            // 
            // txtSenderBases
            // 
            this.txtSenderBases.Location = new System.Drawing.Point(165, 122);
            this.txtSenderBases.Name = "txtSenderBases";
            this.txtSenderBases.Size = new System.Drawing.Size(140, 20);
            this.txtSenderBases.TabIndex = 6;
            // 
            // labelSenderBases
            // 
            this.labelSenderBases.AutoSize = true;
            this.labelSenderBases.Location = new System.Drawing.Point(162, 98);
            this.labelSenderBases.Name = "labelSenderBases";
            this.labelSenderBases.Size = new System.Drawing.Size(65, 13);
            this.labelSenderBases.TabIndex = 7;
            this.labelSenderBases.Text = "Alice bázisai";
            // 
            // txtRecieverBases
            // 
            this.txtRecieverBases.Location = new System.Drawing.Point(329, 122);
            this.txtRecieverBases.Name = "txtRecieverBases";
            this.txtRecieverBases.Size = new System.Drawing.Size(140, 20);
            this.txtRecieverBases.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(326, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Bob bázisai";
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.AutoSize = true;
            this.lblStepDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblStepDescription.Location = new System.Drawing.Point(19, 188);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(457, 15);
            this.lblStepDescription.TabIndex = 10;
            this.lblStepDescription.Text = "Állapot: Készen áll. Nyomd meg a \'Következő lépés\' vagy a \'Teljes futtatás\' gombo" +
    "t!";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblResult.Location = new System.Drawing.Point(19, 735);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(243, 15);
            this.lblResult.TabIndex = 11;
            this.lblResult.Text = "Eredmény: Még nincs lefuttatott szimuláció!";
            // 
            // dgvSimulation
            // 
            this.dgvSimulation.AllowUserToAddRows = false;
            this.dgvSimulation.AllowUserToDeleteRows = false;
            this.dgvSimulation.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvSimulation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSimulation.ColumnHeadersVisible = false;
            this.dgvSimulation.Location = new System.Drawing.Point(23, 220);
            this.dgvSimulation.Name = "dgvSimulation";
            this.dgvSimulation.ReadOnly = true;
            this.dgvSimulation.Size = new System.Drawing.Size(1278, 502);
            this.dgvSimulation.TabIndex = 12;
            // 
            // numEavesDropper
            // 
            this.numEavesDropper.Increment = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numEavesDropper.Location = new System.Drawing.Point(165, 48);
            this.numEavesDropper.Name = "numEavesDropper";
            this.numEavesDropper.Size = new System.Drawing.Size(120, 20);
            this.numEavesDropper.TabIndex = 15;
            this.numEavesDropper.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numEavesDropper.ValueChanged += new System.EventHandler(this.chkEavesActive_CheckedChanged);
            // 
            // numBitCount
            // 
            this.numBitCount.Location = new System.Drawing.Point(24, 48);
            this.numBitCount.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numBitCount.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numBitCount.Name = "numBitCount";
            this.numBitCount.Size = new System.Drawing.Size(98, 20);
            this.numBitCount.TabIndex = 16;
            this.numBitCount.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // labelNumBits
            // 
            this.labelNumBits.AutoSize = true;
            this.labelNumBits.Location = new System.Drawing.Point(21, 21);
            this.labelNumBits.Name = "labelNumBits";
            this.labelNumBits.Size = new System.Drawing.Size(64, 13);
            this.labelNumBits.TabIndex = 17;
            this.labelNumBits.Text = "Bitek száma";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1328, 786);
            this.Controls.Add(this.labelNumBits);
            this.Controls.Add(this.numBitCount);
            this.Controls.Add(this.numEavesDropper);
            this.Controls.Add(this.dgvSimulation);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblStepDescription);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRecieverBases);
            this.Controls.Add(this.labelSenderBases);
            this.Controls.Add(this.txtSenderBases);
            this.Controls.Add(this.labelSenderBits);
            this.Controls.Add(this.txtSenderBits);
            this.Controls.Add(this.btnRunAll);
            this.Controls.Add(this.btnNextStep);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.chkEavesActive);
            this.Name = "Form1";
            this.Text = "BB84 Szimulátor";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSimulation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEavesDropper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBitCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkEavesActive;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnNextStep;
        private System.Windows.Forms.Button btnRunAll;
        private System.Windows.Forms.TextBox txtSenderBits;
        private System.Windows.Forms.Label labelSenderBits;
        private System.Windows.Forms.TextBox txtSenderBases;
        private System.Windows.Forms.Label labelSenderBases;
        private System.Windows.Forms.TextBox txtRecieverBases;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStepDescription;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.DataGridView dgvSimulation;
        private System.Windows.Forms.NumericUpDown numEavesDropper;
        private System.Windows.Forms.NumericUpDown numBitCount;
        private System.Windows.Forms.Label labelNumBits;
    }
}

