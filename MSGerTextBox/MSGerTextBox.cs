using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using my.utils;
using System.Drawing.Drawing2D;

namespace SzNPProjects.TextBox
{
    public partial class MSGerTextBox : UserControl
    { //2015.06.06. - TO!DO: Kurzor megjelenítése, tulajdonságok kezelése (Font), képek kezelése
        private LineRenderer linerenderer; //2015.06.10.
        public MSGerTextBox()
        { //TO!DO: WordWrap
            InitializeComponent();
            Lines.Add("");
            linerenderer = new LineRenderer(panel1); //2015.06.10.
            panel1.Paint += OnPanelPaint; //2015.06.10.
            CursorRendererTimer.Tick += OnCursorRender; //2015.06.11.
            CursorRendererTimer.Interval = SystemInformation.CaretBlinkTime; //2015.06.11.
            CursorRendererTimer.Start(); //2015.06.11.
        }

        public override string Text
        {
            get
            {
                return Lines.Aggregate((entry1, entry2) => entry1 + "\n" + entry2);
            }
            set
            {
                var results = Diff.DiffText(Lines.Aggregate((entry1, entry2) => entry1 + "\n" + entry2), value, false, false, false);
                List<int> changedlines = new List<int>();
                string[] vlines = value.Split(new char[] { '\n', '\r' }); //Itt az Enter-t \r jelzi
                if (vlines.Length > Lines.Count)
                    lines.Add("");
                for (int i = 0; i < results.Length; i++)
                {
                    var result = results[i];
                    if (result.deletedA > result.insertedB)
                        lines.RemoveAt(result.StartA);
                    if (result.StartA >= 0 && result.StartA < Lines.Count && result.StartB >= 0 && result.StartB < vlines.Length) //<-- 2015.06.10.
                        Lines[result.StartA] = vlines[result.StartB];
                }
                //RefreshControls(changedlines); //TODO: Event handler-eket a BindingList-hoz, és ezt átrakni oda
                //linerenderer.Render(); //2015.06.10.
                //this.Refresh(); //2015.06.10.
                panel1.Text = this.Text; //2015.06.10.
                panel1.Refresh(); //2015.06.10.
            }
        }

        private BindingList<string> lines = new BindingList<string>();
        public BindingList<string> Lines
        {
            get
            {
                return lines;
            }
            set
            {
                lines = value;
                //RefreshControls(new int[] { });
                this.Refresh(); //2015.06.10.
            }
        }

        //public int CursorPosition { get; set; }
        private int currentline;
        public int CurrentLine
        {
            get
            {
                return currentline;
            }
            set
            {
                if (value >= Lines.Count)
                    Lines.Add("");
                currentline = value;
            }
        }

        private bool wordwrap; //2015.06.11.
        public bool WordWrap
        { //2015.06.11.
            get
            {
                return wordwrap;
            }
            set
            {
                wordwrap = value;
                panel1.Refresh();
            }
        }

        private int cursorposition; //2015.06.11.
        public int CursorPosition
        { //2015.06.11.
            get
            {
                return cursorposition;
            }
            set
            {
                cursorposition = value;
                panel1.Refresh();
            }
        }

        public Dictionary<string, Image> Emoticons = new Dictionary<string, Image>();

        /*private List<Label> labels = new List<Label>();
        private void RefreshControls(IEnumerable<int> changedlines)
        {
            int y = Lines.Count - 1;
            while (Lines.Count > labels.Count)
                CreateLabel(y++);
            if (changedlines.Count() == 0)
            {
                for(int i=0; i<Lines.Count; i++)
                {
                    labels[i].Text = Lines[i];
                }
            }
            foreach (int item in changedlines)
            {
                if (item >= labels.Count)
                    CreateLabel(item);
                while (item >= lines.Count)
                    lines.Add("");
                labels[item].Text = Lines[item];
            }
        }

        private Label CreateLabel(int y)
        {
            using (Graphics g = this.CreateGraphics())
            {
                var label = new Label { Location = new Point(0, y * (int)(Font.SizeInPoints * g.DpiX / 72)), AutoSize = true };
                labels.Add(label);
                this.Controls.Add(label);
                return label;
            }
        }*/

        private void MSGerTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Lines[CurrentLine] += e.KeyChar;
            TextChangedToPaint = true; //2015.06.14.
            if (e.KeyChar != '\n' && e.KeyChar != '\r') //2015.06.11.
                cursorposition++; //2015.06.11.
            else //2015.06.11.
            { //2015.06.11.
                cursorposition = 0;
                currentline++;
            }
            this.Text += e.KeyChar;
            //RefreshControls(new int[] { CurrentLine });
        }

        /*protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
                CurrentLine++;
            if (char.IsLetterOrDigit((char)keyData) || char.IsSymbol((char)keyData))
            {
                //lines[CurrentLine] += (char)keyData;
                //char[] c = Encoding.Unicode.GetChars(Encoding.Convert(Encoding.ASCII, Encoding.Unicode, Encoding.ASCII.GetBytes(new char[] { (char)keyData })));
                //lines[CurrentLine] += c[0];
                *var converter = new KeysConverter();
                lines[CurrentLine] += converter.ConvertToInvariantString(keyData);*
                //lines[CurrentLine] += new string(new char[] { (char)keyData });
                this.Text += (char)keyData;
                RefreshControls(new int[] { CurrentLine });
            }
            else if(false)
            {

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }*/

        private bool TextChangedToPaint = false; //2015.06.14.
        //protected override void OnPaint(PaintEventArgs e)
        public void OnPanelPaint(object sender, PaintEventArgs e)
        { //2015.06.10.
            //base.OnPaint(e);
            var gr = linerenderer.Render(WordWrap);
            SizeF size;
            /*if (cursorposition == 16) //2015.06.11.
                Console.WriteLine(); //2015.06.11.*/
            string linebefrorecursor = lines[currentline].Substring(0, cursorposition); //2015.06.11.
            if (linebefrorecursor.Length == 0)
                linebefrorecursor = " "; //2015.06.11.
            if (wordwrap) //2015.06.11.
                size = gr.MeasureString(linebefrorecursor, this.Font, panel1.Width); //2015.06.11.
            else //2015.06.11.
                size = gr.MeasureString(linebefrorecursor, this.Font); //2015.06.11.
            //this.HorizontalScroll.Maximum = panel1.Width; //2015.06.11.
            //this.VerticalScroll.Maximum = panel1.Height; //2015.06.11.
            if (TextChangedToPaint)
            {
                this.HorizontalScroll.Value = ((int)size.Width / panel1.Width) * this.HorizontalScroll.Maximum; //2015.06.11.
                //this.VerticalScroll.Value = this.Height; //2015.06.11.
                this.VerticalScroll.Value = ((int)(size.Height * lines.Count) / panel1.Height) * this.VerticalScroll.Maximum; //2015.06.11. //TODO: Ne állítsa be minden rajzoláskor
                //TO!DO: size.Height * lines.Count helyett mérje meg az előző sorok magasságát a sortörések miatt (if(wordwrap))
                CursorRenderPosition1 = new Point((int)size.Width, 0); //2015.06.11. //TODO
                CursorRenderPosition2 = new Point((int)size.Width, (int)size.Height); //2015.06.11.
                TextChangedToPaint = false; //2015.06.14.
            }
        }

        private Timer CursorRendererTimer = new Timer(); //2015.06.11.
        private bool CursorRendered = false; //2015.06.11.
        private Point CursorRenderPosition1; //2015.06.11.
        private Point CursorRenderPosition2; //2015.06.11.
        private void OnCursorRender(object sender, EventArgs e)
        { //2015.06.11.
            using (var gr = panel1.CreateGraphics())
            {
                if (CursorRenderPosition1.IsEmpty || CursorRenderPosition2.IsEmpty)
                    return;
                Color color;
                if (!CursorRendered)
                {
                    color = this.ForeColor;
                }
                else
                {
                    color = this.BackColor;
                }
                gr.DrawLine(new Pen(new SolidBrush(color), SystemInformation.CaretWidth), CursorRenderPosition1, CursorRenderPosition2);
                CursorRendered = !CursorRendered;
            }
        }

        private class LineRenderer
        {
            //public string Text { get; set; }
            public Control OwnerControl { get; private set; } //2015.06.10.
            private Graphics Graphics; //2015.06.10.
            public Graphics Render(bool wordwrap)
            { //2015.06.10.
                //TO!DO
                //Console.WriteLine("Strlen: " + OwnerControl.Text.Length); //2015.06.11.
                if (Graphics == null)
                    Graphics = OwnerControl.CreateGraphics();
                /*Graphics.ResetClip(); //2015.06.11.
                Graphics.Clip = new Region(OwnerControl.Bounds); //2015.06.11.
                Graphics.ResetClip(); //2015.06.11.*/
                Graphics.Clear(OwnerControl.BackColor);
                //Graphics.DrawString(OwnerControl.Text, OwnerControl.Font, new LinearGradientBrush(new Point(OwnerControl.Size.Width / 4 * 1, OwnerControl.Size.Height / 4 * 1), new Point(OwnerControl.Size.Width / 4 * 3, OwnerControl.Size.Height / 4 * 3), Color.Black, Color.Blue), OwnerControl.Bounds, new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
                SizeF strsize;
                if (wordwrap) //2015.06.11.
                    strsize = Graphics.MeasureString(OwnerControl.Text, OwnerControl.Font, OwnerControl.Width);
                else
                    strsize = Graphics.MeasureString(OwnerControl.Text, OwnerControl.Font); //2015.06.11.
                //Console.WriteLine("strsize: " + strsize);
                /*float linecountf = strsize.Width / OwnerControl.Width;
                if (linecountf - (int)linecountf > 0)
                    linecountf++;*/
                if ((int)strsize.Width > OwnerControl.Width)
                    OwnerControl.Width = (int)strsize.Width;
                if ((int)strsize.Height > OwnerControl.Height)
                    OwnerControl.Height = (int)strsize.Height;
                Graphics.Dispose(); //2015.06.11.
                Graphics = OwnerControl.CreateGraphics(); //2015.06.11.
                if (!strsize.IsEmpty)
                    Graphics.DrawString(OwnerControl.Text, OwnerControl.Font,
                        new LinearGradientBrush(new RectangleF(new PointF(), strsize),
                            Color.Black, Color.Blue, LinearGradientMode.Horizontal),
                        new RectangleF(new PointF(), strsize),
                        new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
                //TO!DO: Ténylegesen soronként renderelje, a sortörést a MeasureString-nél ki lehetne használni, és utána külön sornak venni rendereléskor
                return Graphics;
            }
            public LineRenderer(Control control)
            {
                OwnerControl = control;
            }
        }
    }
}
