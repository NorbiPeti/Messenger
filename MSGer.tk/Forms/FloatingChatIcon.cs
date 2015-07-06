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
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
        }

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled();

        public FloatingChatIcon(ChatPanel cp)
        {
            InitializeComponent();
        }

        // defines how far we are extending the Glass margins
        private MARGINS margins;

        // uses PInvoke to setup the Glass area.
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DwmIsCompositionEnabled())
            {
                // Paint the glass effect.
                margins = new MARGINS();
                margins.Top = -1;
                margins.Left = 20;
                DwmExtendFrameIntoClientArea(this.Handle, ref margins);
            }
        }
    }
}
