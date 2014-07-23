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
            listView1.Columns[0].Width = listView1.Width;
            this.Text = Language.GetCuurentLanguage().Strings["settings"];
            label1.Text = Language.GetCuurentLanguage().Strings["name"];
            label2.Text = Language.GetCuurentLanguage().Strings["message"];
            label3.Text = Language.GetCuurentLanguage().Strings["language"];
            nameText.Text = CurrentUser.Name;
            messageText.Text = CurrentUser.Message;

            foreach(var entry in Language.UsedLangs)
            {
                listView1.Items.Add(Language.UsedLangs[entry.Key].Strings["currentlang"], Language.UsedLangs[entry.Key].Strings["currentlang"], 0);
                if (Language.UsedLangs[entry.Key].Equals(Language.GetCuurentLanguage()))
                    listView1.Items[listView1.Items.Count - 1].Selected = true;
            }
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
                    panel1.ScrollControlIntoView(personal);
                    break;
            }
        }

        private void okbtn_Click(object sender, EventArgs e)
        {
            string lang="en";
            foreach (var lng in Language.UsedLangs)
			{
                if(listView1.SelectedItems[0].Text==lng.Value.Strings["currentlang"])
                {
                    lang = lng.Key;
                    break;
                }
			}
            string result = Networking.SendRequest("updatesettings", nameText.Text + "ͦ" + messageText.Text, 0, true);
            if (result != "Success")
                MessageBox.Show(Language.GetCuurentLanguage().Strings["error"] + ": " + result);
            else
            {
                if (Settings.Default.lang != lang)
                {
                    Settings.Default.lang = lang;
                    Settings.Default.Save();
                    MessageBox.Show(Language.GetCuurentLanguage().Strings["restart_needed"]);
                    System.Diagnostics.Process.Start("msger.tk.exe");
                    Environment.Exit(0);
                }
                this.Close();
            }
        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
