namespace HisDemo.KitKontrollstation
{
    partial class LabelChecker
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
            this.components = new System.ComponentModel.Container();
            this.labelID = new System.Windows.Forms.Label();
            this.labelDistinct = new System.Windows.Forms.Label();
            this.listView = new System.Windows.Forms.ListView();
            this.mcaBarcodeReader = new HisDemo.UI.MCABarcodeReader(this.components);
            this.sessionCounter = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.scanStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sessionCounter)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelID
            // 
            this.labelID.AutoSize = true;
            this.labelID.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelID.Location = new System.Drawing.Point(18, 19);
            this.labelID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(108, 26);
            this.labelID.TabIndex = 0;
            this.labelID.Text = "XXXXXX";
            // 
            // labelDistinct
            // 
            this.labelDistinct.AutoSize = true;
            this.labelDistinct.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDistinct.Location = new System.Drawing.Point(206, 19);
            this.labelDistinct.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDistinct.Name = "labelDistinct";
            this.labelDistinct.Size = new System.Drawing.Size(119, 26);
            this.labelDistinct.TabIndex = 1;
            this.labelDistinct.Text = "Distinct: 0";
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listView.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(8, 56);
            this.listView.Margin = new System.Windows.Forms.Padding(2);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(354, 328);
            this.listView.TabIndex = 2;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.List;
            // 
            // mcaBarcodeReader
            // 
            this.mcaBarcodeReader.BarcodeReceived += new HisDemo.UI.MCABarcodeReader.BarcodeReceivedHandler(this.mcaBarcodeReader_BarcodeReceived);
            this.mcaBarcodeReader.BarcodeInvalid += new HisDemo.UI.MCABarcodeReader.BarcodeInvalidHandler(this.mcaBarcodeReader_BarcodeInvalid);
            // 
            // sessionCounter
            // 
            this.sessionCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionCounter.Location = new System.Drawing.Point(484, 19);
            this.sessionCounter.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.sessionCounter.Name = "sessionCounter";
            this.sessionCounter.Size = new System.Drawing.Size(82, 29);
            this.sessionCounter.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(378, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Session:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 386);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(577, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(10, 17);
            this.statusLabel.Text = " ";
            // 
            // scanStatus
            // 
            this.scanStatus.AutoSize = true;
            this.scanStatus.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scanStatus.Location = new System.Drawing.Point(377, 56);
            this.scanStatus.Name = "scanStatus";
            this.scanStatus.Size = new System.Drawing.Size(128, 18);
            this.scanStatus.TabIndex = 6;
            this.scanStatus.Text = "Scan status:";
            // 
            // LabelChecker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 408);
            this.Controls.Add(this.scanStatus);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.sessionCounter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.labelDistinct);
            this.Controls.Add(this.labelID);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "LabelChecker";
            this.Text = "LabelChecker Prototyp";
            this.Load += new System.EventHandler(this.LabelChecker_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sessionCounter)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.MCABarcodeReader mcaBarcodeReader;
        private System.Windows.Forms.Label labelID;
        private System.Windows.Forms.Label labelDistinct;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.NumericUpDown sessionCounter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Label scanStatus;
    }
}