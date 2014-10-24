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
            //string result = Networking.SendRequest("updatesettings", nameText.Text + "ͦ" + messageText.Text, 0, true);
            //if (result != "Success")
            //MessageBox.Show(Language.Translate("error"] + ": " + result);
            //List<byte> bytes = new List<byte>();
            //bytes.AddRange(Encoding.Unicode.GetBytes(CurrentUser.UserID + "ͦ" + nameText.Text + "ͦ" + messageText.Text));
            //byte[][] result = Networking.SendUpdate(Networking.UpdateType.UpdateSettings, bytes.ToArray(), false);
            CurrentUser.Name = nameText.Text;
            CurrentUser.Message = messageText.Text;
            /*List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(CurrentUser.UserID));
            bytes.AddRange(BitConverter.GetBytes(CurrentUser.Name.Length));
            bytes.AddRange(Encoding.Unicode.GetBytes(CurrentUser.Name));
            bytes.AddRange(BitConverter.GetBytes(CurrentUser.Name.Length));
            bytes.AddRange(Encoding.Unicode.GetBytes(CurrentUser.Message));
            byte[][] result = Networking.SendUpdate(Networking.UpdateType.ListUpdate, bytes.ToArray(), false);
            if (result==null || !result[0].All(b => b == 0x00)) //Ha nincs online felhasználó, akkor is továbbhalad
            {*/ //Azért nem kell ez az egész fentebbi rész, mert minden egyes változást elküld mindenkinek
            if (Storage.Settings["lang"] != lang)
            {
                Storage.Settings["lang"] = lang;
                //Settings.Default.Save();
                MessageBox.Show(Language.Translate("restart_needed"));
                //System.Diagnostics.Process.Start("msger.tk.exe");
                //Environment.Exit(0);
                //Program.Exit();
                Program.Restart(true);
            }
            this.Close();
            /*}
            else //Ha az összes ismert címről hibajelentés érkezik, jelezze a hibát
                MessageBox.Show(Language.Translate("error"));*/
        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
