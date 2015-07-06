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
    public partial class MainForm : ThemedForms
    {
        //public static LoginForm LoginDialog;
        //public static Thread LThread;
        public static Thread PartnerListUpdateThread; //2015.05.15.
        //public static Thread MainThread = null;
        public static Notifier taskbarNotifier;
        public MainForm()
        {
            InitializeComponent();
            toolStripMenuItem4.Enabled = false; //2014.04.12.
            toolStripMenuItem8.Enabled = false; //2014.04.12.
            contactList.Enabled = false; //2014.03.05.

            BeforeLogin.SetText(Language.Translate(Language.StringID.BeforeLogin_TranslateMainF));
            #region Helyi beállitás
            fájlToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File);
            Language.ReloadEvent += delegate { fájlToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File); };
            kijelentkezésToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_Logout);
            Language.ReloadEvent += delegate { kijelentkezésToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_Logout); };
            toolStripMenuItem1.Text = Language.Translate(Language.StringID.Menu_File_LoginNewUser);
            Language.ReloadEvent += delegate { toolStripMenuItem1.Text = Language.Translate(Language.StringID.Menu_File_LoginNewUser); };
            állapotToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_Status);
            Language.ReloadEvent += delegate { állapotToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_Status); };
            elérhetőToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_Status_Online);
            Language.ReloadEvent += delegate { elérhetőToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_Status_Online); };
            elfoglaltToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_Status_Busy);
            Language.ReloadEvent += delegate { elfoglaltToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_Status_Busy); };
            nincsAGépnélToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_Status_Away);
            Language.ReloadEvent += delegate { nincsAGépnélToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_Status_Away); };
            rejtveKapcsolódikToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_Status_Hidden);
            Language.ReloadEvent += delegate { rejtveKapcsolódikToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_Status_Hidden); };
            fájlKüldéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_SendFile);
            Language.ReloadEvent += delegate { fájlKüldéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_SendFile); };
            beérkezettFájlokMappájánakMegnyitásaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_OpenReceivedFiles);
            Language.ReloadEvent += delegate { beérkezettFájlokMappájánakMegnyitásaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_OpenReceivedFiles); };
            üzenetekElőzményeinekMegtekintéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_OpenRecentmsgs);
            Language.ReloadEvent += delegate { üzenetekElőzményeinekMegtekintéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_OpenRecentmsgs); };
            bezárásToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_Close);
            Language.ReloadEvent += delegate { bezárásToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_Close); };
            kilépésToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_Exit);
            Language.ReloadEvent += delegate { kilépésToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_File_Exit); };

            ismerősökToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Contacts);
            Language.ReloadEvent += delegate { ismerősökToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Contacts); };
            ismerősFelvételeToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Contacts_Add);
            Language.ReloadEvent += delegate { ismerősFelvételeToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Contacts_Add); };
            ismerősSzerkesztéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Contacts_Edit);
            Language.ReloadEvent += delegate { ismerősSzerkesztéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Contacts_Edit); };
            ismerősTörléseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Contacts_Remove);
            Language.ReloadEvent += delegate { ismerősTörléseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Contacts_Remove); };
            toolStripMenuItem3.Text = Language.Translate(Language.StringID.Menu_Contacts_Invite);
            Language.ReloadEvent += delegate { toolStripMenuItem3.Text = Language.Translate(Language.StringID.Menu_Contacts_Invite); };
            csoportLétrehozásaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Contacts_MakeGroup);
            Language.ReloadEvent += delegate { csoportLétrehozásaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Contacts_MakeGroup); };
            kategóriaLétrehozásaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Contacts_MakeCategory);
            Language.ReloadEvent += delegate { kategóriaLétrehozásaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Contacts_MakeCategory); };
            kategóriaSzerkesztéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Contacts_EditCategory);
            Language.ReloadEvent += delegate { kategóriaSzerkesztéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Contacts_EditCategory); };
            kategóriaTörléseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Contacts_RemoveCategory);
            Language.ReloadEvent += delegate { kategóriaTörléseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Contacts_RemoveCategory); };

            műveletekToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Operations);
            Language.ReloadEvent += delegate { műveletekToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Operations); };
            azonnaliÜzenetKüldéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Sendmsg) + "...";
            Language.ReloadEvent += delegate { azonnaliÜzenetKüldéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Sendmsg) + "..."; };
            egyébKüldéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Operations_SendOther);
            Language.ReloadEvent += delegate { egyébKüldéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Operations_SendOther); };
            emailKüldéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Operations_SendMail);
            Language.ReloadEvent += delegate { emailKüldéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Operations_SendMail); };
            fájlKüldéseToolStripMenuItem1.Text = Language.Translate(Language.StringID.Menu_File_SendFile); //Ugyanaz a szöveg
            Language.ReloadEvent += delegate { fájlKüldéseToolStripMenuItem1.Text = Language.Translate(Language.StringID.Menu_File_SendFile); };
            ismerősSzámitógépénekFelhivásaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Operations_CallContact);
            Language.ReloadEvent += delegate { ismerősSzámitógépénekFelhivásaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Operations_CallContact); };
            videóhivásInditásaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Operations_VideoCall);
            Language.ReloadEvent += delegate { videóhivásInditásaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Operations_VideoCall); };
            onlineFájlokMegtekintéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Operations_ShowOnlineFiles);
            Language.ReloadEvent += delegate { onlineFájlokMegtekintéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Operations_ShowOnlineFiles); };
            közösJátékToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Operations_PlayGame);
            Language.ReloadEvent += delegate { közösJátékToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Operations_PlayGame); };
            távsegitségKéréseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Operations_AskForHelp);
            Language.ReloadEvent += delegate { távsegitségKéréseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Operations_AskForHelp); };

            eszközökToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Tools);
            Language.ReloadEvent += delegate { eszközökToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Tools); };
            mindigLegfelülToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Tools_AlwaysOnTop);
            Language.ReloadEvent += delegate { mindigLegfelülToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Tools_AlwaysOnTop); };
            hangulatjelekToolStripMenuItem.Text = Language.Translate(Language.StringID.Emoticons) + "...";
            Language.ReloadEvent += delegate { hangulatjelekToolStripMenuItem.Text = Language.Translate(Language.StringID.Emoticons) + "..."; };
            megjelenitendőKépVáltásaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Tools_ChangeImage);
            Language.ReloadEvent += delegate { megjelenitendőKépVáltásaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Tools_ChangeImage); };
            háttérMódositásaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Tools_ChangeBackground);
            Language.ReloadEvent += delegate { háttérMódositásaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Tools_ChangeBackground); };
            hangokÉsVideóBeállitásaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Tools_VoiceVideoSettings);
            Language.ReloadEvent += delegate { hangokÉsVideóBeállitásaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Tools_VoiceVideoSettings); };
            beállitásokToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Tools_Settings);
            Language.ReloadEvent += delegate { beállitásokToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Tools_Settings); };
            szkriptíróToolStripMenuItem.Text = Language.Translate(Language.StringID.Scripter);
            Language.ReloadEvent += delegate { szkriptíróToolStripMenuItem.Text = Language.Translate(Language.StringID.Scripter); };

            súgóToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Help);
            Language.ReloadEvent += delegate { súgóToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Help); };
            témakörökToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Help_Contents);
            Language.ReloadEvent += delegate { témakörökToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Help_Contents); };
            aSzolgáltatásÁllapotsaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Help_Status);
            Language.ReloadEvent += delegate { aSzolgáltatásÁllapotsaToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Help_Status); };
            adatvédelmiNyilatkozatToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Help_PrivacyPolicy);
            Language.ReloadEvent += delegate { adatvédelmiNyilatkozatToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Help_PrivacyPolicy); };
            használatiFeltételekToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Help_TermsOfUse);
            Language.ReloadEvent += delegate { használatiFeltételekToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Help_TermsOfUse); };
            visszaélésBejelentéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Help_Report);
            Language.ReloadEvent += delegate { visszaélésBejelentéseToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Help_Report); };
            segitsenAProgramTökéletesitésébenToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Help_ImproveProgram);
            Language.ReloadEvent += delegate { segitsenAProgramTökéletesitésébenToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Help_ImproveProgram); };
            névjegyToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Help_About);
            Language.ReloadEvent += delegate { névjegyToolStripMenuItem.Text = Language.Translate(Language.StringID.Menu_Help_About); };

            textBox1.Text = Language.Translate(Language.StringID.SearchBar, textBox1);

            toolStripMenuItem4.Text = Language.Translate(Language.StringID.IconMenu_Show);
            Language.ReloadEvent += delegate { toolStripMenuItem4.Text = Language.Translate(Language.StringID.IconMenu_Show); };
            toolStripMenuItem8.Text = Language.Translate(Language.StringID.Menu_File_Logout);
            Language.ReloadEvent += delegate { toolStripMenuItem8.Text = Language.Translate(Language.StringID.Menu_File_Logout); };
            toolStripMenuItem9.Text = Language.Translate(Language.StringID.Menu_File_Exit);
            Language.ReloadEvent += delegate { toolStripMenuItem9.Text = Language.Translate(Language.StringID.Menu_File_Exit); };

            LoadMenu(MenuType.ChatIconMenu); //2014.12.12.
            LoadMenu(MenuType.PartnerMenu); //2014.12.13.
            #endregion

            //this.WindowState = FormWindowState.Minimized; //2014.04.19.
            //A betöltő kód áthelyezve a Load()-ba: 2015.05.23.

            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            PartnerListUpdateThread = new Thread(new ThreadStart(new Networking.UpdateListAndChat().Run));
            PartnerListUpdateThread.Name = "Update Partnerlist and Chat";

            //Thread networkthread = new Thread(new ThreadStart(Networking.NetworkThread)); //2014.12.31.
            //networkthread.Name = "Network Thread";

            if (Storage.Settings[SettingType.WindowState] == "1") //2014.04.18. - 2014.08.08.
                this.WindowState = FormWindowState.Maximized;
            else if (Storage.Settings[SettingType.WindowState] == "2")
                this.WindowState = FormWindowState.Minimized;
            else if (Storage.Settings[SettingType.WindowState] == "3")
                this.WindowState = FormWindowState.Normal;

            //taskbarNotifier = new Notifier("popup-bg.bmp", Color.FromArgb(255, 0, 255), "close.bmp", 5000);
            //TODO: Notifier

            //taskbarNotifier.Click += PopupClick;
            //taskbarNotifier.CloseClick += PopupCloseClick;

            toolStripMenuItem4.Enabled = true; //2014.04.12.
            toolStripMenuItem8.Enabled = true; //2014.04.12.

            ChangeChatWindowLayout(false);

            // Start the thread
            //PartnerListUpdateThread.Start();

            //networkthread.Start();

            //while (!networkthread.IsAlive) ;
            //SendLoginToUsers(); //2014.12.18.

            notifyIcon1.Visible = true; //2014.09.22.
            //taskbarNotifier.Show("Teszt cím", "Teszt tartalom\nMásodik sor");

            Language.ReloadEvent += delegate { textBox1.Text = Language.Translate(Language.StringID.SearchBar); }; //2014.12.22. - Nyelvváltáskor törölni fogja a beírt szöveget
        }

        public void ChangeChatWindowLayout(bool changed)
        { //2015.06.14.
            if (Storage.Settings[SettingType.ChatWindow] == "1")
            { //2015.06.14.
                flowLayoutPanel1.Hide();
                flowLayoutPanel2.Hide();
                MainPanel.Anchor = AnchorStyles.None;
                MainPanel.Location = new Point(0, MainPanel.Location.Y);
                MainPanel.Width = this.Width;
                MainPanel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
                this.Width -= flowLayoutPanel1.Width + flowLayoutPanel2.Width;
                this.SetDesktopLocation(this.DesktopLocation.X + flowLayoutPanel1.Width, this.DesktopLocation.Y);
            }
            else
            {
                if (changed)
                {
                    this.SetDesktopLocation(this.DesktopLocation.X - flowLayoutPanel1.Width, this.DesktopLocation.Y);
                    this.Width += flowLayoutPanel1.Width + flowLayoutPanel2.Width;
                    MainPanel.Anchor = AnchorStyles.None;
                    MainPanel.Location = new Point(flowLayoutPanel1.Width + 1, MainPanel.Location.Y);
                    MainPanel.Width = this.Width - flowLayoutPanel1.Width - flowLayoutPanel2.Width;
                    MainPanel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
                    flowLayoutPanel1.Show();
                    flowLayoutPanel2.Show();
                }
            }
        }

        private void SendLoginToUsers()
        { //2014.12.18.
            //var bytes = new List<byte>();
            /*for (int i = 0; i < UserInfo.KnownUsers.Count; i++) //Ha a count 0, nem fogja végrehajtani
            {
                //bytes.AddRange(BitConverter.GetBytes(UserInfo.KnownUsers[i].UserID));
                //bytes.AddRange(BitConverter.GetBytes(UserInfo.KnownUsers[i].LastUpdate));
            }*/
            /*var packet = new Networking.PacketFormat(false, new Networking.PDLoginUser(
                UserInfo.KnownUsers.Select(entry => new KeyValuePair<int, int>(entry.UserID, entry.LastUpdate))));
            var task = Networking.SendUpdateInThread(packet).ContinueWith(new Action<Task<Networking.PacketFormat[]>>(
                //(e, resp) => Networking.ParseUpdateInfo(resp)));
                //(e, resp) => Networking.ParseUpdateInfo(resp.Select(entry => ((Networking.PDLoginUser)Networking.PacketFormat.FromBytes(entry).EData).RStrings))));
                (t) => Networking.ParseUpdateInfo(t.Result.Select(entry => ((Networking.PDLoginUser)entry.EData).RStrings))));*/
            new Networking.PacketSender(new Networking.PDLoginUser(UserInfo.KnownUsers.Select(entry => new KeyValuePair<int, int>(entry.UserID, entry.LastUpdate))))
                .SendAsync().ContinueWith(new Action<Task<Networking.PacketFormat[]>>(
                (t) => Networking.ParseUpdateInfo(t.Result.Select(entry => ((Networking.PDLoginUser)entry.EData).RStrings))));
        }

        enum MenuType
        {
            ChatIconMenu,
            PartnerMenu
        }
        private void LoadMenu(MenuType mt)
        {
            switch (mt) //2014.12.13.
            {
                case MenuType.ChatIconMenu:
                    chatIconMenu.Items.Add(Language.Translate(Language.StringID.Close), null,
                        new EventHandler((sender, e) => ((ChatPanel)chatIconMenu.Tag).Close())); //Tag: A chatikon
                    chatIconMenu.Items[chatIconMenu.Items.Count - 1].Name = "close"; //2014.12.22.
                    Language.ReloadEvent += delegate { chatIconMenu.Items["close"].Text = Language.Translate(Language.StringID.Close); }; //2014.12.22.
                    chatIconMenu.Items.Add(new ToolStripSeparator());
                    LoadMenuPrep(chatIconMenu);
                    break;
                case MenuType.PartnerMenu:
                    listPartnerMenu.Items.Add(Language.Translate(Language.StringID.Sendmsg) + "...", null, PartnerMenu_SendMessage);
                    listPartnerMenu.Items[listPartnerMenu.Items.Count - 1].Name = "menu_operations_sendmsg"; //2014.12.22.
                    Language.ReloadEvent += delegate { listPartnerMenu.Items["menu_operations_sendmsg"].Text = Language.Translate(Language.StringID.Sendmsg) + ".."; }; //2014.12.22.
                    listPartnerMenu.Items.Add(new ToolStripSeparator());
                    LoadMenuPrep(listPartnerMenu);
                    break;
                default:
                    throw new NotImplementedException("Menu type not implemented.");
            }
        }
        private void LoadMenuPrep(ContextMenuStrip menu) //Csak a menüelemeket készíti elő
        { //2014.12.13.
            menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { //A dizájnerből, átalakítva, hogy rögtön le is fordítsa
            new ToolStripMenuItem(Language.Translate(Language.StringID.Contact_SendEmail)), //Ide jönnek majd az event handlerek is
            new ToolStripMenuItem(Language.Translate(Language.StringID.Contact_CopyEmail)),
            new ToolStripMenuItem(Language.Translate(Language.StringID.Contact_Info)),
            new ToolStripSeparator(),
            new ToolStripMenuItem(Language.Translate(Language.StringID.Contact_Block)),
            new ToolStripMenuItem(Language.Translate(Language.StringID.Contact_Remove)),
            new ToolStripSeparator(),
            new ToolStripMenuItem(Language.Translate(Language.StringID.Contact_EditName)),
            new ToolStripSeparator(),
            new ToolStripMenuItem(Language.Translate(Language.StringID.Contact_EventNotifications)),
            new ToolStripSeparator(),
            new ToolStripMenuItem(Language.Translate(Language.StringID.Contact_OpenChatLog))});
            Language.ReloadEvent += delegate
            {
                int i = menu.Items.Count - 1;
                menu.Items[i].Text = Language.Translate(Language.StringID.Contact_OpenChatLog);
                i--;
                menu.Items[i].Text = Language.Translate(Language.StringID.Contact_EventNotifications);
                i--;
                menu.Items[i].Text = Language.Translate(Language.StringID.Contact_EditName);
                i--;
                menu.Items[i].Text = Language.Translate(Language.StringID.Contact_Remove);
                i--;
                menu.Items[i].Text = Language.Translate(Language.StringID.Contact_Block);
                i--;
                menu.Items[i].Text = Language.Translate(Language.StringID.Contact_Info);
                i--;
                menu.Items[i].Text = Language.Translate(Language.StringID.Contact_CopyEmail);
                i--;
                menu.Items[i].Text = Language.Translate(Language.StringID.Contact_SendEmail);
            };
        }

        public void LoadPartnerList() //2014.08.28.
        {
            contactList.AutoUpdate = false;
            UserInfo.AutoUpdate = false; //2014.09.26.
            string[] list = Networking.SendRequest(Networking.RequestType.GetList, "", 0, true).Split(new char[] { 'ͦ' }, StringSplitOptions.RemoveEmptyEntries); //2014.09.26.
            if (list[0].Contains("Fail"))
                MessageBox.Show(list[0]);

            if (!UserInfo.KnownUsers.Any(entry => entry.UserID == CurrentUser.UserID))
            {
                var tmpc = new UserInfo(); //2015.05.15.
                tmpc.UserID = CurrentUser.UserID; //2015.05.15.
                tmpc.UserName = CurrentUser.UserName; //2015.05.15.
                tmpc.LastUpdate = 0; //2015.05.15.
                tmpc.Name = CurrentUser.Name; //2015.05.15.
                tmpc.IsPartner = false; //2015.05.15.
                tmpc.Image = CurrentUser.Image; //2015.06.06.
                UserInfo.KnownUsers.Add(tmpc); //2015.05.15.
            }

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
                    UserInfo.Select(uid).State = 0; //2015.06.25.
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
            PartnerListUpdateThread.Abort();//2015.06.16.
            PartnerListUpdateThread = null;
            CurrentUser.SendChanges = false; //2014.08.30.
            while (ChatPanel.ChatWindows.Count > 0)
            { //2014.09.06. - A Close() hatására törli a gyűjteményből, ezért sorra végig fog haladni rajta
                ChatPanel.ChatWindows[0].Close();
            }
            //LoginDialog = new LoginForm(); //2014.04.04.
            var LoginDialog = new LoginForm(); //2015.05.23.
            LoginDialog.ShowDialog();
            //Nézzük, sikerült-e
            if (CurrentUser.UserID == 0)
                Close();
            Storage.Load(true); //2014.08.07.
            toolStripMenuItem4.Enabled = true; //2014.04.12.
            toolStripMenuItem8.Enabled = true; //2014.04.12.
            CurrentUser.SendChanges = true; //2014.08.30.
            contactList.Items.Clear(); //2014.10.09. - Kijelentkezéskor hozzáad egy üres listelemet egy (Nem elérhető) felirattal, ezt tünteti el
            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            PartnerListUpdateThread = new Thread(new ThreadStart(new Networking.UpdateListAndChat().Run));
            //PartnerListUpdateThread.Name = "Update Partner List";
            PartnerListUpdateThread.Name = "Update Partnerlist and Chat"; //2015.06.30.

            // Start the thread
            PartnerListUpdateThread.Start();

            SendLoginToUsers(); //2014.12.18.

            LoadPartnerList();
            this.Show();
        }

        public void PlaceChatIcon(ChatPanel cp)
        {
            var newicon = new PictureBox();
            //newicon.ImageLocation = cp.ChatPartners[0].ImagePath; //2014.12.31.
            if (cp.ChatPartners[0].Image == null)
                newicon.Image = UserInfo.NoImage; //2015.05.30.
            else
                newicon.Image = cp.ChatPartners[0].Image; //2015.05.30.
            newicon.Size = new Size(100, 100);
            newicon.SizeMode = PictureBoxSizeMode.Zoom;
            newicon.Click += new EventHandler((a, b) => cp.Show());
            newicon.MouseClick += new MouseEventHandler((s, e) =>
            {
                if (e.Button == MouseButtons.Middle)
                    cp.Close();
                else if (e.Button == MouseButtons.Right) //Chat menü
                { //2014.12.13.
                    chatIconMenu.Tag = cp;
                    chatIconMenu.Show(Cursor.Position);
                }
            });
            cp.ChatIcon = newicon;
            //---------------------------------------------------------------------
            int size = 0;
            bool putright = false;
            foreach (Control item in flowLayoutPanel1.Controls)
            {
                size += item.Size.Height;
            }
            size += newicon.Size.Height;
            if (size > flowLayoutPanel1.Size.Height)
                putright = true; //Ha nem fér el bal oldalt, rakja jobbra
            //---------------------------------------------------------------------
            size = 0;
            foreach (Control item in flowLayoutPanel2.Controls)
            {
                size += item.Size.Height;
            }
            size += newicon.Size.Height;
            if (size > flowLayoutPanel2.Size.Height)
                putright = false; //Ha jobbra sem fér el, csak rakja balra
            //---------------------------------------------------------------------
            if (!putright)
                flowLayoutPanel1.Controls.Add(newicon);
            else
                flowLayoutPanel2.Controls.Add(newicon);
        }

        public enum StatType
        {
            MainServer,
            Servers,
            OnlineServers
        }
        public void UpdateStats(StatType type, int value)
        { //Elvileg ha van forgalom, gyorsan frissíti a nyelvet is
            switch (type)
            {
                case StatType.MainServer:
                    if (value == 0)
                    {
                        mainserver.Text = Language.Translate(Language.StringID.Stats_MainServer) + ": " + Language.Translate(Language.StringID.Stats_NoNetwork);
                        mainserver.ForeColor = Color.Red;
                    }
                    else if (value == 1)
                    {
                        mainserver.Text = Language.Translate(Language.StringID.Stats_MainServer) + ": " + Language.Translate(Language.StringID.Stats_Retrying);
                        mainserver.ForeColor = Color.Orange;
                    }
                    else if (value == 2)
                    {
                        mainserver.Text = Language.Translate(Language.StringID.Stats_MainServer) + ": " + Language.Translate(Language.StringID.Stats_Connected);
                        mainserver.ForeColor = Color.Green;
                    }
                    break;
                case StatType.Servers:
                    servers.Text = Language.Translate(Language.StringID.Stats_Servers) + ": " + value;
                    break;
                case StatType.OnlineServers:
                    onlineservers.Text = Language.Translate(Language.StringID.Stats_OnlineServers) + ": " + value;
                    break;
            }
        }
    }
}
