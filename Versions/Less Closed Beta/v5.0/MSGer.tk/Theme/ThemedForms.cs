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
    public partial class ThemedForms : Form
    { //2014.12.21.
        public const int BorderSize = 3;

        private bool overridecontrols = false;
        public ThemedForms()
        {
            InitializeComponent();
            this.Load += ThemedForms_Load;
            overridecontrols = true;
        }

        public void ThemedForms_Load(object sender, EventArgs e)
        {
            List<AnchorStyles> anchors = new List<AnchorStyles>();
            foreach (Control control in containerPanel.Controls)
            {
                anchors.Add(control.Anchor);
                control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            }
            this.SuspendLayout();
            base.FormBorderStyle = FormBorderStyle.None;
            this.Size = base.Size;
            CloseButton.Location = new Point(this.Size.Width - CloseButton.Size.Width, 0);
            MaximizeButton.Location = new Point(this.Size.Width - CloseButton.Size.Width - MaximizeButton.Size.Width, 0);
            MinimizeButton.Location = new Point(this.Size.Width - CloseButton.Size.Width - MaximizeButton.Size.Width - MinimizeButton.Size.Width, 0);
            titleLabel.Location = new Point(BorderSize, titleLabel.Location.Y);
            containerPanel.Location = new Point(BorderSize, titleLabel.Location.Y + titleLabel.Size.Height);
            int i = 0;
            this.ResumeLayout(true);
            foreach (var anchor in anchors)
            {
                containerPanel.Controls[i].Anchor = anchor;
                i++;
            }
            titleLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            CloseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            MaximizeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            MinimizeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            containerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            Theme.SkinControl(Theme.ThemePart.Border, this); //2014.12.24.
            Theme.SkinControl(Theme.ThemePart.MinimizeButton, MinimizeButton); //2014.12.24.
            Theme.SkinControl(Theme.ThemePart.MaximizeButton, MaximizeButton); //2014.12.24.
            Theme.SkinControl(Theme.ThemePart.CloseButton, CloseButton); //2014.12.24.
            foreach (Control control in this.Controls)
            {
                if (control is ToolStrip)
                    ((ToolStrip)control).Renderer = new MSGerToolStripRenderer();
                var controls = control.GetAll(typeof(ToolStrip));
                foreach (ToolStrip toolstrip in controls)
                {
                    //menustrip.Renderer=
                    toolstrip.Renderer = new MSGerToolStripRenderer();
                    break;
                }
            }
        }
        public new Control.ControlCollection Controls
        {
            get
            {
                if (overridecontrols)
                    return containerPanel.Controls;
                else
                    return base.Controls;
            }
        }
        private FormBorderStyle borderstyle = FormBorderStyle.Sizable;
        public new FormBorderStyle FormBorderStyle
        {
            get
            {
                return borderstyle;
            }
            set
            {
                borderstyle = value;
                base.FormBorderStyle = FormBorderStyle.None;
            }
        }
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                titleLabel.Text = value;
                base.Text = value;
            }
        }
        public new Size Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = new Size(value.Width + containerPanel.Location.X + BorderSize, value.Height + containerPanel.Location.Y + BorderSize);
                containerPanel.Size = value;
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MaximizeButton_Click(object sender, EventArgs e)
        {
            if (!MaximizeBox)
                return;
            FormWindowState ws; //Anti-virus program miatt
            if (WindowState == FormWindowState.Normal)
                ws = FormWindowState.Maximized;
            else
                ws = FormWindowState.Normal;

            Timer t = new Timer();
            t.Interval = 10;
            t.Tick += delegate
            {
                t.Stop();
                base.FormBorderStyle = FormBorderStyle.Sizable; //Látszik egy kis időre, de tán így a legkönnyebb
                WindowState = ws;
                base.FormBorderStyle = FormBorderStyle.None;
            };
            t.Start();
        }

        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            if (!MinimizeBox)
                return;
            WindowState = FormWindowState.Minimized;
        }

        private FormWindowState wstate;
        private void ThemedForms_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == wstate)
                return;
            if (this.WindowState == FormWindowState.Maximized && wstate == FormWindowState.Minimized) //minimized-ről váltott maximized-re
            {
                this.WindowState = FormWindowState.Normal;
                base.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Maximized;
                base.FormBorderStyle = FormBorderStyle.None;
            }
            wstate = this.WindowState;
        }

        private bool moving = false;
        private int resizing = 0;
        private Timer moveresizetimer = new Timer();
        private Point moveresizecursor;
        private void ThemedForms_MouseDown(object sender, MouseEventArgs e)
        {
            Point cursorpos = this.PointToClient(Cursor.Position);
            moveresizecursor = this.PointToClient(Cursor.Position); //Mindig ehhez igazítsa
            if (cursorpos.X > containerPanel.Location.X + containerPanel.Size.Width)
                resizing = 1; //right
            else if (cursorpos.X < containerPanel.Location.X)
                resizing = 2; //left
            else if (cursorpos.Y > containerPanel.Location.Y + containerPanel.Size.Height)
                resizing = 3; //bottom
            else if (cursorpos.Y < containerPanel.Location.Y)
                moving = true; //top
            if(!moveresizetimer.Enabled)
            {
                moveresizetimer.Interval = 10;
                moveresizetimer.Tick += moveresizetimer_Tick;
                moveresizetimer.Start();
            }
        }

        void moveresizetimer_Tick(object sender, EventArgs e)
        {
            if (moving)
            {
                int diffx = this.PointToClient(Cursor.Position).X - moveresizecursor.X;
                int diffy = this.PointToClient(Cursor.Position).Y - moveresizecursor.Y;
                this.Location = new Point(this.Location.X + diffx, this.Location.Y + diffy);
            }
            if (resizing > 0 && borderstyle == FormBorderStyle.Sizable)
            {
                if (resizing == 1)
                    base.Size = new Size(Cursor.Position.X - base.Location.X, base.Size.Height);
                else if (resizing == 2)
                {
                    int diff = base.Location.X - Cursor.Position.X;
                    base.Location = new Point(Cursor.Position.X, base.Location.Y);
                    base.Size = new Size(base.Size.Width + diff, base.Size.Height);
                }
                else if (resizing == 3)
                    base.Size = new Size(base.Size.Width, Cursor.Position.Y - base.Location.Y);
            }
        }

        private void ThemedForms_MouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
            resizing = 0;
            moveresizetimer.Stop();
        }

        private void ThemedForms_DoubleClick(object sender, EventArgs e)
        {
            if (this.PointToClient(Cursor.Position).Y < containerPanel.Location.Y)
            {
                if (!MaximizeBox)
                    return; //2015.04.08.
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Normal;
                    base.FormBorderStyle = FormBorderStyle.Sizable;
                    this.WindowState = FormWindowState.Maximized;
                    base.FormBorderStyle = FormBorderStyle.None;
                }
                else
                    this.WindowState = FormWindowState.Normal;
            }
        }

        public Control.ControlCollection GetOriginalControls()
        {
            return base.Controls;
        }
    }
}
