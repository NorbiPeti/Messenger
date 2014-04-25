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
using Khendys.Controls;
using System.Threading;

namespace MSGer.tk
{
    public partial class MainForm : Form
    {
        private LoginForm LoginDialog;
        public static Thread LThread;
        public static Thread MainThread = null;
        public static bool PartnerListThreadActive = true;
        //public static object SelectPartnerSender = null;
        public static ToolStripMenuItem SelectPartnerSender = null;
        public MainForm()
        {
            InitializeComponent();
            Settings.Default.Reload();
            CurrentUser.InitVars();
            contactList.Columns[0].Width = contactList.ItemHeight; //2014.02.28.
            this.Hide();
            LoginDialog = new LoginForm();
            LoginDialog.ShowDialog();
            //Nézzük, sikerült-e
            if (CurrentUser.UserID == 0)
                Close();
            this.Show();
            contactList.Enabled = false; //2014.03.05.
            contactList.Items.Add("Betöltés..."); //2014.03.05.
            MainThread = Thread.CurrentThread;
            
            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            LThread = new Thread(new ThreadStart(UpdatePartnerList));
            LThread.Name = "Update Partner List";

            // Start the thread
            LThread.Start();
        }

        private void LogoutUser(object sender, EventArgs e)
        {
            this.Hide();
            CurrentUser.UserID = 0;
            LoginForm.LButtonText = "Bejelentkezés";
            LoginForm.PassText = "";
            this.Invoke(new LoginForm.MyDelegate(LoginDialog.SetLoginValues));
            PartnerListThreadActive = false;
            LoginDialog.ShowDialog();
            //Nézzük, sikerült-e
            if (CurrentUser.UserID == 0)
                Close();
            contactList.Items.Clear(); //2014.03.05.
            contactList.Enabled = false; //2014.03.05.
            contactList.Items.Add("Betöltés..."); //2014.03.05.
            this.Show();
            PartnerListThreadActive = true; //2014.02.28. - Törli, majd újra létrehozza a listafrissitő thread-et, ha újra bejelentkezett
            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            LThread = new Thread(new ThreadStart(UpdatePartnerList));
            LThread.Name = "Update Partner List";

            // Start the thread
            LThread.Start();
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
        public int UpdateContactList() //2014.03.01.
        {
            //contactList.Update(); //2014.03.01.
            //contactList.Invalidate(true); //2014.03.01.
            contactList.Refresh();
            contactList.Enabled = true; //2014.03.05. - A thread első futásához kell
            return 0;
        }
        public delegate int MyDelegate();
        public void UpdatePartnerList()
        {
            while (PartnerListThreadActive && MainThread.IsAlive)
            {
                string[] row = Networking.SendRequest("getlist", "", 3).Split('ͦ'); //Lekéri a listát, és különválogatja egyben - 2014.02.28.
                //contactList.Items.Clear();
                GLItem[] listViewItem=new GLItem[1024];
                int i = 0;
                for (int x = 0; x < row.Length; x += 4) //Ezt az egyetlen számot (x+=3) kell módositani, és máris alkalmazkodott a hozzáadott adathoz
                {
                    //MessageBox.Show(row[x] + "-" + row[x + 1] + "-" + row[x + 2] + "-" + row[x + 3]);
                    string state = "";
                    if (row[x + 2] == "1")
                        state = " (Elérhető)";
                    if (row[x + 2] == "2")
                        state = " (Elfoglalt)";
                    if (row[x + 2] == "3")
                        state = " (Nincs a gépnél)";
                    //var listViewItem = new GLItem(contactList);
                    listViewItem[i] = new GLItem();
                    //ExRichTextBox item = new ExRichTextBox();
                    PictureBox item = new PictureBox();
                    try
                    {
                        //item.InsertImage(Image.FromFile(CurrentUser.GetPartnerImage(Int32.Parse(row[x + 3])))); //SetVars
                        item.ImageLocation = CurrentUser.GetPartnerImage(Int32.Parse(row[x + 3]));
                    }
                    catch
                    {
                        MessageBox.Show("Az alap profilkép nem található.", "Hiba");
                    }
                    // Set the size of the PictureBox control. 
                    //item.Size = new System.Drawing.Size(contactList.ItemHeight, contactList.ItemHeight);

                    // Set the SizeMode property to the StretchImage value.  This 
                    // will enlarge the image as needed to fit into 
                    // the PictureBox.
                    item.SizeMode = PictureBoxSizeMode.StretchImage;

                    listViewItem[i].SubItems[0].Control = item;
                    listViewItem[i].SubItems[1].Text = row[x] + " " + state + "\n" + row[x + 1];
                    //contactList.Items.Add(listViewItem[i]);
                    /*if (Convert.ToInt32(row[x + 2]) != 0 && Convert.ToInt32(row[x + 2]) != 4)
                        listViewItem.Group = listView1.Groups["listViewGroup1"];
                    else
                        listViewItem.Group = listView1.Groups["listViewGroup2"];
                    listView1.Items.Add(listViewItem);*/
                    if (x + 4 >= row.Length)
                        break;
                    i++;
                }
                contactList.Items.Clear();
                try
                {
                    contactList.Items.AddRange(listViewItem); //2014.03.01.
                }
                catch
                {
                }
                try
                { //Ha már leállt a program, hibát ir
                    this.Invoke(new MyDelegate(UpdateContactList));
                }
                catch
                {
                }
                Thread.Sleep(5000);
            }
        }

        private void ClearSearchBar(object sender, EventArgs e)
        {
            if(textBox1.Text=="Ismerősök keresése...")
                textBox1.Clear();
        }

        private void PutTextInSearchBar(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = "Ismerősök keresése...";
        }
        public static int RightClickedPartner = -1;
        private void ContactItemRightClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(contactList.HotItemIndex + "");
            if (e.Button != MouseButtons.Right || contactList.HotItemIndex>=contactList.Items.Count)
            { //Igy nem reagál arra sem, ha üres területre kattintunk
                return;
            }
            contactList.Items[contactList.HotItemIndex].Selected = true;
            RightClickedPartner = contactList.HotItemIndex;
            partnerMenu.Show(Cursor.Position);
        }
        ChatForm[] ChatWindow=new ChatForm[1024];
        private void OpenSendMessage(object sender, EventArgs e) //2014.03.02. 0:17
        {
            int tmp = contactList.HotItemIndex;
            //MessageBox.Show(RightClickedPartner + " " + contactList.HotItemIndex);
            if (RightClickedPartner == -1)
                RightClickedPartner = tmp;
            //MessageBox.Show(RightClickedPartner + " " + contactList.HotItemIndex + "\n" + contactList.Items.Count + "\n" + tmp);
            //MessageBox.Show("Üzenetküldés:\n" + contactList.Items[RightClickedPartner].SubItems[1].Text);
            //Üzenetküldő form
            if (ChatWindow[RightClickedPartner] == null || ChatWindow[RightClickedPartner].IsDisposed)
                ChatWindow[RightClickedPartner] = new ChatForm();
            //MessageBox.Show("Form "+RightClickedPartner);
            //ChatWindow[RightClickedPartner].ChatPartner = RightClickedPartner;
            UserInfo test = new UserInfo();
            test.ChatWindow = new ChatForm();
            //MessageBox.Show("Form Property " + ChatWindow[RightClickedPartner].ChatPartner);
            ChatWindow[RightClickedPartner].Show();

            //RightClickedPartner = -1; - A form bezárásakor állitsa vissza
        }
    }
}
