using HisDemo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace LabelDruckstation
{
    public partial class MainFrame : Form
    {
        Logger log;
        public MainFrame()
        {
            log = Program.Logger;
            InitializeComponent();
            InitVariantUI();
        }

        private void MainFrame_Load(object sender, EventArgs e)
        {
            var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
            var config = "UNKN";
#if DEBUG
            config = "Debug";
#else
            config = "Release";
#endif
            labelStatus.Text = $"SW: {assemblyName.Name} Version {assemblyName.Version} ({config})";

            toolTip.SetToolTip(statusStrip, $"Logs in: {Program.LogsPath}");

            settingFieldChanged(sender, e);

        }

        private readonly int LabelsPerSet = 63; // CONST!

        private void settingFieldChanged(object sender, EventArgs e)
        {
            int startId = (int)nudStartID.Value;
            int setCount = (int)nudSetCount.Value;

            int labelCount = setCount * LabelsPerSet;
            int endId = startId + labelCount - 1;

            labelIDCount.Text = $"{labelCount:D}";
            labelEndID.Text = $"{endId:D}";
            labelStartIDB.Text = B32ID.Encode(startId);
            labelEndIDB.Text = B32ID.Encode(endId);

            labelCountD10.Text = $"{setCount * 1:D}";
            labelCountS25.Text = $"{setCount * 3:D}";
            labelCountR46.Text = $"{setCount * 3:D}";
        }

        private void buttonPrinterSettings_Click(object sender, EventArgs e)
        {
            printDialog.ShowDialog(this);
        }

        private enum LabelArrayFillStyle
        {
            /// <summary>
            /// Fill top down, then left right
            /// </summary>
            Columns,
            /// <summary>
            /// Fill left right, then top down
            /// </summary>
            Rows
        }
        private class LabelArray
        {
            private readonly int rows;
            private readonly int cols;
            private readonly LabelArrayFillStyle fillStyle;
            private readonly int rowSkip;
            private readonly int colSkip;

            private MCABarcodeID[,] page;
            private List<MCABarcodeID[,]> pages;

            private int rowCursor;
            private int colCursor;

            /// <summary>
            /// Simple layout engine for regular arrays of label blocks
            /// </summary>
            /// <param name="rows">Total Label Rows per Page</param>
            /// <param name="cols">Total Label Columns per Page</param>
            /// <param name="fillStyle">Direction of Label Blocks</param>
            /// <param name="rowSkip">Leave blank rows between blocks in vertical direction</param>
            /// <param name="colSkip">Leave blank columns between blocks in horizontal direction</param>
            public LabelArray(int rows, int cols, LabelArrayFillStyle fillStyle, int rowSkip, int colSkip)
            {
                this.rows = rows;
                this.cols = cols;
                this.fillStyle = fillStyle;
                this.rowSkip = rowSkip;
                this.colSkip = colSkip;
                pages = new List<MCABarcodeID[,]>();
                rowCursor = rows;
                colCursor = cols;
            }

            private void NextPage()
            {
                page = new MCABarcodeID[this.rows, this.cols];
                pages.Add(page);
                rowCursor = 0;
                colCursor = 0;
            }

            public MCABarcodeID[] FlattenAllPagesToRowByRowArray()
            {
                MCABarcodeID[] flat = new MCABarcodeID[pages.Count * rows * cols];
                int i = 0;
                foreach (MCABarcodeID[,] page in pages)
                {
                    for (int row = 0; row < rows; row++)
                        for (int col = 0; col < cols; col++)
                            flat[i++] = page[row, col];
                }
                return flat;
            }

            public int PageCount
            {
                get => pages.Count;
            }

            public int RowCount
            {
                get => rows;
            }

            public int ColumnCount
            {
                get => cols;
            }

            public MCABarcodeID GetLabelAt(int pageIndex, int row, int col)
            {
                return pages[pageIndex][row, col];
            }

            public void ClearAndInitialize()
            {
                pages.Clear();
                pages = new List<MCABarcodeID[,]>();
                rowCursor = rows;
                colCursor = cols;
            }

            public void PlaceBlock(IEnumerable<MCABarcodeID> ids)
            {
                int blockLen = ids.Count();
                if (fillStyle == LabelArrayFillStyle.Rows)
                {
                    // Wrap to next row if we do not fit into column
                    if (colCursor + blockLen > cols)
                    {
                        colCursor = 0;
                        rowCursor += 1 + rowSkip;
                    }
                    // Wrap to next Page if row is out of page
                    if (rowCursor >= rows)
                    {
                        NextPage();
                    }
                    // Final check, else our layout is flawed!
                    if (colCursor + blockLen > cols)
                        throw new Exception("Cannot fit block in full page column!");
                    // Otherwise, we are good to go
                    foreach (MCABarcodeID id in ids)
                    {
                        page[rowCursor, colCursor] = id;
                        colCursor++;
                    }
                    // Done with Block
                    colCursor += colSkip;
                }
                else if (fillStyle == LabelArrayFillStyle.Columns)
                {
                    // Wrap to next column if we do not fit into row
                    if (rowCursor + blockLen > rows)
                    {
                        rowCursor = 0;
                        colCursor += 1 + colSkip;
                    }
                    // Wrap to next Page if column is out of page
                    if (colCursor >= cols)
                    {
                        NextPage();
                    }
                    // Final check, else our layout is flawed!
                    if (rowCursor + blockLen > rows)
                        throw new Exception("Cannot fit block in full page row!");
                    // Otherwise, we are good to go
                    foreach (MCABarcodeID id in ids)
                    {
                        page[rowCursor, colCursor] = id;
                        rowCursor++;
                    }
                    // Done with Block
                    rowCursor += rowSkip;
                }
                else
                {
                    throw new NotImplementedException("Unknown LabelArrayFillStyle!");
                }
            }
        }

        private class LayoutHandler
        {
            private readonly BogenType bogenType;
            private float x0, y0, dx, dy;

            public  static float ptToMm(float point)
            {
                return point * (25.4f / 72.0f);
            }

            /// <summary>
            /// Converts millimeter measurement to hundredths of an inch (as required by some printing functions).
            /// </summary>
            /// <param name="mm">Length in millimeters</param>
            /// <returns>Length in rounded hundredths of an inch</returns>
            public static int mmToH(float mm)
            {
                return (int)(mm * (100.0f / 25.4f));
            }

            /// <summary>
            /// Converts hundredths of an inch (as required by some printing functions) to millimeter measurement.
            /// </summary>
            /// <param name="hundredths">Length in hundredths of an inch</param>
            /// <returns>Length in millimeters</returns>
            public static float hToMm(float hundredths)
            {
                return (hundredths * (25.4f / 100.0f));
            }

            private static Font printFont = new Font("Calibri", 10.0f, FontStyle.Regular, GraphicsUnit.Point);
            private static Font printFontMini = new Font("Calibri", 8.0f, FontStyle.Regular, GraphicsUnit.Point);
            private static Font printFontBold = new Font("Calibri", 10.0f, FontStyle.Bold, GraphicsUnit.Point);
            private static Brush printBrushBlack = new SolidBrush(Color.Black);
            private static Brush printBrushGray = new SolidBrush(Color.LightGray);
            private static Brush printBrushWhite = new SolidBrush(Color.White);
            private static Pen printPenBlack = new Pen(Color.Black, ptToMm(0.5f));
            private static Brush printBrushSpecial21 = new HatchBrush(HatchStyle.BackwardDiagonal, Color.Black, Color.Gray);

            private static Font monoFont8 = new Font("Consolas", 8f, FontStyle.Bold, GraphicsUnit.Point);
            private static Font monoFont18 = new Font("Consolas", 18f, FontStyle.Bold, GraphicsUnit.Point);
            private static Font monoFont14 = new Font("Consolas", 14f, FontStyle.Bold, GraphicsUnit.Point);
            private static Font monoFont6 = new Font("Consolas", 6f, FontStyle.Bold, GraphicsUnit.Point);
            private static Font monoFont5 = new Font("Consolas", 5f, FontStyle.Bold, GraphicsUnit.Point);
            private static Font monoFont4 = new Font("Consolas", 4f, FontStyle.Bold, GraphicsUnit.Point);

            public DateTime PrintDate { get; set; } = DateTime.Now;

            private readonly KitVariant kitVariant;
            private readonly string specType;
            private readonly Brush specBrush;

            private readonly Color cBlue = Color.FromArgb(97, 202, 245);
            private readonly Color cYellow = Color.FromArgb(255,223,49);
            private readonly Color cRed = Color.FromArgb(253,98,12);
            private readonly bool fillSubstituteBackground;

            public LayoutHandler(BogenType bogenType, KitVariant kitVariant, bool fillSubstituteBackground = false)
            {
                this.bogenType = bogenType;
                this.kitVariant = kitVariant;
                this.fillSubstituteBackground = fillSubstituteBackground;

                float x1, y1;
                // xy coords:
                // x0/y0 = top left corner of top left label
                // x1/y1 = top left corner of label 2nd from top 2nd from left
                // delta is calculated thereof
                this.specType = "MCA Specimen";
                switch (kitVariant)
                {
                    // Extensible to change colors etc.
                    default:
                        this.fillSubstituteBackground = false;
                        break;
                }
                switch (bogenType)
                {
                    case BogenType.D10:
                        x0 = 11.1f;
                        y0 = 16.5f;
                        x1 = 23.8f;
                        y1 = 29.2f;
                        break;
                    case BogenType.S25:
                        x0 = 11.5f;
                        y0 = 14.5f;
                        x1 = 38.5f;
                        y1 = 41.5f;
                        break;
                    case BogenType.R46:
                        x0 = 5.5f;
                        y0 = 17.0f;
                        x1 = 56.5f;
                        y1 = 29.6f;
                        break;
                    default:
                        throw new NotImplementedException();
                }
                dx = x1 - x0;
                dy = y1 - y0;
            }

            private static PointF[] polyArrow = new PointF[]
            {
                new PointF(0.0f, 0.0f),
                new PointF(4.2f, 0.0f),
                new PointF(8.4f, 4.0f),
                new PointF(4.2f, 7.9f),
                new PointF(0.0f, 7.9f),
                new PointF(4.2f, 4.0f),
                new PointF(0.0f, 0.0f)
            };

            public void DrawLabel(Graphics g, int row, int col, string bId, string qrId)
            {
                float x = x0 + col * dx;
                float y = y0 + row * dy;

                switch (bogenType)
                {
                    case BogenType.D10:
                        {
                            if (col % 5 < 2)
                            {
                                // Letter Only
                                g.FillRectangle(printBrushGray, x - 1.0f, y + 7.0f, 12f, 3.0f);
                                g.DrawString(bId, monoFont8, printBrushBlack, new RectangleF(
                                    x + 2.2f, y + 1.7f, 6.0f, 10.0f
                                    ));

                            } else
                            {
                                // QR Mainly and Letters small
                                float qrSize = 6.138333333333333f;
                                float qrYO = 1.15f;

                                if (qrId.EndsWith("21"))
                                {
                                    // Special Label ID 21 colored
                                    g.FillRectangle(printBrushBlack, x - 0.5f, y - 0.5f, 11.0f, 11.0f);
                                    g.FillRectangle(printBrushWhite, x-0.5f, y + 6.8f, 11.0f, 1.7f);
                                }
                                DrawQR(g, qrId, x + 5.0f - qrSize / 2, y + qrYO, qrSize, QRCoder.QRCodeGenerator.ECCLevel.L);
                                g.DrawString(bId, monoFont5, printBrushBlack, x + 2.2f, y + qrYO / 2 + qrSize);
                            }   
                        }
                        break;
                    case BogenType.S25:
                        {   
                            g.FillRectangle(printBrushBlack, x - 1.6f, y + 1.6f, 28.2f, 4.4f);
                            g.DrawString(this.specType, monoFont8, printBrushWhite, x + 1.9f, y + 2.1f);

                            var lbit = g.MeasureString(bId, monoFont18);
                            float xbid = 12.5f - lbit.Width / 2;
                            g.DrawString(bId, monoFont18, printBrushBlack, x + xbid, y + 6.2f);

                            g.DrawString($"Print Date:\n{PrintDate:yyyy'/'MM'/'dd}", monoFont5, printBrushBlack, x + 10.5f, y + 14.5f);

                            DrawQR(g, qrId, x + 2.2f, y + 13.2f, 7.2f, QRCoder.QRCodeGenerator.ECCLevel.L);
                            g.DrawString(qrId, monoFont5, printBrushBlack, x + 2.7f, y + 21.0f);
                        }
                        break;
                    case BogenType.R46:
                        {
                            bool drawSpecialL21 = qrId.EndsWith("21");
                            if (drawSpecialL21)
                                g.FillRectangle(printBrushBlack, x+12.0f, y - 0.6f, 47.0f, 12.3f);

                            g.FillRectangle(drawSpecialL21 ? printBrushWhite : printBrushBlack, x + 41.9f, y - 0.6f, 4.5f, 12.3f);
                            var norm = g.Save();
                            g.TranslateTransform(44.0f + x, 1.8f + y);
                            g.RotateTransform(90.0f);
                            g.DrawString("MCA Spec.", monoFont4, drawSpecialL21 ? printBrushBlack : printBrushWhite, 0, 0);
                            g.Restore(norm);

                            // 1.6 1.6
                            norm = g.Save();
                            g.TranslateTransform(x + 1.6f, y + 1.6f);
                            g.FillPolygon(printBrushGray, polyArrow);
                            g.Restore(norm);

                            var lbit = g.MeasureString(bId, monoFont14);
                            float xbid = 34.3f - lbit.Width;
                            g.DrawString(bId, monoFont14, drawSpecialL21 ? printBrushWhite : printBrushBlack, x + xbid, y + 1.8f);

                            DrawQR(g, qrId, x + 34.5f, y + 1.9f, 7.2f, QRCoder.QRCodeGenerator.ECCLevel.L);
                            lbit = g.MeasureString(qrId, monoFont5);
                            xbid = 34.3f - lbit.Width;
                            g.DrawString(qrId, monoFont5, drawSpecialL21 ? printBrushWhite : printBrushBlack, x + xbid, y + 6.5f);
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }

            }

            private static Pen printPenCutLine = new Pen(Color.Gray, 0.15f) {
                DashPattern = new float[]{ 5, 5 }
            };
            private static Pen printPenCutLineThick = new Pen(Color.Gray, 0.35f)
            {
                DashPattern = new float[] { 5, 5, 10, 5 }
            };
            public void DrawCutlines(Graphics g)
            {
                List<float> vLines = new List<float>(); // x-coord-Line
                List<float> hLines = new List<float>(); // y-coord-Line
                List<float> hLinesThick = new List<float>(); // y-coord-Line
                switch (bogenType)
                {
                    case BogenType.D10:
                        {
                            vLines.Add(x0 + 5.0f + dx * 4.5f);
                            vLines.Add(x0 + 5.0f + dx * 9.5f);
                            for (int row = 0; row < 20; row++)
                            {
                                // Some extra thick in the middle to separate 3 subsets associated with other sheets
                                if (row % 7 == 6)
                                    hLinesThick.Add(y0 + 5.0f + (row + 0.5f) * dy);
                                else
                                    hLines.Add(y0 + 5.0f + (row + 0.5f) * dy);
                            }
                        }
                        break;
                    case BogenType.R46:
                        {
                            for (int row = 0; row < 20; row++)
                                hLines.Add(y0 + 5.55f + (row + 0.5f) * dy);
                        }
                        break;
                    case BogenType.S25:
                        {
                            hLines.Add(y0 + 12.5f + 2.5f * dy);
                            hLines.Add(y0 + 12.5f + 5.5f * dy);
                            hLines.Add(y0 + 12.5f + 8.5f * dy);
                            for (int col = 0; col < 6; col++)
                                vLines.Add(x0 + 12.5f + dx * (col + 0.5f));
                        }
                        break;
                }
                
                if (fillSubstituteBackground)
                {
                    g.FillRectangle(specBrush, 2, 2, 206, 293);
                }

                foreach (float x in vLines)
                    g.DrawLine(printPenCutLine, x, 0.0f, x, 297.0f);
                foreach (float y in hLines)
                    g.DrawLine(printPenCutLine, 0.0f, y, 210.0f, y);
                foreach (float y in hLinesThick)
                    g.DrawLine(printPenCutLineThick, 0.0f, y, 210.0f, y);
            }

            private static Brush qrBrushLight = new SolidBrush(Color.White);
            private static Brush qrBrushDark = new SolidBrush(Color.Black);
            public static void DrawQR(Graphics g, string data, float x, float y, float size = 20.0f, QRCoder.QRCodeGenerator.ECCLevel eccLevel = QRCoder.QRCodeGenerator.ECCLevel.H)
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
        }

        private enum BogenType
        {
            D10,
            S25,
            R46,
        }

        private PointF currentOffsetCorrection = new PointF(0, 0);
        private string currentHeader = "", currentFooter = "";
        private bool currentFillSubstituteBackground;

        bool PrintCommon(BogenType bogenType, bool askContinue = true, bool isComfortPrinting = false)
        {
            int startId = (int)nudStartID.Value;
            int setCount = (int)nudSetCount.Value;

            int labelCount = setCount * LabelsPerSet;
            int endId = startId + labelCount - 1;

            if (startId < 0 || setCount < 1 || labelCount < 1 || endId < startId)
                throw new Exception("Internal error! Printing Parameters invalid albeit GUI restrictions!");
            if (endId >= int.MaxValue - 1)
            {
                MessageBox.Show(this, "31-Bit-ID-Raum ausgeschöpft!", "Softwarelimit Erreicht", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (askContinue)
            {
                if (DialogResult.OK != MessageBox.Show(this,
                    "Haben Sie die Allokation der Proben-IDs offiziell durchgeführt?\nBitte nur dann mit dem Druck beginnen (OK).\n\n" +
                    $"Allokations-Bereich {startId:D08} bis {endId:D08} ({labelCount:D} IDs)",
                    "Druck Starten?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
                    return false;

                if (setCount > 100 || labelCount > 2500)
                    if (MessageBox.Show(this, $"Es wird eine exzessive Menge an Labels erzeugt: {setCount} Sets, in Summe {labelCount} Labels. Wirklich fortfahren?",
                        "Sind sie Sicher?", MessageBoxButtons.YesNo) == DialogResult.No)
                        return false;
            }

            currentKitVariant = (KitVariant)cbKitTypeVariant.SelectedItem;

            log.Information($"Calculating Labels for IDs {startId} to {endId}, type {bogenType}");

            LabelArray labels70er = new LabelArray(10, 7, LabelArrayFillStyle.Columns, 0, 0);
            LabelArray labels315er = new LabelArray(21, 15, LabelArrayFillStyle.Rows, 0, 0);
            LabelArray labels84er = new LabelArray(21, 4, LabelArrayFillStyle.Rows, 0, 0);
          

            MCABarcodeID.IDScope scope = MCABarcodeID.IDScope.Specimen;

            MCABarcodeID[] block70er = new MCABarcodeID[3];
            MCABarcodeID[] block315er = new MCABarcodeID[5];
            MCABarcodeID[] block84er = new MCABarcodeID[4];

            string prefixKit;
            string prefixSide;
            string prefixTop;
            bool hasSpecialInvertedLabel21 = true;
            switch (currentKitVariant)
            {
                case KitVariant.AbstrichNormal:
                    prefixKit = "KT";
                    prefixSide = "VS";
                    prefixTop = "VT";
                    break;
                default:
                    throw new NotImplementedException("Unknown kit variant!");
            }

            for (int id = startId; id <= endId; id++)
            {
                int ssKT = 1;
                int ssVS = 1;
                int ssVT = 1;

                // 70er: 3
                if (bogenType == BogenType.S25)
                {
                    for (int sub = 0; sub < block70er.Length; sub++)
                    {
                        block70er[sub] = new MCABarcodeID(scope, (uint)id, $"{prefixKit}{ssKT:D02}");
                        ssKT++;
                    }
                    labels70er.PlaceBlock(block70er);
                }
                // 315er: 5
                if (bogenType == BogenType.D10)
                {
                    for (int sub = 0; sub < block315er.Length; sub++)
                    {
                        block315er[sub] = new MCABarcodeID(scope, (uint)id, $"{prefixTop}{ssVT:D02}");
                        if (sub == 4 && hasSpecialInvertedLabel21)
                        {
                            // Special ID 21 for the other colored label
                            block315er[sub] = new MCABarcodeID(scope, (uint)id, $"{prefixTop}21");
                        }
                        ssVT++;
                    }
                    labels315er.PlaceBlock(block315er);
                }
                // 84er: 4
                if (bogenType == BogenType.R46)
                {
                    for (int sub = 0; sub < block84er.Length; sub++)
                    {
                        block84er[sub] = new MCABarcodeID(scope, (uint)id, $"{prefixSide}{ssVS:D02}");
                        if (sub == 3 && hasSpecialInvertedLabel21)
                        {
                            // Special ID 21 for the other colored label
                            block84er[sub] = new MCABarcodeID(scope, (uint)id, $"{prefixSide}21");
                        }
                        ssVS++;
                    }
                    labels84er.PlaceBlock(block84er);
                }
            }

            currentFillSubstituteBackground = cbPrintSubstituteColorFill.Checked;
            currentOffsetCorrection = new PointF((float)nudXOffset.Value, (float)nudYOffset.Value);
            LayoutHandler handler = new LayoutHandler(bogenType, currentKitVariant, currentFillSubstituteBackground);
            currentLayoutHandler = handler;
            currentPageIndex = 0;
            string variant = $"{currentKitVariant}";
            currentHeader = $"Druckbereich: {labelCount} IDs: {startId:D08} - {endId:D08} ({B32ID.Encode(startId)} - {B32ID.Encode(endId)}) ; Druckdatum {currentLayoutHandler.PrintDate:yyyy'/'MM'/'dd} ; Bogentyp {bogenType} ; Variante {variant}";
            var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
            var config = "UNKN";
#if DEBUG
            config = "Debug";
#else
            config = "Release";
#endif
            currentFooter = $"SW: {assemblyName.Name} Version {assemblyName.Version} ({config}) ; Offset {currentOffsetCorrection.X:0.0}X {currentOffsetCorrection.Y:0.0}Y";

            if (cbAutoTray.Enabled && cbAutoTray.Checked)
                isComfortPrinting = true;

            if (isComfortPrinting)
            {
                selectedComfortPrinter = (ComfortPrinter) comboBoxComfortPrinter.SelectedItem;
                printDocument.PrinterSettings.PrinterName = selectedComfortPrinter.PrinterName;
            }

            printDocument.PrinterSettings.Duplex = System.Drawing.Printing.Duplex.Simplex;
            printDocument.PrinterSettings.Copies = 1;

            printDocument.DocumentName = $"MCA {bogenType} {B32ID.Encode(startId)} - {B32ID.Encode(endId)}";
            currentBogenType = bogenType;
            currentIsComfortPrinting = isComfortPrinting;

            switch (bogenType)
            {
                case BogenType.D10:
                    currentLabelArray = labels315er;
                    break;
                case BogenType.R46:
                    currentLabelArray = labels84er;
                    break;
                case BogenType.S25:
                    currentLabelArray = labels70er;
                    break;
                default:
                    throw new NotImplementedException();
            }
            Enabled = false;
            log.Information($"Starting PrintJob {printDocument.DocumentName}...");
            log.Information($"Header: {currentHeader}");
            log.Information($"Footer: {currentFooter}");
            printDocument.Print();
            log.Information($"PrintJob {printDocument.DocumentName} deployed.");
            Enabled = true;

            return true;
        }

        private LayoutHandler currentLayoutHandler = null;
        private LabelArray currentLabelArray = null;
        private int currentPageIndex = 0;
        private BogenType currentBogenType;
        private bool currentIsComfortPrinting = false;

        private void buttonPrintD10_Click(object sender, EventArgs e)
        {
            PrintCommon(BogenType.D10);
        }

        private void buttonPrintS25_Click(object sender, EventArgs e)
        {
            PrintCommon(BogenType.S25);
        }

        private void buttonPrintR46_Click(object sender, EventArgs e)
        {
            PrintCommon(BogenType.R46);
        }

        private void printDocument_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            if (currentIsComfortPrinting)
            {
                log.Diagnostic($"Comfort Printer is {e.PageSettings.PrinterSettings.PrinterName}");
                switch (currentBogenType)
                {
                    case BogenType.D10:
                        e.PageSettings.PaperSource = selectedComfortPrinter.Source3;
                        break;
                    case BogenType.S25:
                        e.PageSettings.PaperSource = selectedComfortPrinter.Source2;
                        break;
                    case BogenType.R46:
                        e.PageSettings.PaperSource = selectedComfortPrinter.Source1;
                        break;
                }
                log.Diagnostic($"Comfort PaperSource Selection for {currentBogenType}: {e.PageSettings.PaperSource}");
                foreach (System.Drawing.Printing.PaperSize size in e.PageSettings.PrinterSettings.PaperSizes)
                {
                    if (size.Kind == System.Drawing.Printing.PaperKind.A4)
                    {
                        e.PageSettings.PaperSize = size;
                        break;
                    }
                }
                log.Diagnostic($"Comfort PaperSize Selection for {currentBogenType}: {e.PageSettings.PaperSize}");
            }
            e.PageSettings.Color = currentFillSubstituteBackground;
            e.PageSettings.Landscape = false;
        }

        private void printDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private static Font printFont = new Font("Calibri", 8.0f, FontStyle.Regular, GraphicsUnit.Point);
        private static Brush printBrushBlack = new SolidBrush(Color.Black);

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            // Notes on Margins etc.:
            // The PrintDocument PageSettings Margins are a bit weird.
            // Easy way: implement margins ourselfs:
            // e.PageSettings.PrintableArea is the physical position and size of the g.VisibleClipBounds on the page.
            // Usually: X and Y of VisibleClipBounds are 0. X and Y of PrintableArea are size of unprintable border left/top.

            var container = g.BeginContainer();
            g.PageUnit = GraphicsUnit.Millimeter;
            // Adjust g to page absolute coordinate
            g.TranslateTransform(-LayoutHandler.hToMm(e.PageSettings.PrintableArea.X), -LayoutHandler.hToMm(e.PageSettings.PrintableArea.Y));

            // Apply Offset:
            g.TranslateTransform(currentOffsetCorrection.X, currentOffsetCorrection.Y);

            // Draw Cutlines
            currentLayoutHandler.DrawCutlines(g);

            // Header/Footer Info
            g.DrawString($"{currentHeader} ; Bogen {currentPageIndex+1}/{currentLabelArray.PageCount}", printFont, printBrushBlack, 10, 5);
            g.DrawString($"{currentFooter} ; Drucker: {e.PageSettings.PrinterSettings.PrinterName} ; Rechner: {Environment.MachineName}", printFont, printBrushBlack, 10, 297f - 8f);

            // Do actual Labels
            for (int row = 0; row < currentLabelArray.RowCount; row++)
            {
                for (int col = 0; col < currentLabelArray.ColumnCount; col++)
                {
                    MCABarcodeID id = currentLabelArray.GetLabelAt(currentPageIndex, row, col);
                    if (id != null)
                    {
                        currentLayoutHandler.DrawLabel(g, row, col, id.B32ID, id.Value);
                    }
                }
            }

            // Next Page?
            currentPageIndex++;
            e.HasMorePages = currentPageIndex < currentLabelArray.PageCount;

            g.EndContainer(container);
        }

        private void logFileLinkLabel_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", Program.LogsPath);
        }

        private void buttonNextSet_Click(object sender, EventArgs e)
        {
            int startId = (int)nudStartID.Value;
            int setCount = (int)nudSetCount.Value;

            int labelCount = setCount * LabelsPerSet;
            int endId = startId + labelCount - 1;
            nudStartID.Value = endId + 1;
        }

        private ComfortPrinter selectedComfortPrinter = null;

        private void comboBoxComfortPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxComfortPrinter.SelectedItem != null)
            {
                buttonPrintAll.Enabled = true;
                selectedComfortPrinter = (ComfortPrinter)comboBoxComfortPrinter.SelectedItem;
                printDocument.PrinterSettings.PrinterName = selectedComfortPrinter.PrinterName;
                cbAutoTray.Enabled = true;
            }
            else
            {
                buttonPrintAll.Enabled = false;
                selectedComfortPrinter = null;
                cbAutoTray.Enabled = false;
            }
        }

        private class ComfortPrinter
        {
            public System.Drawing.Printing.PaperSource Source1;
            public System.Drawing.Printing.PaperSource Source2;
            public System.Drawing.Printing.PaperSource Source3;
            public string PrinterName;
            public override string ToString()
            {
                return PrinterName;
            }
        }

        private void refreshComfortPrinterList(object sender, EventArgs e)
        {
            buttonRefreshComfortPrinterList.Enabled = false;
            buttonRefreshComfortPrinterList.Update();
            List<ComfortPrinter> printers = new List<ComfortPrinter>();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                var ps = new System.Drawing.Printing.PrinterSettings();
                ps.PrinterName = printer;
                var sources = ps.PaperSources;
                int countLower = 0, countMiddle = 0, countUpper = 0;
                ComfortPrinter cp = new ComfortPrinter();
                cp.PrinterName = printer;
                foreach (System.Drawing.Printing.PaperSource source in sources)
                {
                    switch (source.Kind)
                    {
                        case System.Drawing.Printing.PaperSourceKind.Lower:
                            cp.Source3 = source;
                            countLower++; break;
                        case System.Drawing.Printing.PaperSourceKind.Middle:
                            cp.Source2 = source;
                            countMiddle++; break;
                        case System.Drawing.Printing.PaperSourceKind.Upper:
                            cp.Source1 = source;
                            countUpper++; break;
                    }
                }
                if (countLower == 1 && countMiddle == 1 && countUpper == 1)
                {
                    printers.Add(cp);
                }
                else
                {
                    countLower = 0; countMiddle = 0; countUpper = 0;
                    foreach (System.Drawing.Printing.PaperSource source in sources)
                    {
                        switch (source.SourceName.Substring(source.SourceName.Length - 1))
                        {
                            case "3":
                                cp.Source3 = source;
                                countLower++; break;
                            case "2":
                                cp.Source2 = source;
                                countMiddle++; break;
                            case "1":
                                cp.Source1 = source;
                                countUpper++; break;
                        }
                    }
                    // NOTE: Here one could set up compatible printers
                    // if (countLower == 1 && countMiddle == 1 && countUpper == 1)
                    {
                        printers.Add(cp);
                    }
                }
            }

            comboBoxComfortPrinter.Items.Clear();
            comboBoxComfortPrinter.Items.AddRange(printers.ToArray());
            buttonRefreshComfortPrinterList.Enabled = true;
            buttonRefreshComfortPrinterList.Update();
        }

        private void buttonPrintAll_Click(object sender, EventArgs e)
        {
            if (PrintCommon(BogenType.D10, true, true))
                if (PrintCommon(BogenType.S25, false, true))
                    PrintCommon(BogenType.R46, false, true);
            Enabled = true;
        }

        private enum KitVariant
        {
            AbstrichNormal,
            // Extensible for different speciment types for example
        }

        private void InitVariantUI()
        {
            cbKitTypeVariant.Items.Add(KitVariant.AbstrichNormal);
            cbKitTypeVariant.SelectedIndex = 0;
        }

        private KitVariant currentKitVariant;

        private void cbKitTypeVariant_SelectedIndexChanged(object sender, EventArgs e)
        {
            // do not change index here! do upon print
            switch(cbKitTypeVariant.SelectedItem)
            {
                case KitVariant.AbstrichNormal:
                    labelKitVariantInfo.Text = "Kit-Tüte wie gehabt auf weißen Bögen.";
                    break;
                default:
                    labelKitVariantInfo.Text = "--";
                    break;
            }
        }  

        private void printDocument_EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            currentLabelArray = null;
            currentLayoutHandler = null;
            currentPageIndex = 0;
        }
    }
}
