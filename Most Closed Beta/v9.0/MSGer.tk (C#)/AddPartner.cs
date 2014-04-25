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
        }

        string[] username;
        int[] uid;
        string[] message;
        int[] online;
        string[] shownname;
        string[] email;

        private void searchbtn_Click(object sender, EventArgs e)
        {
            glacialList1.Items.Clear();
            string[] people = Networking.SendRequest("findpeople", nameText.Text, 0, true).Split('ͦ'); //2014.04.18. 0:07 - Még nem készítettem el
            for (int x = 0, y = 0; x + 5 < people.Length; x += 6, y++)
            {
                username = new string[people.Length / 6];
                uid = new int[people.Length / 6];
                message = new string[people.Length / 6];
                online = new int[people.Length / 6];
                shownname = new string[people.Length / 6];
                email = new string[people.Length / 6];

                username[y] = people[x];
                uid[y] = Int32.Parse(people[x + 1]);
                message[y] = people[x + 2];
                online[y] = Int32.Parse(people[x + 3]);
                shownname[y] = people[x + 4];
                email[y] = people[x + 5];

                glacialList1.Items.Add(username[y]);
            }
        }

        private void glacialList1_Click(object sender, EventArgs e)
        {
            int item = glacialList1.HotItemIndex;
            if (item >= glacialList1.Items.Count)
                return;
            //2014.04.18. - Partnerinformáció mutatása
        }

        private void gobtn_Click(object sender, EventArgs e)
        {
            if (glacialList1.SelectedItems.Count == 0)
                return;
            //MessageBox.Show(glacialList1.SelectedItems[0].ToString());
            string username = ((GLItem)glacialList1.SelectedItems[0]).Text;
            string response = Networking.SendRequest("adduser", username, 0, true);
            if (Networking.SendRequest("adduser", username, 0, true) == "Success")
                MessageBox.Show("Felhasználó felvéve az ismerőseid közé.");
            else
                MessageBox.Show("Nem sikerült felvenni a felhasználót az ismerőseid közé.\nLehet, hogy már felvetted, vagy valami hiba történt.\n(" + response + ")");
        }
    }
}
