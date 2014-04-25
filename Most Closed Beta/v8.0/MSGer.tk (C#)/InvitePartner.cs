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
    public partial class InvitePartner : Form
    {
        //string[] Codes=new string[1024];
        //List<string> Codes = new List<string>();
        public InvitePartner()
        {
            InitializeComponent();
        }

        private void InvitePartner_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            string res = Networking.SendRequest("addcode", "", 0, true);
            if (res.Length<"Fail".Length || res.Substring(0, "Fail".Length) == "Fail")
                MessageBox.Show("A kódgenerálás sikertelen.\n\n" + res, "Hiba");
            else
                RefreshList();
        }

        public void RefreshList()
        {
            listBox1.Items.Clear();
            string[] response = Networking.SendRequest("getcodes", "", 0, true).Split('ͦ');
            int x = 0;
            for (int i = 0; i+1 < response.Length; i += 2)
            {
                if (Int32.Parse(response[i + 1]) == 1)
                {
                    if (!hideAccepted.Checked)
                        listBox1.Items.Add("Elfogadott - " + response[i]);
                }
                else if (Int32.Parse(response[i + 1]) == 0)
                    listBox1.Items.Add("Visszaigazolásra vár - " + response[i]);
                else MessageBox.Show("Hiba:\n" + response[i] + "\n" + response[i + 1]);
                //Codes[x] = response[i];
                x++;
            }
        }

        private void removebtn_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;
            //MessageBox.Show(listBox1.Items[listBox1.SelectedIndex].ToString());
            //string res = Networking.SendRequest("remcode", Codes[listBox1.SelectedIndex], 0, true);
            string res = Networking.SendRequest("remcode", listBox1.SelectedItem.ToString().Remove(0, listBox1.SelectedItem.ToString().IndexOf(" - ") + " - ".Length), 0, true);
            if (res.Substring(0, "Fail".Length) == "Fail")
                MessageBox.Show("A kód törlése sikertelen.\n\n" + res, "Hiba");
            //else listBox1.Items.Add(res);
            else RefreshList();
        }

        private void copybtn_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;
            //Clipboard.SetText(Codes[listBox1.SelectedIndex]);
            Clipboard.SetText(listBox1.SelectedItem.ToString().Remove(0, listBox1.SelectedItem.ToString().IndexOf(" - ") + " - ".Length));
        }

        private void hideAccepted_CheckedChanged(object sender, EventArgs e)
        {
            RefreshList();
        }
    }
}
