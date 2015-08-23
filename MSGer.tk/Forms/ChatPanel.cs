using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Diagnostics;
using Handwriting_program;
using FastColoredTextBoxNS;
using System.Text.RegularExpressions;

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
        private TextBoxHelpers.GifImageStyle style;
        private TextBoxHelpers.GifImageStyle styleRecent; //2015.06.16.
        const string RegexSpecSymbolsPattern = @"[\^\$\[\]\(\)\.\\\*\+\|\?\{\}]"; //static-->const: 2015.07.05.
        public ChatPanel()
        {
            InitializeComponent();
            //Amint létrehozom, ez a kód lefut - Nem számit, hogy megjelenik-e

            this.Text = Language.Translate(Language.StringID.Chat_Title, this);
            button1.Text = Language.Translate(Language.StringID.Handwriting, button1); //2015.06.29.

            style = new TextBoxHelpers.GifImageStyle(messageTextBox);
            styleRecent = new TextBoxHelpers.GifImageStyle(recentMsgTextBox); //2015.06.16.
            foreach (var item in TextFormat.TextFormats)
            { //2015.06.26.
                foreach (var item2 in item.Emoticons)
                {
                    style.ImagesByText.Add(item2.Value, item2.Image);
                    styleRecent.ImagesByText.Add(item2.Value, item2.Image);
                }
            }
            style.StartAnimation(); //2015.06.16.
            styleRecent.StartAnimation(); //2015.06.16.

            messageTextBox.OnTextChanged(); //2015.06.16.
            recentMsgTextBox.OnTextChanged(); //2015.06.16.
            recentMsgTextBox.GoEnd(); //2015.06.16.
        } //TODO: A recentMsgTextBox-nál megoldani az OpenLink event-et

        private void ChatForm_Load(object sender, EventArgs e)
        {
            if (ChatPartners.Count == 0)
            {
                new ErrorHandler(ErrorType.Chat_NoPartners, new Exception()); //2015.06.04.
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
            }
            else
            {
                this.Text = ChatName;
            }
            if (Storage.Settings[SettingType.ChatWindow] == "1") //<-- 2015.06.14.
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
                recentMsgTextBox.GoEnd(); //2015.06.16.
                ShowReceivedMessageT(UserInfo.Select(CurrentUser.UserID), messageTextBox.Text, time);
                messageTextBox.Text = "";
            }
            messageTextBox.Select(); //2014.12.13.
            messageTextBox.ReadOnly = false;
        }

        private void MessageTextChanged(object sender, TextChangedEventArgs e)
        {
            if (style == null)
                return;
            e.ChangedRange.ClearStyle(StyleIndex.All);
            foreach (var key in style.ImagesByText.Keys)
            {
                string pattern = Regex.Replace(key, RegexSpecSymbolsPattern, "\\$0");
                e.ChangedRange.SetStyle(style, pattern);
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
                UserInfo[] tmp = new UserInfo[ChatWindows[i].ChatPartners.Count + 1]; //2015.05.15.
                ChatWindows[i].ChatPartners.CopyTo(tmp); //2015.05.15.
                tmp[tmp.Length - 1] = UserInfo.Select(CurrentUser.UserID); //2015.05.15.
                if (tmp.HasSameElementsAs(users))
                    break; //2015.05.15.
            }
            return (i != ChatWindows.Count) ? ChatWindows[i] : null;
        }

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
                if (user == _LastMessageUser && message == _LastMessage && time < _LastMessageStartTime + 1) //1000-->1: 2015.07.05. 0:35
                    return;
                _LastMessageUser = user;
                _LastMessage = message;
                _LastMessageStartTime = Program.DateTimeToUnixTime(DateTime.Now);
                int index = message.IndexOf("//!img"); //2015.07.05.
                int index2 = message.IndexOfAny(new char[] { ' ', '\n' }); //2015.07.05.
                if (index2 == -1) //2015.07.05.
                    index2 = message.Length; //2015.07.05.
                if (index != -1) //2015.07.05.
                {
                    string newlines = ""; //2015.07.05.
                    message = message.Substring(index, index2); //2015.07.05.
                    if (!styleRecent.SentImagesByText.ContainsKey(message)) //2015.07.05.
                        message = "[Invalid image data]"; //2015.07.05.
                    else
                        for (int i = 0; i <= styleRecent.SentImagesByText[message].Height; i += (int)recentMsgTextBox.Font.GetHeight())
                            newlines += "\n"; //2015.07.05.
                    message += newlines; //2015.07.05.
                }
                string txt = "\n" + ((user.UserID == CurrentUser.UserID) ? CurrentUser.Name : user.Name) + " " + Language.Translate(Language.StringID.Said) + " (" + Program.UnixTimeToDateTime(time).ToString("yyyy.MM.dd. HH:mm:ss") + "):\n" + message + "\n";
                recentMsgTextBox.AppendText(txt);
                recentMsgTextBox.GoEnd(); //2015.06.16.
            }));
        }
        private double _LastImageTime;
        private UserInfo _LastImageUser;
        public void ShowReceivedImageT(UserInfo user, Image image, double time)
        { //2015.06.25.
            this.Invoke(new Action(() =>
            {
                if (user == _LastImageUser && time < _LastImageTime + 1) //<-- 2015.07.04.
                    return;
                _LastImageTime = time;
                _LastImageUser = user;
                string txt = "\n" + ((user.UserID == CurrentUser.UserID) ? CurrentUser.Name : user.Name) + " " + Language.Translate(Language.StringID.Said) + " (" + Program.UnixTimeToDateTime(time).ToString("yyyy.MM.dd. HH:mm:ss") + "):\n";
                txt += styleRecent.AddImage(image) + "\n"; //2015.06.26. - style-->styleRecent: 2015.07.04.
                for (int i = 0; i <= image.Height; i += (int)recentMsgTextBox.Font.GetHeight())
                    txt += "\n"; //2015.07.04.
                recentMsgTextBox.AppendText(txt);
                recentMsgTextBox.GoEnd(); //2015.06.16.
            }));
        }

        public long ShowReceivedFileT(UserInfo user, FileInfo file, double time, long progress)
        { //2015.06.30.
            long prog = 0;
            this.Invoke(new Action(() =>
            {
                progressBar1.Value = (int)((progress / file.Length) * 100);
                progressBar1.Visible = true;
                if (progress + Networking.PDSendFile.BufferLength >= file.Length + 1)
                {
                    progressBar1.Visible = false;
                    string txt = "\n" + ((user.UserID == CurrentUser.UserID) ? CurrentUser.Name : user.Name) + " " + Language.Translate(Language.StringID.FileReceived) + " (" + Program.UnixTimeToDateTime(time).ToString("yyyy.MM.dd. HH:mm:ss") + "): " + file.Name + "\n";
                    recentMsgTextBox.AppendText(txt);
                    prog = Progress;
                    Progress = 0;
                }
            }));
            return prog;
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

        private long Progress = 0; //2015.06.30.
        private FileInfo fileinfo; //2015.06.30.
        public void OpenSendFile() //<-- 2015.06.30.
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            fileinfo = new FileInfo(openFileDialog1.FileName);

            Progress = 0; //2015.06.30.
            new Networking.PacketSender(new Networking.PDSendFile(ChatPartners.Select(entry => entry.UserID).ToArray(),
                fileinfo, Program.DateTimeToUnixTime(DateTime.Now), 0)).SendAsync()
                .ContinueWith(new Action<Task<Networking.PacketFormat[]>>(SendFileContinue));
            progressBar1.Visible = true;
        }
        private IEnumerable<IPAddress> SendFileIPs; //2015.06.30.
        private void SendFileContinue(Task<Networking.PacketFormat[]> task) //<-- 2015.06.30.
        {
            if (task.Result.Count() == 0)
                return; //2015.06.30.
            if (SendFileIPs == null)
                SendFileIPs = task.Result.Where(entry => ChatPartners.Any(item => item.UserID == entry.EUserID)) //2015.06.30.
                    .Select(entry => ((Networking.PDSendFile)entry.EData).RespIPs) //2015.06.30.
                    .Aggregate((entry1, entry2) => entry1.Concat(entry2)); //2015.06.30.
            var pf = task.Result.FirstOrDefault(entry => ((Networking.PDSendFile)entry.EData).RespIPs.SequenceEqual(SendFileIPs)); //2015.06.30.
            if (pf == null)
                return; //2015.06.30.
            var pd = ((Networking.PDSendFile)pf.EData);
            if (pd.RProgress < Progress) //Ha a fájl fogadója le van maradva
            { //2015.06.30.
                Progress = pd.RProgress;
            }
            Progress += Networking.PDSendFile.BufferLength; //2015.06.30.
            if (Progress < fileinfo.Length)
            {
                progressBar1.Value = (int)((Progress / fileinfo.Length) * 100); //2015.06.30.
                new Networking.PacketSender(new Networking.PDSendFile(ChatPartners.Select(entry => entry.UserID).ToArray(),
                    fileinfo, Program.DateTimeToUnixTime(DateTime.Now), Progress)).SendAsync()
                    .ContinueWith(new Action<Task<Networking.PacketFormat[]>>(SendFileContinue));
            }
            else
            {
                SendFileIPs = null; //2015.06.30.
                Progress = 0; //2015.06.30.
            }
        }

        public void Close()
        {
            ChatWindows.Remove(this);
            if (Storage.Settings[SettingType.ChatWindow] == "0" ^ SettingsForm.ApplyingSettings) //Ha az új beállítás szerint(!) külön ablakokban kell megjeleníteni, akkor hajtsa végre
            { //2014.10.31.
                this.Dispose();
                if (ChatIcon != null) //Most már nem feltétlenül változik a beállítás
                    ChatIcon.Dispose();
            }
        }

        private void Init()
        { //2014.10.28.
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

        async void Handw_sendbtn_Click(object sender, EventArgs e)
        {
            double time = Program.DateTimeToUnixTime(DateTime.Now); //2015.07.04.
            await new Networking.PacketSender(new Networking.PDSendImage(ChatPartners.Select(entry => entry.UserID).ToArray(), handw.GetBitmap(), time)).SendAsync(); //2015.06.28.
            ShowReceivedImageT(UserInfo.Select(CurrentUser.UserID), handw.GetBitmap(), time); //2015.07.04.
            handw.Clear(); //2015.07.04.
        }

        private void recentMsgTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (style == null)
                return;
            e.ChangedRange.ClearStyle(StyleIndex.All);
            foreach (var key in style.ImagesByText.Keys)
            {
                string pattern = Regex.Replace(key, RegexSpecSymbolsPattern, "\\$0");
                e.ChangedRange.SetStyle(styleRecent, pattern);
            }
            foreach (var key in styleRecent.SentImagesByText.Keys)
            {
                e.ChangedRange.SetStyle(styleRecent, key);
            }
        }
    }
}
