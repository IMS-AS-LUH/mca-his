namespace HisDemo.Aufnahmestation
{
    partial class Aufnahme
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Aufnahme));
            this.pictureBoxPatientPreview = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelMainStage = new System.Windows.Forms.Panel();
            this.stageTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panelMainStatus = new System.Windows.Forms.Panel();
            this.labelMainStatus = new System.Windows.Forms.Label();
            this.splitContainerRight = new System.Windows.Forms.SplitContainer();
            this.labelPatientPreviewTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listViewProtocol = new System.Windows.Forms.ListView();
            this.labelClock = new System.Windows.Forms.Label();
            this.labelProtocolTitle = new System.Windows.Forms.Label();
            this.panelContainerStatus = new System.Windows.Forms.Panel();
            this.labelContainerStatusTitle = new System.Windows.Forms.Label();
            this.timerPatientPreviewRefresh = new System.Windows.Forms.Timer(this.components);
            this.timerPatMonUpdate = new System.Windows.Forms.Timer(this.components);
            this.commandKeyPanel = new HisDemo.UI.CommandKeyPanel();
            this.mcaBarcodeReader = new HisDemo.UI.MCABarcodeReader(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPatientPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelMainStage.SuspendLayout();
            this.panelMainStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).BeginInit();
            this.splitContainerRight.Panel1.SuspendLayout();
            this.splitContainerRight.Panel2.SuspendLayout();
            this.splitContainerRight.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelContainerStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxPatientPreview
            // 
            this.pictureBoxPatientPreview.BackColor = System.Drawing.Color.Silver;
            this.pictureBoxPatientPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxPatientPreview.Location = new System.Drawing.Point(0, 20);
            this.pictureBoxPatientPreview.Name = "pictureBoxPatientPreview";
            this.pictureBoxPatientPreview.Size = new System.Drawing.Size(254, 343);
            this.pictureBoxPatientPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPatientPreview.TabIndex = 1;
            this.pictureBoxPatientPreview.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelMainStage);
            this.splitContainer1.Panel1.Controls.Add(this.panelMainStatus);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainerRight);
            this.splitContainer1.Size = new System.Drawing.Size(1558, 883);
            this.splitContainer1.SplitterDistance = 1300;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // panelMainStage
            // 
            this.panelMainStage.BackColor = System.Drawing.Color.White;
            this.panelMainStage.Controls.Add(this.stageTableLayoutPanel);
            this.panelMainStage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMainStage.Location = new System.Drawing.Point(0, 0);
            this.panelMainStage.Name = "panelMainStage";
            this.panelMainStage.Size = new System.Drawing.Size(1300, 810);
            this.panelMainStage.TabIndex = 1;
            // 
            // stageTableLayoutPanel
            // 
            this.stageTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stageTableLayoutPanel.AutoScroll = true;
            this.stageTableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetDouble;
            this.stageTableLayoutPanel.ColumnCount = 4;
            this.stageTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.stageTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.stageTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.stageTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 239F));
            this.stageTableLayoutPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stageTableLayoutPanel.Location = new System.Drawing.Point(3, 12);
            this.stageTableLayoutPanel.Name = "stageTableLayoutPanel";
            this.stageTableLayoutPanel.Padding = new System.Windows.Forms.Padding(3);
            this.stageTableLayoutPanel.RowCount = 1;
            this.stageTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 849F));
            this.stageTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 849F));
            this.stageTableLayoutPanel.Size = new System.Drawing.Size(1294, 792);
            this.stageTableLayoutPanel.TabIndex = 0;
            // 
            // panelMainStatus
            // 
            this.panelMainStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panelMainStatus.Controls.Add(this.labelMainStatus);
            this.panelMainStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelMainStatus.Location = new System.Drawing.Point(0, 810);
            this.panelMainStatus.Name = "panelMainStatus";
            this.panelMainStatus.Size = new System.Drawing.Size(1300, 73);
            this.panelMainStatus.TabIndex = 0;
            // 
            // labelMainStatus
            // 
            this.labelMainStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMainStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMainStatus.Location = new System.Drawing.Point(0, 0);
            this.labelMainStatus.Name = "labelMainStatus";
            this.labelMainStatus.Padding = new System.Windows.Forms.Padding(6);
            this.labelMainStatus.Size = new System.Drawing.Size(1300, 73);
            this.labelMainStatus.TabIndex = 0;
            this.labelMainStatus.Text = "Lade Anwendung ....";
            this.labelMainStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainerRight
            // 
            this.splitContainerRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRight.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerRight.IsSplitterFixed = true;
            this.splitContainerRight.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRight.Name = "splitContainerRight";
            this.splitContainerRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerRight.Panel1
            // 
            this.splitContainerRight.Panel1.BackColor = System.Drawing.Color.Silver;
            this.splitContainerRight.Panel1.Controls.Add(this.pictureBoxPatientPreview);
            this.splitContainerRight.Panel1.Controls.Add(this.labelPatientPreviewTitle);
            // 
            // splitContainerRight.Panel2
            // 
            this.splitContainerRight.Panel2.Controls.Add(this.panel1);
            this.splitContainerRight.Panel2.Controls.Add(this.panelContainerStatus);
            this.splitContainerRight.Size = new System.Drawing.Size(254, 883);
            this.splitContainerRight.SplitterDistance = 363;
            this.splitContainerRight.TabIndex = 0;
            this.splitContainerRight.TabStop = false;
            // 
            // labelPatientPreviewTitle
            // 
            this.labelPatientPreviewTitle.AutoSize = true;
            this.labelPatientPreviewTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPatientPreviewTitle.Location = new System.Drawing.Point(0, 0);
            this.labelPatientPreviewTitle.Name = "labelPatientPreviewTitle";
            this.labelPatientPreviewTitle.Size = new System.Drawing.Size(242, 20);
            this.labelPatientPreviewTitle.TabIndex = 2;
            this.labelPatientPreviewTitle.Text = "Kontrollansicht Patientenmonitor:";
            this.labelPatientPreviewTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.listViewProtocol);
            this.panel1.Controls.Add(this.labelClock);
            this.panel1.Controls.Add(this.labelProtocolTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(254, 461);
            this.panel1.TabIndex = 1;
            // 
            // listViewProtocol
            // 
            this.listViewProtocol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewProtocol.GridLines = true;
            this.listViewProtocol.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewProtocol.HideSelection = false;
            this.listViewProtocol.Location = new System.Drawing.Point(0, 20);
            this.listViewProtocol.Name = "listViewProtocol";
            this.listViewProtocol.Size = new System.Drawing.Size(254, 368);
            this.listViewProtocol.TabIndex = 5;
            this.listViewProtocol.UseCompatibleStateImageBehavior = false;
            this.listViewProtocol.View = System.Windows.Forms.View.List;
            // 
            // labelClock
            // 
            this.labelClock.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelClock.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelClock.Location = new System.Drawing.Point(0, 388);
            this.labelClock.Name = "labelClock";
            this.labelClock.Size = new System.Drawing.Size(254, 73);
            this.labelClock.TabIndex = 6;
            this.labelClock.Text = "00:00:00\r\n99.99.9999";
            this.labelClock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProtocolTitle
            // 
            this.labelProtocolTitle.AutoSize = true;
            this.labelProtocolTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelProtocolTitle.Location = new System.Drawing.Point(0, 0);
            this.labelProtocolTitle.Name = "labelProtocolTitle";
            this.labelProtocolTitle.Size = new System.Drawing.Size(74, 20);
            this.labelProtocolTitle.TabIndex = 4;
            this.labelProtocolTitle.Text = "Protokoll:";
            this.labelProtocolTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelContainerStatus
            // 
            this.panelContainerStatus.Controls.Add(this.labelContainerStatusTitle);
            this.panelContainerStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelContainerStatus.Location = new System.Drawing.Point(0, 0);
            this.panelContainerStatus.Name = "panelContainerStatus";
            this.panelContainerStatus.Size = new System.Drawing.Size(254, 55);
            this.panelContainerStatus.TabIndex = 0;
            // 
            // labelContainerStatusTitle
            // 
            this.labelContainerStatusTitle.AutoSize = true;
            this.labelContainerStatusTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelContainerStatusTitle.Location = new System.Drawing.Point(0, 0);
            this.labelContainerStatusTitle.Name = "labelContainerStatusTitle";
            this.labelContainerStatusTitle.Size = new System.Drawing.Size(199, 20);
            this.labelContainerStatusTitle.TabIndex = 4;
            this.labelContainerStatusTitle.Text = "Stationsstatus: Unbekannt";
            this.labelContainerStatusTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerPatientPreviewRefresh
            // 
            this.timerPatientPreviewRefresh.Enabled = true;
            this.timerPatientPreviewRefresh.Interval = 250;
            this.timerPatientPreviewRefresh.Tick += new System.EventHandler(this.timerPatientPreviewRefresh_Tick);
            // 
            // timerPatMonUpdate
            // 
            this.timerPatMonUpdate.Tick += new System.EventHandler(this.timerPatMonUpdate_Tick);
            // 
            // commandKeyPanel
            // 
            this.commandKeyPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.commandKeyPanel.ButtonFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commandKeyPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.commandKeyPanel.Location = new System.Drawing.Point(0, 883);
            this.commandKeyPanel.Name = "commandKeyPanel";
            this.commandKeyPanel.Size = new System.Drawing.Size(1558, 150);
            this.commandKeyPanel.TabIndex = 0;
            this.commandKeyPanel.TabStop = false;
            // 
            // mcaBarcodeReader
            // 
            this.mcaBarcodeReader.BarcodeReceived += new HisDemo.UI.MCABarcodeReader.BarcodeReceivedHandler(this.mcaBarcodeReader_BarcodeReceived);
            this.mcaBarcodeReader.BarcodeInvalid += new HisDemo.UI.MCABarcodeReader.BarcodeInvalidHandler(this.mcaBarcodeReader_BarcodeInvalid);
            this.mcaBarcodeReader.QRCodeFilter += new System.EventHandler<UI.MCABarcodeReader.QRCodeFilterEventArgs>(this.mcaBarcodeReader_QRCodeFilter);
            // 
            // Aufnahme
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1558, 1033);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.commandKeyPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Aufnahme";
            this.Text = "Aufnahmestation - MCA-Projekt";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Aufnahme_FormClosing);
            this.Load += new System.EventHandler(this.Aufnahme_Load);
            this.ResizeBegin += new System.EventHandler(this.Aufnahme_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.Aufnahme_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPatientPreview)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelMainStage.ResumeLayout(false);
            this.panelMainStatus.ResumeLayout(false);
            this.splitContainerRight.Panel1.ResumeLayout(false);
            this.splitContainerRight.Panel1.PerformLayout();
            this.splitContainerRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).EndInit();
            this.splitContainerRight.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelContainerStatus.ResumeLayout(false);
            this.panelContainerStatus.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion

        private UI.CommandKeyPanel commandKeyPanel;
        private System.Windows.Forms.PictureBox pictureBoxPatientPreview;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainerRight;
        private System.Windows.Forms.Timer timerPatientPreviewRefresh;
        private System.Windows.Forms.Label labelPatientPreviewTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelProtocolTitle;
        private System.Windows.Forms.Panel panelContainerStatus;
        private System.Windows.Forms.Label labelContainerStatusTitle;
        private System.Windows.Forms.ListView listViewProtocol;
        private UI.MCABarcodeReader mcaBarcodeReader;
        private System.Windows.Forms.Panel panelMainStage;
        private System.Windows.Forms.Panel panelMainStatus;
        private System.Windows.Forms.Label labelMainStatus;
        private System.Windows.Forms.TableLayoutPanel stageTableLayoutPanel;
        private System.Windows.Forms.Timer timerPatMonUpdate;

        private System.Windows.Forms.Label labelClock;
    }
}

