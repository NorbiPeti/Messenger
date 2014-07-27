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
        public Notifier(string background, Color TransparentColor, string closebutton, int waittime) //waittime: 2014.04.17.
        {
            if (!File.Exists(background))
                throw new FileNotFoundException("A megadott háttér nem található.");
            if (!File.Exists(closebutton))
                throw new FileNotFoundException("A megadott bezáró ikon nem található.");
            InitializeComponent();
            this.BackgroundImage = Image.FromFile(background);
            this.TransparencyKey = TransparentColor;
            CloseButton.ImageLocation = closebutton;
            this.Show();
            this.Hide();
            NotifierTimer = new Timer();
            NotifierTimer.Interval = waittime;
            NotifierTimer.Tick += NotifierTimer_Tick;
        }

        void NotifierTimer_Tick(object sender, EventArgs e)
        {
            NotifierTimer.Stop();
            this.Hide();
        }

        public Notifier(Image background, Color TransparentColor, Image closebutton)
        {
            InitializeComponent();
            this.BackgroundImage = background;
            this.TransparencyKey = TransparentColor;
            CloseButton.Image = closebutton;
            this.Show();
            this.Hide();
        }
        public void Show(string title, string content) //(kép) - 2014.04.15.
        {
            WorkAreaRectangle = Screen.GetWorkingArea(WorkAreaRectangle); //2014.04.17.
            Title.Text = title;
            Content.Text = content;
            this.WindowState = FormWindowState.Normal;
            SetBounds(WorkAreaRectangle.Right - BackgroundImage.Width - 17, WorkAreaRectangle.Bottom - 1, BackgroundImage.Width, 0);
            this.Show();
            NotifierTimer.Start();
        }
    }
}
