using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SzNPProjects;

namespace Test_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            /*var tmp = new RichListView();
            tmp.ItemHeight = 50;
            tmp.Columns.Add(new RichListViewColumn());
            tmp.Columns[0].Width = 100;
            Console.WriteLine("Main call");
            tmp.Items.Add(new RichListViewItem());
            this.Controls.Add(tmp);
            tmp.Show();*/

            //var col = new RichListViewColumn();
            //col.Width = 50;
            //richListView1.Columns.Add(col);
            var item=new RichListViewItem();
            //item.Control.Text = "Test :P";
            richListView1.Items.Add(item);
            richListView1.Items[0].SubItems[0].Text = "Test :P";
            richListView1.Items[0].SubItems[1].Text = "Second col";
            richListView1.Items.Add(new RichListViewItem());
            richListView1.Items[1].SubItems[0].Text = "Only one col of text";
            richListView1.ItemClicked += richListView1_ItemClicked;
            richListView1.ItemDoubleClicked += richListView1_ItemDoubleClicked;
        }

        void richListView1_ItemDoubleClicked(object sender, int e)
        {
            MessageBox.Show(e + 1 + ". item double clicked");
        }

        void richListView1_ItemClicked(object sender, int e)
        {
            //MessageBox.Show(e + 1 + ". item clicked");
        }
    }
}
