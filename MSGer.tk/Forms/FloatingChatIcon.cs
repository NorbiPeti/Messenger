using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    public partial class FloatingChatIcon : Form
    {
        public FloatingChatIcon(ChatPanel cp)
        {
            InitializeComponent();
        }

        // defines how far we are extending the Glass margins
        private NativeMethods.MARGINS margins;

        // uses PInvoke to setup the Glass area.
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (NativeMethods.DwmIsCompositionEnabled())
            {
                // Paint the glass effect.
                margins = new NativeMethods.MARGINS();
                margins.Top = -1;
                margins.Left = 20;
                NativeMethods.DwmExtendFrameIntoClientArea(this.Handle, ref margins);
            }
        }
    }
}
