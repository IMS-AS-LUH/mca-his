using System;
using System.Drawing;
using System.Windows.Forms;

namespace HisDemo.UI
{
    public class CommandKeyPanel : UserControl
    {

        private struct SubPanel
        {
            public Panel panel;
            public Label keyLabel;
            public Button functionButton;
            public CommandKeyCallback callback;
            public object argument;
            public Color color;
        }

        private readonly SubPanel[] subPanels = new SubPanel[12];
        private readonly TableLayoutPanel layoutPanel = new TableLayoutPanel();

        private Timer feedbackResetTimer;

        public Font ButtonFont
        {
            get => subPanels[0].functionButton.Font;
            set
            {
                foreach (SubPanel subPanel in subPanels)
                    subPanel.functionButton.Font = value;
            }
        }

        public delegate void CommandKeyCallback(object argument);

        public void SetFunction(int index, string text, CommandKeyCallback callback, object argument = null, Color? color = null)
        {
            if (index < 0 || index > 11)
                throw new ArgumentOutOfRangeException("index");
            subPanels[index].functionButton.Text = text;
            subPanels[index].color = color ?? Color.Empty;
            subPanels[index].functionButton.BackColor = subPanels[index].color;
            subPanels[index].functionButton.Enabled = callback != null;
            subPanels[index].callback = callback;
            subPanels[index].argument = callback == null ? null : argument;
        }
        public void ClearFunction(int index)
        {
            SetFunction(index, "-/-", null);
        }

        public bool ProcessCmdKey(Keys keyData)
        {
            if (keyData >= Keys.F1 && keyData <= Keys.F12)
            {
                int i = keyData - Keys.F1;
                if (i >= 0 && i < 12)
                {
                    ExecuteButton(i);
                }
                return true;
            }
            return false;
        }

        public CommandKeyPanel()
        {
            SuspendLayout();

            //BackColor = Color.Gray;
            Name = "CommandKeyPanel";

            Controls.Add(layoutPanel);
            layoutPanel.Dock = DockStyle.Fill;
            layoutPanel.ColumnCount = 12;
            layoutPanel.RowCount = 1;

            layoutPanel.ColumnStyles.Clear();
            layoutPanel.RowStyles.Clear();
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));

            TabStop = false;

            for (int i = 0; i < 12; i++)
            {
                subPanels[i] = new SubPanel();
                Panel panel = new Panel();
                Label keyLabel = new Label();
                Button functionButton = new Button();

                keyLabel.Text = $"F{i + 1:D}:";
                keyLabel.BackColor = Color.LightGray;
                keyLabel.Margin = new Padding(5);
                panel.Controls.Add(functionButton);
                functionButton.Dock = DockStyle.Fill;
                functionButton.AutoSize = false;
                functionButton.TextAlign = ContentAlignment.MiddleCenter;
                panel.Controls.Add(keyLabel);
                keyLabel.Dock = DockStyle.Top;
                keyLabel.AutoSize = false;
                keyLabel.TextAlign = ContentAlignment.MiddleCenter;

                panel.Dock = DockStyle.Fill;
                panel.TabStop = false;
                keyLabel.TabStop = false;
                functionButton.TabStop = false;

                subPanels[i].panel = panel;
                subPanels[i].keyLabel = keyLabel;
                subPanels[i].functionButton = functionButton;
                subPanels[i].callback = null;
                subPanels[i].color = Color.Empty;

                functionButton.Click += FunctionButton_Click;
                functionButton.Tag = i;

                ClearFunction(i);

                layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f / 12));
                layoutPanel.Controls.Add(panel);
                layoutPanel.SetColumn(panel, i);
            }

            feedbackResetTimer = new Timer();
            feedbackResetTimer.Interval = 250;
            feedbackResetTimer.Tick += FeedbackResetTimer_Tick;

            ResumeLayout(false);
        }

        private void FeedbackResetTimer_Tick(object sender, EventArgs e)
        {
            feedbackResetTimer.Stop();
            foreach (var panel in subPanels)
            {
                panel.functionButton.BackColor = panel.color;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                feedbackResetTimer.Dispose();
                feedbackResetTimer = null;
            }
            base.Dispose(disposing);
        }

        private void FunctionButton_Click(object sender, EventArgs e)
        {
            int i = (int)((Button)sender).Tag;
            // Disable and potentially re-enable button: Prevent user from "re-triggering" by Space/Enter/Return key
            bool previousState = subPanels[i].functionButton.Enabled;
            subPanels[i].functionButton.Enabled = false;
            subPanels[i].functionButton.Enabled = previousState;
            ExecuteButton(i);
        }

        private void ExecuteButton(int i)
        {
            subPanels[i].functionButton.BackColor = Color.White;
            feedbackResetTimer.Stop();
            feedbackResetTimer.Start();
            // Execute associated callback
            subPanels[i].callback?.Invoke(subPanels[i].argument);
        }
    }
}
