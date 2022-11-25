using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;

namespace HisDemo.UI
{
    public class B32IDTextBox : TextBox
    {
        public override Font Font { get; set; } = new Font("monospace", 16.0f, FontStyle.Bold);

        public override string Text {
            get => base.Text;
            set => base.Text = value;
        }

        protected bool colorizeInvalidID = true;
        public bool ColorizeInvalidID {
            get => colorizeInvalidID;
            set
            {
                colorizeInvalidID = value;
                UpdateStyleExtension();
            }
        }

        protected Color idleBackColor = SystemColors.Window;
        public override Color BackColor {
            get => idleBackColor;
            set
            {
                idleBackColor = value;
                UpdateStyleExtension();
            }
        }

        protected Color invalidIdBackColor = Color.LightSalmon;
        public Color InvalidIdBackColor
        {
            get => invalidIdBackColor;
            set
            {
                invalidIdBackColor = value;
                UpdateStyleExtension();
            }
        }

        protected void UpdateStyleExtension()
        {
            if (ColorizeInvalidID && !IsValidID)
            {
                base.BackColor = invalidIdBackColor;
            }
            else
            {
                base.BackColor = idleBackColor;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            UpdateStyleExtension();
            base.OnTextChanged(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            UpdateStyleExtension();
        }

        public uint? ID
        {
            get
            {
                try
                {
                    return B32ID.Decode(base.Text);
                }
                catch (B32ID.DecodingException)
                {
                    return null;
                }
            }
        }

        public bool IsValidID
        {
            get
            {
                try
                {
                    B32ID.Decode(base.Text);
                    return true;
                }
                catch (B32ID.DecodingException)
                {
                    return false;
                }
            }
        }

        public string DecodingErrorMessage
        {
            get
            {
                try
                {
                    B32ID.Decode(base.Text);
                    return null;
                }
                catch (B32ID.DecodingException ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}
