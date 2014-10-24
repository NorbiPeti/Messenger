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
        //public List<string> PendingMessages = new List<string>();
        //public Thread UpdateT;
        public ChatForm()
        {
            InitializeComponent();
            //Amint létrehozom, ez a kód lefut - Nem számit, hogy megjelenik-e

            this.Text = Language.Translate("chat_title");
            showicons.Text = Language.Translate("chat_showicons");
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            if (ChatPartners.Count == 0)
                MessageBox.Show(Language.Translate("error") + ": " + Language.Translate("chat_nowindow"));
            if (ChatPartners.Count == 1)
            {
                partnerName.Text = UserInfo.Select(ChatPartners[0]).Name;
                TextFormat.Parse(partnerName);
                partnerMsg.Text = UserInfo.Select(ChatPartners[0]).Message;
                TextFormat.Parse(partnerMsg);
                switch (UserInfo.Select(ChatPartners[0]).State)
                {
                    case 0:
                        {
                            statusLabel.Text = Language.Translate("offline");
                            break;
                        }
                    case 1:
                        {
                            statusLabel.Text = Language.Translate("menu_file_status_online");
                            break;
                        }
                    case 2:
                        {
                            statusLabel.Text = Language.Translate("menu_file_status_busy");
                            break;
                        }
                    case 3:
                        {
                            statusLabel.Text = Language.Translate("menu_file_status_away");
                            break;
                        }
                    default:
                        {
                            statusLabel.Text = Language.Translate("networking_alone");
                            break;
                        }
                }
                //UpdateT = new Thread(new ThreadStart(UpdateMessages));
                //UpdateT.Name = "Message Update Thread (" + partnerName.Text + ")";
                //UpdateT.Start();
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
            //PendingMessages.Add(messageTextBox.Text);
            if (!Networking.SendChatMessage(this, messageTextBox.Text))
                MessageBox.Show(Language.Translate("networking_alone"));
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
        //public void UpdateMessages()
        //{
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
         * 
         * 2014.08.08.
         * Az UpdateListAndChat thread kezelje ezt is,
         * az updatemessages packet-et csak akkor küldje el, ha új üzenet érkezik,
         * a régebbi üzeneteket (getrecentmessages) tárolja el lokálisan, mint minden egyéb adatát
         */
        //while (ChatWindows.Count != 0 && !this.IsDisposed && MainForm.MainThread.IsAlive)
        //{
        //}
        //}

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ChatWindows.Remove(this);
        }

        public static ChatForm GetChatFormByUsers(IEnumerable<int> users) //2014.08.08. - IEnumerable: 2014.08.16.
        {
            int i;
            for (i = 0; i < ChatWindows.Count; i++)
            {
                if (ChatWindows[i].ChatPartners.HasSameElementsAs(users))
                    break;
            }
            return (i != ChatWindows.Count) ? ChatWindows[i] : null; //== --> !=: 2014.09.22.
        }

        //public static string TMessage;
        public string TMessage;
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
                //if (CurrentUser.CopyToMemoryOnFileSend)
                if (new FileInfo(openFileDialog1.FileName).Length > Int64.Parse(Storage.Settings["filelen"]))
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
            //string ret = Networking.SendRequest("setip", spform.Partners[0] + 'ͦ' + localIP.ToString() + ":" + Settings.Default.port + ":" + openFileDialog1.FileName, 0, true);
            //var ipAddr = IPAddress.Parse(ret);
            IPAddress ipAddr = null; //Használja fel a partner ismert IP-címét
            Socket sListener;
            SocketPermission permission;
            permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, "", SocketPermission.AllPorts);
            sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            var ipEndPoint = new IPEndPoint(ipAddr, Int32.Parse(Storage.Settings["port"]));
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

        public string CurrentMessage = "";
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
    }
}
