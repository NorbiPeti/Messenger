using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Handwriting_program
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            handwriting1.PaintSize = trackBar1.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            handwriting1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            handwriting1.GetBitmap().Save("bmp.bmp");
        }
    }
}
