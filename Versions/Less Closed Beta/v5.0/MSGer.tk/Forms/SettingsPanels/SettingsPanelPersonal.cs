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
    public partial class SettingsPanelPersonal : SettingsPanel
    {
        public static bool ApplyingSettings = false;
        public SettingsPanelPersonal()
        {
            InitializeComponent();
            languageList.Columns[0].Width = languageList.Width;
            this.Text = Language.Translate(Language.StringID.Settings);
            personal.Text = Language.Translate(Language.StringID.Settings_Personal);

            nameLabel.Text = Language.Translate(Language.StringID.Name);
            messageLabel.Text = Language.Translate(Language.StringID.Message);
            languageLabel.Text = Language.Translate(Language.StringID.Language);
            nameText.Text = CurrentUser.Name;
            messageText.Text = CurrentUser.Message;

            foreach(var entry in Language.Languages)
            {
                //listView1.Items.Add(Language.UsedLangs[entry.Key].Strings["currentlang"], Language.UsedLangs[entry.Key].Strings["currentlang"], 0);
                languageList.Items.Add(entry.Strings["currentlang"], entry.Strings["currentlang"], 0); //2015.05.16.
                //if (Language.UsedLangs[entry.Key].Equals(Language.CurrentLanguage))
                //if (entry.Key == Language.CurrentLanguage.ToString()) //2015.04.03.
                if (entry == Language.CurrentLanguage) //2015.05.16.
                    languageList.Items[languageList.Items.Count - 1].Selected = true;
            }
        }

        public override bool SaveSettings()
        {
            CurrentUser.Name = nameText.Text;
            CurrentUser.Message = messageText.Text;
            var langs = Language.Languages;
            Language lang = Language.CurrentLanguage;
            if(languageList.SelectedItems.Count!=0)
            {
                foreach (var lng in langs)
                {
                    if (lng.Strings.ContainsKey("currentlang") && languageList.SelectedItems[0].Text == lng.Strings["currentlang"])
                    {
                        lang = lng;
                        break;
                    }
                }
                if (Language.CurrentLanguage != lang)
                {
                    Language.CurrentLanguage = lang;
                    Language.ReloadLangs();
                }
            }
            return false;
        }
    }
}
