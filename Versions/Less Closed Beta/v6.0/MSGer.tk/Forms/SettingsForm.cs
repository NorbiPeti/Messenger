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
            //listView1.Columns[0].Width = listView1.Width;
            this.Text = Language.Translate(Language.StringID.Settings);

            ActivePanel = new SettingsPanelPersonal(); //2015.05.23.
            ShownPanels.Add(ActivePanel); //2015.05.23.
            /*glacialList1.Items[0].Text = Language.Translate(Language.StringID.Settings_Personal);
            glacialList1.Items[1].Text = Language.Translate(Language.StringID.Settings_Layout);
            glacialList1.Items[2].Text = Language.Translate(Language.StringID.Settings_Packs); //2015.05.23.*/
            glacialList1.Items.Add(Language.Translate(Language.StringID.Settings_Personal));
            glacialList1.Items.Add(Language.Translate(Language.StringID.Settings_Layout));
            glacialList1.Items.Add(Language.Translate(Language.StringID.Settings_Packs));
            glacialList1.Items.Add(Language.Translate(Language.StringID.Settings_Network));

            /*layout.Text = Language.Translate(Language.StringID.Settings_Layout); //2014.10.28.
            label1.Text = Language.Translate(Language.StringID.Name);
            label2.Text = Language.Translate(Language.StringID.Message);
            label3.Text = Language.Translate(Language.StringID.Language);
            chatwindow.Text = Language.Translate(Language.StringID.Settings_ChatWindow); //2014.10.28.
            chatwindowTabs.Text = Language.Translate(Language.StringID.Settings_ChatWindowTabs); //2014.10.28.

            scripts.Text = Language.Translate(Language.StringID.Settings_Scripts);
            Scripterbtn.Text = Language.Translate(Language.StringID.Scripter);

            nameText.Text = CurrentUser.Name;
            messageText.Text = CurrentUser.Message;
            chatwindow.Checked = (Storage.Settings[SettingType.ChatWindow] == "1"); //2014.10.28.
            //isserver.Enabled = false; //2015.01.12.

            //foreach (var entry in Language.UsedLangs)
            foreach(var entry in Language.Languages)
            {
                //listView1.Items.Add(Language.UsedLangs[entry.Key].Strings["currentlang"], Language.UsedLangs[entry.Key].Strings["currentlang"], 0);
                listView1.Items.Add(entry.Strings["currentlang"], entry.Strings["currentlang"], 0); //2015.05.16.
                //if (Language.UsedLangs[entry.Key].Equals(Language.CurrentLanguage))
                //if (entry.Key == Language.CurrentLanguage.ToString()) //2015.04.03.
                if (entry == Language.CurrentLanguage) //2015.05.16.
                    listView1.Items[listView1.Items.Count - 1].Selected = true;
            }*/
        }

        private List<SettingsPanel> ShownPanels = new List<SettingsPanel>();
        private void glacialList1_Click(object sender, EventArgs e)
        {
            int tmp = glacialList1.HotItemIndex;
            if (tmp > glacialList1.Items.Count)
                return;
            switch (tmp)
            {
                case 0:
                    //Személyes
                    //panel1.ScrollControlIntoView(personal);
                    //ActivePanel.Dispose();
                    SetActivePanel<SettingsPanelPersonal>(); //2015.05.23.
                    break;
                case 1:
                    //Kinézet
                    //panel1.ScrollControlIntoView(layout);
                    SetActivePanel<SettingsPanelLayout>(); //2015.05.23.
                    break;
                case 2:
                    //Csomagok
                    //panel1.ScrollControlIntoView(scripts);
                    SetActivePanel<SettingsPanelPacks>(); //2015.05.23.
                    break;
                case 3:
                    //Hálózat
                    SetActivePanel<SettingsPanelNetwork>(); //2015.05.24.
                    break;
            }
        }

        private void SetActivePanel<T>() where T : SettingsPanel
        {
            T panel = null; //2015.05.23.
            /*try
            {
                panel = ShownPanels.Single(entry => entry.GetType() == typeof(T)) as T;
            }
            catch { }*/
            panel = ShownPanels.FirstOrDefault(entry => entry.GetType() == typeof(T)) as T; //Single-->FirstOrDefault: 2015.06.06.
            if (panel == null)
            { //2015.05.23.
                ActivePanel = Activator.CreateInstance<T>();
                ShownPanels.Add(ActivePanel);
            }
            else
                ActivePanel = panel;
        }

        private SettingsPanel activepanel;
        private SettingsPanel ActivePanel
        { //2015.05.23.
            get
            {
                return activepanel;
            }
            set
            {
                if (activepanel != null)
                    activepanel.Hide();
                activepanel = value;
                panel1.Controls.Add(activepanel);
                activepanel.Dock = DockStyle.Fill;
                activepanel.Show();
            }
        }
        private void okbtn_Click(object sender, EventArgs e)
        {
            ApplyingSettings = true;
            //CurrentUser.Name = nameText.Text;
            //CurrentUser.Message = messageText.Text;
            bool reopen = false;
            //string lang = "en";
            //var langs = Language.Languages;
            //Language lang = Language.CurrentLanguage;
            /*if(listView1.SelectedItems.Count!=0) //2014.10.28. - Eddig valószínűleg hiba történt a SelectedItems[0]-nál
            {
                //foreach (var lng in Language.UsedLangs)
                foreach (var lng in langs)
                {
                    if (lng.Strings.ContainsKey("currentlang") && listView1.SelectedItems[0].Text == lng.Strings["currentlang"])
                    {
                        //lang = lng.Key;
                        lang = lng;
                        break;
                    }
                }
                //if (Storage.Settings["lang"] != lang)
                if (Language.CurrentLanguage != lang)
                {
                    //Storage.Settings["lang"] = lang;
                    Language.CurrentLanguage = lang;
                    Language.ReloadLangs();
                }
            }*/
            foreach (SettingsPanel panel in ShownPanels) //2015.05.23.
                if (panel.SaveSettings()) //2015.05.23.
                    reopen = true; //2015.05.23.
            if (reopen)
            {
                ChatPanel.ReopenChatWindows(true);
                Program.MainF.ChangeChatWindowLayout(true); //2015.06.14.
            }
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
