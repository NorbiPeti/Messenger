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
    public partial class SettingsPanelPacks : SettingsPanel
    {
        public SettingsPanelPacks()
        {
            InitializeComponent();

            packs.Text = Language.Translate(Language.StringID.Settings_Packs);
            Scripterbtn.Text = Language.Translate(Language.StringID.Scripter);
        }

        public override bool SaveSettings()
        {
            return false;
        }

        private void Scripterbtn_Click(object sender, EventArgs e)
        {
            new ScripterWindow().Show();
        }

        private void themedesignerbtn_Click(object sender, EventArgs e)
        { //2015.05.23.
            new ThemeDesigner().Show();
        }
    }
}
