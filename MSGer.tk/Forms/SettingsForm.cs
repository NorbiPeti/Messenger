using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
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
            this.Text = Language.Translate(Language.StringID.Settings);

            ActivePanel = new SettingsPanelPersonal(); //2015.05.23.
            ShownPanels.Add(ActivePanel); //2015.05.23.
            glacialList1.Items.Add(Language.Translate(Language.StringID.Settings_Personal));
            glacialList1.Items.Add(Language.Translate(Language.StringID.Settings_Layout));
            glacialList1.Items.Add(Language.Translate(Language.StringID.Settings_Packs));
            glacialList1.Items.Add(Language.Translate(Language.StringID.Settings_Network));
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
                    SetActivePanel<SettingsPanelPersonal>(); //2015.05.23.
                    break;
                case 1:
                    //Kinézet
                    SetActivePanel<SettingsPanelLayout>(); //2015.05.23.
                    break;
                case 2:
                    //Csomagok
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
            bool reopen = false;
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
