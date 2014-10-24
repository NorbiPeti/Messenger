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
using System.Reflection;
using SzNPProjects;
using System.Net.Sockets;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace MSGer.tk
{
    public partial class MainForm : Form
    {
        public static LoginForm LoginDialog;
        public static Thread LThread;
        public static Thread MainThread = null;
        public static Notifier taskbarNotifier;
        public MainForm()
        {
            BeforeLogin.SetText("Starting...");
            InitializeComponent();
            Thread.CurrentThread.Name = "Main Thread";
            toolStripMenuItem4.Enabled = false; //2014.04.12.
            toolStripMenuItem8.Enabled = false; //2014.04.12.

            this.WindowState = FormWindowState.Minimized; //2014.04.19.

            BeforeLogin.SetText("Loading program settings...");
            Storage.Load(false); //Töltse be a nyelvet, legutóbb használt E-mail-t...

            BeforeLogin.SetText("Checking available ports...");
            //2014.09.04. - Amint lehet állítsa be a helyes IP-t, majd azt hagyja úgy, akármi történjék
            while (true)
            {
                if (IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners().Select(entry => entry.Port).Contains(Int32.Parse(Storage.Settings["port"])))
                    Storage.Settings["port"] = (Int32.Parse(Storage.Settings["port"]) + 1).ToString();
                else
                    break;
            }
            Networking.ReceiverConnection = new UdpClient(Int32.Parse(Storage.Settings["port"])); //2014.09.04.
            Networking.SenderConnection.AllowNatTraversal(true); //2014.09.04.

            BeforeLogin.SetText("Loading languages...");
            new Language();

            BeforeLogin.SetText(Language.Translate("beforelogin_translatemainf"));
            #region Helyi beállitás
            //try
            //{
            fájlToolStripMenuItem.Text = Language.Translate("menu_file");
            kijelentkezésToolStripMenuItem.Text = Language.Translate("menu_file_logout");
            toolStripMenuItem1.Text = Language.Translate("menu_file_loginnewuser");
            állapotToolStripMenuItem.Text = Language.Translate("menu_file_status");
            elérhetőToolStripMenuItem.Text = Language.Translate("menu_file_status_online");
            elfoglaltToolStripMenuItem.Text = Language.Translate("menu_file_status_busy");
            nincsAGépnélToolStripMenuItem.Text = Language.Translate("menu_file_status_away");
            rejtveKapcsolódikToolStripMenuItem.Text = Language.Translate("menu_file_status_hidden");
            fájlKüldéseToolStripMenuItem.Text = Language.Translate("menu_file_sendfile");
            beérkezettFájlokMappájánakMegnyitásaToolStripMenuItem.Text = Language.Translate("menu_file_openreceivedfiles");
            üzenetekElőzményeinekMegtekintéseToolStripMenuItem.Text = Language.Translate("menu_file_openrecentmsgs");
            bezárásToolStripMenuItem.Text = Language.Translate("menu_file_close");
            kilépésToolStripMenuItem.Text = Language.Translate("menu_file_exit");

            ismerősökToolStripMenuItem.Text = Language.Translate("menu_contacts");
            ismerősFelvételeToolStripMenuItem.Text = Language.Translate("menu_contacts_add");
            ismerősSzerkesztéseToolStripMenuItem.Text = Language.Translate("menu_contacts_edit");
            ismerősTörléseToolStripMenuItem.Text = Language.Translate("menu_contacts_remove");
            toolStripMenuItem3.Text = Language.Translate("menu_contacts_invite");
            csoportLétrehozásaToolStripMenuItem.Text = Language.Translate("menu_contacts_makegroup");
            kategóriaLétrehozásaToolStripMenuItem.Text = Language.Translate("menu_contacts_makecategory");
            kategóriaSzerkesztéseToolStripMenuItem.Text = Language.Translate("menu_contacts_editcategory");
            kategóriaTörléseToolStripMenuItem.Text = Language.Translate("menu_contacts_removecategory");

            műveletekToolStripMenuItem.Text = Language.Translate("menu_operations");
            azonnaliÜzenetKüldéseToolStripMenuItem.Text = Language.Translate("menu_operations_sendmsg");
            egyébKüldéseToolStripMenuItem.Text = Language.Translate("menu_operations_sendother");
            emailKüldéseToolStripMenuItem.Text = Language.Translate("menu_operations_sendother_sendmail");
            fájlKüldéseToolStripMenuItem1.Text = Language.Translate("menu_file_sendfile"); //Ugyanaz a szöveg
            ismerősSzámitógépénekFelhivásaToolStripMenuItem.Text = Language.Translate("menu_operations_callcontact");
            videóhivásInditásaToolStripMenuItem.Text = Language.Translate("menu_operations_videocall");
            onlineFájlokMegtekintéseToolStripMenuItem.Text = Language.Translate("menu_operations_showonlinefiles");
            közösJátékToolStripMenuItem.Text = Language.Translate("menu_operations_playgame");
            távsegitségKéréseToolStripMenuItem.Text = Language.Translate("menu_operations_askforhelp");

            eszközökToolStripMenuItem.Text = Language.Translate("menu_tools");
            mindigLegfelülToolStripMenuItem.Text = Language.Translate("menu_tools_alwaysontop");
            hangulatjelekToolStripMenuItem.Text = Language.Translate("menu_tools_emoticons");
            megjelenitendőKépVáltásaToolStripMenuItem.Text = Language.Translate("menu_tools_changeimage");
            háttérMódositásaToolStripMenuItem.Text = Language.Translate("menu_tools_changebackground");
            hangokÉsVideóBeállitásaToolStripMenuItem.Text = Language.Translate("menu_tools_voicevideosettings");
            beállitásokToolStripMenuItem.Text = Language.Translate("menu_tools_settings");

            súgóToolStripMenuItem.Text = Language.Translate("menu_help");
            témakörökToolStripMenuItem.Text = Language.Translate("menu_help_contents");
            aSzolgáltatásÁllapotsaToolStripMenuItem.Text = Language.Translate("menu_help_status");
            adatvédelmiNyilatkozatToolStripMenuItem.Text = Language.Translate("menu_help_privacypolicy");
            használatiFeltételekToolStripMenuItem.Text = Language.Translate("menu_help_termsofuse");
            visszaélésBejelentéseToolStripMenuItem.Text = Language.Translate("menu_help_report");
            segitsenAProgramTökéletesitésébenToolStripMenuItem.Text = Language.Translate("menu_help_improveprogram");
            névjegyToolStripMenuItem.Text = Language.Translate("menu_help_about");

            textBox1.Text = Language.Translate("searchbar");
            //contactList.Items[0].SubItems[0].Text = Language.Translate("loading"); - 2014.08.28. - Nincs már rá szükség (hibát is ír, mivel nincs listaelem)

            üzenetküldésToolStripMenuItem.Text = Language.Translate("menu_operations_sendmsg");
            emailKüldéseemailToolStripMenuItem.Text = Language.Translate("contact_sendemail");
            toolStripMenuItem2.Text = Language.Translate("contact_copyemail");
            információToolStripMenuItem.Text = Language.Translate("contact_info");
            ismerősLetiltásaToolStripMenuItem.Text = Language.Translate("contact_block");
            ismerősTörléseToolStripMenuItem.Text = Language.Translate("contact_remove");
            becenévSzerkesztéseToolStripMenuItem.Text = Language.Translate("contact_editname");
            eseményértesitésekToolStripMenuItem.Text = Language.Translate("contact_eventnotifications");
            beszélgetésnaplóMegnyitásaToolStripMenuItem.Text = Language.Translate("contact_openchatlog");

            toolStripMenuItem4.Text = Language.Translate("iconmenu_show");
            toolStripMenuItem8.Text = Language.Translate("menu_file_logout");
            toolStripMenuItem9.Text = Language.Translate("menu_file_exit");
            //}
            //catch
            //{
            //MessageBox.Show("Error while loading translations.");
            //}
            #endregion

            BeforeLogin.SetText(Language.Translate("beforelogin_loadtextformat"));
            //2014.05.16.
            new TextFormat();

            BeforeLogin.SetText(Language.Translate("beforelogin_checkforupdates"));
            //2014.04.25.
            string response = Networking.SendRequest("checkforupdates",
                Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""),
                0, false);
            if (response == "outofdate")
            {
                var res = MessageBox.Show(Language.Translate("outofdate"), Language.Translate("outofdate_caption"), MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                    System.Diagnostics.Process.Start("http://msger.url.ph/download.php?version=latest");
            }
            else if (response != "fine")
                MessageBox.Show(Language.Translate("error") + ": " + response);

            //2014.09.06.
            if (Storage.Settings["isserver"] == "")
            {
                if (MessageBox.Show(Language.Translate("isserver_msg"), "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Storage.Settings["isserver"] = "1";
                else
                    Storage.Settings["isserver"] = "0";
            }

            BeforeLogin.SetText(Language.Translate("beforelogin_loginform"));
            try
            {
                LoginDialog = new LoginForm();
                BeforeLogin.Destroy();
                LoginDialog.ShowDialog();
            }
            catch (Exception e)
            {
                ErrorHandling.FormError(LoginDialog, e);
            }
            //Nézzük, sikerült-e
            if (CurrentUser.UserID == 0)
                return; //2014.09.06.
            contactList.Enabled = false; //2014.03.05.
            MainThread = Thread.CurrentThread;

            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            LThread = new Thread(new ThreadStart(new UpdateListAndChat().Run));
            LThread.Name = "Update Partnerlist and Chat";

            Thread keepupthread = new Thread(new ThreadStart(Networking.KeepUpThread));
            keepupthread.Name = "Keep Up Thread";

            Thread keepupuserst = new Thread(new ThreadStart(Networking.KeepUpUsersThread)); //2014.09.26.
            keepupuserst.Name = "Keep Up Users Thread";

            Storage.Load(true); //2014.08.07.

            if (Storage.Settings["windowstate"] == "1") //2014.04.18. - 2014.08.08.
                this.WindowState = FormWindowState.Maximized;
            else if (Storage.Settings["windowstate"] == "2")
                this.WindowState = FormWindowState.Minimized;
            else if (Storage.Settings["windowstate"] == "3")
                this.WindowState = FormWindowState.Normal;

            taskbarNotifier = new Notifier("popup-bg.bmp", Color.FromArgb(255, 0, 255), "close.bmp", 5000);

            taskbarNotifier.Click += PopupClick;
            taskbarNotifier.CloseClick += PopupCloseClick;

            toolStripMenuItem4.Enabled = true; //2014.04.12.
            toolStripMenuItem8.Enabled = true; //2014.04.12.

            // Start the thread
            LThread.Start();

            keepupthread.Start();

            keepupuserst.Start();

            //2014.08.19. - Küldje el a bejelentkezés hírét, hogy frissítéseket kapjon
            byte[] strb = Encoding.Unicode.GetBytes(CurrentUser.IP.ToString());
            byte[] tmpfinal = new byte[8 * UserInfo.KnownUsers.Count + strb.Length + 4]; //Hosszúság, IP, ismert felh. ID, frissítési idő
            Array.Copy(BitConverter.GetBytes(strb.Length), tmpfinal, 4);
            Array.Copy(strb, 0, tmpfinal, 4, strb.Length);
            //if (tmpfinal.Length != 0)
            if (UserInfo.KnownUsers.Count != 0)
            {
                for (int i = 0; i < UserInfo.KnownUsers.Count; i++)
                {
                    byte[] tmptmp = BitConverter.GetBytes(UserInfo.KnownUsers[i].UserID);
                    Array.Copy(tmptmp, 0, tmpfinal, i * 4 + strb.Length + 4, 4);
                    tmptmp = BitConverter.GetBytes(UserInfo.KnownUsers[i].LastUpdate);
                    Array.Copy(tmptmp, 0, tmpfinal, i * 4 + strb.Length + 4, 4);
                }
            }
            Networking.ParseUpdateInfo(Networking.SendUpdate(Networking.UpdateType.LoginUser, tmpfinal, false));

            notifyIcon1.Visible = true; //2014.09.22.
            taskbarNotifier.Show("Teszt cím", "Teszt tartalom\nMásodik sor");
        }

        private void LoadPartnerList() //2014.08.28.
        {
            contactList.AutoUpdate = false;
            UserInfo.AutoUpdate = false; //2014.09.26.
            //string[] list = Networking.SendRequest("getlist", "", 0, true).Split('ͦ');
            string[] list = Networking.SendRequest("getlist", "", 0, true).Split(new char[] { 'ͦ' }, StringSplitOptions.RemoveEmptyEntries); //2014.09.26.
            if (list[0].Contains("Fail"))
                MessageBox.Show(list[0]);
            UserInfo.KnownUsers = UserInfo.KnownUsers.Select(entry => { entry.IsPartner = false; return entry; }).ToList(); //2014.09.26.
            for (int i = 0; i + 1 < list.Length; i += 2)
            {
                string username = list[i];
                int uid = Int32.Parse(list[i + 1]);
                if (!UserInfo.IDIsInList(UserInfo.KnownUsers, uid))
                { //Ha nem tud róla semmit, akkor töltse le a felhasználónevét, és jelenítse meg azt
                    var tmp = new UserInfo();
                    tmp.UserID = uid;
                    tmp.UserName = username;
                    tmp.LastUpdate = 0; //Lényegében nem tud róla túl sokat, ezért ha lehet, frissítse
                    tmp.Name = username;
                    tmp.IsPartner = true;
                    UserInfo.KnownUsers.Add(tmp);
                }
                else
                {
                    UserInfo.Select(uid).IsPartner = true; //2014.09.26.
                    UserInfo.Select(uid).UserName = username; //2014.09.26. - Nem megváltoztatható, ha egy felhasználó megpróbálja, nem foglalkozik vele
                }
            }
            CurrentUser.State = 1; //2014.08.31. 0:48
            UserInfo.AutoUpdate = true;
            foreach (var entry in UserInfo.KnownUsers)
            {
                entry.Update(); //Áthelyeztem, mert az értékek frissítésekor is szükség van rá
            }
            contactList.AutoUpdate = true;
            contactList.Enabled = true;
            contactList.Refresh();
        }

        private void PopupCloseClick(object sender, EventArgs e)
        {
            MessageBox.Show("Close");
        }

        private void PopupClick(object sender, EventArgs e)
        {
            MessageBox.Show("Click");
        }

        private void LogoutUser(object sender, EventArgs e)
        {
            this.Hide();
            toolStripMenuItem4.Enabled = false; //2014.04.12.
            toolStripMenuItem8.Enabled = false; //2014.04.12.
            Storage.Save(true); //2014.08.28.
            SetOnlineState(null, null); //2014.04.11. - Erre nincs beállitva, ezért automatikusan 0-ra, azaz kijelentkeztetettre állítja az állapotát
            contactList.Items.Clear(); //2014.09.19.
            UserInfo.KnownUsers.Clear(); //2014.09.19.
            Storage.Dispose();
            LThread = null;
            CurrentUser.SendChanges = false; //2014.08.30.
            while (ChatForm.ChatWindows.Count > 0)
            { //2014.09.06. - A Close() hatására törli a gyűjteményből, ezért sorra végig fog haladni rajta
                ChatForm.ChatWindows[0].Close();
            }
            LoginDialog = new LoginForm(); //2014.04.04.
            LoginDialog.ShowDialog();
            //Nézzük, sikerült-e
            if (CurrentUser.UserID == 0)
                Close();
            Storage.Load(true); //2014.08.07.
            toolStripMenuItem4.Enabled = true; //2014.04.12.
            toolStripMenuItem8.Enabled = true; //2014.04.12.
            CurrentUser.SendChanges = true; //2014.08.30.
            contactList.Items.Clear(); //2014.10.09. - Kijelentkezéskor hozzáad egy üres listelemet egy (Nem elérhető) felirattal, ezt tünteti el
            LoadPartnerList();
            this.Show();
            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            LThread = new Thread(new ThreadStart(new UpdateListAndChat().Run));
            LThread.Name = "Update Partner List";

            // Start the thread
            LThread.Start();
        }

        private void LoginNewUser(object sender, EventArgs e)
        {
            Storage.Save(true); //2014.09.19.
            Process.Start(((Program.ProcessName.Contains("vshost")) ? Program.ProcessName.Replace(".vshost", "") : Program.ProcessName) + ".exe", "multi");
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
            if (sender == null) //2014.08.30. - Erre nagyon sokáig nem volt felkészítve, és ezt kihasználtam a kijelentkezéshez
            {
                Networking.SendRequest("setstate", 0 + "", 0, true); //Kijelentkezés
                byte[] tmpb = Encoding.Unicode.GetBytes(CurrentUser.IP.ToString());
                byte[] sendb = new byte[4 + tmpb.Length];
                Array.Copy(BitConverter.GetBytes(tmpb.Length), sendb, 4);
                Array.Copy(tmpb, 0, sendb, 4, tmpb.Length);
                Networking.SendUpdate(Networking.UpdateType.LogoutUser, sendb, false);
            }
            CurrentUser.State = state; //2014.08.28.
        }

        private void SelectPartner(object sender, EventArgs e)
        {
            var form = new SelectPartnerForm((ToolStripMenuItem)sender);
            DialogResult dr = form.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //2014.04.25.
                string[] partners = form.Partners;
                ChatForm tmpchat = new ChatForm();
                for (int i = 0; i < partners.Length; i++)
                {
                    if (partners[i] != "") //2014.04.17.
                    {
                        for (int j = 0; j < UserInfo.KnownUsers.Count; j++)
                        {
                            if (!UserInfo.KnownUsers[j].IsPartner)
                                continue;
                            int tmp; //2014.04.17.
                            if (!Int32.TryParse(partners[i], out tmp))
                                tmp = -1;
                            if (UserInfo.KnownUsers[j].UserName == partners[i] || UserInfo.KnownUsers[j].Email == partners[i] || UserInfo.KnownUsers[j].UserID == tmp)
                            { //Egyezik a név, E-mail vagy ID - UserName: 2014.04.17.
                                tmpchat.ChatPartners.Add(UserInfo.KnownUsers[j].UserID); //2014.08.28.
                            }
                        }
                    }
                }
                if (tmpchat.ChatPartners.Count != 0)
                {
                    ChatForm.ChatWindows.Add(tmpchat);
                    if (sender == fájlKüldéseToolStripMenuItem)
                    {
                        tmpchat.Show();
                        tmpchat.OpenSendFile(form);
                    }
                    if (sender == azonnaliÜzenetKüldéseToolStripMenuItem)
                    {
                        tmpchat.Show();
                    }
                }
            }
        }

        private void ClearSearchBar(object sender, EventArgs e)
        {
            if (textBox1.Text == Language.Translate("searchbar"))
                textBox1.Clear();
        }

        private void PutTextInSearchBar(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = Language.Translate("searchbar");
        }
        public static int RightClickedPartner = -1;

        public static void OpenSendMessage(int uid)
        {
            //Üzenetküldő form
            int ChatNum = -1;
            for (int i = 0; i < ChatForm.ChatWindows.Count; i++)
            {
                if (ChatForm.ChatWindows[i].ChatPartners.Count == 1 && ChatForm.ChatWindows[i].ChatPartners.Contains(uid))
                { //Vele, és csak vele beszél
                    ChatNum = i;
                    break;
                }
            }
            if (ChatNum == -1)
            { //Nincs még chatablaka
                ChatForm.ChatWindows.Add(new ChatForm());
                ChatForm.ChatWindows[ChatForm.ChatWindows.Count - 1].ChatPartners.Add(uid);
                ChatForm.ChatWindows[ChatForm.ChatWindows.Count - 1].Show();
                ChatForm.ChatWindows[ChatForm.ChatWindows.Count - 1].Focus(); //2014.08.08.
            }
            else
            {
                ChatForm.ChatWindows[ChatNum].Show();
                ChatForm.ChatWindows[ChatNum].Focus();
            }
        }

        private void OnMainFormLoad(object sender, EventArgs e)
        {
            if (CurrentUser.UserID == 0)
                Program.Exit();

            LoadPartnerList(); //Be kell töltenie a MainForm-nak, hogy hivatkozhasson rá

            CurrentUser.SendChanges = true; //2014.08.30.
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

        private void névjegyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AboutBox1()).ShowDialog();
        }

        private void mindigLegfelülToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = mindigLegfelülToolStripMenuItem.Checked;
        }

        private void beállitásokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new SettingsForm()).Show();
        }

        private void contactList_ItemDoubleClicked(object sender, int e)
        {
            int uid = UserInfo.GetUserIDFromListID(e);
            OpenSendMessage(uid);
        }

        private void bezárásToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void információToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RightClickedPartner == -1)
                return;
            for (int i = 0; i < UserInfo.KnownUsers.Count; i++)
            {
                if (UserInfo.KnownUsers[i].ListID != RightClickedPartner)
                    continue;
                (new PartnerInformation(UserInfo.KnownUsers[i])).ShowDialog();
                break;
            }
        }

        private void contactList_ItemRightClicked(object sender, int e)
        {
            contactList.Items[e].Selected = true;
            RightClickedPartner = e;
            partnerMenu.Show(Cursor.Position);
        }

        private void PartnerMenu_SendMessage(object sender, EventArgs e)
        {
            if (RightClickedPartner == -1)
                return;
            int uid = UserInfo.GetUserIDFromListID(RightClickedPartner);
            OpenSendMessage(uid);
            RightClickedPartner = -1;
        }
    }
}
