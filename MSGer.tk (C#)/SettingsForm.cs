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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            this.Text = Language.GetCuurentLanguage().Strings["settings"];
            nameText.Text = CurrentUser.Name;
            messageText.Text = CurrentUser.Message;
        }

        private void glacialList1_Click(object sender, EventArgs e)
        {
            int tmp = glacialList1.HotItemIndex;
            if (tmp > glacialList1.Items.Count)
                return;
            switch(tmp)
            {
                case 0:
                    //Személyes
                    //MessageBox.Show("Személyes...");
                    panel1.ScrollControlIntoView(personal);
                    break;
            }
        }

        private void okbtn_Click(object sender, EventArgs e)
        {
            string result = Networking.SendRequest("updatesettings", nameText.Text + "ͦ" + messageText.Text, 0, true);
            if (result != "Success")
                MessageBox.Show(Language.GetCuurentLanguage().Strings["error"] + ": " + result);
            else
                this.Close();
        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
