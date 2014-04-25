using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using GlacialComponents.Controls;

namespace MSGer.tk
{
    public partial class MainForm : Form
    {
        private LoginForm LoginDialog;
        //public static object SelectPartnerSender = null;
        public static ToolStripMenuItem SelectPartnerSender = null;
        public MainForm()
        {
            InitializeComponent();
            this.Hide();
            LoginDialog = new LoginForm();
            LoginDialog.ShowDialog();
            //Nézzük, sikerült-e
            if (UserInfo.UserID == 0)
                Close();
            this.Show();

            string[] row = Networking.SendRequest("getlist", "", 3).Split('-'); //Lekéri a listát, és különválogatja egyben - 2014.02.28.
            for (int x = 0; x < row.Length; x+=3)
            {
                string state = "";
                MessageBox.Show("0: " + row[x] + " 1: " + row[x + 1] + " 2: " + row[x + 2]);
                if (row[x + 2] == "1")
                    state = " (Elérhető)";
                if (row[x + 2] == "2")
                    state = " (Elfoglalt)";
                if (row[x + 2] == "3")
                    state = " (Nincs a gépnél)";
                var listViewItem = new GLItem(contactList);
                //listViewItem.SubItems.Add(row[x]);
                //listViewItem.SubItems.Add(row[x + 1] + state); //1. oszlop: Kép; 2. oszlop: Név - Üzenet
                listViewItem.SubItems[1].Text = row[x] + " " + state + " - " + row[x + 1];
                contactList.Items.Add(listViewItem);
                /*if (Convert.ToInt32(row[x + 2]) != 0 && Convert.ToInt32(row[x + 2]) != 4)
                    listViewItem.Group = listView1.Groups["listViewGroup1"];
                else
                    listViewItem.Group = listView1.Groups["listViewGroup2"];
                listView1.Items.Add(listViewItem);*/
                this.Show();
                if (x + 4 >= row.Length)
                    break;
            }
        }

        private void LogoutUser(object sender, EventArgs e)
        {
            this.Hide();
            UserInfo.UserID = 0;
            LoginForm.LButtonText = "Bejelentkezés";
            LoginForm.PassText = "";
            this.Invoke(new LoginForm.MyDelegate(LoginDialog.SetLoginValues));
            LoginDialog.ShowDialog();
            //Nézzük, sikerült-e
            if (UserInfo.UserID == 0)
                Close();
            this.Show();
        }

        private void LoginNewUser(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("MSGer.tk.exe");
        }

        private void SetOnlineState(object sender, EventArgs e)
        {
            int state = 0;
            if (sender == elérhetőToolStripMenuItem)
                state = 1;
            if (sender == elfoglaltToolStripMenuItem)
                state = 2;
            if (sender == nincsAGépnélToolStripMenuItem)
                state = 3;
            if (sender == rejtveKapcsolódikToolStripMenuItem)
                state = 4;
            //HTTP
            if (Networking.SendRequest("setstate", state + "", 2) != "Success")
                MessageBox.Show("Hiba történt az állapot beállitása során.");
        }

        private void SelectPartner(object sender, EventArgs e)
        {
            SelectPartnerSender = (ToolStripMenuItem)sender;
            DialogResult dr = new DialogResult();
            dr = (new SelectPartnerForm()).ShowDialog();
            if (dr == DialogResult.OK)
            {
                if (SelectPartnerSender == fájlKüldéseToolStripMenuItem)
                {
                    ;
                }
                if (SelectPartnerSender == azonnaliÜzenetKüldéseToolStripMenuItem)
                {
                    ;
                }
            }
            /*else if (dr == DialogResult.Cancel)
                ;*/
        }
    }
}
