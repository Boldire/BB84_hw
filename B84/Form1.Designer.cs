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
            this.labelStepDescription = new System.Windows.Forms.Label();
            this.labelResult = new System.Windows.Forms.Label();
            this.dgvSimulation = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSimulation)).BeginInit();
            this.SuspendLayout();
            // 
            // chkEavesActive
            // 
            this.chkEavesActive.AutoSize = true;
            this.chkEavesActive.Location = new System.Drawing.Point(22, 25);
            this.chkEavesActive.Name = "chkEavesActive";
            this.chkEavesActive.Size = new System.Drawing.Size(75, 17);
            this.chkEavesActive.TabIndex = 0;
            this.chkEavesActive.Text = "Lehallgató";
            this.chkEavesActive.UseVisualStyleBackColor = true;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(127, 21);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(179, 23);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "Véletlen generálás";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.Véletlengenerálás_Click);
            // 
            // btnNextStep
            // 
            this.btnNextStep.Location = new System.Drawing.Point(356, 21);
            this.btnNextStep.Name = "btnNextStep";
            this.btnNextStep.Size = new System.Drawing.Size(165, 23);
            this.btnNextStep.TabIndex = 2;
            this.btnNextStep.Text = "Következő lépés";
            this.btnNextStep.UseVisualStyleBackColor = true;
            // 
            // btnRunAll
            // 
            this.btnRunAll.Location = new System.Drawing.Point(584, 19);
            this.btnRunAll.Name = "btnRunAll";
            this.btnRunAll.Size = new System.Drawing.Size(109, 23);
            this.btnRunAll.TabIndex = 3;
            this.btnRunAll.Text = "Teljes futtatás";
            this.btnRunAll.UseVisualStyleBackColor = true;
            // 
            // txtSenderBits
            // 
            this.txtSenderBits.Location = new System.Drawing.Point(22, 95);
            this.txtSenderBits.Name = "txtSenderBits";
            this.txtSenderBits.Size = new System.Drawing.Size(100, 20);
            this.txtSenderBits.TabIndex = 4;
            // 
            // labelSenderBits
            // 
            this.labelSenderBits.AutoSize = true;
            this.labelSenderBits.Location = new System.Drawing.Point(19, 70);
            this.labelSenderBits.Name = "labelSenderBits";
            this.labelSenderBits.Size = new System.Drawing.Size(58, 13);
            this.labelSenderBits.TabIndex = 5;
            this.labelSenderBits.Text = "Küldő bitjei";
            // 
            // txtSenderBases
            // 
            this.txtSenderBases.Location = new System.Drawing.Point(202, 95);
            this.txtSenderBases.Name = "txtSenderBases";
            this.txtSenderBases.Size = new System.Drawing.Size(100, 20);
            this.txtSenderBases.TabIndex = 6;
            // 
            // labelSenderBases
            // 
            this.labelSenderBases.AutoSize = true;
            this.labelSenderBases.Location = new System.Drawing.Point(199, 70);
            this.labelSenderBases.Name = "labelSenderBases";
            this.labelSenderBases.Size = new System.Drawing.Size(69, 13);
            this.labelSenderBases.TabIndex = 7;
            this.labelSenderBases.Text = "Küldő bázisai";
            // 
            // txtRecieverBases
            // 
            this.txtRecieverBases.Location = new System.Drawing.Point(354, 95);
            this.txtRecieverBases.Name = "txtRecieverBases";
            this.txtRecieverBases.Size = new System.Drawing.Size(100, 20);
            this.txtRecieverBases.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(351, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Vevő bázisai";
            // 
            // labelStepDescription
            // 
            this.labelStepDescription.AutoSize = true;
            this.labelStepDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStepDescription.Location = new System.Drawing.Point(485, 70);
            this.labelStepDescription.Name = "labelStepDescription";
            this.labelStepDescription.Size = new System.Drawing.Size(399, 13);
            this.labelStepDescription.TabIndex = 10;
            this.labelStepDescription.Text = "Állapot: Készen áll. Nyomd meg a \'Következő lépés\' vagy a \'Teljes futtatás\' gombo" +
    "t!";
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Location = new System.Drawing.Point(19, 720);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(210, 13);
            this.labelResult.TabIndex = 11;
            this.labelResult.Text = "Eredmény: Még nincs lefuttatott szimuláció!";
            // 
            // dgvSimulation
            // 
            this.dgvSimulation.AllowUserToAddRows = false;
            this.dgvSimulation.AllowUserToDeleteRows = false;
            this.dgvSimulation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSimulation.ColumnHeadersVisible = false;
            this.dgvSimulation.Location = new System.Drawing.Point(127, 203);
            this.dgvSimulation.Name = "dgvSimulation";
            this.dgvSimulation.ReadOnly = true;
            this.dgvSimulation.Size = new System.Drawing.Size(240, 150);
            this.dgvSimulation.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1328, 786);
            this.Controls.Add(this.dgvSimulation);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.labelStepDescription);
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
        private System.Windows.Forms.Label labelStepDescription;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.DataGridView dgvSimulation;
    }
}

