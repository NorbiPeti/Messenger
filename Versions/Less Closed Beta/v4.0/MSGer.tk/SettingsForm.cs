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
    public partial class SettingsForm : ThemedForms
    {
        public static bool ApplyingSettings = false;
        public SettingsForm()
        {
            InitializeComponent();
            listView1.Columns[0].Width = listView1.Width;
            this.Text = Language.Translate("settings");
            //Language.Translate(this, "settings");

            glacialList1.Items[0].Text = Language.Translate("settings_personal");
            personal.Text = Language.Translate("settings_personal");
            glacialList1.Items[1].Text = Language.Translate("settings_layout");

            layout.Text = Language.Translate("settings_layout"); //2014.10.28.
            label1.Text = Language.Translate("name");
            label2.Text = Language.Translate("message");
            label3.Text = Language.Translate("language");
            chatwindow.Text = Language.Translate("settings_chatwindow"); //2014.10.28.
            chatwindowTabs.Text = Language.Translate("settings_chatwindowTabs"); //2014.10.28.
            //isserver.Text = Language.Translate("settings_isserver"); //2014.11.15.

            nameText.Text = CurrentUser.Name;
            messageText.Text = CurrentUser.Message;
            chatwindow.Checked = (Storage.Settings["chatwindow"] == "1"); //2014.10.28.
            //isserver.Checked = (Storage.Settings["isserver"] == "1"); //2014.11.15.
            isserver.Enabled = false; //2015.01.12.

            foreach (var entry in Language.UsedLangs)
            {
                listView1.Items.Add(Language.UsedLangs[entry.Key].Strings["currentlang"], Language.UsedLangs[entry.Key].Strings["currentlang"], 0);
                if (Language.UsedLangs[entry.Key].Equals(Language.GetCurrentLanguage()))
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
                case 1:
                    //Kinézet
                    panel1.ScrollControlIntoView(layout);
                    break;
            }
        }

        private void okbtn_Click(object sender, EventArgs e)
        {
            ApplyingSettings = true;
            CurrentUser.Name = nameText.Text;
            CurrentUser.Message = messageText.Text;
            bool reopen = false;
            if (chatwindow.Checked && Storage.Settings["chatwindow"] == "0")
            {
                reopen = true;
                Storage.Settings["chatwindow"] = "1";
            }
            else if (!chatwindow.Checked && Storage.Settings["chatwindow"] == "1")
            {
                reopen = true;
                Storage.Settings["chatwindow"] = "0";
            }
            //Storage.Settings["isserver"] = isserver.Checked ? "1" : "0"; //2014.11.15.
            string lang = "en";
            if(listView1.SelectedItems.Count!=0) //2014.10.28. - Eddig valószínűleg hiba történt a SelectedItems[0]-nál
            {
                foreach (var lng in Language.UsedLangs)
                {
                    if (lng.Value.Strings.ContainsKey("currentlang") && listView1.SelectedItems[0].Text == lng.Value.Strings["currentlang"])
                    {
                        lang = lng.Key;
                        break;
                    }
                }
                if (Storage.Settings["lang"] != lang)
                {
                    Storage.Settings["lang"] = lang;
                    //MessageBox.Show(Language.Translate("restart_needed"));
                    //Program.Restart(true);
                    Language.ReloadLangs();
                }
            }
            if (reopen)
                ChatPanel.ReopenChatWindows(true);
            ApplyingSettings = false;
            this.Close();
        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.SettingsF = null;
        }
    }
}
