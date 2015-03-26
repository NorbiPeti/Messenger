using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Handwriting_program
{
    public partial class Handwriting : UserControl //2014.09.11-12.
    {
        public Handwriting()
        {
            InitializeComponent();
            /*pictureBox1.Image = new Bitmap(this.Width, this.Height);
            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;*/
            //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            //this.ResizeRedraw = true;
            pen = new Pen(PaintColor, PaintSize);
            //pen.Brush = new SolidBrush(PaintColor);
            pen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Round);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            //gr = this.CreateGraphics();
            gr = panel1.CreateGraphics(); //2014.11.08.
            gr.SmoothingMode = SmoothingMode.HighQuality;
            thread = new Thread(new ThreadStart(ThreadFunc));
            thread.IsBackground = true;
            MainT = Thread.CurrentThread;
            thread.Start();
            //bmp = new Bitmap(this.Width, this.Height);
            //bmp = new Bitmap(panel1.Width, panel1.Height, PixelFormat.Format64bppArgb); //2014.11.08. - 2014.12.05. - PixelFormat
            bmp = new Bitmap(panel1.Width, panel1.Height);
        }

        private Color paintcolor = Color.Black;
        public Color PaintColor
        {
            get
            {
                return paintcolor;
            }
            set
            {
                paintcolor = value;
                //pen.Dispose();
                //pen = new Pen(PaintColor, PaintSize);
                pen.Color = value;
            }
        }
        private int paintsize = 1;
        public int PaintSize
        {
            get
            {
                return paintsize;
            }
            set
            {
                paintsize = value;
                //pen.Dispose();
                //pen = new Pen(PaintColor, PaintSize);
                pen.Width = value;
            }
        }
        public bool Erase { get; set; }
        //private Timer timer = new Timer();
        //private Timer resizeT = new Timer();
        private Thread thread;
        //private bool resizing = false;
        private Pen pen;
        private Point prevp;
        private Graphics gr;
        private Graphics bmpgr;
        private Bitmap bmp;
        private bool drawing = false;
        private Thread MainT;
        private void Handwriting_MouseDown(object sender, MouseEventArgs e)
        {
            /*timer.Interval = 100;
            timer.Tick += timer_Tick;
            timer.Start();*/
            drawing = true;
        }

        //void timer_Tick(object sender, EventArgs e)
        void ThreadFunc()
        {
            while (MainT.IsAlive)
            {
                if (!drawing)
                {
                    prevp = new Point();
                    continue;
                }
                //GC.Collect();
                /*var bm = new Bitmap(pictureBox1.Image);
                Point p = pictureBox1.PointToClient(Cursor.Position);*/
                Point p = new Point();
                //this.Invoke(new Action(() => p = this.PointToClient(Cursor.Position)));
                this.Invoke(new Action(() => p = panel1.PointToClient(Cursor.Position))); //2014.11.08.
                //Graphics gr = this.CreateGraphics();
                //this.Invoke(new Action(() => gr = this.CreateGraphics()));
                this.Invoke(new Action(() => gr = panel1.CreateGraphics())); //2014.11.08.
                /*if (bmp != null)
                {
                    bmp.Dispose();
                    bmp = null;
                }
                bmp = new Bitmap(this.Width, this.Height);*/
                this.Invoke(new Action(() => bmpgr = Graphics.FromImage(bmp)));
                var tmppen = (Pen)pen.Clone();
                if (Erase)
                    tmppen.Color = Color.White; //2014.11.08.
                gr.DrawLine(tmppen, p, ((prevp.X != 0 && prevp.Y != 0) ? prevp : p));
                //if (Erase)
                    //tmppen.Color = Color.Transparent; //2014.11.08.
                    //tmppen.Color = Color.Empty; //2014.12.05.
                bmpgr.DrawLine(tmppen, p, ((prevp.X != 0 && prevp.Y != 0) ? prevp : p));
                if (Erase)
                    bmp.MakeTransparent(Color.White); //2014.12.05.
                prevp = p;
                //Bitmap tmpbmp = new Bitmap(this.Width, this.Height);
                //Bitmap tmpbmp = new Bitmap(panel1.Width, panel1.Height, PixelFormat.Format64bppArgb); //2014.11.08. - 2014.12.05. - PixelFormat
                Bitmap tmpbmp = new Bitmap(panel1.Width, panel1.Height);
                using (Graphics g = Graphics.FromImage(tmpbmp))
                    //g.DrawImage(bmp, 0, 0, this.Width, this.Height);
                    g.DrawImage(bmp, 0, 0, panel1.Width, panel1.Height); //2014.11.08.
                bmp.Dispose();
                bmp = tmpbmp;

                /*if (bmp != null)
                {
                    bmp.Dispose();
                    bmp = null;
                }
                bmp = new Bitmap(this.Width, this.Height);
                this.Invoke(new Action(() => this.DrawToBitmap(bmp, new Rectangle(new Point(), this.Size))));*/
                GC.Collect();
            }
            /*if (p.X < 0 || p.X >= bm.Width || p.Y < 0 || p.Y >= bm.Height)
                return;
            for (int i = 0; i < PaintSize; i++)
            {
                *try { bm.SetPixel(p.X - i, p.Y - i, PaintColor); }
                catch (ArgumentOutOfRangeException)
                {
                }*
                if (p.X - i >= 0 && p.Y - i >= 0)
                    bm.SetPixel(p.X - i, p.Y - i, PaintColor);
                if (p.X - i >= 0 && p.Y + i < bm.Height)
                    bm.SetPixel(p.X - i, p.Y + i, PaintColor);
                if (p.X + i < bm.Width && p.Y - i >= 0)
                    bm.SetPixel(p.X + i, p.Y - i, PaintColor);
                if (p.X + i < bm.Width && p.Y + i < bm.Height)
                    bm.SetPixel(p.X + i, p.Y + i, PaintColor);
                if (p.Y - i >= 0)
                    bm.SetPixel(p.X, p.Y - i, PaintColor);
                if (p.Y + i < bm.Height)
                    bm.SetPixel(p.X, p.Y + i, PaintColor);
                if (p.X - i >= 0)
                    bm.SetPixel(p.X - i, p.Y, PaintColor);
                if (p.X + i < bm.Width)
                    bm.SetPixel(p.X + i, p.Y, PaintColor);
                for (int j = 0; j < i; j++)
                {
                    if (p.X - j >= 0 && p.Y + i < bm.Height)
                        bm.SetPixel(p.X - j, p.Y + i, PaintColor);
                    if (p.X + j < bm.Width && p.Y + i < bm.Height)
                        bm.SetPixel(p.X + j, p.Y + i, PaintColor);
                    if (p.X - j >= 0 && p.Y - i >= 0)
                        bm.SetPixel(p.X - j, p.Y - i, PaintColor);
                    if (p.X + j < bm.Width && p.Y - i >= 0)
                        bm.SetPixel(p.X + j, p.Y - i, PaintColor);
                //}
                //for (int j = 0; j < i; j++)
                //{
                    if (p.X - i >= 0 && p.Y + j < bm.Height)
                        bm.SetPixel(p.X - i, p.Y + j, PaintColor);
                    if (p.X + i < bm.Height && p.Y + j < bm.Height)
                        bm.SetPixel(p.X + i, p.Y + j, PaintColor);
                    if (p.X - i >= 0 && p.Y - j >= 0)
                        bm.SetPixel(p.X - i, p.Y - j, PaintColor);
                    if (p.X + i < bm.Width && p.Y - j >= 0)
                        bm.SetPixel(p.X + i, p.Y - j, PaintColor);
                }
            }
            
            pictureBox1.Image.Dispose();
            pctureBox1.Image = bm;*/
        }

        private void Handwriting_MouseUp(object sender, MouseEventArgs e)
        {
            //timer.Stop();
            drawing = false;

            //prevp = null;
            prevp = new Point();
        }

        public void Clear()
        {
            //pictureBox1.Image = new Bitmap(this.Width, this.Height);
            //Graphics gr = this.CreateGraphics();
            //gr.Clear(this.BackColor);
            gr.Clear(panel1.BackColor); //2014.11.08.
            bmpgr = Graphics.FromImage(bmp);
            bmpgr.Clear(Color.Transparent);
        }

        /*private void pictureBox1_Resize(object sender, EventArgs e)
        {
            if(!resizeT.Enabled)
            {
                resizeT.Interval = 100;
                resizeT.Tick += resizeT_Tick;
                resizeT.Start();
            }
            resizing = true;
            //pictureBox1.Size = this.Size;
            /*try
            {
                var tmp = pictureBox1.Image;
                pictureBox1.Image = new Bitmap(pictureBox1.Image, this.Size);
                tmp.Dispose();
            }
            catch (ArgumentException)
            {
            }*/
            //Refresh();
        //}

        /*void resizeT_Tick(object sender, EventArgs e)
        {
            if (!resizing)
            {
                pictureBox1.Size = this.Size;
                try
                {
                    var tmp = pictureBox1.Image;
                    pictureBox1.Image = new Bitmap(pictureBox1.Image, this.Size);
                    tmp.Dispose();
                }
                catch (ArgumentException)
                {
                }
                Refresh();
                resizeT.Stop();
            }
            else
                resizing = false; //If true set it to false and if it doesn't turn back to true then the resizing is done
        }*/

        public Bitmap GetBitmap()
        {
            /*var bmp = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bmp, new Rectangle(new Point(), this.Size));*/
            return bmp;
        }

        private void Handwriting_Paint(object sender, PaintEventArgs e)
        {
            /*var bmp = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bmp, new Rectangle(new Point(), this.Size));*/
            if (bmp != null)
                e.Graphics.DrawImage(bmp, new Point());
        }

        private void colorbtn_Click(object sender, EventArgs e)
        {
            var cpicker = new ColorDialog();
            if (cpicker.ShowDialog() == DialogResult.Cancel)
                return;
            PaintColor = cpicker.Color;
        }

        private void erasebtn_Click(object sender, EventArgs e)
        {
            if (Erase)
            {
                Erase = false;
                //erasebtn.Text = "erase"; //Képekkel helyettesíteni
                erasebtn.FlatAppearance.BorderSize = 1; //2014.11.08.
            }
            else
            {
                Erase = true;
                //erasebtn.Text = "draw"; //Képekkel helyettesíteni
                erasebtn.FlatAppearance.BorderSize = 3; //2014.11.08.
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            pen.Width = trackBar1.Value;
        }

        /*private void Handwriting_MouseClick(object sender, MouseEventArgs e)
        {
            Handwriting_MouseUp(sender, e);
        }*/
    }
}
