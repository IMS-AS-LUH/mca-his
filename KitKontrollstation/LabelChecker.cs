using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace HisDemo.KitKontrollstation
{
    public partial class LabelChecker : Form
    {
        public LabelChecker()
        {
            InitializeComponent();
        }

        private void LabelChecker_Load(object sender, EventArgs e)
        {
            if (!mcaBarcodeReader.Connect())
            {
                this.Enabled = false;
                MessageBox.Show(this, "Barcode-Scanner nicht gefunden.", "Startfehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        delegate void VoidStringDelegate(string rawData);
        private void mcaBarcodeReader_BarcodeInvalid(string rawData)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidStringDelegate(this.mcaBarcodeReader_BarcodeInvalid), rawData);
                return;
            }
            // Unknown codes will be listet as invalid in the UI and alert the User with a long beep sequence
            listView.Items.Add($"Invalid: {rawData}");
            mcaBarcodeReader.Beep(10);
        }

        int currentID = -1;
        LabelTypeCheck currentType = new DummyLabel();
        bool finishedID = true;
        bool overrideUnfinished = false;
        List<MCABarcodeID> scanned = new List<MCABarcodeID>();
        List<string> subscopes = new List<string>();

        delegate void VoidObjectDelegate(object mcaBarcodeId);

        private void mcaBarcodeReader_BarcodeReceived(object mcaBarcodeId)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidObjectDelegate(this.mcaBarcodeReader_BarcodeReceived), mcaBarcodeId);
                return;
            }
            MCABarcodeID mCA = (MCABarcodeID)mcaBarcodeId;

            if (mCA.Scope != MCABarcodeID.IDScope.Specimen)
            {
                // Non-Specimen IDs alert the User
                mcaBarcodeReader.Beep(10);
                listView.Items.Add($"SCANNED NON SPECIMEN!");
                listView.Items.Add(mCA.ToString());
            }

            if (mCA.ID != currentID)
            {
                // Mismatch in ID - for now, this is indicated as new kit.
                if (!finishedID && !overrideUnfinished && currentType.RequiredDistinctLabels != 0)
                {
                    // previous ID did not finish, send warning
                    listView.BackColor = Color.Red;
                    mcaBarcodeReader.Beep(7);
                    DisplayMessageOverlay("Vorheriges Set nicht abgeschlossen!", $"Erneut scannen zum fortfahren");
                    statusLabel.Text = $"Previous Set did not finish! Scan again to start the new ID!";
                    overrideUnfinished = true;
                    return;
                    
                }
                finishedID = false;
                overrideUnfinished = false;
                scanned.Clear();
                subscopes.Clear();
                listView.Items.Clear();

                currentID = (int)mCA.ID;
                currentType = getLabelData(mCA);
                labelID.Text = $"{mCA.B32ID} ({currentID:D})";
                statusLabel.Text = $"New ID scanned: {mCA.B32ID} of Type: {currentType.Name}";
                mcaBarcodeReader.Beep(1);
                listView.BackColor = Color.White;



            }

            if (!scanned.Contains(mCA))
            {
                // New Code matching current ID scope.
                // Just counts upwords without extra notification
                scanned.Add(mCA);
                subscopes.Add(mCA.SubScope);

                if (scanned.Count == currentType.RequiredDistinctLabels && currentType.RequiredDistinctLabels != 0)
                {
                    listView.BackColor = Color.Lime;
                    sessionCounter.Value += 1;
                    finishedID = true;
                    mcaBarcodeReader.Beep(1);
                    System.Threading.Thread.Sleep(100);
                    // mcaBarcodeReader.Beep(1);
                }
                else if (scanned.Count > currentType.RequiredDistinctLabels && currentType.RequiredDistinctLabels != 0)
                {
                    listView.BackColor = Color.Red;
                    mcaBarcodeReader.Beep(7);
                    DisplayMessageOverlay("Zu viele Codes!", $"Es sollten nur >> {currentType.RequiredDistinctLabels} << unterschiedliche Label sein!");
                    statusLabel.Text = $"Too many Sub-IDs. There should only be {currentType.RequiredDistinctLabels} distinct labels";
                    if (finishedID)
                    {
                        sessionCounter.Value -= 1;
                    }
                    
                }
                listView.Items.Add(mCA.ToString());
                mcaBarcodeReader.Beep(1);
                scanStatus.Text = currentType.GetScanStatus(subscopes);
            }
            else
            {
                // Code scanned that matches but was already scanned
                // This is not a big deal, but inform the User of the fact.
                listView.Items.Add($"Rescan of: {mCA}");
                mcaBarcodeReader.Beep(1);
            }

            // Update the distinct count display
            labelDistinct.Text = $"Distinct: {scanned.Count}";
        }

        private delegate void DisplayMessageOverlayDelegate(string title, string message, bool green = false, bool fatal = false);
        private void DisplayMessageOverlay(string title, string message, bool green = false, bool fatal = false)
        {

            if (InvokeRequired)
                Invoke(new DisplayMessageOverlayDelegate(this.DisplayMessageOverlay), title, message, green, fatal);
            else
            {
                var mo = new MessageOverlay();
                mo.Title = title;
                mo.Message = message;
                if (green) mo.BlinkColor = Color.Lime;
                if (fatal)
                {
                    mo.lockSpeed = 0.005f;
                    mo.BlinkColor = Color.Red;
                }
                mo.ShowDialog(this);
                mo.Dispose();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            // Oem7 = Ä-Taste
            if (keyData == (Keys.Oem7 | Keys.Control | Keys.Alt))
            {
                listView.BackColor = Color.Yellow;
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        private LabelTypeCheck getLabelData(MCABarcodeID mCA)
        {
            List<LabelTypeCheck> allAvailable = new List<LabelTypeCheck>();
            allAvailable.Add(new RachenLabel());

            foreach (var type in allAvailable)
            {
                if (type.ContainsSubScope(mCA.SubScope))
                {
                    return type;
                }
            }

            return new DummyLabel(); 

        }

        abstract private class LabelTypeCheck
        {
            public string Name { get; set; }
            public int RequiredDistinctLabels { get; set; }
            private int maxLabelChars = 0;

            public List<string> neededSubScopes = new List<string>();
            public List<LabelSpec> labelOrder = new List<LabelSpec>();

            public bool ContainsSubScope(string subScope)
            {
                return neededSubScopes.Contains(subScope);
            }

            public void UpdateNeededSubScopes()
            {
                this.neededSubScopes.Clear();
                this.maxLabelChars = 0;
                foreach (var labelType in labelOrder)
                {
                    if (labelType.name.Length > this.maxLabelChars)
                    {
                        this.maxLabelChars = labelType.name.Length;
                    }
                    foreach (var scope in labelType.subscopes)
                    {
                        this.neededSubScopes.Add(scope);
                    }
                }
                this.RequiredDistinctLabels = this.neededSubScopes.Count;
            }

            public string GetScanStatus(List<string> scannedSubscopes)
            {
                string outstring = $"Scan status:\nTyp: {Name}\n";
                foreach (var labelType in labelOrder)
                {
                    outstring += labelType.name + new string(' ', this.maxLabelChars - labelType.name.Length ) +": ";
                    int inside = 0;
                    int outside = 0;
                    foreach (var scope in labelType.subscopes)
                    {
                        if (scannedSubscopes.Contains(scope))
                        {
                            inside += 1;
                        }
                        else
                        {
                            outside += 1;
                        }
                    }
                    outstring += String.Concat(System.Linq.Enumerable.Repeat(" X", inside)) + String.Concat(System.Linq.Enumerable.Repeat(" -", outside)) + "\n";
                }
                return outstring;
            }

            public class LabelSpec
            {
                public string name { get; set; }
                public List<string> subscopes;
                public LabelSpec()
                {
                    this.name = "";
                    this.subscopes = new List<string>();
                }
                public LabelSpec(string labelName, List<string> containedSubscopes)
                {
                    this.name = labelName;
                    this.subscopes = containedSubscopes;
                }
            }
        }

        // unknown label dummy
        private class DummyLabel : LabelTypeCheck
        {
            public DummyLabel()
            {
                this.Name = "Unbekannt";
                this.labelOrder.Clear();
                this.UpdateNeededSubScopes();
            }
        }

        // white labels
        private class RachenLabel : LabelTypeCheck
        {
            public RachenLabel()
            {
                this.Name = "Rachenabstrich";
                this.labelOrder.Clear();
                this.labelOrder.Add(new LabelTypeCheck.LabelSpec("Tüte", new List<string>(new string[] { "KT01", "KT02", "KT03" })));
                this.labelOrder.Add(new LabelTypeCheck.LabelSpec("Oben", new List<string>(new string[] { "VT03", "VT04", "VT21" })));
                this.labelOrder.Add(new LabelTypeCheck.LabelSpec("Seite", new List<string>(new string[] { "VS01", "VS02", "VS03" , "VS21"})));
                this.UpdateNeededSubScopes();
            }
        }
    }
}
