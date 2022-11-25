namespace HisDemo.Aufnahmestation
{
    partial class ServiceMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceMenu));
            this.label1 = new System.Windows.Forms.Label();
            this.buttonMusterDatensatz = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonQuestionnaireDump = new System.Windows.Forms.Button();
            this.buttonTeachMode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(16, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(819, 112);
            this.label1.TabIndex = 0;
            this.label1.Text = "ACHTUNG! Diese Funktionen sind nur durch IT-Mitarbeiter oder auf deren direkte Anweisung hin zu verwenden!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonMusterDatensatz
            // 
            this.buttonMusterDatensatz.Location = new System.Drawing.Point(459, 128);
            this.buttonMusterDatensatz.Margin = new System.Windows.Forms.Padding(4);
            this.buttonMusterDatensatz.Name = "buttonMusterDatensatz";
            this.buttonMusterDatensatz.Size = new System.Drawing.Size(339, 89);
            this.buttonMusterDatensatz.TabIndex = 2;
            this.buttonMusterDatensatz.Text = "Lade Muster-Datensatz";
            this.buttonMusterDatensatz.UseVisualStyleBackColor = true;
            this.buttonMusterDatensatz.Click += new System.EventHandler(this.buttonMusterDatensatz_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.Location = new System.Drawing.Point(53, 128);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(339, 89);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "ABBRUCH";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonQuestionnaireDump
            // 
            this.buttonQuestionnaireDump.Location = new System.Drawing.Point(53, 299);
            this.buttonQuestionnaireDump.Margin = new System.Windows.Forms.Padding(4);
            this.buttonQuestionnaireDump.Name = "buttonQuestionnaireDump";
            this.buttonQuestionnaireDump.Size = new System.Drawing.Size(339, 45);
            this.buttonQuestionnaireDump.TabIndex = 3;
            this.buttonQuestionnaireDump.Text = "Questionnaire Dump";
            this.buttonQuestionnaireDump.UseVisualStyleBackColor = true;
            this.buttonQuestionnaireDump.Click += new System.EventHandler(this.buttonQuestionnaireDump_Click);
            // 
            // buttonTeachMode
            // 
            this.buttonTeachMode.Location = new System.Drawing.Point(459, 281);
            this.buttonTeachMode.Name = "buttonTeachMode";
            this.buttonTeachMode.Size = new System.Drawing.Size(246, 63);
            this.buttonTeachMode.TabIndex = 4;
            this.buttonTeachMode.Text = "Schulungs-Modus";
            this.buttonTeachMode.UseVisualStyleBackColor = true;
            this.buttonTeachMode.Click += new System.EventHandler(this.buttonTeachMode_Click);
            // 
            // ServiceMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 374);
            this.Controls.Add(this.buttonTeachMode);
            this.Controls.Add(this.buttonQuestionnaireDump);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonMusterDatensatz);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ServiceMenu";
            this.Text = "Service-Menü";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonMusterDatensatz;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonQuestionnaireDump;
        private System.Windows.Forms.Button buttonTeachMode;
    }
}