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
    public partial class AddPartner : Form
    {
        public AddPartner()
        {
            InitializeComponent();
            this.Text = Language.Translate("addcontact");
            label1.Text = Language.Translate("addcontact");
            label2.Text = Language.Translate("addcontact_nameemail");
            searchbtn.Text = Language.Translate("addcontact_search");
            glacialList1.Columns[0].Text = Language.Translate("reg_username");
            gobtn.Text = Language.Translate("addcontact_add");
        }

        /*string[] username;
        int[] uid;
        string[] message;
        int[] online;
        string[] shownname;
        string[] email;*/
        List<UserInfo> FoundUsers = new List<UserInfo>();

        private void searchbtn_Click(object sender, EventArgs e)
        {
            glacialList1.Items.Clear();
            //string[] people = Networking.SendRequest("findpeople", nameText.Text, 0, true).Split('ͦ'); //2014.04.18. 0:07 - Még nem készítettem el
            /*byte[][] tmpret = Networking.SendUpdate(Networking.UpdateType.FindPeople, Encoding.Unicode.GetBytes(nameText.Text), false);
            if (tmpret == null)
            {
                glacialList1.Items.Add(Language.Translate("networking_alone"));
                return;
            }
            for (int i = 0; i < tmpret.Length; i++)
            {
                byte[] tmpb = tmpret[i];
                string tmpstr = Encoding.Unicode.GetString(tmpb, 1, tmpb.Length - 1); //0. byte==UserID
                string[] people = tmpstr.Split('ͦ');
                for (int x = 0, y = 0; x + 5 < people.Length; x += 6, y++)
                {*/
                    /*username = new string[people.Length / 6];
                    uid = new int[people.Length / 6];
                    message = new string[people.Length / 6];
                    online = new int[people.Length / 6];
                    shownname = new string[people.Length / 6];
                    email = new string[people.Length / 6];*/

                    //username[y] = people[x];
                    /*var tmp = new UserInfo();
                    tmp.UserName = people[x];
                    tmp.UserID = Int32.Parse(people[x + 1]);
                    tmp.Message = people[x + 2];
                    tmp.State = Int32.Parse(people[x + 3]);
                    tmp.Name = people[x + 4];
                    tmp.Email = people[x + 5];

                    if (!FoundUsers.Contains(tmp)) //2014.08.19.
                    {

                        //glacialList1.Items.Add(username[y]);
                        glacialList1.Items.Add(tmp.UserName);

                        FoundUsers.Add(tmp); //2014.08.16.
                    }
            }
            }*/
            //2014.09.09. - Miután először az egészet megcsináltam, néhány nappal később az egészet elölről kezdem
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
            //(new PartnerInformation(FoundUsers[item].Name, FoundUsers[item].State, FoundUsers[item].Message, FoundUsers[item].UserName, FoundUsers[item].UserID, FoundUsers[item].Email)).ShowDialog(); //2014.08.16.
            if (FoundUsers.Count < item)
                (new PartnerInformation(FoundUsers[item])).ShowDialog();
        }

        private void gobtn_Click(object sender, EventArgs e)
        {
            if (glacialList1.SelectedItems.Count == 0 || FoundUsers.Count == 0)
                return;
            string username = ((GLItem)glacialList1.SelectedItems[0]).Text;
            string response = Networking.SendRequest("adduser", username, 0, true);
            //if (Networking.SendRequest("adduser", username, 0, true) == "Success")
            if (response == "Success")
                MessageBox.Show("Felhasználó felvéve az ismerőseid közé.");
            else
                MessageBox.Show("Nem sikerült felvenni a felhasználót az ismerőseid közé.\nLehet, hogy már felvetted, vagy valami hiba történt.\n(" + response + ")");
        }
    }
}
