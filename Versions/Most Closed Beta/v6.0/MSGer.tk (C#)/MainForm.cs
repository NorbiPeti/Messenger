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
        public static LoginForm LoginDialog;
        public static Thread LThread;
        public static Thread MainThread = null;
        public static bool PartnerListThreadActive = true;
        //public static object SelectPartnerSender = null;
        public static ToolStripMenuItem SelectPartnerSender = null;
        public MainForm()
        {
            InitializeComponent();
            //Settings.Default.Reload();
            //CurrentUser.InitVars();
            contactList.Columns[0].Width = contactList.ItemHeight; //2014.02.28.
            this.Hide();
            try
            {
                LoginDialog = new LoginForm();
                LoginDialog.ShowDialog();
                //NullReferenceException a = null;
                //throw a;
            }
            catch (Exception e)
            {
                ErrorHandling.FormError(LoginDialog, e);
            }
            //Nézzük, sikerült-e
            if (CurrentUser.UserID != 0)
            {
                //this.Show();
                contactList.Enabled = false; //2014.03.05.
                //contactList.Items.Add("Betöltés..."); //2014.03.05.
                MainThread = Thread.CurrentThread;

                // Create the thread object, passing in the Alpha.Beta method
                // via a ThreadStart delegate. This does not start the thread.
                LThread = new Thread(new ThreadStart(UpdatePartnerList));
                LThread.Name = "Update Partner List";

                // Start the thread
                LThread.Start();
            }
        }

        private void LogoutUser(object sender, EventArgs e)
        {
            this.Hide();
            //
            UserInfo.Partners = new UserInfo[1024]; //Újabb tömböt rendel hozzá, ezért a régi jó esetben törlésre kerül - 2014.03.11.
            //Type lol = sender.GetType();
            //MessageBox.Show("Just some stuff:\n" + lol.ToString());
            //lol = lol.GetType();
            //MessageBox.Show("Just some stuff:\n" + lol.ToString()); //System.RuntimeType
            //
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
            if (Networking.SendRequest("setstate", state + "", 2, true) != "Success")
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
                string[] row = Networking.SendRequest("getlist", "", 3, true).Split('ͦ'); //Lekéri a listát, és különválogatja egyben - 2014.02.28.
                //contactList.Items.Clear();
                GLItem[] listViewItem=new GLItem[1024];
                int i = 0;
                int[] tmp;
                if (Settings.Default.picupdatetime.Length == 0)
                    tmp = new int[1024];
                else
                    tmp = Settings.Default.picupdatetime.Split(',').Select(s => Int32.Parse(s)).ToArray();
                UserInfo[] tmpuser=UserInfo.Partners; //Tömbrendezés - Első igazán bonyolult művelet (a többihez csak a parancsokat nem ismertem) - 2014.03.07.
                for (int x = 0; x < row.Length; x += 6) //Ezt az egyetlen számot (x+=3) kell módositani, és máris alkalmazkodott a hozzáadott adathoz
                {
                    if (row.Length < 5) //2014.03.19. - Ha nincs ismerőse
                        break;
                    for (int y = 0; y < UserInfo.Partners.Length; y++)
                    {
                        if(UserInfo.Partners[y]!=null && Int32.Parse(row[x+3])==UserInfo.Partners[y].UserID) //Ha null az értéke, már nem is ellenőrzi a másik feltételt
                        { //Átrendezi a tömböt az új sorrendbe - Ha változott - 2014.03.07.
                            tmpuser[i] = UserInfo.Partners[y];
                            tmpuser[i].Number = i; //És elmenti az új helyét - 2014.03.13.
                        }
                    }
                    if (tmpuser[i] == null)
                    {
                        tmpuser[i] = new UserInfo();
                    }
                    tmpuser[i].UserID = Int32.Parse(row[x + 3]); //Beállitja az ID-ket
                    tmpuser[i].Number = i;
                    tmpuser[i].Name = row[x];
                    tmpuser[i].Message = row[x + 1];
                    tmpuser[i].State = row[x + 2];
                    tmpuser[i].UserName = row[x + 4];
                    tmpuser[i].Email = row[x + 5];
                    /*if (tmp == null) - Ha tmp==null, akkor az alábbi utasitás sem fog tudni lefutni...
                        tmp[i] = 0; //Beállitja 0-ra, ha nincs még elmentve*/
                    string state = "";
                    if (row[x + 2] == "1")
                        state = " (Elérhető)";
                    if (row[x + 2] == "2")
                        state = " (Elfoglalt)";
                    if (row[x + 2] == "3")
                        state = " (Nincs a gépnél)";
                    listViewItem[i] = new GLItem();
                    PictureBox item = new PictureBox();
                    tmpuser[i].PicUpdateTime = tmp[i];
                    string imgpath = tmpuser[i].GetImage();
                    if (imgpath!="noimage.png" || File.Exists("noimage.png")) //2014.03.13.
                        item.ImageLocation = imgpath;
                    else
                        MessageBox.Show("Az alap profilkép nem található.\nMásold be a noimage.png-t, vagy telepitsd újra a programot.\n(Ez az üzenet minden egyes ismerősödnél megjelenik.)", "Hiba");
                    item.SizeMode = PictureBoxSizeMode.StretchImage;

                    listViewItem[i].SubItems[0].Control = item;
                    listViewItem[i].SubItems[1].Text = row[x] + " " + state + "\n" + row[x + 1];
                    i++;
                }
                Settings.Default.picupdatetime = String.Join(",", tmp.Select(ix => ix.ToString()).ToArray());
                UserInfo.Partners = tmpuser;
                contactList.Items.Clear();
                try
                {
                    contactList.Items.AddRange(listViewItem); //2014.03.01.
                }
                catch(ObjectDisposedException)
                {
                    break;
                }
                catch (NullReferenceException)
                {
                    //Mindig megtörténik - A tömb mérete miatt
                }
                try
                { //Ha már leállt a program, hibát ir
                    this.Invoke(new MyDelegate(UpdateContactList));
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (InvalidOperationException)
                {
                    break;
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
            if (RightClickedPartner == -1)
                RightClickedPartner = tmp;
            //Üzenetküldő form
            /*if (ChatWindow[RightClickedPartner] == null || ChatWindow[RightClickedPartner].IsDisposed)
                ChatWindow[RightClickedPartner] = new ChatForm();
            UserInfo test = new UserInfo();
            test.ChatWindow = new ChatForm();
            ChatWindow[RightClickedPartner].Show();*/
            if (UserInfo.Partners[RightClickedPartner].ChatWindow == null || UserInfo.Partners[RightClickedPartner].ChatWindow.IsDisposed)
                UserInfo.Partners[RightClickedPartner].ChatWindow = new ChatForm();
            UserInfo.Partners[RightClickedPartner].ChatWindow.Show();

            //RightClickedPartner = -1; - A form bezárásakor állitsa vissza
        }

        private void OnMainFormLoad(object sender, EventArgs e)
        {
            if (CurrentUser.UserID == 0)
                Close();
        }

        private void InvitePartner(object sender, EventArgs e)
        {
            (new InvitePartner()).ShowDialog();
        }
    }
}
