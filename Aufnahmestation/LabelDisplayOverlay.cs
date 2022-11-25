using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HisDemo.Aufnahmestation
{
    public partial class LabelDisplayOverlay : Form
    {
        public LabelDisplayOverlay()
        {
            InitializeComponent();
        }

        public enum FunctionType
        {
            /// <summary>
            /// Only show info, close with return.
            /// </summary>
            Notify,
            /// <summary>
            /// Request confirmation or cancel
            /// </summary>
            ConfirmOrCancel
        }

        private FunctionType function = FunctionType.Notify;
        public FunctionType Function { 
            get => function; 
            set
            {
                function = value;
                switch (function)
                {
                    case FunctionType.Notify:
                        labelContinue.Text = "Drücken Sie die Enter- oder Return-Taste um fortzufahren.";
                        break;
                    case FunctionType.ConfirmOrCancel:
                        labelContinue.Text = "Abbruch mit Escape, Bestätigen mit Enter-/Return-Taste";
                        break;
                }
            }
        }

        public bool Confirmed { get; protected set; } = false;

        protected override void OnShown(EventArgs e)
        {
            Confirmed = false;
            panelLock?.Dispose();
            panelLock = new Panel();
            panelInner.Controls.Add(panelLock);
            panelLock.Location = labelContinue.Location;
            panelLock.Size = labelContinue.Size;
            panelLock.BackColor = Color.DimGray;
            panelLock.BringToFront();
            timerUnlockStep.Enabled = true;
            lockPos = 0;
            base.OnShown(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (locked)
                return true;

            if (keyData == Keys.Return || keyData == Keys.Enter)
            {
                Confirmed = true;
                this.Close();
                return true;
            }
            else if (keyData == Keys.Escape && Function == FunctionType.ConfirmOrCancel)
            {
                Confirmed = false;
                this.Close();
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }

        public string Title
        {
            get => labelTitle.Text;
            set => labelTitle.Text = value;
        }

        public string Message
        {
            get => labelMessage.Text;
            set => labelMessage.Text = value;
        }

        private Aufnahme.DatasetSavingSubfile[] _subfiles = null;
        public Aufnahme.DatasetSavingSubfile[] Subfiles
        {
            get => _subfiles;
            set { _subfiles = value; ReloadSubfilesPanel(); }
        }

        private void ReloadSubfilesPanel()
        {
            flpImages.Controls.Clear();
            if (_subfiles == null) return;
            foreach(var subfile in _subfiles)
            {
                Label l = new Label();
                l.ImageList = imageListLabels;
                l.ImageAlign = ContentAlignment.MiddleLeft;
                l.ImageKey = "white";
                string type = subfile.specimenType;
                l.BackColor = Color.White;
                l.ForeColor = Color.Black;
                switch (subfile.specimenType)
                {
                    // This is an example how to display other specimen types
                    case "SpecimenTypeX":
                        type = "X";
                        l.ImageKey = "yellow";
                        break;
                    case "Legacy":
                        type = null;
                        break;
                }
                l.AutoSize = true;
                l.Font = fontMono;
                l.Padding = new Padding(4, 4, 12, 4);
                l.TextAlign = ContentAlignment.MiddleLeft;
                l.Margin = new Padding(3, 3, 3, 9);
                l.Text = "      " + (type == null ? subfile.s32ID : $"{type}: {subfile.s32ID}");
                flpImages.Controls.Add(l);
            }
        }

        private Font fontMono = new Font("Consolas", 16, FontStyle.Bold, GraphicsUnit.Pixel);

        public Color BlinkColor { get; set; } = Color.Black;

        private void timerBlink_Tick(object sender, EventArgs e)
        {
            if (BlinkColor == Color.Red)
            {
                panelOuter.BackColor = panelOuter.BackColor == Color.Yellow ? BlinkColor : Color.Yellow;
                panelInner.BackColor = panelOuter.BackColor == Color.Yellow ? Color.Red : Color.Black;
                panelInner.ForeColor = Color.White;
            }
            else
            {
                panelOuter.BackColor = panelOuter.BackColor == Color.Yellow ? BlinkColor : Color.Yellow;
            }
        }

        private bool locked = true;
        private Panel panelLock = null;
        private float lockPos = 0;
        public float lockSpeed = 0.04f;
        private void timerUnlockStep_Tick(object sender, EventArgs e)
        {
            lockPos += lockSpeed;
            if (lockPos >= 1.0f)
            {
                timerUnlockStep.Enabled = false;
                panelLock.Visible = false;
                panelLock.Dispose();
                panelLock = null;
                labelContinue.ForeColor = Color.Black;
                locked = false;
            } else
            {
                float s = labelContinue.Size.Width * (1.0f - lockPos);
                if (s < 1.0f) s = 1;
                panelLock.Size = new Size((int)s, labelContinue.Height);
                panelLock.Location = new Point((int)((labelContinue.Size.Width-s) / 2) + labelContinue.Location.X, labelContinue.Location.Y);
            }
        }
    }
}
