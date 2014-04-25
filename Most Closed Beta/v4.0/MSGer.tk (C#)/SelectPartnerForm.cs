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
    public partial class SelectPartnerForm : Form
    {
        public SelectPartnerForm()
        {
            InitializeComponent();
            //this.Text = MainForm.SelectPartnerSender.ToString();
            //titleText.Text = MainForm.SelectPartnerSender.ToString();
            this.Text = MainForm.SelectPartnerSender.Text; //2014.02.28.
            titleText.Text = MainForm.SelectPartnerSender.Text;
            
        }
    }
}
