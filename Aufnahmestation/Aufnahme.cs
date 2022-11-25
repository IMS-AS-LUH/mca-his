using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using HisDemo.DTO;
using HisDemo.UI;

namespace HisDemo.Aufnahmestation
{
    public partial class Aufnahme : Form
    {
        private readonly PatientenBildschirm patientenBildschirm;
        private readonly Logger log;

        PreliminaryDataHandler preliminaryDataHandler;

        List<string> allowedQRImportQIDs = new List<string>();

        private AufnahmePrintDocument printDocument;

        private bool tempdeactivateprint = false;

        /// <summary>
        /// MUST ONLY BE SET, But NEVER RESET!
        /// If entered once, app has to be shut down to quit mode.
        /// Thus eleminiating side effects.
        /// </summary>
        private bool TeachMode = false;
        public Aufnahme()
        {
            log = new Logger(Program.Logger, "GUI");

            preliminaryDataHandler = new PreliminaryDataHandler(
                System.IO.Path.Combine(Program.StoragePath,"records"),
                (Program.SecondaryStoragePath != null) ? System.IO.Path.Combine(Program.SecondaryStoragePath, "records") : null);

            DoubleBuffered = true;

            this.printDocument = new HisDemo.Aufnahmestation.AufnahmePrintDocument();

            InitializeComponent();
            SetDoubleBuffered(stageTableLayoutPanel);

            // 
            // printDocument
            // 
            this.printDocument.DocumentName = "MCA Aufnahmebogen";
            this.printDocument.SignatureLocality = Program.Config.Software.SignatureLocality;

            patientenBildschirm = new PatientenBildschirm();
            patientenBildschirm.GotFocus += PatientenBildschirm_GotFocus;

            printDocument.DataHandler = preliminaryDataHandler;

        }

        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;
            System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // WX_EX_COMPOSITED Style
                return cp;
            }
        }

        Dictionary<string, string> personalAddressCopySchema = new Dictionary<string, string>()
        {
            {"PersonalAddressStreet", "PrimaryLocationStreet" },
            {"PersonalAddressZIPCode", "PrimaryLocationZIPCode" },
            {"PersonalAddressLocality", "PrimaryLocationLocality" }
        };

        private bool CanQNCopyOverPersonalAddress()
        {
            foreach(var key in personalAddressCopySchema.Keys)
            {
                if (!AllLoadedQIDs.Contains(key) || !AllLoadedQIDs.Contains(personalAddressCopySchema[key]))
                    return false;
            }
            return true;
        }

        private void CopyOverPersonalAddress()
        {
            if (!DisplayConfirmationOverlay("Wohnsitzt Adressübernahme", "Primären Aufenthaltsort als Wohnsitz übernehmen? Eingabe wird ggf. überschrieben."))
                return;

            Focus();
            preventFieldValueWrites = true;

            foreach (var key in personalAddressCopySchema.Keys)
            {
                preliminaryDataHandler.SetValue(key, preliminaryDataHandler.GetValue(personalAddressCopySchema[key]));
            }

            // Go there again to reload new value
            DisplayStage(currentStage);
            preventFieldValueWrites = false;
            DisplayStage(currentStage);
        }

        private void PatientenBildschirm_GotFocus(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void SetPrintingCommandKeyPanel()
        {
            commandKeyPanel.SetFunction(9, (Program.Config.Software.DeactivatePrint) ? "Datensatz Speichern" : "Drucken und Datensatz Speichern", x => ActionSaveDataset());
        }

        private void Aufnahme_Load(object sender, EventArgs e)
        {
            log.Diagnostic("Loading UI");

            patientenBildschirm.ShowInTaskbar = false;

            int PatientScreensLeft = 1;

            log.Diagnostic($"Found {Screen.AllScreens.Length} screens.");
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.Primary)
                {
                    this.WindowState = FormWindowState.Maximized;
                    log.Diagnostic($"Primary: {screen.DeviceName}");
#if DEBUG
                    this.Location = screen.WorkingArea.Location;
                    this.Size = screen.WorkingArea.Size;
#else
                    this.Location = screen.Bounds.Location;
                    this.Size = screen.Bounds.Size;
                    FormBorderStyle = FormBorderStyle.FixedSingle;
                    SizeGripStyle = SizeGripStyle.Hide;
                    MaximizeBox = true;
                    MinimizeBox = false;
                    WindowState = FormWindowState.Maximized;
                    Focus();
#endif
                }
                else if (PatientScreensLeft > 0)
                {
                    patientenBildschirm.Show(this);
                    patientenBildschirm.Location = screen.WorkingArea.Location;
                    patientenBildschirm.Size = screen.WorkingArea.Size;
                    patientenBildschirm.WindowState = FormWindowState.Maximized;
                    log.Diagnostic($"Parient: {screen.DeviceName}");
                    PatientScreensLeft--;
                }
            }

            patientenBildschirm.DisplayClear();

            log.Diagnostic("Connecting Barcode Reader");
            PushStatus("Barcodeleser suchen ...");

            if (!mcaBarcodeReader.Connect())
            {
                log.Warning("Failed to connect Barcode reader!", "BarcodeReader");
                DisplayMessageOverlay("Barcode-Leser nicht verbunden.", "Bitte Technik Informieren.\nDie Proben-IDs müssen von Hand eingegeben werden.\n\nMögliche andere Ursache: Applikation läuft bereits.");
            }
            else
                log.Information("Connected to Barcode reader.", "BarcodeReader");

            PushStatus("Eingabemaske laden ...");

            // F1 reserved
            commandKeyPanel.SetFunction(0, "IT-Kontakt / Support", x => HelpDisplay());
            // F2 reserved for special action set in InitStages()
            // F3, F4 Stage Nav
            commandKeyPanel.SetFunction(2, "Vorheriges Formular", x => ChangeStage(true));
            commandKeyPanel.SetFunction(3, "Nächstes Formular", x => ChangeStage(false));
            // F5, F6 Reserved
            // F7, F8 Form Nav
            commandKeyPanel.SetFunction(6, "Vorheriges Feld", x => ChangeField(true));
            commandKeyPanel.SetFunction(7, "Nächstes Feld", x => ChangeField(false));
            // F9, F10
            if (Program.Config.Software.ShowPrinterSettingsDialog)
            {
                commandKeyPanel.SetFunction(8, "Drucker- Einstellungen", x => ShowPrinterSettings());
            }

            SetPrintingCommandKeyPanel();
            
            // F11, F12
            commandKeyPanel.SetFunction(11, "Abbruch, neuer Patient", x => CancelPatient());

            InitStages();

            ExecuteDatasetClear();
        }

        void ShowPrinterSettings()
        {
            using (PrintDialog pd = new PrintDialog())
            {
                pd.Document = printDocument;
                pd.AllowCurrentPage = false;
                pd.AllowPrintToFile = false;
                pd.AllowSelection = false;
                pd.AllowSomePages = false;
                pd.ShowHelp = false;
                pd.ShowNetwork = false;
                pd.ShowDialog();
            }
        }

        void HelpDisplay()
        {
            string dbgRel = "Release";
#if DEBUG
            dbgRel = "Debug";
#endif
            DisplayMessageOverlay("Kontakt / Support Info", $"Ansprechpartner für Software:\n<PERSON>: <MOBILNUMMER>\n\nRechnername: {Environment.MachineName}\nSW-Version: {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version} - {dbgRel}", true);
        }

        private System.Security.Cryptography.RandomNumberGenerator crng = System.Security.Cryptography.RandomNumberGenerator.Create();

        public struct DatasetSavingSubfile
        {
            public string s32ID;
            public string specimenType;
            public override string ToString()
            {
                return $"{specimenType} : {s32ID}";
            }
        }

        private bool _helperAddIDToSave(ref List<DatasetSavingSubfile> specimenIDsToStore, string type, string QID)
        {
            string s32ID = preliminaryDataHandler.GetValue(QID)?.ToString()?.Trim();
            if (s32ID != null && s32ID.Length > 0 && s32ID != "AAAA") // AAAA = none/Empty/Null
            {
                specimenIDsToStore.Add(new DatasetSavingSubfile()
                {
                    s32ID = s32ID,
                    specimenType = type
                });
                return true;
            }
            return false;
        }
        void ActionSaveDataset()
        {
            Focus();
            Enabled = false;

            {
                for (int qni = 0; qni < questionnaires.Count; qni++)
                {
                    Questionnaire qn = questionnaires[qni];
                    for (int qi = 0; qi < qn.Questions.Count; qi++)
                    {
                        Question q = qn.Questions[qi];
                        if (q.QuestionType == QuestionType.JustText) continue;
                        string val = preliminaryDataHandler.GetValue(q.QID)?.ToString();
                        if (q.Required)
                        {
                            if (val == null || val.Trim().Length <= 0)
                            {
                                DisplayMessageOverlay("Pflichtfeld nicht ausgefüllt!",
                                    $"Mindestens ein Feld ist noch nicht ausgefüllt:\n{q.QuestionText}");
                                DisplayStage(qni);
                                FocusField(qi);
                                Enabled = true;
                                return;
                            }

                            if (q.MustAnswerYes && val != "Ja")
                            {
                                DisplayMessageOverlay("Zwingende Einwilligung nicht erteilt!",
                                    $"Folgende Einwilligung muss zwingend erteilt werden:\n{q.QuestionText}");
                                DisplayStage(qni);
                                FocusField(qi);
                                Enabled = true;
                                return;
                            }
                        }
                    }
                }

            }

            {
                // Check contact explicit
                int qnidContact = 6; // Note: Should be adjusted according to Questionnaire-Provider
                string phone = preliminaryDataHandler.GetValue("ContactPhone")?.ToString();
                string email = preliminaryDataHandler.GetValue("ContactEmail")?.ToString();
                if ((phone == null || phone.Trim().Length <= 0) && (email == null || email.Trim().Length <= 0))
                {
                    DisplayMessageOverlay("Kontaktdaten nicht ausgefüllt!",
                        "Sie müssen mindestens einen Kontaktweg angeben!");
                    DisplayStage(qnidContact);
                    Enabled = true;
                    return;
                }

                if ((preliminaryDataHandler.GetValue("ConsentExternalStudyPhone")?.ToString() == "Ja" ||
                     preliminaryDataHandler.GetValue("ConsentExternalStudyWhatsApp")?.ToString() == "Ja")
                    && (phone == null || phone.Trim().Length <= 0))
                {
                    DisplayMessageOverlay("Kontaktdaten für ExternalStudy Fehlen!",
                        "Für die eingewilligte Kontaktaufnahme durch ExternalStudy muss die Telefonnummer angegeben werden.");
                    DisplayStage(qnidContact);
                    Enabled = true;
                    return;
                }

                if ((preliminaryDataHandler.GetValue("ConsentExternalStudyEMail")?.ToString() == "Ja")
                    && (email == null || email.Trim().Length <= 0))
                {
                    DisplayMessageOverlay("Kontaktdaten für ExternalStudy Fehlen!",
                        "Für die eingewilligte Kontaktaufnahme durch ExternalStudy muss die E-Mail adresse angegeben werden.");
                    DisplayStage(qnidContact);
                    Enabled = true;
                    return;
                }
            }

            if (!DisplayConfirmationOverlay("Datensatz endgültig speichern?",
                "Haben Sie alle Eingaben geprüft? Nach dem Speichern sind keine Änderungen mehr möglich!", true))
            {
                DisplayMessageOverlay("Speicherung abgebrochen!", "Sie haben die Speicherung abgebrochen.");
                Enabled = true;
                return;
            }

            // Extended to support multiple specimen and IDs per save process.
            // We need to early-dup-check all before then saving all in "transaction style"
            List<DatasetSavingSubfile> specimenIDsToStore = new List<DatasetSavingSubfile>();
            List<DatasetSavingSubfile> specimenIDsToDisplay = new List<DatasetSavingSubfile>(); // in overlay dialog
            
            _helperAddIDToSave(ref specimenIDsToStore, "Legacy", "SpecimenIDTaken");
            specimenIDsToDisplay.AddRange(specimenIDsToStore);
            // Here, more specimen can be added depending on usage

            if (specimenIDsToStore.Count <= 0)
            {
                DisplayMessageOverlay("Keine Probe genommen!", "Es wurde keine Probe zugeordnet. Es können nur Daten gespeichert werden, wenn mindestens eine Probe vorliegt.");
                Enabled = true;
                return;
            }

            List<string> dupCheckBuffer = new List<string>();
            foreach (var spec in specimenIDsToStore)
            {
                if (dupCheckBuffer.Contains(spec.s32ID))
                {
                    log.Warning($"Detected colliding IDs within one session!", "PreliminaryDataSaver");
                    mcaBarcodeReader.Beep(10);
                    mcaBarcodeReader.Beep(10);
                    mcaBarcodeReader.Beep(10);
                    DisplayMessageOverlay("Speicherung fehlgeschlagen!",
                        $"STOPP!!\nEs wurde mehrfach die selbe Proben-ID angegeben.\n\nFalls kein Tippfehler, Probennahme im gesamten Container sofort stopen und umgehend IT kontaktieren!", false, true);
                    Enabled = true;
                    return;
                }
                dupCheckBuffer.Add(spec.s32ID);
            }

            foreach (var subfile in specimenIDsToStore)
            {
                try
                {
                    string s32ID = subfile.s32ID;
                    log.Information($"Early-Check for Duplication of Specimen ID {s32ID} ({subfile.specimenType})...");

                    string dsetID = "";
                    bool isExisting = false;

                    {
                        uint b32ID = B32ID.Decode(s32ID);
                        dsetID = $"MCA_Preliminary_{b32ID:D08}_{s32ID}"; // Must be reproducable for collision check! Thus there is no date in this name!
                        isExisting = preliminaryDataHandler.IsDatasetExisting(dsetID);
                    }

                    if (isExisting)
                    {
                        log.Warning($"Tried to beggin save duplicate ID dataset.", "PreliminaryDataSaver");
                        mcaBarcodeReader.Beep(10);
                        mcaBarcodeReader.Beep(10);
                        mcaBarcodeReader.Beep(10);
                        DisplayMessageOverlay("Speicherung fehlgeschlagen!",
                            $"STOPP!!\nDie Proben-ID {s32ID} wurde bereits verwendet.\n\nFalls kein Tippfehler, Probennahme im gesamten Container sofort stopen und umgehend IT kontaktieren!", false, true);
                        Enabled = true;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    log.Fatal($"Early-Check Duplication error: {ex} - {ex.Message}", "PreliminaryDataSaver");
                    mcaBarcodeReader.Beep(10);
                    mcaBarcodeReader.Beep(10);
                    mcaBarcodeReader.Beep(10);
                    DisplayMessageOverlay("Speicherung fehlgeschlagen!",
                        $"Unerwarteter Fehler beim Early-Duplicate-Check!\nBitte umgehend IT kontaktieren.\n\nSoftwarefehler:\n{ex} - {ex.Message}", false, true);
                    Enabled = true;
                    return;
                }
            }

            {
                // Add a rudimentary recall pin (64 bit) for possible future database lookup authorization
                byte[] rbytes = new byte[8];
                crng.GetBytes(rbytes);
                string pin = Convert.ToBase64String(rbytes);

                preliminaryDataHandler.SetValue("PRL2PIN", pin);
                
            }

            bool doPrint = true;

            if (TeachMode)
            {
                preliminaryDataHandler.SetValue("__TEACH_MODE", true); // for Print and just to be sure!
            }

            // deactivate printing via config
            if (Program.Config.Software.DeactivatePrint)
            {
                doPrint = false;
            }

            if (doPrint)
            {
                try
                {
                    printDocument.Print();
                }
                catch (Exception ex)
                {
                    log.Fatal($"{ex} - {ex.Message}", "AufnahmePrintDocument");
                    DisplayMessageOverlay("Druck fehlgeschlagen!",
                        $"Fehler beim Druck! Wenn Fehler bestehen bleibt Abbrechen und IT kontaktieren!\n\n{ex}: {ex.Message}", false, true);
                    Enabled = true;
                    return;
                }


                if (!DisplayConfirmationOverlay("Druck erfolgreich?",
                    "Der Druck wurde gestartet. Bitte prüfen Sie die vollständigkeit des Druckes:\n\n1. Beidseitig Infotext mit Daten-Auszug\n2. Beidseitig Einwilligungen zur Unterschrift\n3. Kopie von 2 für Proband",
                    true))
                {
                    DisplayMessageOverlay("Speicherung abgebrochen!",
                        "Die Speicherung wurde abgebrochen.\nWiederholen Sie den Vorgang, um den Druck erneut zu versuchen.");
                    Enabled = true;
                    return;
                }
            }

            {
                if (!DisplayConfirmationOverlay("Unterschrift abwarten",
                    "Lassen Sie das Dokument unterschreiben. ALLE Unterschriften MÜSSEN ERFOLGEN! Sie basieren auf den angegebenen Einwilligungen.\nAnsonsten: Abbruch und Einwilligung korrigieren. NICHT PER HAND STREICHEN!\n\nSind alle Unterschriften erfolgt?",
                    true))
                {
                    DisplayMessageOverlay("Speicherung abgebrochen!",
                        "Die Speicherung wurde abgebrochen.\nÄndern Sie bei Bedarf die Einwilligung und wiederholen Sie den Vorgang.");
                    Enabled = true;
                    return;
                }

                if (!DisplayLabelDisplayOverlay("Proben zuordnen",
                    $"Die Probennahme kann nun erfolgen falls noch nicht geschehen. Bitte die Probe(n) mit den korrekten zugeordneten Etikett(en) markieren.",
                    specimenIDsToDisplay.ToArray()))
                {
                    DisplayMessageOverlay("Speicherung abgebrochen!",
                        "Die Speicherung wurde abgebrochen.\nÄndern Sie bei Bedarf die Proben-ID und wiederholen Sie den Vorgang.");
                    Enabled = true;
                    return;
                }
            }

            if (TeachMode)
            {
                log.Warning("**** TEACH MODE: DATA FAKE SAVING, NO ACTUAL SAVE. *****");
                DisplayMessageOverlay("*** SCHULUNGSMODUS! ***", $"Es wurden keine Daten gespeichert! Im Normalbetrieb käme jetzt folgender Hinweis:\n\nDatensatz wurde gespeichert.\nFelder werden für nächste Eingabe geleert.", true);
            }
            else
            {
                List<string> sayOkForThese = new List<string>();
                foreach (var subfile in specimenIDsToStore)
                {
                    try
                    {
                        string s32ID = subfile.s32ID;
                        log.Information($"Trying to write dataset for ID {s32ID} ({subfile.specimenType})...");
                        preliminaryDataHandler.SetValue("SpecimenIDTaken", s32ID);
                        preliminaryDataHandler.SetValue("SpecimenTypeTaken", subfile.specimenType);

                        
                        {
                            preliminaryDataHandler.SetValue("PRELIMINARYSoftware", System.Reflection.Assembly.GetExecutingAssembly().FullName);
                            preliminaryDataHandler.SetValue("CAPTUREDATE", $"{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}");

                            

                            preliminaryDataHandler.SetValue("MACHINE", $"{Environment.MachineName}");
                            string domain = "-";
                            try { domain = Environment.UserDomainName; } catch { }

                            preliminaryDataHandler.SetValue("USER", $"{domain}/{Environment.UserName}");
                        }

                        foreach (string key in Program.Config.GetFields(SambaFileConfig.Section.AddOrOverwriteOnSave))
                        {
                            preliminaryDataHandler.SetValue(key, Program.Config.GetField(SambaFileConfig.Section.AddOrOverwriteOnSave, key));
                        }

                        uint b32ID = B32ID.Decode(s32ID);
                        string dsetID = $"MCA_Preliminary_{b32ID:D08}_{s32ID}"; // Make reproducable for collision check!! NO DATE!
                        preliminaryDataHandler.WriteDataset(dsetID);

                        log.Information($"Written Dataset {dsetID}...");
                        sayOkForThese.Add(dsetID);
                    }
                    catch (PreliminaryDataHandler.DuplicateDatasetException)
                    {
                        log.Fatal($"Tried to save duplicate ID dataset!!", "PreliminaryDataSaver");
                        mcaBarcodeReader.Beep(10);
                        mcaBarcodeReader.Beep(10);
                        mcaBarcodeReader.Beep(10);
                        DisplayMessageOverlay("Speicherung fehlgeschlagen!", $"STOPP!!\nDiese Proben-ID wurde bereits verwendet.\n\nFalls kein Tippfehler, Probennahme im gesamten Container sofort stopen und umgehend IT kontaktieren!", false, true);
                        Enabled = true;
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.Fatal($"Save error: {ex} - {ex.Message}", "PreliminaryDataSaver");
                        mcaBarcodeReader.Beep(10);
                        mcaBarcodeReader.Beep(10);
                        mcaBarcodeReader.Beep(10);
                        DisplayMessageOverlay("Speicherung fehlgeschlagen!", $"Die Daten konnten nicht gespeichert werden!\nBitte umgehend IT kontaktieren.\n\nSoftwarefehler:\n{ex} - {ex.Message}", false, true);
                        Enabled = true;
                        return;
                    }
                }

                DisplayMessageOverlay("Speicherung erfolgreich.", $"Datensatz wurde gespeichert.\nFelder werden für nächste Eingabe geleert.\n\n{string.Join("\n", sayOkForThese)}", true, false, true);
            }
            Enabled = true;
            ExecuteDatasetClear();
            
        }

        List<Questionnaire> questionnaires;
        int currentStage;
        int stageCount;

        void InitStages()
        {
            questionnaires = Preliminary.QuestionnaireProvider.GetQuestionnaires(Program.Config.Software.DisableConsentPage);

            printDocument.Questionnaires = questionnaires;
            currentStage = 0;
            stageCount = questionnaires.Count; // see DisplayStage(...)

            AllLoadedQIDs = new List<string>();

            foreach (DTO.Questionnaire qn in questionnaires)
            {
                foreach (DTO.Question q in qn.Questions)
                {
                    if (q.QuestionType != QuestionType.JustText && q.QID != null)
                    {
                        AllLoadedQIDs.Add(q.QID);
                        if (qn.ShowInPrint)
                            allowedQRImportQIDs.Add(q.QID);
                    }
                }
            }

            if (CanQNCopyOverPersonalAddress())
                commandKeyPanel.SetFunction(1, "Adress- Kopie Wohnsitz", x => CopyOverPersonalAddress());

            DisplayStage(0);
        }

        List<string> AllLoadedQIDs;

        #region WIN32_TEXTBOX_PLACEHOLDER_WORKAROUND
        private static class USER32DLL
        {
            // Source: https://www.fluxbytes.com/csharp/set-placeholder-text-for-textbox-cue-text/
            private const int EM_SETCUEBANNER = 0x1501;
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam,
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);
            public static void SetTextBoxPlaceholder(TextBox ctrl, string placeholder, bool showEvenIfFocus = true)
            {
                SendMessage(ctrl.Handle, EM_SETCUEBANNER, showEvenIfFocus ? 1 : 0, placeholder);
            }
        }
        #endregion

        private bool stageIsChanging = false;
        void DisplayStage(int id = 0)
        {

            // Before changing stage, ensure current field changes are potentially written
            this.Focus();

            stageIsChanging = true;
            Enabled = false;
            stageTableLayoutPanel.Visible = false;
            //SuspendLayout();
            stageTableLayoutPanel.SuspendLayout();
            fieldCount = 0;

            lastCheckedInvalidButKeptValue = null;

            stageTableLayoutPanel.Controls.Clear();

            string title = "";
            if (id >= questionnaires.Count)
            {
                Die("DisplayStage called with unknown ID past Questionnaires.");
                Enabled = true;
                return;
            }
            else
            {
                if (id < 0)
                {
                    Die("DisplayStage called with subzero ID.");
                    Enabled = true;
                    return;
                }

                Questionnaire questionnaire = questionnaires[id];
                title = questionnaire.Title;

                stageTableLayoutPanel.BackColor = Color.White;

                stageTableLayoutPanel.RowStyles.Clear();
                int row = 0;
                foreach (Question q in questionnaire.Questions)
                {
                    stageTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                    Label labelNR = new Label();
                    labelNR.Text = $"({id + 1}.{row + 1})";
                    labelNR.ForeColor = Color.LightGray; // So nobody thinks these are of any meaning
                    stageTableLayoutPanel.Controls.Add(labelNR, 0, row);
                    labelNR.AutoSize = true;
                    Label labelText = new Label();
                    labelText.Text = q.QuestionText;
                    stageTableLayoutPanel.Controls.Add(labelText, 1, row);
                    labelText.AutoSize = true;
                    TextBox ctrl = new TextBox();
                    stageTableLayoutPanel.Controls.Add(ctrl, 2, row);
                    ctrl.Dock = DockStyle.Fill;
                    ctrl.GotFocus += StageFieldControl_GotFocus;
                    ctrl.KeyDown += StageFieldControl_KeyDown;
                    ctrl.LostFocus += StageFieldControl_LostFocus;
                    ctrl.Tag = q;
                    Label labelReq = new Label();
                    labelReq.AutoSize = true;
                    stageTableLayoutPanel.Controls.Add(labelReq, 3, row);

                    if (q.QuestionType == QuestionType.JustText)
                    {
                        ctrl.Visible = false;
                        stageTableLayoutPanel.SetColumnSpan(labelText, 2);
                    }
                    else
                    {
                        string val = preliminaryDataHandler.GetValue(q.QID)?.ToString() ?? q.Default;
                        ctrl.Text = val;
                        switch(q.QuestionType)
                        {
                            case QuestionType.B32IDOfSpecimen:
                                USER32DLL.SetTextBoxPlaceholder(ctrl, "(Scannen oder Eingeben)");
                                break;
                            case QuestionType.Date:
                                USER32DLL.SetTextBoxPlaceholder(ctrl, "(tt.mm.jjjj)");
                                break;
                            case QuestionType.Time:
                                USER32DLL.SetTextBoxPlaceholder(ctrl, "(hh:mm)");
                                break;
                            case QuestionType.EmailAddress:
                                USER32DLL.SetTextBoxPlaceholder(ctrl, "(xxx@yyy.zz, notfalls: u)");
                                break;
                            case QuestionType.Gender:
                                USER32DLL.SetTextBoxPlaceholder(ctrl, "(Männlich/Weiblich/Divers)");
                                break;
                            case QuestionType.Number:
                                USER32DLL.SetTextBoxPlaceholder(ctrl, "(Zahl)");
                                break;
                            case QuestionType.PLZ:
                                USER32DLL.SetTextBoxPlaceholder(ctrl, "(Postleitzahl)");
                                break;
                            case QuestionType.PhoneNumber:
                                USER32DLL.SetTextBoxPlaceholder(ctrl, "(z.B. 015771234568)");
                                break;
                            case QuestionType.YesNo:
                                USER32DLL.SetTextBoxPlaceholder(ctrl, "(Ja/Nein)");
                                break;
                            case QuestionType.YesNoUnknown:
                                USER32DLL.SetTextBoxPlaceholder(ctrl, "(Ja/Nein/Unbekannt)");
                                break;
                            case QuestionType.YesNoAlone:
                                USER32DLL.SetTextBoxPlaceholder(ctrl, "(Ja/Nein/Allein lebend)");
                                break;
                            case QuestionType.NotSometimesOften:
                                USER32DLL.SetTextBoxPlaceholder(ctrl, "(Nie/Manchmal/Oft)");
                                break;
                        }
                        
                    }
                
                    if (Program.Config.Software.QRCodeObligation)
                    {
                        if (allowedQRImportQIDs.Contains(q.QID))
                        {
                            ctrl.ReadOnly = true;
                        }
                    }
                    if (tempdeactivateprint)
                    {
                        if (allowedQRImportQIDs.Contains(q.QID))
                        {
                            ctrl.ReadOnly = true;
                        }
                    }


                    if (ctrl != null && LockOutName)
                    {
                        if (q.QID == "NameGiven" || q.QID == "NameFamily" || q.QID == "BirthDate")
                        {
                            ctrl.ReadOnly = true;
                        }
                    }
                    if (ctrl != null && (Program.Config.GetField(SambaFileConfig.Section.PrefillLocked, q.QID) != null || q.AlwaysReadOnly))
                    {
                        ctrl.ReadOnly = true;
                    }

                    row++;
                }
                fieldCount = row;
            }

            stageIsChanging = false;

            for (int row = 0; row < fieldCount; row++)
                FieldValueChanged(row);

            UpdateFormGridDisplay();

            stageTableLayoutPanel.ResumeLayout();
            stageTableLayoutPanel.Visible = true;

            currentStage = id;
            PushStatus($"Formular {currentStage + 1} von {stageCount}: {title}");
            Enabled = true;
            FocusField(0);
        }

        bool preventMultiUpdate = false;
        private void StageFieldControl_LostFocus(object sender, EventArgs e)
        {
            if (stageIsChanging) return;
            FieldValueChanged(stageTableLayoutPanel.GetRow((Control)sender));
            if (!preventMultiUpdate)
            {
                preventMultiUpdate = true;
                UpdateFormGridDisplay();
                preventMultiUpdate = false;
            }
        }

        private void StageFieldControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (stageIsChanging) return;
            if (e.KeyData == Keys.Enter || e.KeyData == Keys.Return)
            {
                FieldValueChanged(stageTableLayoutPanel.GetRow((Control)sender));
                ChangeField();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void StageFieldControl_GotFocus(object sender, EventArgs e)
        {
            if (stageIsChanging) return;
            recentFieldFocusIndex = stageTableLayoutPanel.GetRow((Control)sender);

            if (!preventMultiUpdate)
            {
                preventMultiUpdate = true;
                UpdateFormGridDisplay();
                preventMultiUpdate = false;
            }
        }

        bool feedbackDuplicationPrevent = false;
        void FieldInvalidFeedbach(string message)
        {
            if (feedbackDuplicationPrevent) return;
            feedbackDuplicationPrevent = true;
            mcaBarcodeReader.Beep();
            //listViewProtocol.Items.Add(message);
            DisplayMessageOverlay("Eingabe ungültig!", message); // Fixme: prevents kb leave somehow!
            feedbackDuplicationPrevent = false;
        }

        System.Globalization.NumberStyles numberStyle = System.Globalization.NumberStyles.Integer | System.Globalization.NumberStyles.AllowDecimalPoint;
        IFormatProvider formatProvider = System.Globalization.CultureInfo.GetCultureInfo("de_de");

        bool preventFieldValueWrites = false;

        private string lastCheckedInvalidButKeptValue = null;

        void FieldValueChanged(int row)
        {
            if (stageIsChanging || preventFieldValueWrites) return;
            Control ctrl = (Control)stageTableLayoutPanel.GetControlFromPosition(2, row);
            // Do not handle non-entry functions
            if (ctrl.GetType() == typeof(Label))
                return;
            TextBox tb = (TextBox)ctrl;
            Label labelReq = (Label)stageTableLayoutPanel.GetControlFromPosition(3, row);
            Question q = (Question)tb.Tag;

            string raw = tb.Text.Trim();

            bool isOk = true;
            if (q.Required)
            {
                labelReq.Text = "Pflichtfeld";
                isOk &= tb.Text.Length > 0;
            } else
            {
                labelReq.Text = "";
            }

            bool clearInvalid = false;
            bool keepButInvalid = false;

            if (raw.Length > 0)
                switch (q.QuestionType)
                {
                    case QuestionType.Plaintext:
                        if (raw.Length < q.Minimum || raw.Length > q.Maximum)
                        {
                            clearInvalid = true;
                            FieldInvalidFeedbach($"Geben Sie zwischen {q.Minimum} und {q.Maximum} Zeichen ein!");
                        }
                        break;
                    case QuestionType.B32IDOfSpecimen:
                        uint sid;
                        try
                        {
                            sid = B32ID.Decode(raw.Replace('0', 'O'));
                            raw = B32ID.Encode(sid);
                        } catch (B32ID.DecodingException dex)
                        {
                            clearInvalid = true;
                            FieldInvalidFeedbach($"Die eingegebene Proben-ID ist ungültig.\nBitte noch einmal eingeben!\n\n({dex.Message})");
                        }
                        break;
                    case QuestionType.YesNo:
                        raw = raw.ToLower();
                        if (Regex.IsMatch(raw, "^(ja|j|yes|y)$", RegexOptions.IgnoreCase))
                            raw = "Ja";
                        else if (Regex.IsMatch(raw, "^(nein|n|no)$", RegexOptions.IgnoreCase))
                            raw = "Nein";
                        else
                        {
                            clearInvalid = true;
                            FieldInvalidFeedbach("Dies ist ein Ja/Nein Feld. Sie können nur J,Ja,N,Nein eingeben.");
                        }
                        break;
                    case QuestionType.YesNoUnknown:
                        raw = raw.ToLower();
                        if (Regex.IsMatch(raw, "^(ja|j|yes|y)$", RegexOptions.IgnoreCase))
                            raw = "Ja";
                        else if (Regex.IsMatch(raw, "^(nein|n|no)$", RegexOptions.IgnoreCase))
                            raw = "Nein";
                        else if (Regex.IsMatch(raw, "^(unbekannt|u|un|unb)$", RegexOptions.IgnoreCase))
                            raw = "Unbekannt";
                        else
                        {
                            clearInvalid = true;
                            FieldInvalidFeedbach("Dies ist ein Ja/Nein/Unbekannt Feld. Sie können nur J,Ja,N,Nein,U,Unbekannt eingeben.");
                        }
                        break;
                    case QuestionType.YesNoAlone:
                        raw = raw.ToLower();
                        if (Regex.IsMatch(raw, "^(ja|j|yes|y)$", RegexOptions.IgnoreCase))
                            raw = "Ja";
                        else if (Regex.IsMatch(raw, "^(nein|n|no)$", RegexOptions.IgnoreCase))
                            raw = "Nein";
                        else if (Regex.IsMatch(raw, "^(allein|a|al|allein lebend)$", RegexOptions.IgnoreCase))
                            raw = "Allein lebend";
                        else
                        {
                            clearInvalid = true;
                            FieldInvalidFeedbach("Dies ist ein Ja/Nein/Allein lebend Feld. Sie können nur J,Ja,N,Nein,A,Allein eingeben.");
                        }
                        break;
                    case QuestionType.Gender:
                        raw = raw.ToLower();
                        if (Regex.IsMatch(raw, "^(m|male|mann|männlich)$", RegexOptions.IgnoreCase))
                            raw = "Männlich";
                        else if (Regex.IsMatch(raw, "^(w|weib|weiblich|f|female|frau)$", RegexOptions.IgnoreCase))
                            raw = "Weiblich";
                        else if (Regex.IsMatch(raw, "^(d|div|divers|diverse)$", RegexOptions.IgnoreCase))
                            raw = "Divers";
                        else
                        {
                            clearInvalid = true;
                            FieldInvalidFeedbach("Dies ist ein Männlich/Weiblich/Divers Feld. Sie können nur M,Männlich,W,Weiblich,D,Divers eingeben.");
                        }
                        break;
                    case QuestionType.EmailAddress:
                        // Ugly fix for unbekannt
                        if (Regex.IsMatch(raw, @"^(u|aa[a]+|un|unbekannt|unbekannt@unbekannt.de)$"))
                        {
                            raw = "unbekannt@unbekannt.unbekannt";
                        }
                        // TODO: Implement correct regex
                        if (!Regex.IsMatch(raw, @"^[-+.'\w]+@[-+.'\w]+\.[-+.'\w]{2,}$"))
                        {
                            keepButInvalid = true;
                            if (lastCheckedInvalidButKeptValue != raw)
                                FieldInvalidFeedbach("Die Eingegebene E-Mail Adresse scheint ungültig zu sein. Bitte überprüfen!");
                            lastCheckedInvalidButKeptValue = raw;
                        }
                        break;
                    case QuestionType.PhoneNumber:
                        // TODO: Implement correct regex
                        raw = raw.Replace("-", "").Replace(" ", "");
                        if (!Regex.IsMatch(raw, "^(00|\\+)?[0-9]{4,20}$"))
                        {
                            keepButInvalid = true;
                            if (lastCheckedInvalidButKeptValue != raw)
                                FieldInvalidFeedbach("Die Eingegebene Telefonnummer scheint ungültig zu sein. Bitte überprüfen!");
                            lastCheckedInvalidButKeptValue = raw;
                        }
                        break;
                    case QuestionType.PLZ:
                        if (!Regex.IsMatch(raw, "^[0-9]{5}$"))
                        {
                            keepButInvalid = true;
                            if (lastCheckedInvalidButKeptValue != raw)
                                FieldInvalidFeedbach("Die Eingegebene Postleitzahl scheint ungültig zu sein. Bitte überprüfen!");
                            lastCheckedInvalidButKeptValue = raw;
                        }
                        break;
                    case QuestionType.Number:
                        raw = raw.ToLower();
                        if (q.DecimalPlaces <= 0)
                        {
                            int parsedInt = 0;
                            if (Regex.IsMatch(raw, "^(\\-)?[0-9]+$") && int.TryParse(raw, out parsedInt))
                            {
                                if ((parsedInt >= q.Minimum && parsedInt <= q.Maximum) || (q.NumberAcceptZeroAsNull && parsedInt == 0))
                                {
                                    raw = $"{parsedInt:D}";
                                }
                                else
                                {
                                    clearInvalid = true;
                                    FieldInvalidFeedbach($"Die Zahl muss zwischen {q.Minimum:D} und {q.Maximum:D} liegen!");
                                }
                            }
                            else
                            {
                                clearInvalid = true;
                                FieldInvalidFeedbach("Geben Sie eine Ganzzahl ein!");
                            }
                        } 
                        else
                        {
                            decimal parsedDec = 0;
                            if (Regex.IsMatch(raw, "^(\\-)?[0-9]*((\\.|,)[0-9]*)?$") && 
                                decimal.TryParse(raw.Replace('.',','), numberStyle, formatProvider, out parsedDec))
                            {
                                parsedDec = Decimal.Round(parsedDec, q.DecimalPlaces, MidpointRounding.AwayFromZero);
                                if ((parsedDec >= q.Minimum && parsedDec <= q.Maximum) || (q.NumberAcceptZeroAsNull && parsedDec == 0))
                                {
                                    raw = parsedDec.ToString($"F{q.DecimalPlaces}", formatProvider);
                                }
                                else
                                {
                                    clearInvalid = true;
                                    FieldInvalidFeedbach($"Die Zahl muss zwischen {q.Minimum:D} und {q.Maximum:D} liegen!");
                                }
                            }
                            else
                            {
                                clearInvalid = true;
                                FieldInvalidFeedbach("Geben Sie eine Ganzzahl ein!");
                            }
                        }
                        break;
                    case QuestionType.Date:
                        raw = raw.ToLower();
                        DateTime parsedDate;
                        if (Regex.IsMatch(raw, "^[0-9]{1,2}.[0-9]{1,2}.[0-9]{4}$", RegexOptions.IgnoreCase) &&
                            DateTime.TryParse(raw, formatProvider, System.Globalization.DateTimeStyles.AssumeUniversal, out parsedDate))
                            raw = parsedDate.ToString("dd.MM.yyyy", formatProvider);
                        else
                        {
                            clearInvalid = true;
                            FieldInvalidFeedbach("Geben Sie ein gültiges Datum im Format Tag.Monat.Jahr mit 4-Stelligem Jahr an!");
                        }
                        break;
                    case QuestionType.Time:
                        raw = raw.ToLower();
                        DateTime parsedTime;
                        if (Regex.IsMatch(raw, "^[0-2]?[0-9](:[0-5]?[0-9]){1,2}?$", RegexOptions.IgnoreCase) &&
                            DateTime.TryParse($"01.01.1970 {raw}", formatProvider, System.Globalization.DateTimeStyles.AssumeLocal, out parsedTime))
                            raw = parsedTime.ToString("HH:mm:ss", formatProvider);
                        else
                        {
                            clearInvalid = true;
                            FieldInvalidFeedbach("Geben Sie eine gültige Zeit im Format Stunde:Minute (optional :Sekunde) ein!");
                        }
                        break;
                    case QuestionType.NotSometimesOften:
                        raw = raw.ToLower();
                        if (Regex.IsMatch(raw, "^(nie|n|ni)$", RegexOptions.IgnoreCase))
                            raw = "Nie";
                        else if (Regex.IsMatch(raw, "^(manchmal|m|man)$", RegexOptions.IgnoreCase))
                            raw = "Manchmal";
                        else if (Regex.IsMatch(raw, "^(oft|o|of)$", RegexOptions.IgnoreCase))
                            raw = "Oft";
                        else
                        {
                            clearInvalid = true;
                            FieldInvalidFeedbach("Dies ist ein Nie/Manchmal/Oft Feld. Sie können nur N,Nie,M,Manchmal,O,Oft eingeben.");
                        }
                        break;
                }
         
            if (clearInvalid || keepButInvalid)
            {
                if (!keepButInvalid)
                    raw = "";
                isOk = false;
                labelReq.Text = "!!" + labelReq.Text;
            }

            tb.Text = raw;

            tb.ForeColor = (tb.Text == q.Default) ? Color.Gray : Color.Empty;

            labelReq.ForeColor = isOk ? Color.DarkGreen : Color.DarkRed;
            // Note: Always write so we can track deleted entries!
            preliminaryDataHandler.SetValue(q.QID, tb.Text.Trim());

            UpdatePatientMonitorData();
        }

        private int fieldCount = 0;
        void UpdateFormGridDisplay()
        {
            if (stageIsChanging) return;
            stageTableLayoutPanel.SuspendLayout();
            for (int row = 0; row < fieldCount; row++)
            {
                Label labelText = (Label)stageTableLayoutPanel.GetControlFromPosition(1, row);
                Control ctrl = (Control)stageTableLayoutPanel.GetControlFromPosition(2, row);
                labelText.BackColor = ctrl.Focused ? Color.LightSkyBlue : Color.Empty;
            }
            stageTableLayoutPanel.ResumeLayout();
        }

        private int recentFieldFocusIndex = 0;
        void FocusField(int index, bool prevIfNotFocusable = false)
        {
            Focus();
            if (index >= 0 && index < stageTableLayoutPanel.RowStyles.Count)
            {
                Control ctrl = stageTableLayoutPanel.GetControlFromPosition(2, index);
                if (ctrl?.GetType() == typeof(Label))
                    FocusField(index + (prevIfNotFocusable ? -1 : 1), prevIfNotFocusable);
                else
                    ctrl?.Focus();
            }
        }

        void Die(string message)
        {
            log.Fatal(message);
            DisplayMessageOverlay("Programmfehler", $"Bitte wenden Sie sich an die Technik.\nProgramm muss neu gestartet werden.\nReason: {message}", false, true);
            Application.Exit();
        }

        void ChangeStage(bool previous = false)
        {
            Focus();
            int wantedStage = currentStage + (previous ? -1 : 1);
            if (wantedStage < 0 || wantedStage >= stageCount)
                mcaBarcodeReader.Beep();
            else
            {
                DisplayStage(wantedStage);
            }
        }

        void ChangeField(bool previous = false)
        {
            FocusField(recentFieldFocusIndex + (previous ? -1 : 1), previous);
        }

        void CancelPatient()
        {
            Focus();
            if (preliminaryDataHandler.HasChangedSincePrefill)
            {
                if (!DisplayConfirmationOverlay("Ungespeicherte Eingabe löschen?",
                    "Achtung! Sie haben seit dem letzten Speichern Daten eingegeben.\nWollen Sie diese wirklich verwerfen und einen neuen Patienten anlegen?"))
                    return;
            }
            ExecuteDatasetClear();
        }

        void _innerLoadPrefillValues(bool overrideSuggested)
        {
            foreach (string key in Program.Config.GetFields(SambaFileConfig.Section.PrefillSuggested))
            {
                if (overrideSuggested || PreliminaryDataHandler.IsEssentiallyNull(preliminaryDataHandler.GetValue(key)))
                    preliminaryDataHandler.SetValue(key, Program.Config.GetField(SambaFileConfig.Section.PrefillSuggested, key));
            }
            foreach (string key in Program.Config.GetFields(SambaFileConfig.Section.PrefillLocked))
            {
                preliminaryDataHandler.SetValue(key, Program.Config.GetField(SambaFileConfig.Section.PrefillLocked, key));
            }
        }

        void ExecuteDatasetClear()
        {
            log.Information($"Execute Dataset Clear");
            Focus();
            preventFieldValueWrites = true;
            LockOutName = false;
            preliminaryDataHandler.ClearDataset();

            _innerLoadPrefillValues(true);

            // reset temporary print setting
            if (Program.Config.Software.AutomaticPrintQrDetection)
            {
                Program.Config.Software.DeactivatePrint = false;
                tempdeactivateprint = false;
                SetPrintingCommandKeyPanel();
            }

            DisplayStage(0);
            preliminaryDataHandler.HasChangedSincePrefill = false;
            preventFieldValueWrites = false;
            DisplayStage(0);
        }


        void PushStatus(string status)
        {
            labelMainStatus.Text = status;
            //listViewProtocol.Items.Add(status);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (commandKeyPanel.ProcessCmdKey(keyData))
                return true;

            // Oem7 = Ä-Taste
            if (keyData == (Keys.Oem7 | Keys.Control | Keys.Alt))
            {
                ExecuteServiceMenu();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ExecuteServiceMenu()
        {
            log.Warning($"Execute Service Menu","ServiceMenu");
            using (ServiceMenu sm = new ServiceMenu())
            {
                sm.Questionnaires = questionnaires;
                sm.ShowDialog(this);
                switch (sm.ResultAction)
                {
                    case ServiceMenu.Action.TeachMode:
                        if (TeachMode)
                        {
                            DisplayMessageOverlay("Schulungsmodus aktiv", "Der Schulungsmodus ist bereits aktiv.");
                        }
                        else if (DisplayConfirmationOverlay("Schulungsmodus aktivieren?","Im Schulungsmodus werden keine echten IDs aquiriert und keine Datensätze gespeichert. Verlassen nur über Beenden der Anwendung."))
                        {
                            this.Text = "**** SCHULUNGSMODUS **** - KEINE SPEICHERUNG!";
                            this.BackColor = Color.Red;
                            TeachMode = true;
                            log.Warning("**** TEACH MODE ACTIVATED *****");
                        }
                        break;
                    case ServiceMenu.Action.LoadExampleDataset:
                        log.Information($"Loading Example Dataset", "ServiceMenu");
                        Focus();
                        preventFieldValueWrites = true;
                        LockOutName = false;
                        preliminaryDataHandler.ClearDataset();
                        var p = preliminaryDataHandler;

                        {


                            p.SetValue("NameGiven", "Maximilan");
                            p.SetValue("NameFamily", "Mustermann");
                            p.SetValue("BirthDate", "01.01.1970");

                            p.SetValue("PrimaryLocationStreet", "Musterstraße 1");
                            p.SetValue("PrimaryLocationZIPCode", "12345");
                            p.SetValue("PrimaryLocationLocality", "Musterhausen");
                            p.SetValue("PersonalAddressStreet", "Beispielweg 5");
                            p.SetValue("PersonalAddressZIPCode", "98765");
                            p.SetValue("PersonalAddressLocality", "Beispieldorf");

                            p.SetValue("DoctorName", "Dr. M. Muster");
                            p.SetValue("DoctorContact", "Musterpraxis, Praxisstraße 1, 45678 Praxisstadt");
                            p.SetValue("Krankenkasse", "Musterkasse");


                            p.SetValue("OccupationPartnerInsitution", "Musterinstitut");

                            p.SetValue("QNGender", "Männlich");
                            p.SetValue("QNLivingAlone", "Ja");
                            p.SetValue("QNPrivateCare", "Nein");
                            p.SetValue("QNOccupationMedical", "");
                            p.SetValue("QNOccupationSocial", "Musteruniversität");

                            p.SetValue("QNConfirmedCaseContact", "Nein");

                            p.SetValue("QNPregnant", "Nein");
                            p.SetValue("QNSmoker", "Ja");
                            p.SetValue("QNTakingCortison", "Unbekannt");
                            p.SetValue("QNTakingImmunosuppressives", "Unbekannt");
                            p.SetValue("QNFluShotSinceOctober2019", "Nein");

                            p.SetValue("ContactPhone", "0123456789");
                            p.SetValue("ContactEmail", "muster@example.net");

                            p.SetValue("ConsentMCATest", "Ja");
                            p.SetValue("ConsentPersonalData", "Ja");
                            p.SetValue("ConsentSpecimenStorageUsageAndRecontact", "Nein");

                            p.SetValue("ConsentExternalStudyPhone", "Ja");
                            p.SetValue("ConsentExternalStudyWhatsApp", "Nein");
                            p.SetValue("ConsentExternalStudyEMail", "Nein");

                        }

                        _innerLoadPrefillValues(true);

                        log.Information($"Loading Example Dataset", "ServiceMenu");

                        DisplayStage(0);
                        preliminaryDataHandler.HasChangedSincePrefill = false;
                        preventFieldValueWrites = false;
                        DisplayStage(0);
                        break;
                }
            }
        }

        private Bitmap bitmap = null;

        private void timerPatientPreviewRefresh_Tick(object sender, EventArgs e)
        {
            labelClock.Text = DateTime.Now.ToString("HH:mm:ss\nddd, dd.MM.yy");
            if (patientenBildschirm.Visible)
            {
                try
                {
                    pictureBoxPatientPreview.SuspendLayout();
                    pictureBoxPatientPreview.Image = null;
                    Size size = patientenBildschirm.Size;
                    if (bitmap != null && size.Equals(bitmap.Size))
                    {
                        bitmap.Dispose();
                        bitmap = null;
                    }
                    if (bitmap == null)
                    {
                        bitmap = new Bitmap(size.Width, size.Height);
                    }

                    using (Graphics G = Graphics.FromImage(bitmap))
                    {
                        G.Clear(Color.Navy);

                        // Method A: Render Form to Bitmap --> Potential other windows might overlay actual display undetected
                        // patientenBildschirm.DrawToBitmap(bitmap, patientenBildschirm.DisplayRectangle);

                        // Method B: Perform an actual screenshot --> Gives a more reliable representation of what the patient actually sees
                        G.CopyFromScreen(patientenBildschirm.DesktopLocation, Point.Empty, size);
                    }

                    pictureBoxPatientPreview.Image = bitmap;
                }
                catch
                {
                    pictureBoxPatientPreview.Image = null;
                    throw new Exception();
                }
            }
            else { pictureBoxPatientPreview.Image = null; }
            pictureBoxPatientPreview.ResumeLayout();
            pictureBoxPatientPreview.Refresh();
        } 

        private delegate void DisplayMessageOverlayDelegate(string title, string message, bool green = false, bool fatal = false, bool skipKeyInput = false);
        private void DisplayMessageOverlay(string title, string message, bool green = false, bool fatal = false, bool skipKeyInput = false)
        {
            
            if (InvokeRequired)
                Invoke(new DisplayMessageOverlayDelegate(this.DisplayMessageOverlay), title, message, green, fatal, skipKeyInput);
            else
            {
                mcaBarcodeReader.AcceptData = false;
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
                mcaBarcodeReader.AcceptData = true;
            }
        }

        private delegate bool DisplayConfirmationOverlayDelegate(string title, string message, bool green = false, bool fatal = false);
        private bool DisplayConfirmationOverlay(string title, string message, bool green = false, bool fatal = false)
        {

            if (InvokeRequired)
                return (bool) Invoke(new DisplayConfirmationOverlayDelegate(this.DisplayConfirmationOverlay), title, message, green, fatal);
            else
            {
                mcaBarcodeReader.AcceptData = false;
                var mo = new MessageOverlay();
                mo.Title = title;
                mo.Message = message;
                mo.Function = MessageOverlay.FunctionType.ConfirmOrCancel;
                
                if (green) mo.BlinkColor = Color.Lime;
                if (fatal)
                {
                    mo.lockSpeed = 0.005f;
                    mo.BlinkColor = Color.Red;
                }
                mo.ShowDialog(this);
                bool confirm = mo.Confirmed;
                mo.Dispose();
                mcaBarcodeReader.AcceptData = true;
                return confirm;
            }
        }

        private delegate bool DisplayLabelDisplayOverlayDelegate(string title, string message, DatasetSavingSubfile[] subfiles);
        private bool DisplayLabelDisplayOverlay(string title, string message, DatasetSavingSubfile[] subfiles)
        {

            if (InvokeRequired)
                return (bool)Invoke(new DisplayLabelDisplayOverlayDelegate(this.DisplayLabelDisplayOverlay), title, message, subfiles);
            else
            {
                mcaBarcodeReader.AcceptData = false;
                var mo = new LabelDisplayOverlay();
                mo.Title = title;
                mo.Message = message;
                mo.Function = LabelDisplayOverlay.FunctionType.ConfirmOrCancel;
                mo.BlinkColor = Color.Lime;
                mo.Subfiles = subfiles;
                mo.ShowDialog(this);
                bool confirm = mo.Confirmed;
                mo.Dispose();
                mcaBarcodeReader.AcceptData = true;
                return confirm;
            }
        }

        private readonly char[] splitEqualSign = { '=' };

        private bool LockOutName = false;

        private void HandleDataReload(MCABarcodeID barcodeID)
        {
            // Parse code
            Dictionary<string, string> dataToImport = new Dictionary<string, string>();
            string prl2PINLink = null;
            try
            {
                if (preliminaryDataHandler.HasChangedSincePrefill || LockOutName)
                {
                    mcaBarcodeReader.Beep(3);
                    DisplayMessageOverlay("Datenimport nur in leeres Formular", "Es sind bereits Eingaben vorhanden. Die Eingabe muss zuvor komplett gelöscht werden (F12).");
                    return;
                }
                foreach (string pair in barcodeID.Auxiliary.Split('&'))
                {
                    string[] parts = pair.Split(splitEqualSign, 2, StringSplitOptions.None);
                    if (parts.Length != 2)
                        throw new Exception("Fehler im Datenformat");
                    string key = parts[0].Trim();
                    {
                        string value = System.Net.WebUtility.UrlDecode(parts[1]);
                        if (value.Contains('\n') || value.Contains('\r'))
                            throw new Exception("QR-Code enthält invalide Daten (ERR:CRLF)!");
                        if (allowedQRImportQIDs.Contains(key))
                            dataToImport.Add(key, value);
                        else if (key == "PRL2PIN")
                        {
                            prl2PINLink = value;
                        }
                    }
                }

            } catch (Exception x)
            {
                mcaBarcodeReader.Beep(6);
                DisplayMessageOverlay("Datenimport Fehlgeschlagen!", $"Die Daten aus dem gescannten Code konnten nicht übernommen werden.\n\n{x.Message}");
                return;
            }
            // Need to go there so we defocus previous user input
            Focus();
            preventFieldValueWrites = true;
            LockOutName = true;
            foreach (string key in dataToImport.Keys)
            {
                preliminaryDataHandler.SetValue(key, dataToImport[key]);
            }
            preliminaryDataHandler.SetValue("PRL2ReloadedFrom", barcodeID.ScopedIDValue);
            preliminaryDataHandler.SetValue("PRL2ReloadedPIN", prl2PINLink);
            _innerLoadPrefillValues(false);
            mcaBarcodeReader.Beep(1);
            DisplayMessageOverlay("Datenimport erfolgreich!", "Die Daten aus dem QR-Code wurden NACH MÖGLICHKEIT in die entsprechenden Felder übernommen.\nBitte in jedem Falle prüfen und Patienten gegenlesen lassen.", true);

            // Automatically deactivate printer if configured
            if (Program.Config.Software.AutomaticPrintQrDetection)
            {
                Program.Config.Software.DeactivatePrint = true;
                tempdeactivateprint = true;
                SetPrintingCommandKeyPanel();
            }

            // Go there again to reload new value
            DisplayStage(0);
            preventFieldValueWrites = false;
            DisplayStage(0);
        }
        private void mcaBarcodeReader_QRCodeFilter(object sender, MCABarcodeReader.QRCodeFilterEventArgs args)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<MCABarcodeReader.QRCodeFilterEventArgs>(mcaBarcodeReader_QRCodeFilter), sender, args);
                return;
            }
            try
            {
                args.QRData = QREncryptionWrapper.DecryptOrPassQRData(args.QRData);
            } catch (Exception x)
            {
                log.Warning($"QREncryptionWrapper.DecryptOrPassQRData failed: {x}");
            }
        }

        private delegate void mcaBarcodeReader_BarcodeInvalidDelegate(string str);
        private void mcaBarcodeReader_BarcodeInvalid(string rawData)
        {
            if (InvokeRequired)
            {
                Invoke(new mcaBarcodeReader_BarcodeInvalidDelegate(mcaBarcodeReader_BarcodeInvalid), rawData);
                return;
            }
          
            mcaBarcodeReader.Beep(6);
            log.Warning($"Invalid barcode scanned.\nRaw barcode data: {rawData}", "BarcodeReader");
            DisplayMessageOverlay("Barcode nicht erkannt!", "Der soeben gescannte Barcode entspricht nicht dem bekannten Schema.");
        }

        private delegate void mcaBarcodeReader_BarcodeReceivedDelegate(object obj);
        private void mcaBarcodeReader_BarcodeReceived(object mcaBarcodeId)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new mcaBarcodeReader_BarcodeReceivedDelegate(mcaBarcodeReader_BarcodeReceived), mcaBarcodeId);
                return;
            }
            MCABarcodeID id = (MCABarcodeID)mcaBarcodeId;

            if (id.Scope != MCABarcodeID.IDScope.Specimen) {
                mcaBarcodeReader.Beep(6);
                log.Warning($"Invalid barcode scanned.\nScope Mismatch: {id}", "BarcodeReader");
                DisplayMessageOverlay("Falscher Barcode-Typ gescannt!", "Sie können nur Proben-IDs scannen!");
                return;
            }

            if (id.SubScope == "PRL2")
            {
                // Reload data from (encrypted) QR code
                HandleDataReload(id);
                return;
            }
            
            if (!id.SubScope.StartsWith("KT"))
            {
                mcaBarcodeReader.Beep(6);
                log.Warning($"Invalid barcode scanned.\nSubscope not in Whitelist: {id}", "BarcodeReader");
                DisplayMessageOverlay("Falscher Barcode-Typ gescannt!", "Bitte für Probenzuordnung nur das Label für die Probenröhrchen scannen!");
                return;
            }

            string s32ID = preliminaryDataHandler.GetValue("SpecimenIDTaken")?.ToString();

            if (s32ID != null && s32ID.Length > 0)
            {
                uint b32ID = B32ID.Decode(s32ID);
                if (id.ID != b32ID)
                {
                    mcaBarcodeReader.Beep(10);
                    DisplayMessageOverlay("Falsche Probenzuordnung", "Achtung! Sie haben für diesen Datensatz bereits eine andere Proben-ID eingetragen! Korrigieren Sie wenn nötig!", false, true);
                    log.Warning("Barcode scan not matching already entered ID in UI!");
                    return;
                }
            }

            // Need to go there so we defocus previous user input
            Focus();
            preventFieldValueWrites = true;
            preliminaryDataHandler.SetValue("SpecimenIDTaken", id.B32ID);
            // Go there again to reload new value
            DisplayStage(stageCount - 1);
            preventFieldValueWrites = false;
            DisplayStage(stageCount - 1);
            mcaBarcodeReader.Beep(1);
        }

        private void Aufnahme_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.Information($"UI Closing Request: {e.CloseReason}");
            #if !DEBUG
            if (!DisplayConfirmationOverlay("Applikation beenden?", "Sind Sie sicher, dass sie die Software beenden möchten?"))
            {
                log.Information("Cancelled by User");
                e.Cancel = true;
                return;
            }
            #endif
            log.Information("UI is Closing...");
        }

        void UpdatePatientMonitorData()
        {
            timerPatMonUpdate.Start();
        }

        private void timerPatMonUpdate_Tick(object sender, EventArgs e)
        {
            timerPatMonUpdate.Stop();

            if (!preliminaryDataHandler.HasAnyData)
            {
                patientenBildschirm.DisplayClear();
                return;
            }

            string patshow = "";

            int qni = currentStage;
            if (qni < questionnaires.Count)
            {
                Questionnaire qn = questionnaires[qni];
                if (qn.ShowToPatient)
                {

                    patshow += $"{qn.Title}\n\n";
                    for (int qi = 0; qi < qn.Questions.Count; qi++)
                    {
                        Question q = qn.Questions[qi];
                        if (q.QuestionType == QuestionType.JustText)
                        {
                            patshow += $"{q.QuestionText}\n";
                        }
                        else
                        {
                            string val = preliminaryDataHandler.GetValue(q.QID)?.ToString();
                            if (val == null || val.Length <= 0)
                                val = "(keine Angabe)";
                            patshow += $"- {q.ShortText ?? q.QuestionText}:  {val}\n";
                        }
                    }
                    patshow += "\n";
                }
            }

            patientenBildschirm.DisplaySingleMessage(patshow, false);

        }

        private void Aufnahme_ResizeBegin(object sender, EventArgs e)
        {
            stageTableLayoutPanel.Visible = false;
            stageTableLayoutPanel.SuspendLayout();
        }

        private void Aufnahme_ResizeEnd(object sender, EventArgs e)
        {
            stageTableLayoutPanel.ResumeLayout();
            stageTableLayoutPanel.Visible = true;
        }
    }
}
