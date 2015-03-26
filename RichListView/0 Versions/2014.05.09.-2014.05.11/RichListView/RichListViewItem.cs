using Khendys.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SzNPProjects
{
    public class RichListViewItem
    {
        /*public Control Control { get; set; }
        public string[] SubItems { get; internal set; }*/
        public Control[] SubItems { get; internal set; }
        public RichListView Parent { get; internal set; }
        private Color backcolor;
        public Color BackColor
        {
            get
            {
                return backcolor;
            }
            set
            {
                for (int i = 0; i < SubItems.Length; i++)
                    SubItems[i].BackColor = value;
                backcolor = value;
            }
        }
        private Color forecolor;
        public Color ForeColor
        {
            get
            {
                return forecolor;
            }
            set
            {
                for (int i = 0; i < SubItems.Length; i++)
                    SubItems[i].ForeColor = value;
                forecolor = value;
            }
        }
        public RichListViewItem()
        {
            //_CreateInstance();
        }
        //public RichListViewItem(Control control)
        public RichListViewItem(Control[] subitems)
        {
            //Control = control;
            SubItems = subitems;
            _CreateInstance();
        }
        internal void _CreateInstance()
        {
            /*if (Control == null)
                Control = new ExRichTextBox();
            ((ExRichTextBox)Control).ReadOnly = true;*/
            if (SubItems == null)
                SubItems = new Control[Parent.Columns.Length];
            for (int i = 0; i < Parent.Columns.Length; i++)
            {
                if (SubItems[i] == null)
                {
                    SubItems[i] = new ExRichTextBox();
                    SubItems[i].Parent = this.Parent;
                }
                if (SubItems[i].GetType().IsSubclassOf(typeof(TextBoxBase))) //If it's a TextBox, set it to read-only
                {
                    ((TextBoxBase)SubItems[i]).ReadOnly = true;
                    ((TextBoxBase)SubItems[i]).BorderStyle = BorderStyle.None;
                    ((TextBoxBase)SubItems[i]).Cursor = Cursors.Arrow;
                }
                this.SubItems[i].BackColor = this.Parent.BackColor;
                this.SubItems[i].ForeColor = this.Parent.ForeColor;
                SubItems[i].Click += _ItemClicked;
                SubItems[i].DoubleClick += _ItemDoubleClicked;
            }
        }
        private void _ItemClicked(object sender, EventArgs e)
        {/*
            for(int i=0; i<SubItems.Length; i++)
            {
                if(sender==SubItems[i])
                {
                    //Parent.ItemClicked(sender, i);
                    Parent._ItemClicked(sender, i);
                    break;
                }
            }*/
            Parent._ItemClicked(this);
        }
        private void _ItemDoubleClicked(object sender, EventArgs e)
        {
            Parent._ItemDoubleClicked(this);
        }

        internal void Show()
        {
            if (Parent == null)
                throw new NoParentException("Try to reference ListViewItem which is not assigned to a ListView control.");
            for (int i = 0; i < SubItems.Length; i++)
                SubItems[i].Show();
        }

        internal void SetBounds(int Y, int height)
        {
            if (Parent == null)
                throw new NoParentException("Try to reference ListViewItem which is not assigned to a ListView control.");
            int lastcolwidth = Parent.Width;
            for (int i = 0; i < SubItems.Length; i++)
            {
                SubItems[i].SetBounds(((i - 1 < 0) ? 0 : Parent.Columns[i - 1].Width),
                    Y,
                    ((Parent.ColumnAutoFill && i + 1 == Parent.Columns.Length) ? lastcolwidth : Parent.Columns[i].Width),
                    height);
                lastcolwidth -= Parent.Columns[i].Width;
            }
        }
    }
}
