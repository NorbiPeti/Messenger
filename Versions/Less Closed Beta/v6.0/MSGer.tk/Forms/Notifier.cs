using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    public partial class Notifier : Form
    { //2014.04.15.
        private Rectangle WorkAreaRectangle;
        private Timer NotifierTimer;

        public event EventHandler CloseClick; //2014.08.29.
        public Notifier(string background, Color TransparentColor, string closebutton, int waittime) //waittime: 2014.04.17.
        {
            if (!File.Exists(background))
                throw new FileNotFoundException("A megadott háttér nem található.");
            if (!File.Exists(closebutton))
                throw new FileNotFoundException("A megadott bezáró ikon nem található.");
            InitializeComponent();
            //this.BackgroundImage = Image.FromFile(background);
            this.BackgroundImage = Program.LoadImageFromFile(background); //2015.06.06.
            this.TransparencyKey = TransparentColor;
            //CloseButton.ImageLocation = closebutton;
            CloseButton.Image = Program.LoadImageFromFile(background); //2015.06.06.
            //var size = Image.FromFile(closebutton).Size;
            var size = CloseButton.Image.Size; //2015.06.06.
            CloseButton.Left = CloseButton.Right - size.Width; //2014.08.29.
            CloseButton.Width = size.Width; //2014.08.29.
            CloseButton.Height = size.Height; //2014.08.29.
            NotifierTimer = new Timer();
            NotifierTimer.Interval = waittime;
            NotifierTimer.Tick += NotifierTimer_Tick;
        }

        void NotifierTimer_Tick(object sender, EventArgs e)
        {
            NotifierTimer.Stop();
            this.Hide();
        }

        public Notifier(Image background, Color TransparentColor, Image closebutton, int waittime) //waittime: 2014.08.28.
        {
            InitializeComponent();
            this.BackgroundImage = background;
            this.TransparencyKey = TransparentColor;
            CloseButton.Image = closebutton;
        }
        public void Show(string title, string content) //(kép) - 2014.04.15.
        {
            WorkAreaRectangle = Screen.GetWorkingArea(WorkAreaRectangle); //2014.04.17.
            Title.Text = title;
            Content.Text = content;
            this.WindowState = FormWindowState.Normal;
            SetBounds(WorkAreaRectangle.Right - BackgroundImage.Width - 17, WorkAreaRectangle.Bottom - 100, BackgroundImage.Width, 100);
            this.Show();
            NotifierTimer.Start();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        { //2014.08.29.
            CloseClick(sender, e);
        }
    }
}
