//Hangulatjelek használata
//#define emoticons

using Khendys.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HdSystemLibrary.IO;

namespace MSGer.tk
{
    public partial class ChatForm : Form
    {
        public static List<ChatForm> ChatWindows = new List<ChatForm>();
        public List<int> ChatPartners = new List<int>();
        public List<string> PendingMessages = new List<string>();
        public Thread UpdateT;
        public ChatForm()
        {
            InitializeComponent();
            //Amint létrehozom, ez a kód lefut - Nem számit, hogy megjelenik-e
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            if (ChatPartners.Count == 0)
                MessageBox.Show(Language.GetCuurentLanguage().Strings["error"] + ": " + Language.GetCuurentLanguage().Strings["chat_nowindow"]);
            if (ChatPartners.Count == 1)
            {
                partnerName.Text = UserInfo.Partners[ChatPartners[0]].Name;
                TextFormat.Parse(partnerName);
                partnerMsg.Text = UserInfo.Partners[ChatPartners[0]].Message;
                TextFormat.Parse(partnerMsg);
                switch(UserInfo.Partners[ChatPartners[0]].State)
                {
                    case "0":
                        {
                            statusLabel.Text = Language.GetCuurentLanguage().Strings["offline"];
                            break;
                        }
                    case "1":
                        {
                            statusLabel.Text = Language.GetCuurentLanguage().Strings["menu_file_status_online"];
                            break;
                        }
                    case "2":
                        {
                            statusLabel.Text = Language.GetCuurentLanguage().Strings["menu_file_status_busy"];
                            break;
                        }
                    case "3":
                        {
                            statusLabel.Text = Language.GetCuurentLanguage().Strings["menu_file_status_away"];
                            break;
                        }
                }
                UpdateT = new Thread(new ThreadStart(UpdateMessages));
                UpdateT.Name = "Message Update Thread (" + partnerName.Text + ")";
                UpdateT.Start();
            }
        }

        public bool InternalMessageChange = false;
        public int SelectionStart = 0;
        public int SelectionLength = 0;
        public int TextLength = 0;
        private void SendMessage(object sender, KeyEventArgs e)
        {
            //SendMessage
            if (e.KeyCode != Keys.Enter || e.Shift || messageTextBox.Text.Length == 0)
                return;
            messageTextBox.ReadOnly = true;
            /*
             * 2014.03.08. 0:03
             * A fenti kódra válaszul a másik felhasználó esetleges új válaszát is irja be; tehát frissitse az üzeneteket
             * Az üzenetellenőrző thread folyamatosan fusson, amint végrehajtotta a parancsokat, kezdje újra (nincs Thread.Sleep)
             * 
             * 2014.03.19.
             * Csinálja úgy, mint a képeknél, hogy a legutóbbi üzenetlekérés dátumához igazodjon, és csak a legújabb üzeneteket
             * töltse le
             */
            PendingMessages.Add(messageTextBox.Text);
            messageTextBox.Focus();
            messageTextBox.Text = "";
            messageTextBox.ReadOnly = false;
        }

        private void MessageTextChanged(object sender, EventArgs e)
        {
            if (!InternalMessageChange)
            {
                if (messageTextBox.Text == "\n")
                    messageTextBox.Text = "";
                #if emoticons
                TextFormat.Parse((ExExRichTextBox)sender);
                #endif
            }
        }

        private void OpenLink(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }
        public void UpdateMessages()
        {
            /*
             * 2014.03.21.
             * updatemessages: küldje el, hogy mikor kapott utoljára üzenetet ÉS az új üzeneteket,
             * a szerver pedig először válaszoljon a szerinte aktuális időponttal,
             * majd küldje el az annál újabb üzeneteket
             * getrecentmessages: ezt csak önmagában küldje,
             * a szerver pedig válaszoljon a legutóbbi x üzenettel,
             * ahol x egy beállitható szám (alapból 10)
             * ----
             * Az új üzeneteket egy listában tárolja, majd amikor továbbitja őket, törölje a listából fokozatosan
             * (ahogy összeállitja a karakterláncot, mindig törölje azt az üzenetet a listából
             */
            while (ChatWindows.Count != 0 && !this.IsDisposed && MainForm.MainThread.IsAlive)
            {
                try
                {
                    string tmpstring = "";
                    for (int i = 0; i < ChatPartners.Count; i++)
                    {
                        tmpstring += UserInfo.Partners[ChatPartners[i]].UserID;
                        if (i + 1 < ChatPartners.Count)
                            tmpstring += ","; //Több emberrel is beszélhet
                    }
                    tmpstring += "ͦ"; //2014.03.27.
                    if (PendingMessages.Count == 0)
                    {
                        tmpstring += "NoMSG" + "ͦ";
                    }
                    while (PendingMessages.Count > 0)
                    {
                        tmpstring += PendingMessages[0] + "ͦ"; //Nem az i-nél tevékenykedik, hanem a 0-nál
                        PendingMessages.RemoveAt(0);
                    }
                    if (tmpstring.Length > 0)
                    { //Küldje el a lekérést
                        string[] response = Networking.SendRequest("updatemessages", tmpstring, 0, true).Split('ͦ');
                        if (response == null || response.Length == 0 || response[0] == "Fail")
                            MessageBox.Show(Language.GetCuurentLanguage().Strings["msgupdate_error"]);
                        //0 - Frissitési idő; 1 - Üzenetküldő; 2 - Üzenet; 3 - Üzenetküldés időpontja
                        if (response[0] != "NoChange")
                        {
                            for (int x = 0; x + 2 < response.Length; x += 3)
                            {
                                string[] cmd = response[x + 1].Split(' ');
                                switch (cmd[0])
                                {
                                    case "//sendfile":
                                        string[] ipportname = cmd[1].Split(':');
                                        IPAddress ipAddr = IPAddress.Parse(ipportname[0]);
                                        var permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, "", SocketPermission.AllPorts);
                                        var ipEndPoint = new IPEndPoint(ipAddr, Int32.Parse(ipportname[1]));
                                        var receiverSock = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                                        receiverSock.Connect(ipEndPoint);
                                        var ns = new NetworkStream(receiverSock);
                                        var fs = new FileStream(ipportname[2], FileMode.Create);
                                        break;
                                }
                                TMessage = "\n" + ((Int32.Parse(response[x]) == CurrentUser.UserID) ? CurrentUser.Name : UserInfo.Partners[Int32.Parse(response[x])].Name) + " " + Language.GetCuurentLanguage().Strings["said"] + " (" + Program.UnixTimeToDateTime(response[x + 2]).ToString("yyyy.MM.dd. HH:mm:ss") + "):\n" + response[x + 1]+"\n";
                                this.Invoke(new LoginForm.MyDelegate(SetThreadValues));
                            }
                        }
                    }
                }
                catch(InvalidOperationException)
                {
                    break;
                }
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ChatWindows.Remove(this);
        }

        public static string TMessage;
        public int SetThreadValues()
        {
            recentMsgTextBox.AppendText(TMessage);
            TextFormat.Parse(recentMsgTextBox);
            TMessage = "";
            recentMsgTextBox.SelectionStart = recentMsgTextBox.TextLength; //2014.04.10.
            recentMsgTextBox.ScrollToCaret(); //2014.04.10.
            return 0;
        }
        public void OpenSendFile(SelectPartnerForm spform)
        {
            //A küldő a szerver - 2014.06.15.
            //Fogadás: //sendfile üzenet
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            Stream st = new FileStream(openFileDialog1.FileName, FileMode.Open);
            try
            {
                if (CurrentUser.CopyToMemoryOnFileSend)
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
            catch(OutOfMemoryException)
            { //A MemoryStream-et nem hozza létre, ezzel elméletileg memóriát felszabadítva
                st.Seek(0, SeekOrigin.Begin);
            }
            IPHostEntry host;
            IPAddress localIP = IPAddress.Parse("127.0.0.1");
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip;
                    break;
                }
            }
            string ret = Networking.SendRequest("setip", spform.Partners[0] + 'ͦ' + localIP.ToString() + ":" + Settings.Default.port + ":" + openFileDialog1.FileName, 0, true);
            var ipAddr = IPAddress.Parse(ret);
            Socket sListener;
            SocketPermission permission;
            permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, "", SocketPermission.AllPorts);
            sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            var ipEndPoint = new IPEndPoint(ipAddr, Settings.Default.port);
            sListener.Listen(1);
            ST = st; //Átadja az adatfolyamot a nyilvánosabb változónak
            AsyncCallback aCallback = new AsyncCallback(SendFile_AcceptCallback);
            sListener.BeginAccept(aCallback, sListener);
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
    }
}
