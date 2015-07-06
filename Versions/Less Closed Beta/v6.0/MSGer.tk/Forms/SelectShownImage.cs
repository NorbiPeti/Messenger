using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    public partial class SelectShownImage : ThemedForms
    { //2015.05.24.
        public SelectShownImage()
        {
            InitializeComponent();
            label1.Text = Language.Translate(Language.StringID.SelectImage); //2015.06.06.
            button1.Text = Language.Translate(Language.StringID.Modify); //2015.06.06.
            this.Text = label1.Text; //2015.06.06.
            cancelbtn.Text = Language.Translate(Language.StringID.Button_Cancel); //2015.06.06.
            //pictureBox1.Image = CurrentUser.Image;
            pictureBox1.Image = CurrentUser.Image.Clone() as Image; //2015.06.06.
            openFileDialog1.Filter = "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|"
            + "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff"; //2015.06.06.
        }

        private void button1_Click(object sender, EventArgs e)
        { //2015.06.06.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = Program.LoadImageFromFile(openFileDialog1.FileName);
            }
        }

        private void okbtn_Click(object sender, EventArgs e)
        { //2015.06.06.
            CurrentUser.Image = pictureBox1.Image;
            this.Close();
        }
    }
}
