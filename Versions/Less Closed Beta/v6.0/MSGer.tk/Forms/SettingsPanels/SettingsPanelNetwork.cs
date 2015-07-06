using System;
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
    public partial class SettingsPanelNetwork : SettingsPanel
    { //2015.05.24.
        public SettingsPanelNetwork()
        {
            InitializeComponent();
            network.Text = Language.Translate(Language.StringID.Settings_Network);
            portNum.Value = CurrentUser.Port;
        }

        public override bool SaveSettings()
        {
            CurrentUser.Port = (int)portNum.Value;
            return false;
        }
    }
}
