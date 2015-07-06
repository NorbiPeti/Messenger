using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using HdSystemLibrary.IO;
using System.Diagnostics;
using Handwriting_program;

namespace MSGer.tk
{
    public partial class ChatPanel : UserControl
    {
        public static List<ChatPanel> ChatWindows = new List<ChatPanel>();
        public List<UserInfo> ChatPartners = new List<UserInfo>();
        private string chatname = "";
        public string ChatName //2014.12.13. - A beszélgetés neve
        {
            get
            {
                return chatname;
            }
            set
            {
                chatname = value;
                this.Text = chatname;
            }
        }
        public ChatPanel()
        {
            InitializeComponent();
            //Amint létrehozom, ez a kód lefut - Nem számit, hogy megjelenik-e

            this.Text = Language.Translate(Language.StringID.Chat_Title, this);
            showicons.Text = Language.Translate(Language.StringID.Chat_ShowIcons, showicons);
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            if (ChatPartners.Count == 0)
            {
                MessageBox.Show(Language.Translate(Language.StringID.Error) + ": " + Language.Translate(Language.StringID.Chat_NoWindow));
                Close(); //2015.05.21.
                return; //2015.05.21. - Ezesetben ne folytassa
            }
            if (ChatName.Length == 0)
            {
                this.Text = "";
                foreach (var item in ChatPartners)
                    this.Text += item.Name + ", ";
                this.Text = this.Text.Remove(this.Text.Length - 2);
                partnerName.Text = this.Text;
                this.Text += " - " + Language.Translate(Language.StringID.Chat_Title);
                Language.ReloadEvent += delegate
                {
                    if (ChatName.Length != 0)
                        return;
                    this.Text = "";
                    foreach (var item in ChatPartners)
                        this.Text += item.Name + ", ";
                    this.Text = this.Text.Remove(this.Text.Length - 2);
                    partnerName.Text = this.Text;
                    this.Text += " - " + Language.Translate(Language.StringID.Chat_Title);
                };
            }
            else
            {
                this.Text = ChatName;
            }
            Parent.Parent.Text = this.Text; //2014.12.22.
            messageTextBox.Select();
        }

        public bool InternalMessageChange = false;
        public int SelectionStart = 0;
        public int SelectionLength = 0;
        public int TextLength = 0;
        private async void SendMessage(object sender, KeyEventArgs e)
        {
            //SendMessage
            //if (e.KeyCode != Keys.Enter || e.Shift || messageTextBox.Text.Length == 0 || !messageTextBox.Visible) //Visible: 2014.11.07.
            if (e.KeyCode != Keys.Enter || e.Shift || !messageTextBox.Visible)
                return;
            e.SuppressKeyPress = true; //2015.05.21.
            if (messageTextBox.Text.Length == 0)
            { //2015.05.21.
                return;
            }
            messageTextBox.ReadOnly = true;
            double time = Program.DateTimeToUnixTime(DateTime.Now);
            if (ChatPartners.Any(entry => entry.UserID != CurrentUser.UserID) && !await Networking.SendChatMessage(this, messageTextBox.Text, time)) //UserID==CurrentUser.UserID: 2015.05.23.
                MessageBox.Show(Language.Translate(Language.StringID.Networking_Alone));
            else //else: 2014.10.31.
            {
                ShowReceivedMessageT(UserInfo.Select(CurrentUser.UserID), messageTextBox.Text, time);
                messageTextBox.Text = "";
            }
            messageTextBox.Select(); //2014.12.13.
            messageTextBox.ReadOnly = false;
        }

        private void MessageTextChanged(object sender, EventArgs e)
        {
            if (!InternalMessageChange)
            {
                /*if (messageTextBox.Text == "\n")
                    messageTextBox.Text = "";*/
#if emoticons
                TextFormat.Parse((ExExRichTextBox)sender);
#endif
            }
        }

        private void OpenLink(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        public static ChatPanel GetChatPanelByUsers(IEnumerable<UserInfo> users) //2014.08.08. - IEnumerable: 2014.08.16. - GetChatFormByUsers --> GetChatPanelByUsers: 2015.05.15.
        {
            if (users.Any(entry => entry == null))
                return null; //2015.05.15.
            int i;
            for (i = 0; i < ChatWindows.Count; i++)
            {
                //if (ChatWindows[i].ChatPartners.HasSameElementsAs(users))
                UserInfo[] tmp = new UserInfo[ChatWindows[i].ChatPartners.Count + 1]; //2015.05.15.
                ChatWindows[i].ChatPartners.CopyTo(tmp); //2015.05.15.
                tmp[tmp.Length - 1] = UserInfo.Select(CurrentUser.UserID); //2015.05.15.
                if (tmp.HasSameElementsAs(users))
                    break; //2015.05.15.
            }
            return (i != ChatWindows.Count) ? ChatWindows[i] : null;
        }

        /*public string TMessage;
        public int SetThreadValues()
        {
            recentMsgTextBox.AppendText(TMessage);
            TextFormat.Parse(recentMsgTextBox);
            TMessage = "";
            recentMsgTextBox.SelectionStart = recentMsgTextBox.TextLength; //2014.04.10.
            recentMsgTextBox.ScrollToCaret(); //2014.04.10.
            return 0;
        }*/

        private UserInfo _LastMessageUser;
        private string _LastMessage;
        private double _LastMessageStartTime;

        /// <summary>
        /// Thread-safe
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        public void ShowReceivedMessageT(UserInfo user, string message, double time)
        { //2015.05.15.
            this.Invoke(new Action(() =>
            {
                if (user == _LastMessageUser && message == _LastMessage && time < _LastMessageStartTime + 1000)
                    return;
                _LastMessageUser = user;
                _LastMessage = message;
                _LastMessageStartTime = Program.DateTimeToUnixTime(DateTime.Now);
                string txt = "\n" + ((user.UserID == CurrentUser.UserID) ? CurrentUser.Name : user.Name) + " " + Language.Translate(Language.StringID.Said) + " (" + Program.UnixTimeToDateTime(time).ToString("yyyy.MM.dd. HH:mm:ss") + "):\n" + message + "\n";
                recentMsgTextBox.AppendText(txt);
                TextFormat.Parse(recentMsgTextBox);
                recentMsgTextBox.SelectionStart = recentMsgTextBox.TextLength;
                recentMsgTextBox.ScrollToCaret();
            }));
        }

        public static ChatPanel Create(IEnumerable<UserInfo> users)
        { //2015.05.15.
            ChatPanel cf = null;
            Program.MainF.Invoke(new Action(() => //Invoke: 2015.05.16.
            {
                ChatPanel.ChatWindows.Add(cf = new ChatPanel());
                cf.ChatPartners.AddRange(users);
                if (cf.ChatPartners.Count > 1) //2015.05.23. - Így saját magunkkal is beszélhetünk...
                    cf.ChatPartners.RemoveAll(entry => entry.UserID == CurrentUser.UserID); //2015.05.16.
                cf.Init();
            }));
            return cf;
        }

        public void OpenSendFile(SelectPartnerForm spform)
        {
            //A küldő a szerver - 2014.06.15.
            //Fogadás: //sendfile üzenet
            /*if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            Stream st = new FileStream(openFileDialog1.FileName, FileMode.Open);
            try
            {
                if (new FileInfo(openFileDialog1.FileName).Length > Int64.Parse(Storage.Settings[SettingType.FileLen]))
                {
                    List<byte> buf = new List<byte>();
                    int b;
                    do
                    {
                        b = st.ReadByte();
                        buf.Add((byte)b);
                    }
                    while (b != -1);
                    st = new MemoryStream(buf.ToArray(), false);
                }
            }
            catch (OutOfMemoryException)
            { //A MemoryStream-et nem hozza létre, ezzel elméletileg memóriát felszabadítva
                st.Seek(0, SeekOrigin.Begin);
            }
            IPHostEntry host;
            //IPAddress localIP = IPAddress.Parse("127.0.0.1");
            IPAddress localIP = IPAddress.IPv6Loopback; //2015.05.10.
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip;
                    break;
                }
            }
            IPAddress ipAddr = null; //Használja fel a partner ismert IP-címét
            Socket sListener;
            SocketPermission permission;
            permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, "", SocketPermission.AllPorts);
            sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            var ipEndPoint = new IPEndPoint(ipAddr, Int32.Parse(Storage.Settings[SettingType.Port]));
            sListener.Listen(1);
            ST = st; //Átadja az adatfolyamot a nyilvánosabb változónak
            AsyncCallback aCallback = new AsyncCallback(SendFile_AcceptCallback);
            sListener.BeginAccept(aCallback, sListener);*/
        }
        private Stream ST;
        private void SendFile_AcceptCallback(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            var ns = new NetworkStream(handler);
            ns.CopyFrom(ST, new CopyFromArguments(new ProgressChange(SendFile_ProgressChange)));
        }

        private void SendFile_ProgressChange(long bytesRead, long totalBytesToRead)
        {
            Console.WriteLine("SendFile: " + bytesRead + " / " + totalBytesToRead);
        }

        private string CurrentMessage = "";
        private void showicons_CheckedChanged(object sender, EventArgs e)
        {
            if (showicons.Checked)
            {
                CurrentMessage = messageTextBox.Text;
                messageTextBox.Enabled = false;
                TextFormat.Parse(messageTextBox);
            }
            else
            {
                messageTextBox.Text = CurrentMessage;
                messageTextBox.Enabled = true;
            }
        }

        public void Close()
        {
            ChatWindows.Remove(this);
            if (Storage.Settings[SettingType.ChatWindow] == "0" ^ SettingsForm.ApplyingSettings) //Ha az új beállítás szerint(!) külön ablakokban kell megjeleníteni, akkor hajtsa végre
            { //2014.10.31.
                this.Dispose();
                //MessageBox.Show("Close"); //2015.05.23.
                if (ChatIcon != null) //Most már nem feltétlenül változik a beállítás
                    ChatIcon.Dispose();
            }
        }

        private void Init()
        { //2014.10.28.
            /*if (!Storage.LoggedInSettings.ContainsKey("chatwindow")) //2014.12.05.
                Storage.LoggedInSettings.Add("chatwindow", "0"); //2014.12.05.*/
            if (Storage.Settings[SettingType.ChatWindow] == "")
                Storage.Settings[SettingType.ChatWindow] = "0"; //2015.05.21.
            if (Storage.Settings[SettingType.ChatWindow] == "1")
            {
                //ChatForm a ChatPanel-lel
                var cf = new ChatForm();
                cf.Controls.Add(this);
                cf.FormClosing += cf_FormClosing;
                this.Dock = DockStyle.Fill;
                cf.Show();
                cf.Select(); //2014.12.13.
                cf.Focus(); //2015.05.16.
            }
            else
            {
                Program.MainF.Controls.Add(this);
                if (ChatPartners.Count == 0)
                    return; //2015.05.21. - Azt várja meg, hogy a Load metódus hibát jelezzen
                Program.MainF.PlaceChatIcon(this);
                this.BringToFront();
                this.Show();
            }
        }

        void cf_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Close();
        }

        internal static void ReopenChatWindows(bool settingchanged)
        {
            for (int i = 0; i < ChatWindows.Count; i++)
            {
                var tmp = ChatWindows[i].ChatPartners;
                if (settingchanged)
                {
                    if (Storage.Settings[SettingType.ChatWindow] == "0") //Ha az új beállítás szerint(!) külön ablakokban kell megjeleníteni, akkor hajtsa végre
                        ((Form)ChatWindows[i].Parent.Parent).Close(); //Ezzel meghívja a saját Close()-ját is
                    else
                        ChatWindows[i].Close();
                }
                else
                {
                    if (Storage.Settings[SettingType.ChatWindow] == "1") //Ha a régi beállítás szerint(!) külön ablakokban kell megjeleníteni, akkor hajtsa végre
                        ((Form)ChatWindows[i].Parent.Parent).Close(); //Ezzel meghívja a saját Close()-ját is
                    else
                        ChatWindows[i].Close();
                }
                var tmp2 = new ChatPanel();
                tmp2.ChatPartners = tmp;
                tmp2.Init();
                ChatWindows.Add(tmp2);
            }
        }

        public new void Show()
        {
            if (Storage.Settings[SettingType.ChatWindow] == "0")
            {
                foreach (var item in ChatWindows)
                {
                    item.Hide();
                }
                //A ChatIcon-hoz a legközelebbi helyre rakja - Vagy fedje be kb. a partnerlistát, úgyis elég nagy
                this.Location = new Point(150, Program.MainF.contactList.Location.Y);
            }
            base.Show();
        }

        public PictureBox ChatIcon { get; set; }

        public Handwriting handw;
        private void button1_Click(object sender, EventArgs e)
        {
            if (messageTextBox.Visible)
            {
                if (handw == null)
                {
                    handw = new Handwriting();
                    handw.Parent = this.splitContainer2.Panel2;
                    handw.Bounds = messageTextBox.Bounds;
                    handw.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                    handw.sendbtn.Text = Language.Translate(Language.StringID.Sendbtn_Send, handw.sendbtn);
                    handw.sendbtn.Click += Handw_sendbtn_Click;
                }
                else
                    handw.Show();
                messageTextBox.Hide();
            }
            else
            {
                handw.Hide();
                messageTextBox.Show();
            }
        }

        void Handw_sendbtn_Click(object sender, EventArgs e)
        {
            handw.GetBitmap().Save("test.bmp");
        }
    }
}
