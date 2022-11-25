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
    public partial class PatientenBildschirm : Form
    {
        public PatientenBildschirm()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => labelHead.Text;
            set => labelHead.Text = value;
        }

        public void DisplayClear()
        {
            panelMain.Controls.Clear();
        }

        public void DisplaySingleMessage(string message, bool centered = true)
        {
            panelMain.Controls.Clear();

            Label label = new Label
            {
                AutoSize = false,
                Padding = new Padding(30),
                Dock = DockStyle.Fill,
                TextAlign = centered ? ContentAlignment.MiddleCenter : ContentAlignment.MiddleLeft,
                Text = message
            };
            panelMain.Controls.Add(label);
            panelMain.PerformLayout();

        }
    }
}
