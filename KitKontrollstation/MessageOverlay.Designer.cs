namespace HisDemo.KitKontrollstation
{
    partial class MessageOverlay
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
            this.panelInner = new System.Windows.Forms.Panel();
            this.labelContinue = new System.Windows.Forms.Label();
            this.labelMessage = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.timerBlink = new System.Windows.Forms.Timer(this.components);
            this.timerUnlockStep = new System.Windows.Forms.Timer(this.components);
            this.panelInner.SuspendLayout();
            this.panelOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelInner
            // 
            this.panelInner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInner.BackColor = System.Drawing.Color.White;
            this.panelInner.Controls.Add(this.labelContinue);
            this.panelInner.Controls.Add(this.labelMessage);
            this.panelInner.Controls.Add(this.labelTitle);
            this.panelInner.Location = new System.Drawing.Point(8, 8);
            this.panelInner.Margin = new System.Windows.Forms.Padding(8);
            this.panelInner.Name = "panelInner";
            this.panelInner.Size = new System.Drawing.Size(1011, 510);
            this.panelInner.TabIndex = 0;
            // 
            // labelContinue
            // 
            this.labelContinue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelContinue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelContinue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelContinue.Location = new System.Drawing.Point(19, 428);
            this.labelContinue.Margin = new System.Windows.Forms.Padding(8);
            this.labelContinue.Name = "labelContinue";
            this.labelContinue.Size = new System.Drawing.Size(973, 75);
            this.labelContinue.TabIndex = 2;
            this.labelContinue.Text = "Drücken Sie die Enter- oder Return-Taste um fortzufahren.";
            this.labelContinue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMessage
            // 
            this.labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessage.Location = new System.Drawing.Point(17, 125);
            this.labelMessage.Margin = new System.Windows.Forms.Padding(8);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(973, 288);
            this.labelMessage.TabIndex = 1;
            this.labelMessage.Text = "Message";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(27, 8);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(8);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(964, 102);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Error / Warning";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelOuter
            // 
            this.panelOuter.BackColor = System.Drawing.Color.Yellow;
            this.panelOuter.Controls.Add(this.panelInner);
            this.panelOuter.Location = new System.Drawing.Point(20, 19);
            this.panelOuter.Margin = new System.Windows.Forms.Padding(8);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(1027, 525);
            this.panelOuter.TabIndex = 1;
            // 
            // timerBlink
            // 
            this.timerBlink.Enabled = true;
            this.timerBlink.Interval = 500;
            this.timerBlink.Tick += new System.EventHandler(this.timerBlink_Tick);
            // 
            // timerUnlockStep
            // 
            this.timerUnlockStep.Interval = 50;
            this.timerUnlockStep.Tick += new System.EventHandler(this.timerUnlockStep_Tick);
            // 
            // MessageOverlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1067, 562);
            this.ControlBox = false;
            this.Controls.Add(this.panelOuter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageOverlay";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MessageOverlay";
            this.panelInner.ResumeLayout(false);
            this.panelOuter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelInner;
        private System.Windows.Forms.Label labelContinue;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelOuter;
        private System.Windows.Forms.Timer timerBlink;
        private System.Windows.Forms.Timer timerUnlockStep;
    }
}