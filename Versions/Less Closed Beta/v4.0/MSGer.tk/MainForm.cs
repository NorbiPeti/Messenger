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
        public static LoginForm LoginDialog;
        public static Thread LThread;
        public static Thread MainThread = null;
        public static Notifier taskbarNotifier;
        public MainForm()
        {
            BeforeLogin.SetText("Starting...");
            Program.MainF = this;
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
            Language.ReloadEvent += delegate { fájlToolStripMenuItem.Text = Language.Translate("menu_file"); };
            kijelentkezésToolStripMenuItem.Text = Language.Translate("menu_file_logout");
            Language.ReloadEvent += delegate { kijelentkezésToolStripMenuItem.Text = Language.Translate("menu_file_logout"); };
            toolStripMenuItem1.Text = Language.Translate("menu_file_loginnewuser");
            Language.ReloadEvent += delegate { toolStripMenuItem1.Text = Language.Translate("menu_file_loginnewuser"); };
            állapotToolStripMenuItem.Text = Language.Translate("menu_file_status");
            Language.ReloadEvent += delegate { állapotToolStripMenuItem.Text = Language.Translate("menu_file_status"); };
            elérhetőToolStripMenuItem.Text = Language.Translate("menu_file_status_online");
            Language.ReloadEvent += delegate { elérhetőToolStripMenuItem.Text = Language.Translate("menu_file_status_online"); };
            elfoglaltToolStripMenuItem.Text = Language.Translate("menu_file_status_busy");
            Language.ReloadEvent += delegate { elfoglaltToolStripMenuItem.Text = Language.Translate("menu_file_status_busy"); };
            nincsAGépnélToolStripMenuItem.Text = Language.Translate("menu_file_status_away");
            Language.ReloadEvent += delegate { nincsAGépnélToolStripMenuItem.Text = Language.Translate("menu_file_status_away"); };
            rejtveKapcsolódikToolStripMenuItem.Text = Language.Translate("menu_file_status_hidden");
            Language.ReloadEvent += delegate { rejtveKapcsolódikToolStripMenuItem.Text = Language.Translate("menu_file_status_hidden"); };
            fájlKüldéseToolStripMenuItem.Text = Language.Translate("menu_file_sendfile");
            Language.ReloadEvent += delegate { fájlKüldéseToolStripMenuItem.Text = Language.Translate("menu_file_sendfile"); };
            beérkezettFájlokMappájánakMegnyitásaToolStripMenuItem.Text = Language.Translate("menu_file_openreceivedfiles");
            Language.ReloadEvent += delegate { beérkezettFájlokMappájánakMegnyitásaToolStripMenuItem.Text = Language.Translate("menu_file_openreceivedfiles"); };
            üzenetekElőzményeinekMegtekintéseToolStripMenuItem.Text = Language.Translate("menu_file_openrecentmsgs");
            Language.ReloadEvent += delegate { üzenetekElőzményeinekMegtekintéseToolStripMenuItem.Text = Language.Translate("menu_file_openrecentmsgs"); };
            bezárásToolStripMenuItem.Text = Language.Translate("menu_file_close");
            Language.ReloadEvent += delegate { bezárásToolStripMenuItem.Text = Language.Translate("menu_file_close"); };
            kilépésToolStripMenuItem.Text = Language.Translate("menu_file_exit");
            Language.ReloadEvent += delegate { kilépésToolStripMenuItem.Text = Language.Translate("menu_file_exit"); };

            ismerősökToolStripMenuItem.Text = Language.Translate("menu_contacts");
            Language.ReloadEvent += delegate { ismerősökToolStripMenuItem.Text = Language.Translate("menu_contacts"); };
            ismerősFelvételeToolStripMenuItem.Text = Language.Translate("menu_contacts_add");
            Language.ReloadEvent += delegate { ismerősFelvételeToolStripMenuItem.Text = Language.Translate("menu_contacts_add"); };
            ismerősSzerkesztéseToolStripMenuItem.Text = Language.Translate("menu_contacts_edit");
            Language.ReloadEvent += delegate { ismerősSzerkesztéseToolStripMenuItem.Text = Language.Translate("menu_contacts_edit"); };
            ismerősTörléseToolStripMenuItem.Text = Language.Translate("menu_contacts_remove");
            Language.ReloadEvent += delegate { ismerősTörléseToolStripMenuItem.Text = Language.Translate("menu_contacts_remove"); };
            toolStripMenuItem3.Text = Language.Translate("menu_contacts_invite");
            Language.ReloadEvent += delegate { toolStripMenuItem3.Text = Language.Translate("menu_contacts_invite"); };
            csoportLétrehozásaToolStripMenuItem.Text = Language.Translate("menu_contacts_makegroup");
            Language.ReloadEvent += delegate { csoportLétrehozásaToolStripMenuItem.Text = Language.Translate("menu_contacts_makegroup"); };
            kategóriaLétrehozásaToolStripMenuItem.Text = Language.Translate("menu_contacts_makecategory");
            Language.ReloadEvent += delegate { kategóriaLétrehozásaToolStripMenuItem.Text = Language.Translate("menu_contacts_makecategory"); };
            kategóriaSzerkesztéseToolStripMenuItem.Text = Language.Translate("menu_contacts_editcategory");
            Language.ReloadEvent += delegate { kategóriaSzerkesztéseToolStripMenuItem.Text = Language.Translate("menu_contacts_editcategory"); };
            kategóriaTörléseToolStripMenuItem.Text = Language.Translate("menu_contacts_removecategory");
            Language.ReloadEvent += delegate { kategóriaTörléseToolStripMenuItem.Text = Language.Translate("menu_contacts_removecategory"); };

            műveletekToolStripMenuItem.Text = Language.Translate("menu_operations");
            Language.ReloadEvent += delegate { műveletekToolStripMenuItem.Text = Language.Translate("menu_operations"); };
            azonnaliÜzenetKüldéseToolStripMenuItem.Text = Language.Translate("menu_operations_sendmsg");
            Language.ReloadEvent += delegate { azonnaliÜzenetKüldéseToolStripMenuItem.Text = Language.Translate("menu_operations_sendmsg"); };
            egyébKüldéseToolStripMenuItem.Text = Language.Translate("menu_operations_sendother");
            Language.ReloadEvent += delegate { egyébKüldéseToolStripMenuItem.Text = Language.Translate("menu_operations_sendother"); };
            emailKüldéseToolStripMenuItem.Text = Language.Translate("menu_operations_sendother_sendmail");
            Language.ReloadEvent += delegate { emailKüldéseToolStripMenuItem.Text = Language.Translate("menu_operations_sendother_sendmail"); };
            fájlKüldéseToolStripMenuItem1.Text = Language.Translate("menu_file_sendfile"); //Ugyanaz a szöveg
            Language.ReloadEvent += delegate { fájlKüldéseToolStripMenuItem1.Text = Language.Translate("menu_file_sendfile"); };
            ismerősSzámitógépénekFelhivásaToolStripMenuItem.Text = Language.Translate("menu_operations_callcontact");
            Language.ReloadEvent += delegate { ismerősSzámitógépénekFelhivásaToolStripMenuItem.Text = Language.Translate("menu_operations_callcontact"); };
            videóhivásInditásaToolStripMenuItem.Text = Language.Translate("menu_operations_videocall");
            Language.ReloadEvent += delegate { videóhivásInditásaToolStripMenuItem.Text = Language.Translate("menu_operations_videocall"); };
            onlineFájlokMegtekintéseToolStripMenuItem.Text = Language.Translate("menu_operations_showonlinefiles");
            Language.ReloadEvent += delegate { onlineFájlokMegtekintéseToolStripMenuItem.Text = Language.Translate("menu_operations_showonlinefiles"); };
            közösJátékToolStripMenuItem.Text = Language.Translate("menu_operations_playgame");
            Language.ReloadEvent += delegate { közösJátékToolStripMenuItem.Text = Language.Translate("menu_operations_playgame"); };
            távsegitségKéréseToolStripMenuItem.Text = Language.Translate("menu_operations_askforhelp");
            Language.ReloadEvent += delegate { távsegitségKéréseToolStripMenuItem.Text = Language.Translate("menu_operations_askforhelp"); };

            eszközökToolStripMenuItem.Text = Language.Translate("menu_tools");
            Language.ReloadEvent += delegate { eszközökToolStripMenuItem.Text = Language.Translate("menu_tools"); };
            mindigLegfelülToolStripMenuItem.Text = Language.Translate("menu_tools_alwaysontop");
            Language.ReloadEvent += delegate { mindigLegfelülToolStripMenuItem.Text = Language.Translate("menu_tools_alwaysontop"); };
            hangulatjelekToolStripMenuItem.Text = Language.Translate("menu_tools_emoticons");
            Language.ReloadEvent += delegate { hangulatjelekToolStripMenuItem.Text = Language.Translate("menu_tools_emoticons"); };
            megjelenitendőKépVáltásaToolStripMenuItem.Text = Language.Translate("menu_tools_changeimage");
            Language.ReloadEvent += delegate { megjelenitendőKépVáltásaToolStripMenuItem.Text = Language.Translate("menu_tools_changeimage"); };
            háttérMódositásaToolStripMenuItem.Text = Language.Translate("menu_tools_changebackground");
            Language.ReloadEvent += delegate { háttérMódositásaToolStripMenuItem.Text = Language.Translate("menu_tools_changebackground"); };
            hangokÉsVideóBeállitásaToolStripMenuItem.Text = Language.Translate("menu_tools_voicevideosettings");
            Language.ReloadEvent += delegate { hangokÉsVideóBeállitásaToolStripMenuItem.Text = Language.Translate("menu_tools_voicevideosettings"); };
            beállitásokToolStripMenuItem.Text = Language.Translate("menu_tools_settings");
            Language.ReloadEvent += delegate { beállitásokToolStripMenuItem.Text = Language.Translate("menu_tools_settings"); };

            súgóToolStripMenuItem.Text = Language.Translate("menu_help");
            Language.ReloadEvent += delegate { súgóToolStripMenuItem.Text = Language.Translate("menu_help"); };
            témakörökToolStripMenuItem.Text = Language.Translate("menu_help_contents");
            Language.ReloadEvent += delegate { témakörökToolStripMenuItem.Text = Language.Translate("menu_help_contents"); };
            aSzolgáltatásÁllapotsaToolStripMenuItem.Text = Language.Translate("menu_help_status");
            Language.ReloadEvent += delegate { aSzolgáltatásÁllapotsaToolStripMenuItem.Text = Language.Translate("menu_help_status"); };
            adatvédelmiNyilatkozatToolStripMenuItem.Text = Language.Translate("menu_help_privacypolicy");
            Language.ReloadEvent += delegate { adatvédelmiNyilatkozatToolStripMenuItem.Text = Language.Translate("menu_help_privacypolicy"); };
            használatiFeltételekToolStripMenuItem.Text = Language.Translate("menu_help_termsofuse");
            Language.ReloadEvent += delegate { használatiFeltételekToolStripMenuItem.Text = Language.Translate("menu_help_termsofuse"); };
            visszaélésBejelentéseToolStripMenuItem.Text = Language.Translate("menu_help_report");
            Language.ReloadEvent += delegate { visszaélésBejelentéseToolStripMenuItem.Text = Language.Translate("menu_help_report"); };
            segitsenAProgramTökéletesitésébenToolStripMenuItem.Text = Language.Translate("menu_help_improveprogram");
            Language.ReloadEvent += delegate { segitsenAProgramTökéletesitésébenToolStripMenuItem.Text = Language.Translate("menu_help_improveprogram"); };
            névjegyToolStripMenuItem.Text = Language.Translate("menu_help_about");
            Language.ReloadEvent += delegate { névjegyToolStripMenuItem.Text = Language.Translate("menu_help_about"); };

            textBox1.Text = Language.Translate("searchbar", textBox1);
            //contactList.Items[0].SubItems[0].Text = Language.Translate("loading"); - 2014.08.28. - Nincs már rá szükség (hibát is ír, mivel nincs listaelem)

            //üzenetküldésToolStripMenuItem.Text = Language.Translate("menu_operations_sendmsg");
            //emailKüldéseemailToolStripMenuItem.Text = Language.Translate("contact_sendemail");
            //toolStripMenuItem2.Text = Language.Translate("contact_copyemail");
            //információToolStripMenuItem.Text = Language.Translate("contact_info");
            //ismerősLetiltásaToolStripMenuItem.Text = Language.Translate("contact_block");
            //ismerősTörléseToolStripMenuItem.Text = Language.Translate("contact_remove"); - Kétszer benne volt, ugyanilyen névvel, csak 1-re végződve lett volna a megfelelő
            //becenévSzerkesztéseToolStripMenuItem.Text = Language.Translate("contact_editname");
            //eseményértesitésekToolStripMenuItem.Text = Language.Translate("contact_eventnotifications");
            //beszélgetésnaplóMegnyitásaToolStripMenuItem.Text = Language.Translate("contact_openchatlog");

            toolStripMenuItem4.Text = Language.Translate("iconmenu_show");
            Language.ReloadEvent += delegate { toolStripMenuItem4.Text = Language.Translate("iconmenu_show"); };
            toolStripMenuItem8.Text = Language.Translate("menu_file_logout");
            Language.ReloadEvent += delegate { toolStripMenuItem8.Text = Language.Translate("menu_file_logout"); };
            toolStripMenuItem9.Text = Language.Translate("menu_file_exit");
            Language.ReloadEvent += delegate { toolStripMenuItem9.Text = Language.Translate("menu_file_exit"); };
            //}
            //catch
            //{
            //MessageBox.Show("Error while loading translations.");
            //}

            LoadMenu(MenuType.ChatIconMenu); //2014.12.12.
            LoadMenu(MenuType.PartnerMenu); //2014.12.13.
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
                    //System.Diagnostics.Process.Start("http://msger.url.ph/download.php?version=latest");
                    //(new UpdateDialog()).Show(); //2014.12.13. - Elvileg át lehet nevezni a programot, miközben fut (ami érdekes) - De inkább csinálok külön programot
                { //2014.12.13.
                    Process.Start("Updater.exe", "\"" + Language.Translate("updater") + "\" \"" + Language.Translate("updater_info") + "\"");
                    Program.Exit(false);
                }
            }
            else if (response != "fine")
                MessageBox.Show(Language.Translate("error") + ": " + response);

            //BeforeLogin.SetText(Language.Translate("beforelogin_server")); //2015.01.07.

            //2014.09.06.
            /*if (Storage.Settings["isserver"] == "")
            {
                if (MessageBox.Show(Language.Translate("isserver_msg"), "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Storage.Settings["isserver"] = "1";
                else
                    Storage.Settings["isserver"] = "0";
            }*/

            /*if(Storage.Settings["isserver"]=="1") //2015.01.07.
            {
                NATUPNPLib.UPnPNAT upnpnat = new NATUPNPLib.UPnPNAT();
                NATUPNPLib.IStaticPortMappingCollection mappings = upnpnat.StaticPortMappingCollection;
                if (mappings == null)
                {
                    //MessageBox.Show(Language.Translate("portforward_noaccess"));
                }
                //else
                //{
                    foreach (NATUPNPLib.IStaticPortMapping mapping in mappings)
                    {
                        if (mapping.Protocol == "UDP" && mapping.InternalPort.ToString() == Storage.Settings["port"])
                        {
                            if (MessageBox.Show(Language.Translate("portforward_existsremove"), "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                mappings.Remove(mapping.ExternalPort, "UDP");
                            else
                                Storage.Settings["port"] = (Int32.Parse(Storage.Settings["port"]) + 1).ToString();
                        }

                    }
                    int port = int.Parse(Storage.Settings["port"]);
                    mappings.Add(port, "UDP", port, Dns.GetHostEntry(Dns.GetHostName()).AddressList.Single(entry =>
                    entry.AddressFamily == AddressFamily.InterNetwork
                        && (entry.ToString().Contains("192.168.0.") || entry.ToString().Contains("192.168.1.") || entry.ToString().Contains("10.0.0.") || entry.ToString().Contains("172.16.0.")) //Helyi IP-k
                        ).ToString(), true, "MSGer.tk chat program");
                    Networking.ReceiverConnection = new UdpClient(Int32.Parse(Storage.Settings["port"]));
                //}
            }*/

            //TO!DO: Nem kell az "isserver" beállítás, először kliensként próbáljon meg csatlakozni, majd szerverként fogadja az új klienseket
            // a pwnat segítségével
            //2015.03.15. - Nem kell az sem: IPv6 - A legtöbb eszköz már támogatja

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

            /*Thread keepupthread = new Thread(new ThreadStart(Networking.KeepUpThread));
            keepupthread.Name = "Keep Up Thread";
            
            Thread keepupuserst = new Thread(new ThreadStart(Networking.KeepUpUsersThread)); //2014.09.26.
            keepupuserst.Name = "Keep Up Users Thread";*/

            Thread networkthread = new Thread(new ThreadStart(Networking.NetworkThread)); //2014.12.31.
            networkthread.Name = "Network Thread";

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

            //keepupthread.Start();

            //keepupuserst.Start();

            networkthread.Start();

            //2014.08.19. - Küldje el a bejelentkezés hírét, hogy frissítéseket kapjon
            /*byte[] strb = Encoding.Unicode.GetBytes(CurrentUser.IP.ToString());
            byte[] tmpfinal = new byte[8 * UserInfo.KnownUsers.Count + strb.Length + 4 + 1]; //Hosszúság, IP, ismert felh. ID, frissítési idő
            Array.Copy(BitConverter.GetBytes(strb.Length), tmpfinal, 4);
            Array.Copy(strb, 0, tmpfinal, 4, strb.Length);
            //if (tmpfinal.Length != 0)
            for (int i = 0; i < UserInfo.KnownUsers.Count; i++) //Ha a count 0, nem fogja végrehajtani
            {
                byte[] tmptmp = BitConverter.GetBytes(UserInfo.KnownUsers[i].UserID);
                Array.Copy(tmptmp, 0, tmpfinal, i * 4 + strb.Length + 4, 4);
                tmptmp = BitConverter.GetBytes(UserInfo.KnownUsers[i].LastUpdate);
                Array.Copy(tmptmp, 0, tmpfinal, i * 4 + strb.Length + 4, 4);
            }
            tmpfinal[tmpfinal.Length - 1] = (Storage.Settings["isserver"] == "1") ? (byte)0x01 : (byte)0x00; //Mivel bejelentkezéstől függetlenül menti el, gépfüggő, hogy itt mit küld el
            Networking.ParseUpdateInfo(Networking.SendUpdate(Networking.UpdateType.LoginUser, tmpfinal, false));*/

            while (!networkthread.IsAlive) ;
            SendLoginToUsers(); //2014.12.18.

            notifyIcon1.Visible = true; //2014.09.22.
            taskbarNotifier.Show("Teszt cím", "Teszt tartalom\nMásodik sor");

            Language.ReloadEvent += delegate { textBox1.Text = Language.Translate("searchbar"); }; //2014.12.22. - Nyelvváltáskor törölni fogja a beírt szöveget
        }

        private void SendLoginToUsers()
        { //2014.12.18.
            var bytes = new List<byte>();
            //var tmpb = Encoding.Unicode.GetBytes(CurrentUser.IP.ToString());
            //bytes.AddRange(BitConverter.GetBytes(tmpb.Length));
            //bytes.AddRange(tmpb);
            for (int i = 0; i < UserInfo.KnownUsers.Count; i++) //Ha a count 0, nem fogja végrehajtani
            {
                bytes.AddRange(BitConverter.GetBytes(UserInfo.KnownUsers[i].UserID));
                bytes.AddRange(BitConverter.GetBytes(UserInfo.KnownUsers[i].LastUpdate));
            }
            //bytes.Add((Storage.Settings["isserver"] == "1") ? (byte)0x01 : (byte)0x00); //Mivel bejelentkezéstől függetlenül menti el, gépfüggő, hogy itt mit küld el
            //Networking.ParseUpdateInfo(Networking.SendUpdate(Networking.UpdateType.LoginUser, bytes.ToArray(), false));
            Networking.SendUpdateInThread(Networking.UpdateType.LoginUser, bytes.ToArray(), new EventHandler<byte[][]>(
                (e, resp) => Networking.ParseUpdateInfo(resp)));
        }

        enum MenuType
        {
            ChatIconMenu,
            PartnerMenu
        }
        private void LoadMenu(MenuType mt)
        {
            switch(mt) //2014.12.13.
            {
                case MenuType.ChatIconMenu:
                    chatIconMenu.Items.Add(Language.Translate("close"), null,
                        new EventHandler((sender, e) => ((ChatPanel)chatIconMenu.Tag).Close())); //Tag: A chatikon
                    chatIconMenu.Items[chatIconMenu.Items.Count-1].Name="close"; //2014.12.22.
                    Language.ReloadEvent += delegate { chatIconMenu.Items["close"].Text = Language.Translate("close"); }; //2014.12.22.
                    chatIconMenu.Items.Add(new ToolStripSeparator());
                    /*chatIconMenu.Items.AddRange(
                        partnerMenu.Items.Cast<ToolStripItem>().Select(entry=>{
                            return new ToolStripMenuItem((ToolStripItem)entry.Clone();
                        }).ToArray()); //Alapvetően eltávolítaná az eredeti menüből, és hibát jelezne*/
                    LoadMenuPrep(chatIconMenu);
                    break;
                case MenuType.PartnerMenu:
                    listPartnerMenu.Items.Add(Language.Translate("menu_operations_sendmsg"), null, PartnerMenu_SendMessage);
                    listPartnerMenu.Items[listPartnerMenu.Items.Count-1].Name="menu_operations_sendmsg"; //2014.12.22.
                    Language.ReloadEvent += delegate { listPartnerMenu.Items["menu_operations_sendmsg"].Text = Language.Translate("menu_operations_sendmsg"); }; //2014.12.22.
                    listPartnerMenu.Items.Add(new ToolStripSeparator());
                    //listPartnerMenu.Items.AddRange(partnerMenu.Items.Cast<ToolStripItem>().ToArray());
                    LoadMenuPrep(listPartnerMenu);
                    break;
                default:
                    throw new NotImplementedException("Menu type not implemented.");
            }
        }
        private void LoadMenuPrep(ContextMenuStrip menu) //Csak a menüelemeket készíti elő
        { //2014.12.13.
            menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { //A dizájnerből, átalakítva, hogy rögtön le is fordítsa
            new ToolStripMenuItem(Language.Translate("contact_sendemail")), //Ide jönnek majd az event handlerek is
            new ToolStripMenuItem(Language.Translate("contact_copyemail")),
            new ToolStripMenuItem(Language.Translate("contact_info")),
            new ToolStripSeparator(),
            new ToolStripMenuItem(Language.Translate("contact_block")),
            new ToolStripMenuItem(Language.Translate("contact_remove")),
            new ToolStripSeparator(),
            new ToolStripMenuItem(Language.Translate("contact_editname")),
            new ToolStripSeparator(),
            new ToolStripMenuItem(Language.Translate("contact_eventnotifications")),
            new ToolStripSeparator(),
            new ToolStripMenuItem(Language.Translate("contact_openchatlog"))});
            Language.ReloadEvent += delegate
            {
                int i = menu.Items.Count - 1;
                menu.Items[i].Text = Language.Translate("contact_openchatlog");
                i--;
                menu.Items[i].Text = Language.Translate("contact_eventnotifications");
                i--;
                menu.Items[i].Text = Language.Translate("contact_editname");
                i--;
                menu.Items[i].Text = Language.Translate("contact_remove");
                i--;
                menu.Items[i].Text = Language.Translate("contact_block");
                i--;
                menu.Items[i].Text = Language.Translate("contact_info");
                i--;
                menu.Items[i].Text = Language.Translate("contact_copyemail");
                i--;
                menu.Items[i].Text = Language.Translate("contact_sendemail");
            };
        }

        public void LoadPartnerList() //2014.08.28.
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
            while (ChatPanel.ChatWindows.Count > 0)
            { //2014.09.06. - A Close() hatására törli a gyűjteményből, ezért sorra végig fog haladni rajta
                ChatPanel.ChatWindows[0].Close();
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
            //LoadPartnerList();
            //this.Show();
            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            LThread = new Thread(new ThreadStart(new UpdateListAndChat().Run));
            LThread.Name = "Update Partner List";

            // Start the thread
            LThread.Start();

            SendLoginToUsers(); //2014.12.18.

            LoadPartnerList();
            this.Show();
        }

        public void PlaceChatIcon(ChatPanel cp)
        {
            var newicon = new PictureBox();
            //newicon.ImageLocation = cp.ChatPartners[0].GetImage();
            newicon.ImageLocation = cp.ChatPartners[0].ImagePath; //2014.12.31.
            newicon.Size = new Size(100, 100);
            newicon.SizeMode = PictureBoxSizeMode.Zoom;
            newicon.Click += new EventHandler((a, b) => cp.Show());
            newicon.MouseClick += new MouseEventHandler((s, e) => {
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
                        mainserver.Text = Language.Translate("stats_mainserver") + ": " + Language.Translate("stats_nonetwork");
                        mainserver.ForeColor = Color.Red;
                    }
                    else if (value == 1)
                    {
                        mainserver.Text = Language.Translate("stats_mainserver") + ": " + Language.Translate("stats_retrying");
                        mainserver.ForeColor = Color.Orange;
                    }
                    else if (value == 2)
                    {
                        mainserver.Text = Language.Translate("stats_mainserver") + ": " + Language.Translate("stats_connected");
                        mainserver.ForeColor = Color.Green;
                    }
                    break;
                case StatType.Servers:
                    servers.Text = Language.Translate("stats_servers") + ": " + value;
                    break;
                case StatType.OnlineServers:
                    onlineservers.Text = Language.Translate("stats_onlineservers") + ": " + value;
                    break;
            }
        }
    }
}
