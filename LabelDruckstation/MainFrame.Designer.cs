namespace LabelDruckstation
{
    partial class MainFrame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrame));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.labelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.logFileLinkLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.label7 = new System.Windows.Forms.Label();
            this.nudYOffset = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.nudXOffset = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.labelCountR46 = new System.Windows.Forms.Label();
            this.labelCountS25 = new System.Windows.Forms.Label();
            this.labelCountD10 = new System.Windows.Forms.Label();
            this.buttonPrintR46 = new System.Windows.Forms.Button();
            this.buttonPrintS25 = new System.Windows.Forms.Button();
            this.buttonPrintD10 = new System.Windows.Forms.Button();
            this.buttonPrinterSettings = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonNextSet = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.labelEndIDB = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelStartIDB = new System.Windows.Forms.Label();
            this.nudStartID = new System.Windows.Forms.NumericUpDown();
            this.labelEndID = new System.Windows.Forms.Label();
            this.nudSetCount = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelIDCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbPrintSubstituteColorFill = new System.Windows.Forms.CheckBox();
            this.labelKitVariantInfo = new System.Windows.Forms.Label();
            this.cbKitTypeVariant = new System.Windows.Forms.ComboBox();
            this.cbAutoTray = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonRefreshComfortPrinterList = new System.Windows.Forms.Button();
            this.comboBoxComfortPrinter = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonPrintAll = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXOffset)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSetCount)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelStatus,
            this.logFileLinkLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 839);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(0, 0, 14, 0);
            this.statusStrip.Size = new System.Drawing.Size(984, 42);
            this.statusStrip.TabIndex = 0;
            // 
            // labelStatus
            // 
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(115, 32);
            this.labelStatus.Text = "Loading...";
            // 
            // logFileLinkLabel
            // 
            this.logFileLinkLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logFileLinkLabel.ForeColor = System.Drawing.Color.Blue;
            this.logFileLinkLabel.Name = "logFileLinkLabel";
            this.logFileLinkLabel.Size = new System.Drawing.Size(147, 32);
            this.logFileLinkLabel.Text = "Log-Dateien";
            this.logFileLinkLabel.Click += new System.EventHandler(this.logFileLinkLabel_Click);
            // 
            // printDocument
            // 
            this.printDocument.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument_BeginPrint);
            this.printDocument.EndPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument_EndPrint);
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
            this.printDocument.QueryPageSettings += new System.Drawing.Printing.QueryPageSettingsEventHandler(this.printDocument_QueryPageSettings);
            // 
            // printDialog
            // 
            this.printDialog.AllowPrintToFile = false;
            this.printDialog.Document = this.printDocument;
            this.printDialog.ShowNetwork = false;
            this.printDialog.UseEXDialog = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(312, 450);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(159, 25);
            this.label7.TabIndex = 48;
            this.label7.Text = "mm nach unten";
            // 
            // nudYOffset
            // 
            this.nudYOffset.DecimalPlaces = 1;
            this.nudYOffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudYOffset.Location = new System.Drawing.Point(208, 448);
            this.nudYOffset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudYOffset.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudYOffset.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            this.nudYOffset.Name = "nudYOffset";
            this.nudYOffset.Size = new System.Drawing.Size(96, 31);
            this.nudYOffset.TabIndex = 47;
            this.toolTip.SetToolTip(this.nudYOffset, "Korrektur der Ausrichtung des Druckbildes: Wie viele Millimeter muss das Gedruckt" +
        "e auf dem Papier nach unten verschoben werden?");
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 450);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(191, 25);
            this.label9.TabIndex = 46;
            this.label9.Text = "Offset-Korrektur Y:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(312, 412);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(164, 25);
            this.label6.TabIndex = 45;
            this.label6.Text = "mm nach rechts";
            // 
            // nudXOffset
            // 
            this.nudXOffset.DecimalPlaces = 1;
            this.nudXOffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudXOffset.Location = new System.Drawing.Point(208, 410);
            this.nudXOffset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudXOffset.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudXOffset.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            this.nudXOffset.Name = "nudXOffset";
            this.nudXOffset.Size = new System.Drawing.Size(96, 31);
            this.nudXOffset.TabIndex = 44;
            this.toolTip.SetToolTip(this.nudXOffset, "Korrektur der Ausrichtung des Druckbildes: Wie viele Millimeter muss das Gedruckt" +
        "e auf dem Papier nach rechts verschoben werden?");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 412);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(190, 25);
            this.label4.TabIndex = 43;
            this.label4.Text = "Offset-Korrektur X:";
            // 
            // labelCountR46
            // 
            this.labelCountR46.AutoSize = true;
            this.labelCountR46.Location = new System.Drawing.Point(512, 502);
            this.labelCountR46.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCountR46.Name = "labelCountR46";
            this.labelCountR46.Size = new System.Drawing.Size(26, 25);
            this.labelCountR46.TabIndex = 42;
            this.labelCountR46.Text = "--";
            // 
            // labelCountS25
            // 
            this.labelCountS25.AutoSize = true;
            this.labelCountS25.Location = new System.Drawing.Point(512, 563);
            this.labelCountS25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCountS25.Name = "labelCountS25";
            this.labelCountS25.Size = new System.Drawing.Size(26, 25);
            this.labelCountS25.TabIndex = 41;
            this.labelCountS25.Text = "--";
            // 
            // labelCountD10
            // 
            this.labelCountD10.AutoSize = true;
            this.labelCountD10.Location = new System.Drawing.Point(512, 627);
            this.labelCountD10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCountD10.Name = "labelCountD10";
            this.labelCountD10.Size = new System.Drawing.Size(26, 25);
            this.labelCountD10.TabIndex = 40;
            this.labelCountD10.Text = "--";
            // 
            // buttonPrintR46
            // 
            this.buttonPrintR46.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPrintR46.ForeColor = System.Drawing.Color.DarkRed;
            this.buttonPrintR46.Location = new System.Drawing.Point(12, 487);
            this.buttonPrintR46.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonPrintR46.Name = "buttonPrintR46";
            this.buttonPrintR46.Size = new System.Drawing.Size(496, 56);
            this.buttonPrintR46.TabIndex = 39;
            this.buttonPrintR46.Text = "Bögen Drucken: Rechteck (46x11-R)";
            this.toolTip.SetToolTip(this.buttonPrintR46, "Startet einen Druckauftrag mit der rechts angegebenen Anzahl von Blättern des Bog" +
        "en-Typs Avery Zweckform 46x11-R (Außen auf die Eppis, selbst überlappend)");
            this.buttonPrintR46.UseVisualStyleBackColor = true;
            this.buttonPrintR46.Click += new System.EventHandler(this.buttonPrintR46_Click);
            // 
            // buttonPrintS25
            // 
            this.buttonPrintS25.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPrintS25.ForeColor = System.Drawing.Color.DarkRed;
            this.buttonPrintS25.Location = new System.Drawing.Point(12, 548);
            this.buttonPrintS25.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonPrintS25.Name = "buttonPrintS25";
            this.buttonPrintS25.Size = new System.Drawing.Size(496, 56);
            this.buttonPrintS25.TabIndex = 38;
            this.buttonPrintS25.Text = "Bögen Drucken: Quadrat (25x25-S)";
            this.toolTip.SetToolTip(this.buttonPrintS25, "Startet einen Druckauftrag mit der rechts angegebenen Anzahl von Blättern des Bog" +
        "en-Typs Avery Zweckform 25x25-S (Außen auf die Tüte).");
            this.buttonPrintS25.UseVisualStyleBackColor = true;
            this.buttonPrintS25.Click += new System.EventHandler(this.buttonPrintS25_Click);
            // 
            // buttonPrintD10
            // 
            this.buttonPrintD10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPrintD10.ForeColor = System.Drawing.Color.DarkRed;
            this.buttonPrintD10.Location = new System.Drawing.Point(12, 610);
            this.buttonPrintD10.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonPrintD10.Name = "buttonPrintD10";
            this.buttonPrintD10.Size = new System.Drawing.Size(496, 56);
            this.buttonPrintD10.TabIndex = 37;
            this.buttonPrintD10.Text = "Bögen Drucken: Rund (10-RND)";
            this.toolTip.SetToolTip(this.buttonPrintD10, "Startet einen Druckauftrag mit der rechts angegebenen Anzahl von Blättern des Bog" +
        "en-Typs Avery Zweckform 10-RND (Eppi/QiA-Deckel).");
            this.buttonPrintD10.UseVisualStyleBackColor = true;
            this.buttonPrintD10.Click += new System.EventHandler(this.buttonPrintD10_Click);
            // 
            // buttonPrinterSettings
            // 
            this.buttonPrinterSettings.Location = new System.Drawing.Point(12, 346);
            this.buttonPrinterSettings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonPrinterSettings.Name = "buttonPrinterSettings";
            this.buttonPrinterSettings.Size = new System.Drawing.Size(398, 56);
            this.buttonPrinterSettings.TabIndex = 36;
            this.buttonPrinterSettings.Text = "Druckereinst. (\"Drucken\" = OK)";
            this.toolTip.SetToolTip(this.buttonPrinterSettings, "Öffnet einen Windows-Druckdialog um die Druckerauswahl und Einstellungen festzule" +
        "gen. Zustand wird nach Programmende nicht gespeichert!");
            this.buttonPrinterSettings.UseVisualStyleBackColor = true;
            this.buttonPrinterSettings.Click += new System.EventHandler(this.buttonPrinterSettings_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonNextSet);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.labelEndIDB);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.labelStartIDB);
            this.groupBox1.Controls.Add(this.nudStartID);
            this.groupBox1.Controls.Add(this.labelEndID);
            this.groupBox1.Controls.Add(this.nudSetCount);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.labelIDCount);
            this.groupBox1.Location = new System.Drawing.Point(12, 104);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(528, 237);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Druckbereich";
            // 
            // buttonNextSet
            // 
            this.buttonNextSet.Location = new System.Drawing.Point(356, 88);
            this.buttonNextSet.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonNextSet.Name = "buttonNextSet";
            this.buttonNextSet.Size = new System.Drawing.Size(152, 46);
            this.buttonNextSet.TabIndex = 35;
            this.buttonNextSet.Text = "Inkrement";
            this.toolTip.SetToolTip(this.buttonNextSet, "Ändert die Start ID auf die aktuelle Letzte ID plus 1.");
            this.buttonNextSet.UseVisualStyleBackColor = true;
            this.buttonNextSet.Click += new System.EventHandler(this.buttonNextSet_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 33);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 25);
            this.label3.TabIndex = 25;
            this.label3.Text = "Start-ID:";
            // 
            // labelEndIDB
            // 
            this.labelEndIDB.AutoSize = true;
            this.labelEndIDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEndIDB.Location = new System.Drawing.Point(340, 190);
            this.labelEndIDB.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEndIDB.Name = "labelEndIDB";
            this.labelEndIDB.Size = new System.Drawing.Size(26, 26);
            this.labelEndIDB.TabIndex = 34;
            this.labelEndIDB.Text = "--";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 73);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 25);
            this.label2.TabIndex = 26;
            this.label2.Text = "Anzahl Sätze:";
            // 
            // labelStartIDB
            // 
            this.labelStartIDB.AutoSize = true;
            this.labelStartIDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStartIDB.Location = new System.Drawing.Point(340, 33);
            this.labelStartIDB.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStartIDB.Name = "labelStartIDB";
            this.labelStartIDB.Size = new System.Drawing.Size(26, 26);
            this.labelStartIDB.TabIndex = 33;
            this.labelStartIDB.Text = "--";
            // 
            // nudStartID
            // 
            this.nudStartID.Location = new System.Drawing.Point(172, 31);
            this.nudStartID.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudStartID.Maximum = new decimal(new int[] {
            300000,
            0,
            0,
            0});
            this.nudStartID.Name = "nudStartID";
            this.nudStartID.Size = new System.Drawing.Size(160, 31);
            this.nudStartID.TabIndex = 27;
            this.toolTip.SetToolTip(this.nudStartID, "Numerische ID des ersten zu druckenden Label-Sets.");
            this.nudStartID.ValueChanged += new System.EventHandler(this.settingFieldChanged);
            // 
            // labelEndID
            // 
            this.labelEndID.AutoSize = true;
            this.labelEndID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEndID.Location = new System.Drawing.Point(166, 190);
            this.labelEndID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEndID.Name = "labelEndID";
            this.labelEndID.Size = new System.Drawing.Size(26, 26);
            this.labelEndID.TabIndex = 32;
            this.labelEndID.Text = "--";
            // 
            // nudSetCount
            // 
            this.nudSetCount.Location = new System.Drawing.Point(172, 71);
            this.nudSetCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudSetCount.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.nudSetCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSetCount.Name = "nudSetCount";
            this.nudSetCount.Size = new System.Drawing.Size(160, 31);
            this.nudSetCount.TabIndex = 28;
            this.toolTip.SetToolTip(this.nudSetCount, "Anzahl der zu druckenden Sätze zu je 63 Labels.");
            this.nudSetCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSetCount.ValueChanged += new System.EventHandler(this.settingFieldChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 190);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 25);
            this.label8.TabIndex = 31;
            this.label8.Text = "Letzte ID:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 154);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 25);
            this.label5.TabIndex = 29;
            this.label5.Text = "Anzahl IDs:";
            // 
            // labelIDCount
            // 
            this.labelIDCount.AutoSize = true;
            this.labelIDCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIDCount.Location = new System.Drawing.Point(166, 154);
            this.labelIDCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelIDCount.Name = "labelIDCount";
            this.labelIDCount.Size = new System.Drawing.Size(28, 26);
            this.labelIDCount.TabIndex = 30;
            this.labelIDCount.Text = "--";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkRed;
            this.label1.Location = new System.Drawing.Point(4, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(588, 90);
            this.label1.TabIndex = 24;
            this.label1.Text = "ACHTUNG: Labels sind \"scharf\" und dürfen nur mit koordiniertem ID-Pooling erstell" +
    "t werden!\r\n";
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.cbAutoTray);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.labelCountD10);
            this.panel1.Controls.Add(this.nudYOffset);
            this.panel1.Controls.Add(this.buttonPrintR46);
            this.panel1.Controls.Add(this.labelCountS25);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.buttonPrintS25);
            this.panel1.Controls.Add(this.labelCountR46);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.buttonPrintD10);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.nudXOffset);
            this.panel1.Controls.Add(this.buttonPrinterSettings);
            this.panel1.Location = new System.Drawing.Point(18, 15);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(950, 819);
            this.panel1.TabIndex = 49;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbPrintSubstituteColorFill);
            this.groupBox4.Controls.Add(this.labelKitVariantInfo);
            this.groupBox4.Controls.Add(this.cbKitTypeVariant);
            this.groupBox4.Location = new System.Drawing.Point(12, 675);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Size = new System.Drawing.Size(448, 140);
            this.groupBox4.TabIndex = 53;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Kit-Variante";
            // 
            // cbPrintSubstituteColorFill
            // 
            this.cbPrintSubstituteColorFill.AutoSize = true;
            this.cbPrintSubstituteColorFill.Enabled = false;
            this.cbPrintSubstituteColorFill.Location = new System.Drawing.Point(276, 33);
            this.cbPrintSubstituteColorFill.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbPrintSubstituteColorFill.Name = "cbPrintSubstituteColorFill";
            this.cbPrintSubstituteColorFill.Size = new System.Drawing.Size(154, 29);
            this.cbPrintSubstituteColorFill.TabIndex = 2;
            this.cbPrintSubstituteColorFill.Text = "Ersatzfarbe";
            this.toolTip.SetToolTip(this.cbPrintSubstituteColorFill, "Wenn aktiv, wird statt dem normalen S/W-Druck auf Vorgefärbte Bögen der Druck mit" +
        " vollflächiger Farbe gedruckt (um ersatzweise auf Weiße bögen zu drucken).");
            this.cbPrintSubstituteColorFill.UseVisualStyleBackColor = true;
            // 
            // labelKitVariantInfo
            // 
            this.labelKitVariantInfo.Location = new System.Drawing.Point(8, 75);
            this.labelKitVariantInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelKitVariantInfo.Name = "labelKitVariantInfo";
            this.labelKitVariantInfo.Size = new System.Drawing.Size(436, 62);
            this.labelKitVariantInfo.TabIndex = 1;
            this.labelKitVariantInfo.Text = "(...)";
            // 
            // cbKitTypeVariant
            // 
            this.cbKitTypeVariant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKitTypeVariant.FormattingEnabled = true;
            this.cbKitTypeVariant.Location = new System.Drawing.Point(8, 31);
            this.cbKitTypeVariant.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbKitTypeVariant.Name = "cbKitTypeVariant";
            this.cbKitTypeVariant.Size = new System.Drawing.Size(260, 33);
            this.cbKitTypeVariant.TabIndex = 0;
            this.cbKitTypeVariant.SelectedIndexChanged += new System.EventHandler(this.cbKitTypeVariant_SelectedIndexChanged);
            // 
            // cbAutoTray
            // 
            this.cbAutoTray.AutoSize = true;
            this.cbAutoTray.Checked = true;
            this.cbAutoTray.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoTray.Enabled = false;
            this.cbAutoTray.Location = new System.Drawing.Point(416, 362);
            this.cbAutoTray.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbAutoTray.Name = "cbAutoTray";
            this.cbAutoTray.Size = new System.Drawing.Size(118, 29);
            this.cbAutoTray.TabIndex = 51;
            this.cbAutoTray.Text = "Komfort";
            this.toolTip.SetToolTip(this.cbAutoTray, "Wenn aktiviert wird - falls Komfortdrucker ausgewählt - automatisch das Fach ausg" +
        "ewählt (Druckeinstellung wird überschrieben).");
            this.cbAutoTray.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::LabelDruckstation.Properties.Resources.MCADruckstationIcon;
            this.pictureBox1.Location = new System.Drawing.Point(560, 10);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(376, 212);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 50;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonRefreshComfortPrinterList);
            this.groupBox2.Controls.Add(this.comboBoxComfortPrinter);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.buttonPrintAll);
            this.groupBox2.Location = new System.Drawing.Point(560, 238);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(376, 427);
            this.groupBox2.TabIndex = 49;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Komfort-Druck";
            // 
            // buttonRefreshComfortPrinterList
            // 
            this.buttonRefreshComfortPrinterList.Location = new System.Drawing.Point(262, 31);
            this.buttonRefreshComfortPrinterList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonRefreshComfortPrinterList.Name = "buttonRefreshComfortPrinterList";
            this.buttonRefreshComfortPrinterList.Size = new System.Drawing.Size(108, 58);
            this.buttonRefreshComfortPrinterList.TabIndex = 42;
            this.buttonRefreshComfortPrinterList.Text = "Laden";
            this.toolTip.SetToolTip(this.buttonRefreshComfortPrinterList, "Liest die liste Kompatibler drucker (neu) ein.");
            this.buttonRefreshComfortPrinterList.UseVisualStyleBackColor = true;
            this.buttonRefreshComfortPrinterList.Click += new System.EventHandler(this.refreshComfortPrinterList);
            // 
            // comboBoxComfortPrinter
            // 
            this.comboBoxComfortPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxComfortPrinter.FormattingEnabled = true;
            this.comboBoxComfortPrinter.Location = new System.Drawing.Point(12, 56);
            this.comboBoxComfortPrinter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxComfortPrinter.Name = "comboBoxComfortPrinter";
            this.comboBoxComfortPrinter.Size = new System.Drawing.Size(244, 33);
            this.comboBoxComfortPrinter.TabIndex = 41;
            this.comboBoxComfortPrinter.SelectedIndexChanged += new System.EventHandler(this.comboBoxComfortPrinter_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(6, 27);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(213, 25);
            this.label11.TabIndex = 40;
            this.label11.Text = "Kompatibler Drucker:";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 108);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(364, 237);
            this.label10.TabIndex = 39;
            this.label10.Text = resources.GetString("label10.Text");
            // 
            // buttonPrintAll
            // 
            this.buttonPrintAll.Enabled = false;
            this.buttonPrintAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPrintAll.ForeColor = System.Drawing.Color.DarkRed;
            this.buttonPrintAll.Location = new System.Drawing.Point(6, 346);
            this.buttonPrintAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonPrintAll.Name = "buttonPrintAll";
            this.buttonPrintAll.Size = new System.Drawing.Size(364, 65);
            this.buttonPrintAll.TabIndex = 38;
            this.buttonPrintAll.Text = "Alle Bögen drucken";
            this.toolTip.SetToolTip(this.buttonPrintAll, "Startet einen Druckauftrag mit der rechts angegebenen Anzahl von Blättern des Bog" +
        "en-Typs Avery Zweckform 10-RND (Eppi/QiA-Deckel).");
            this.buttonPrintAll.UseVisualStyleBackColor = true;
            this.buttonPrintAll.Click += new System.EventHandler(this.buttonPrintAll_Click);
            // 
            // toolTip
            // 
            this.toolTip.IsBalloon = true;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.ToolTipTitle = "Erklärung:";
            this.toolTip.UseAnimation = false;
            this.toolTip.UseFading = false;
            // 
            // MainFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 881);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(980, 861);
            this.Name = "MainFrame";
            this.Text = "MCA Label Druckstation";
            this.Load += new System.EventHandler(this.MainFrame_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXOffset)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSetCount)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel labelStatus;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.PrintDialog printDialog;
        private System.Windows.Forms.Label labelEndIDB;
        private System.Windows.Forms.Label labelStartIDB;
        private System.Windows.Forms.Label labelEndID;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelIDCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudSetCount;
        private System.Windows.Forms.NumericUpDown nudStartID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonPrinterSettings;
        private System.Windows.Forms.Button buttonPrintD10;
        private System.Windows.Forms.Button buttonPrintR46;
        private System.Windows.Forms.Button buttonPrintS25;
        private System.Windows.Forms.Label labelCountR46;
        private System.Windows.Forms.Label labelCountS25;
        private System.Windows.Forms.Label labelCountD10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudYOffset;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudXOffset;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonPrintAll;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripStatusLabel logFileLinkLabel;
        private System.Windows.Forms.Button buttonNextSet;
        private System.Windows.Forms.ComboBox comboBoxComfortPrinter;
        private System.Windows.Forms.Button buttonRefreshComfortPrinterList;
        private System.Windows.Forms.CheckBox cbAutoTray;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cbKitTypeVariant;
        private System.Windows.Forms.Label labelKitVariantInfo;
        private System.Windows.Forms.CheckBox cbPrintSubstituteColorFill;
    }
}

