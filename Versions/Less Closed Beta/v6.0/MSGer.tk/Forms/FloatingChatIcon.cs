using System;
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
            //pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //Graphics g = Graphics.FromImage(pictureBox1.Image);
            //g.DrawEllipse(new Pen(Color.FromArgb(50, Color.Blue), 10), new Rectangle(10, 10, 50, 50));
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
        /*protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaint(e);
            if (DwmIsCompositionEnabled())
            {
                // paint background black to enable include glass regions
                e.Graphics.Clear(Color.Black);
                // revert the non-glass rectangle back to it's original colour
                *Rectangle clientArea = new Rectangle(
                        margins.Left,
                        margins.Top,
                        this.ClientRectangle.Width - margins.Left - margins.Right,
                        this.ClientRectangle.Height - margins.Top - margins.Bottom
                    );
                Brush b = new SolidBrush(this.BackColor);
                e.Graphics.FillRectangle(b, clientArea);*
            }
        }*/
    }
}
