using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisDemo.Aufnahmestation
{
    public class AufnahmePrintDocument : PrintDocument
    {

        public AufnahmePrintDocument() : base()
        {
            OriginAtMargins = false;
            DocumentName = "MCA Aufnahmebogen";
        }

        protected int currentPage;

        protected MCABarcodeID barcodeRefID;
        protected string footerLine;
        protected override void OnBeginPrint(PrintEventArgs e)
        {

            SignatureTimestamp = DateTime.Now;
            currentPage = 1;

            barcodeRefID = new MCABarcodeID(MCABarcodeID.IDScope.Specimen, B32ID.Decode(TryGetValue("SpecimenIDTaken")));
            barcodeRefID.SubScope = "PRL1";

            footerLine = $"SW {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version} - Druckdatum {SignatureTimestamp:dd.MM.yyyy HH:mm}";

        }

        private Font printFont = new Font("Calibri", 10.0f, FontStyle.Regular, GraphicsUnit.Point);
        private Font printFontMini = new Font("Calibri", 8.0f, FontStyle.Regular, GraphicsUnit.Point);
        private Font printFontBold = new Font("Calibri", 10.0f, FontStyle.Bold, GraphicsUnit.Point);
        private Brush printBrushBlack = new SolidBrush(Color.Black);
        private Brush printBrushGray = new SolidBrush(Color.LightGray);
        private Pen printPenBlack = new Pen(Color.Black, ptToMm(0.5f));

        protected string emfLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public string SignatureLocality { get; set; } = "";
        public DateTime SignatureTimestamp { get; protected set; }

        protected override void OnEndPrint(PrintEventArgs e)
        {
            base.OnEndPrint(e);
        }

        private static float ptToMm(float point)
        {
            return point * (25.4f / 72.0f);
        }


        /// <summary>
        /// Converts millimeter measurement to hundredths of an inch (as required by some printing functions).
        /// </summary>
        /// <param name="mm">Length in millimeters</param>
        /// <returns>Length in rounded hundredths of an inch</returns>
        private static int mmToH(float mm)
        {
            return (int)(mm * (100.0f / 25.4f));
        }

        /// <summary>
        /// Converts hundredths of an inch (as required by some printing functions) to millimeter measurement.
        /// </summary>
        /// <param name="hundredths">Length in hundredths of an inch</param>
        /// <returns>Length in millimeters</returns>
        private static float hToMm(float hundredths)
        {
            return (hundredths * (25.4f / 100.0f));
        }

        protected override void OnQueryPageSettings(QueryPageSettingsEventArgs e)
        {
            e.PageSettings.Color = false;
            e.PageSettings.Landscape = false;
        }

        // Do not use props (--> Designer will interfere with this)
        public PreliminaryDataHandler DataHandler = null;
        public List<DTO.Questionnaire> Questionnaires = new List<DTO.Questionnaire>();

        protected string TryGetValue(string key)
        {
            if (DataHandler == null)
                return null;
            string v = DataHandler.GetValue(key)?.ToString();
            if (v == null || v.Trim().Length <= 0)
                return null;
            else
                return v;
        }

        protected void DrawSignatureBlock(Graphics g, float y)
        {
            string str = $"{TryGetValue("NameGiven") ?? "VORNAME"} {TryGetValue("NameFamily") ?? "NACHNAME"}\n{SignatureLocality}, den {SignatureTimestamp:dd.MM.yyyy} um {SignatureTimestamp:HH:mm} Uhr";

            var r = new RectangleF(30.0f, y+3.0f, 160.0f, 17.0f);

            var sf = new StringFormat(StringFormat.GenericTypographic);
            sf.Trimming = StringTrimming.None;
            sf.FormatFlags = StringFormatFlags.NoClip; // Ensure issues are clearly visible!

            g.DrawString(str, printFontBold, printBrushBlack, r, sf);

            g.DrawLine(printPenBlack, r.Left + r.Width / 2, r.Bottom - 5, r.Right, r.Bottom - 5);
            string sig = "(Unterschrift)";
            float w = g.MeasureString(sig, printFont).Width;
            float xm = (r.Left + r.Width / 2 + r.Right) / 2;
            g.DrawString(sig, printFontMini, printBrushBlack, xm - w/2, r.Bottom - 5);

        }

        protected void DrawNoSignatureBlock(Graphics g, float y)
        {
            
            var r = new RectangleF(30.0f, y + 3.0f, 160.0f, 17.0f);

            var sf = new StringFormat(StringFormat.GenericTypographic);
            sf.Trimming = StringTrimming.None;
            sf.FormatFlags = StringFormatFlags.NoClip; // Ensure issues are clearly visible!

            g.DrawString("- EINWILLIGUNG NICHT ERTEILT -", printFontBold, printBrushBlack, r, sf);

        }

        protected Image LoadEMF(string name)
        {
            return Image.FromFile(System.IO.Path.Combine(emfLocation, "PrintLayout", $"{ name}.emf"));
        }

        protected void DrawEMF(Graphics g, string name)
        {
            g.DrawImage(LoadEMF(name), 0, 0);
        }

        private Brush qrBrushLight = new SolidBrush(Color.White);
        private Brush qrBrushDark = new SolidBrush(Color.Black);
        protected void DrawQR(Graphics g, string data, float x, float y, float size = 20.0f, QRCoder.QRCodeGenerator.ECCLevel eccLevel = QRCoder.QRCodeGenerator.ECCLevel.H)
        {
            var generator = new QRCoder.QRCodeGenerator();
            var codeData = generator.CreateQrCode(data, eccLevel);
            /* Default:
            var code = new QRCoder.QRCode(codeData);
            var img = code.GetGraphic(64, Color.Black, Color.White, true);
            g.DrawImage(img, new RectangleF(x, y, size, size));
            */
            // Vector Drawing: Sharper Image
            var state = g.Save();
            // Move to QR Location
            g.TranslateTransform(x, y);
            // Calculate module scaling (includes quiet zones)
            int modules = codeData.ModuleMatrix.Count;
            float sizePerModule = size / modules;
            g.ScaleTransform(sizePerModule, sizePerModule);
            // Render solid Background
            g.FillRectangle(qrBrushLight, 0, 0, modules, modules);
            // Render black boxes on top
            for (int m = 0; m < modules; m++)
            {
                for (int n = 0; n < modules; n++)
                {
                    if (codeData.ModuleMatrix[m][n])
                    {
                        g.FillRectangle(qrBrushDark, n, m, 1, 1);
                    }
                }
            }

            g.Restore(state);
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            base.OnPrintPage(e);

            Graphics g = e.Graphics;

            // Note: The PrintDocument PageSettings Margins are a bit weird.
            // Easy way: implement margins ourself:
            // e.PageSettings.PrintableArea is the physical position and size of the g.VisibleClipBounds on the page.
            // Usually: X and Y of VisibleClipBounds are 0. X and Y of PrintableArea are size of unprintable border left/top.

            var container = g.BeginContainer();
            g.PageUnit = GraphicsUnit.Millimeter;
            // Adjust g to page absolute coordinate
            g.TranslateTransform(-hToMm(e.PageSettings.PrintableArea.X), -hToMm(e.PageSettings.PrintableArea.Y));

            int pageCount = 6; // Adjust if more pages!
            switch (currentPage)
            {
                case 1:
                    {
                        DrawEMF(g, "InfotextMCA1");

                        string qrString = "";
                        foreach (DTO.Questionnaire qn in Questionnaires)
                        {
                            if (!qn.ShowInPrint) continue;
                            foreach (DTO.Question q in qn.Questions)
                            {
                                if (q.QuestionType != QuestionType.JustText && q.QID != null)
                                {
                                    string val = DataHandler.GetValue(q.QID)?.ToString()?.Trim();
                                    if (val != null && val.Length > 0)
                                        qrString += $"{q.QID}={System.Net.WebUtility.UrlEncode(val)}&";
                                }
                            }
                        }

                        string pin = DataHandler.GetValue("PRL2PIN")?.ToString()?.Trim();
                        if (pin != null && pin.Length > 0)
                            qrString += $"PRL2PIN={System.Net.WebUtility.UrlEncode(pin)}&";
                        qrString += $"SWv={System.Net.WebUtility.UrlEncode(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())}";
                        

                        MCABarcodeID mbid = new MCABarcodeID(MCABarcodeID.IDScope.Specimen, barcodeRefID.ID, "PRL2", qrString);

                        DrawQR(g, QREncryptionWrapper.EncryptQRDataIfEnabled(mbid.Value), 156, 14, 40, QRCoder.QRCodeGenerator.ECCLevel.M);

                    }
                    break;
                case 2:
                    {
                        DrawEMF(g, "InfotextMCA2");
                        RectangleF rectData = new RectangleF(30, 186, 160, 93);
                        using (StringFormat sf = new StringFormat(StringFormat.GenericTypographic))
                        {
                            sf.Alignment = StringAlignment.Near;
                            sf.Trimming = StringTrimming.EllipsisWord;

                            string data = "";
                            foreach(DTO.Questionnaire qn in Questionnaires)
                            {
                                if (!qn.ShowInPrint) continue;
                                data += $"{qn.Title}\n";
                                foreach(DTO.Question q in qn.Questions)
                                {
                                    if (q.QuestionType == QuestionType.JustText)
                                    {
                                        data += $"    {q.QuestionText}\n";
                                    }
                                    else
                                    {
                                        string val = DataHandler.GetValue(q.QID)?.ToString();
                                        if (val == null || val.Length <= 0)
                                            val = "(keine Angabe)";
                                        data += $"    -  {q.ShortText ?? q.QuestionText}:  {val}\n";
                                    }
                                }
                            }
                            
                            g.DrawString(data, printFont, printBrushBlack, rectData, sf);
                        }
                    }
                    break;
                case 3:
                case 5:
                    {
                        DrawEMF(g, "ConsentMCA");

                        DrawQR(g, barcodeRefID.ScopedIDValue, 170, 15, 26);

                        if (TryGetValue("ConsentMCATest") == "Ja")
                            DrawSignatureBlock(g, 119.9f);
                        else
                            DrawNoSignatureBlock(g, 119.9f);

                        if (TryGetValue("ConsentPersonalData") == "Ja")
                            DrawSignatureBlock(g, 178.8f);
                        else
                            DrawNoSignatureBlock(g, 178.8f);

                        if (TryGetValue("ConsentSpecimenStorageAndUsage") == "Ja")
                            DrawSignatureBlock(g, 259.2f);
                        else
                            DrawNoSignatureBlock(g, 259.2f);

                        string hausarzt = TryGetValue("DoctorName");
                        string ha_addr = TryGetValue("DoctorContact");
                        if (ha_addr != null && ha_addr.Length > 0)
                            hausarzt += ", " + ha_addr;

                        g.DrawString(hausarzt, printFontBold, printBrushBlack, 40, 106); // w138 h6
                    }
                    break;
                case 4:
                case 6:
                    {
                        DrawEMF(g, "ConsentRecontact");
                        DrawQR(g, barcodeRefID.ScopedIDValue, 170, 15, 26);
                        bool needSig = false;

                        // MCA:
                        string options = "";
                        if (TryGetValue("ConsentMCARecontactEMail") == "Ja")
                            options += $"- per E-Mail ({TryGetValue("ContactEmail")})\n";
                        if (TryGetValue("ConsentMCARecontactPhone") == "Ja")
                            options += $"- per Telefon ({TryGetValue("ContactPhone")})\n";
                        if (options.Length > 0)
                            needSig = true;
                        else
                            options = "- kein Kontakt gewünscht -";
                        g.DrawString(options, printFontBold, printBrushBlack, 41, 174);

                        // External Study:
                        options = "";
                        if (TryGetValue("ConsentExternalStudyWhatsApp") == "Ja")
                            options += $"- per WhatApp ({TryGetValue("ContactPhone")})\n";
                        if (TryGetValue("ConsentExternalStudyPhone") == "Ja")
                            options += $"- per Telefon ({TryGetValue("ContactPhone")})\n";
                        if (TryGetValue("ConsentExternalStudyEMail") == "Ja")
                            options += $"- per E-Mail ({TryGetValue("ContactEmail")})\n";
                        if (options.Length > 0)
                            needSig = true;
                        else
                            options = "- kein Kontakt gewünscht -";
                        g.DrawString(options, printFontBold, printBrushBlack, 41, 210);

                        // Shared Sig
                        if (needSig)
                        {
                            DrawSignatureBlock(g, 228.5f);
                        }
                        else
                        {
                            DrawNoSignatureBlock(g, 228.5f);
                        }
                        
                    }
                    break;
                default: // Fallback
                    break;

            }

            DrawWatermarks(g);

            float yFooter = 284.0f;
            g.DrawLine(printPenBlack, 25.0f, yFooter, 195.0f, yFooter);

            using (StringFormat sf = new StringFormat(StringFormat.GenericTypographic))
            {
                sf.Alignment = StringAlignment.Far;
                RectangleF rfFooter = new RectangleF(25.0f, yFooter + 0.5f, 210 - 25 - 15, 5);
                g.DrawString(footerLine, printFont, printBrushBlack, rfFooter, sf);
            }
            
            g.EndContainer(container);

            e.HasMorePages = currentPage < pageCount;
            currentPage++;
            
        }


        protected void DrawWatermarks(Graphics g)
        {
            if (DataHandler.GetValue("__TEACH_MODE") != null && !Program.Config.Software.HideTeachModeWatermarkOnPrint)
            {
                var state = g.Save();
                g.RotateTransform(30);
                var f = new Font("Consolas", 24, FontStyle.Bold, GraphicsUnit.Pixel);
                var b = new SolidBrush(Color.FromArgb(100, Color.Black));
                g.DrawString("*** SCHULUNG ***", f, b, 5, 0);
                g.Restore(state);
            }
        }
    }
}
