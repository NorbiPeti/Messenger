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
using CustomUIControls;

namespace MSGer.tk
{
    public partial class MainForm : Form
    {
        public static LoginForm LoginDialog;
        public static Thread LThread;
        public static Thread MainThread = null;
        public static bool PartnerListThreadActive = true;
        public static ToolStripMenuItem SelectPartnerSender = null;
        //public static TaskbarNotifier taskbarNotifier;
        public static Notifier taskbarNotifier;
        public MainForm()
        {
            InitializeComponent();
            Thread.CurrentThread.Name = "Main Thread";
            contactList.Columns[0].Width = contactList.ItemHeight; //2014.02.28.
            //this.Hide();
            toolStripMenuItem4.Enabled = false; //2014.04.12.
            toolStripMenuItem8.Enabled = false; //2014.04.12.
            if (Settings.Default.windowstate == 1) //2014.04.18.
                this.WindowState = FormWindowState.Maximized;
            else if (Settings.Default.windowstate == 2)
                this.WindowState = FormWindowState.Minimized;
            else if (Settings.Default.windowstate == 3)
                this.WindowState = FormWindowState.Normal;
            try
            {
                LoginDialog = new LoginForm();
                LoginDialog.ShowDialog();
            }
            catch (Exception e)
            {
                ErrorHandling.FormError(LoginDialog, e);
            }
            //Nézzük, sikerült-e
            if (CurrentUser.UserID != 0)
            {
                contactList.Enabled = false; //2014.03.05.
                MainThread = Thread.CurrentThread;

                // Create the thread object, passing in the Alpha.Beta method
                // via a ThreadStart delegate. This does not start the thread.
                LThread = new Thread(new ThreadStart(UpdatePartnerList));
                LThread.Name = "Update Partner List";

                // Start the thread
                LThread.Start();

                /*taskbarNotifier = new TaskbarNotifier();
                //if (File.Exists("skin.bmp")) //Kötelező megadni!
                if (!File.Exists("popup-bg.bmp"))
                    MessageBox.Show("Hiba: Hiányzik a popup-bg.bmp fájl.");
                if (!File.Exists("close.bmp"))
                    MessageBox.Show("Hiba: Hiányzik a close.bmp fájl.");
                taskbarNotifier.SetBackgroundBitmap("popup-bg.bmp",
                                    Color.FromArgb(255, 0, 255));
                //if (File.Exists("close.bmp")) //Kötelező megadni!
                taskbarNotifier.SetCloseBitmap("close.bmp",
                        Color.FromArgb(255, 0, 255), new Point(180, 10));
                taskbarNotifier.TitleRectangle = new Rectangle(40, 9, 70, 25);
                taskbarNotifier.ContentRectangle = new Rectangle(8, 41, 133, 68);
                taskbarNotifier.TitleClick += new EventHandler(PopupClick);
                taskbarNotifier.ContentClick += new EventHandler(PopupClick);
                taskbarNotifier.CloseClick += new EventHandler(PopupCloseClick);
                taskbarNotifier.Show("Bejelentkezés", "Sikeresen bejelentkeztél a programba.", 500, 5000, 500);*/

                taskbarNotifier = new Notifier("popup-bg.bmp", Color.FromArgb(255, 0, 255), "close.bmp", 5000);
                taskbarNotifier.Show("Teszt cím", "Teszt tartalom\nMásodik sor");

                toolStripMenuItem4.Enabled = true; //2014.04.12.
                toolStripMenuItem8.Enabled = true; //2014.04.12.
            }
        }

        private void PopupCloseClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            MessageBox.Show("Close");
        }

        private void PopupClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            MessageBox.Show("Click");
        }

        private void LogoutUser(object sender, EventArgs e)
        {
            this.Hide();
            toolStripMenuItem4.Enabled = false; //2014.04.12.
            toolStripMenuItem8.Enabled = false; //2014.04.12.
            SetOnlineState(null, null); //2014.04.11. - Erre nincs beállitva, ezért automatikusan 0-ra, azaz kijelentkeztetettre állítja az állapotát
            CurrentUser.UserID = 0;
            PartnerListThreadActive = false;
            LoginDialog = new LoginForm(); //2014.04.04.
            LoginDialog.ShowDialog();
            //Nézzük, sikerült-e
            if (CurrentUser.UserID == 0)
                Close();
            toolStripMenuItem4.Enabled = true; //2014.04.12.
            toolStripMenuItem8.Enabled = true; //2014.04.12.
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

        public void SetOnlineState(object sender, EventArgs e)
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
            if (Networking.SendRequest("setstate", state + "", 0, true) != "Success")
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
        }
        public int UpdateContactList() //2014.03.01.
        {
            contactList.Refresh();
            contactList.Enabled = true; //2014.03.05. - A thread első futásához kell
            return 0;
        }
        public delegate int MyDelegate();
        public void UpdatePartnerList()
        {
            while (PartnerListThreadActive && MainThread.IsAlive)
            {
                string[] row = Networking.SendRequest("getlist", "", 0, true).Split('ͦ'); //Lekéri a listát, és különválogatja egyben - 2014.02.28.
                List<GLItem> listViewItem = new List<GLItem>();
                List<int> tmp;
                if (Settings.Default.picupdatetime.Length != 0)
                    tmp = Settings.Default.picupdatetime.Split(',').Select(s => Int32.Parse(s)).ToList<int>();
                else tmp = new List<int>();
                List<UserInfo> tmpuser = UserInfo.Partners;
                int i = 0;
                for (int x = 0; x < row.Length-1; x += 6) //Ezt az egyetlen számot (x+=3) kell módositani, és máris alkalmazkodott a hozzáadott adathoz
                { //-1: 2014.04.04. - A végére is odarak egy elválasztó jelet, ami miatt eggyel több elem lesz a tömbben
                    if (row.Length < 5) //2014.03.19. - Ha nincs ismerőse
                        break;
                    for (int y = 0; y < UserInfo.Partners.Count; y++)
                    {
                        if(UserInfo.Partners[y]!=null && Int32.Parse(row[x+3])==UserInfo.Partners[y].UserID) //Ha null az értéke, már nem is ellenőrzi a másik feltételt
                        { //Átrendezi a tömböt az új sorrendbe - Ha változott - 2014.03.07.
                            tmpuser[i] = UserInfo.Partners[y];
                            tmpuser[i].ListID = i; //És elmenti az új helyét - 2014.03.13.
                        }
                    }
                    if(i>=tmpuser.Count)
                    {
                        tmpuser.Add(new UserInfo());
                    }
                    tmpuser[i].UserID = Int32.Parse(row[x + 3]); //Beállitja az ID-ket
                    tmpuser[i].ListID = i;
                    tmpuser[i].Name = row[x];
                    tmpuser[i].Message = row[x + 1];
                    tmpuser[i].State = row[x + 2];
                    tmpuser[i].UserName = row[x + 4];
                    tmpuser[i].Email = row[x + 5];
                    string state = "";
                    if (row[x + 2] == "1")
                        state = " (Elérhető)";
                    if (row[x + 2] == "2")
                        state = " (Elfoglalt)";
                    if (row[x + 2] == "3")
                        state = " (Nincs a gépnél)";
                    listViewItem.Add(new GLItem());
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
                    contactList.Items.AddRange(listViewItem.ToArray()); //2014.03.01. - ToArray: 2014.03.21.
                }
                catch(ObjectDisposedException)
                {
                    break;
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
        private void OpenSendMessage(object sender, EventArgs e) //2014.03.02. 0:17
        {
            int tmp = contactList.HotItemIndex;
            if (RightClickedPartner == -1)
                RightClickedPartner = tmp;
            if (RightClickedPartner == -1 || RightClickedPartner >= contactList.Items.Count)
                return;
            //Üzenetküldő form
            int ChatNum = -1;
            for (int i = 0; i < ChatForm.ChatWindows.Count; i++)
            {
                if (ChatForm.ChatWindows[i].ChatPartners.Count==1 && ChatForm.ChatWindows[i].ChatPartners.Contains(RightClickedPartner))
                { //Vele, és csak vele beszél
                    ChatNum = i;
                    break;
                }
            }
            if(ChatNum==-1)
            { //Nincs még chatablaka
                ChatForm.ChatWindows.Add(new ChatForm());
                ChatForm.ChatWindows[ChatForm.ChatWindows.Count - 1].ChatPartners.Add(RightClickedPartner);
                ChatForm.ChatWindows[ChatForm.ChatWindows.Count - 1].Show();
            }
            else
            {
                ChatForm.ChatWindows[ChatNum].Show();
                ChatForm.ChatWindows[ChatNum].Focus();
            }

            RightClickedPartner = -1;
        }

        private void OnMainFormLoad(object sender, EventArgs e)
        {
            if (CurrentUser.UserID == 0)
                Program.Exit();
        }

        private void InvitePartner(object sender, EventArgs e)
        {
            (new InvitePartner()).ShowDialog();
        }

        private void BeforeExit(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (CurrentUser.UserID != 0) //2014.04.18.
            {
                this.Show();
                this.Focus();
            }
        }

        private void ExitProgram(object sender, EventArgs e)
        {
            Program.Exit();
        }

        private void ismerősFelvételeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AddPartner()).ShowDialog();
        }
    }
}
