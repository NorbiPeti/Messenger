﻿using Khendys.Controls;
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
        public Control[] SubItems { get; set; }
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
        private bool selected = false;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (value)
                {
                    Parent.SelectedIndex = Parent.Items.IndexOf(this);
                    this.BackColor = Parent.SelectionColor;
                }
                else
                    this.BackColor = Parent.BackColor;
                selected = value;
            }
        }
        public RichListViewItem()
        {
            Action func = RichListViewItem2;
            func();
        }
        public RichListViewItem(Control[] subitems)
        {
            Action<Control[]> func = RichListViewItem2;
            func(subitems);
        }
        public RichListViewItem(int colnum)
        {
            Action<int> func = RichListViewItem2;
            func(colnum);
            SubItems = new Control[colnum];
        }

        public void RichListViewItem2(int colnum)
        {
            SubItems = new Control[colnum];
        }
        public void RichListViewItem2()
        {
        }
        public void RichListViewItem2(Control[] subitems)
        {
            SubItems = subitems;
        }
        internal void _CreateInstance()
        {
            Action func = _CreateInstance2;
            func();
        }
        internal void _CreateInstance2()
        {
            if (SubItems == null)
                SubItems = new Control[Parent.Columns.Length];
            for (int i = 0; i < Parent.Columns.Length; i++)
            {
                if (SubItems[i] == null)
                {
                    SubItems[i] = new ExRichTextBox();
                    SubItems[i].Cursor = Cursors.Arrow;
                }
                //Console.WriteLine("this.Parent=" + this.Parent);
                //Console.WriteLine("SubItems[i].Parent=" + SubItems[i].Parent);
                SubItems[i].Parent = this.Parent;
                if (SubItems[i].GetType().IsSubclassOf(typeof(TextBoxBase))) //If it's a TextBox, set it to read-only
                {
                    ((TextBoxBase)SubItems[i]).ReadOnly = true;
                    ((TextBoxBase)SubItems[i]).BorderStyle = BorderStyle.None;
                    //((TextBoxBase)SubItems[i]).
                }
                /*if (SubItems[i].GetType()==typeof(ExExRichTextBox))
                {
                    ((ExExRichTextBox)SubItems[i]).SetTransparent(true);
                    //SubItems[i] = ((ExExRichTextBox)SubItems[i]).SetTransparent(true);
                }*/
                bool success = true;
                Control parent = this.Parent;
                do
                {
                    success = true;
                    try
                    {
                        this.SubItems[i].BackColor = parent.BackColor;
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("RichListView: Transparency is not supported. Using container BackColor.");
                        this.SubItems[i].BackColor = this.Parent.Parent.BackColor;
                        success = false;
                        parent = parent.Parent;
                    }
                } while (!success);
                this.SubItems[i].ForeColor = this.Parent.ForeColor;
                SubItems[i].Click += _ItemClicked;
                SubItems[i].DoubleClick += _ItemDoubleClicked;
                SubItems[i].MouseClick += _ItemMouseClicked;
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

        private void _ItemMouseClicked(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                Parent._ItemRightClicked(this);
        }

        internal void Show()
        {
            if (Parent == null)
                throw new NoParentException("Try to reference ListViewItem which is not assigned to a ListView control.");
            for (int i = 0; i < SubItems.Length; i++)
                SubItems[i].Show();
        }

        internal void Hide()
        {
            if (Parent == null)
                throw new NoParentException("Try to reference ListViewItem which is not assigned to a ListView control.");
            for (int i = 0; i < SubItems.Length; i++)
                SubItems[i].Hide();
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

        internal void Remove()
        {
            for (int i = 0; i < SubItems.Length; i++)
                SubItems[i].Dispose();
        }
    }/*
    public class ExExRichTextBox : ExRichTextBox
    {
        public ExExRichTextBox()
        {
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
        public void SetTransparent(bool value)
        {
            var tmp = new ExExRichTextBox();
            Console.WriteLine("SetTransparent");
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, value);
            //tmp.SetStyle(ControlStyles.SupportsTransparentBackColor, value);
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
            Console.WriteLine("New value: " + this.GetStyle(ControlStyles.SupportsTransparentBackColor));
            //return tmp;
        }
    }*/
}
