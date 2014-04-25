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
    public partial class ChatForm : Form
    {/* 2014.03.07. A partnerinformáció birtokolja a chatablakot, és nem forditva
        public int ChatPartner
        {
            get;
            set;
        }*/
        public ChatForm()
        {
            InitializeComponent();
            //Amint létrehozom, ez a kód lefut - Nem számit, hogy megjelenik-e
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(ChatPartner + "");
            //ChatPartner = 2;
            //MessageBox.Show(ChatPartner + "");
        }
    }
}
