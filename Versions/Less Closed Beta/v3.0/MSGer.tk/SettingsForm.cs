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
            this.Text = Language.Translate("settings");
            glacialList1.Items[0].Text = Language.Translate("settings_personal");
            personal.Text = Language.Translate("settings_personal");
            label1.Text = Language.Translate("name");
            label2.Text = Language.Translate("message");
            label3.Text = Language.Translate("language");
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
            string lang = "en";
            foreach (var lng in Language.UsedLangs)
            {
                if (lng.Value.Strings.ContainsKey("currentlang") && listView1.SelectedItems[0].Text == lng.Value.Strings["currentlang"])
                {
                    lang = lng.Key;
                    break;
                }
            }
            CurrentUser.Name = nameText.Text;
            CurrentUser.Message = messageText.Text;
            if (Storage.Settings["lang"] != lang)
            {
                Storage.Settings["lang"] = lang;
                MessageBox.Show(Language.Translate("restart_needed"));
                Program.Restart(true);
            }
            this.Close();
        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
