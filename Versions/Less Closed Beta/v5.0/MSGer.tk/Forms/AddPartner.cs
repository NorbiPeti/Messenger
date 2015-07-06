using GlacialComponents.Controls;
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
    public partial class AddPartner : ThemedForms
    {
        public AddPartner()
        {
            InitializeComponent();
            this.Text = Language.Translate(Language.StringID.AddContact);
            label1.Text = Language.Translate(Language.StringID.AddContact);
            label2.Text = Language.Translate(Language.StringID.AddContact_NameEmail);
            searchbtn.Text = Language.Translate(Language.StringID.AddContact_Search);
            glacialList1.Columns[0].Text = Language.Translate(Language.StringID.UserName);
            gobtn.Text = Language.Translate(Language.StringID.AddContact_Add);
        }

        List<UserInfo> FoundUsers = new List<UserInfo>();

        private void searchbtn_Click(object sender, EventArgs e)
        {
            glacialList1.Items.Clear();
            foreach (var tmp in UserInfo.KnownUsers)
            {
                if ((tmp.UserName.Contains(nameText.Text) || tmp.Name.Contains(nameText.Text) || tmp.UserID.ToString() == nameText.Text) && !FoundUsers.Contains(tmp))
                {
                    glacialList1.Items.Add(tmp.UserName);

                    FoundUsers.Add(tmp);
                }
            }
        }

        private void glacialList1_Click(object sender, EventArgs e)
        {
            int item = glacialList1.HotItemIndex;
            if (item >= glacialList1.Items.Count)
                return;
            //2014.04.18. - Partnerinformáció mutatása
            //2014.08.16. - Megvalósítás
            if (FoundUsers.Count < item)
                (new PartnerInformation(FoundUsers[item])).ShowDialog();
        }

        private void gobtn_Click(object sender, EventArgs e)
        {
            if (glacialList1.SelectedItems.Count == 0 || FoundUsers.Count == 0)
                return;
            string username = ((GLItem)glacialList1.SelectedItems[0]).Text;
            string response = Networking.SendRequest(Networking.RequestType.AddUser, username, 0, true);
            if (response == "Success")
                MessageBox.Show("Felhasználó felvéve az ismerőseid közé.");
            else
                MessageBox.Show("Nem sikerült felvenni a felhasználót az ismerőseid közé.\nLehet, hogy már felvetted, vagy valami hiba történt.\n(" + response + ")");
        }
    }
}
