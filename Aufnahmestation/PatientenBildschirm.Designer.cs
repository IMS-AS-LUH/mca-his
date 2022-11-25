namespace HisDemo.Aufnahmestation
{
    partial class PatientenBildschirm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatientenBildschirm));
            this.labelHead = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // labelHead
            // 
            this.labelHead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(197)))), ((int)(((byte)(97)))));
            this.labelHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHead.Location = new System.Drawing.Point(0, 0);
            this.labelHead.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelHead.Name = "labelHead";
            this.labelHead.Size = new System.Drawing.Size(1613, 79);
            this.labelHead.TabIndex = 0;
            this.labelHead.Text = "Herzlich Willkommen an der MCA-Teststation";
            this.labelHead.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelMain
            // 
            this.panelMain.BackgroundImage = global::HisDemo.Aufnahmestation.Properties.Resources.mca_logo;
            this.panelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 79);
            this.panelMain.Margin = new System.Windows.Forms.Padding(2);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1613, 934);
            this.panelMain.TabIndex = 1;
            // 
            // PatientenBildschirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(28F, 55F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1613, 1013);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.labelHead);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(9, 8, 9, 8);
            this.Name = "PatientenBildschirm";
            this.Text = "PatientenBildschirm";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelHead;
        private System.Windows.Forms.Panel panelMain;
    }
}