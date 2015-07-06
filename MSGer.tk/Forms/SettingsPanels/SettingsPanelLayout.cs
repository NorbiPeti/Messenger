using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    public partial class SettingsPanelLayout : SettingsPanel
    { //2015.05.23.
        public SettingsPanelLayout()
        {
            InitializeComponent();

            layout.Text = Language.Translate(Language.StringID.Settings_Layout);
            chatwindow.Text = Language.Translate(Language.StringID.Settings_ChatWindow);
            chatwindowTabs.Text = Language.Translate(Language.StringID.Settings_ChatWindowTabs);
            chatwindow.Checked = (Storage.Settings[SettingType.ChatWindow] == "1");
        }

        /// <summary>
        /// SaveSettings
        /// </summary>
        /// <returns>reopen</returns>
        public override bool SaveSettings()
        {
            if (chatwindow.Checked && Storage.Settings[SettingType.ChatWindow] == "0")
            {
                Storage.Settings[SettingType.ChatWindow] = "1";
                return true;
            }
            else if (!chatwindow.Checked && Storage.Settings[SettingType.ChatWindow] == "1")
            {
                Storage.Settings[SettingType.ChatWindow] = "0";
                return true;
            }
            return false;
        }
    }
}
