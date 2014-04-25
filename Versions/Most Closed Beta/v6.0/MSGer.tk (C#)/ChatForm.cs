using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    public partial class ChatForm : Form
    {/* 2014.03.07. A partnerinformáció birtokolja a chatablakot, és nem forditva; bár a partnerinformáció indexszáma változhat
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
        
        private void SendMessage(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || e.Shift)
                return;
            messageTextBox.ReadOnly = true;
            //Networking.SendRequest("sendmessage", messageTextBox.Text, 2); //Még nincs kész a PHP - 2014.03.08. 0:01
            /*
             * 2014.03.08. 0:03
             * A fenti kódra válaszul a másik felhasználó esetleges új válaszát is irja be; tehát frissitse az üzeneteket
             * Az üzenetellenőrző thread folyamatosan fusson, amint végrehajtotta a parancsokat, kezdje újra (nincs Thread.Sleep)
             */
            recentMsgTextBox.AppendText("Üzenet:\n");
            recentMsgTextBox.AppendText(messageTextBox.Text + "\n");
            messageTextBox.Focus();
            messageTextBox.Text = "";
            messageTextBox.ReadOnly = false;
        }

        private void MessageTextChanged(object sender, EventArgs e)
        {
            if (messageTextBox.Text == "\n")
                messageTextBox.Text = "";
        }

        private void OpenLink(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }
    }
}
