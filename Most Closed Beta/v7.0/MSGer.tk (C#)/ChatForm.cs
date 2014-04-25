using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    public partial class ChatForm : Form
    {/* 2014.03.07. A partnerinformáció birtokolja a chatablakot, és nem forditva; bár a partnerinformáció indexszáma változhat
        public int ChatPartner
        {
            get;
            set;
        }*/
        //public int ChatPartner = -1; //Csak kell, a könnyebb kezelésért
        //public int[] MultiChat = new int[1024];
        //public static ChatForm[] ChatWindows=new ChatForm[1024];
        public static List<ChatForm> ChatWindows = new List<ChatForm>();
        public List<int> ChatPartners = new List<int>();
        public List<string> PendingMessages = new List<string>();
        public int LastCheck = 0;
        public Thread UpdateT;
        public ChatForm()
        {
            InitializeComponent();
            //Amint létrehozom, ez a kód lefut - Nem számit, hogy megjelenik-e
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(ChatPartner + "");
            //ChatPartner = 2;
            //MessageBox.Show(ChatPartner + "");
            /*
            for (int i = 0; i < UserInfo.Partners.Length; i++)
            {
                if(UserInfo.Partners[i]!=null && UserInfo.Partners[i].ChatWindow==this)
                {
                    ChatPartner = i;
                    break;
                }
            }*/
            /*if (ChatPartner == -1)
                MessageBox.Show("Hiba: Az ablakot létrehozó partner nem található.");*/
            if (ChatPartners.Count == 0)
                MessageBox.Show("Hiba: Az ablakot létrehozó partner nem található.");
            if (ChatPartners.Count == 1)
            {
                //MessageBox.Show("ChatPartner: " + ChatPartners[0]);
                partnerName.Text = UserInfo.Partners[ChatPartners[0]].Name;
                partnerMsg.Text = UserInfo.Partners[ChatPartners[0]].Message;
                UpdateT = new Thread(new ThreadStart(UpdateMessages));
                UpdateT.Name = "Message Update Thread (" + partnerName.Text + ")";
                UpdateT.Start();
            }
        }

        private void SendMessage(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || e.Shift || messageTextBox.Text.Length==0)
                return;
            messageTextBox.ReadOnly = true;
            //Networking.SendRequest("sendmessage", messageTextBox.Text, 2); //Még nincs kész a PHP - 2014.03.08. 0:01
            /*
             * 2014.03.08. 0:03
             * A fenti kódra válaszul a másik felhasználó esetleges új válaszát is irja be; tehát frissitse az üzeneteket
             * Az üzenetellenőrző thread folyamatosan fusson, amint végrehajtotta a parancsokat, kezdje újra (nincs Thread.Sleep)
             * 
             * 2014.03.19.
             * Csinálja úgy, mint a képeknél, hogy a legutóbbi üzenetlekérés dátumához igazodjon, és csak a legújabb üzeneteket
             * töltse le
             */
            //recentMsgTextBox.AppendText("Üzenet:\n");
            //recentMsgTextBox.AppendText(messageTextBox.Text + "\n");
            PendingMessages.Add(messageTextBox.Text);
            messageTextBox.Focus();
            messageTextBox.Text = "";
            messageTextBox.ReadOnly = false;
        }

        private void MessageTextChanged(object sender, EventArgs e)
        {
            if (messageTextBox.Text == "\n")
                messageTextBox.Text = "";
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
            //while(UserInfo.Partners[ChatPartner].ChatWindow.IsDisposed && MainForm.MainThread.IsAlive)
            while (!ChatForm.ChatWindows[ChatPartners[0]].IsDisposed && MainForm.MainThread.IsAlive)
            {
                /*MessageBox.Show(String.Concat(PendingMessages.ToArray()));
                PendingMessages.Clear();*/
                //Thread.Sleep(2000);
                string tmpstring = "";
                tmpstring += LastCheck + "ͦ";
                for (int i = 0; i < ChatPartners.Count; i++)
                {
                    tmpstring += ChatPartners[i];
                    if (i + 1 < ChatPartners.Count)
                        tmpstring += ","; //Több emberrel is beszélhet
                }
                tmpstring += "ͦ"; //2014.03.27.
                //int count = PendingMessages.Count; //Külön kell tennem, mert máskülönben folyamatosan csökken, és ezért a... Hoppá... While kell...
                //for(int i=0; i<count; i++)
                //{... //Ha nincs új üzenet, akkor a Count=0, tehát egyszer sem fut le ez (tapasztalat...) - Tökéletes
                if(PendingMessages.Count==0)
                {
                    tmpstring += "NoMSG" + "ͦ";
                }
                while(PendingMessages.Count>0)
                {
                    tmpstring += PendingMessages[0] + "ͦ"; //Nem az i-nél tevékenykedik, hanem a 0-nál
                    PendingMessages.RemoveAt(0);
                    MessageBox.Show("tmpstring: " + tmpstring);
                }
                //MessageBox.Show("tmpstring: " + tmpstring);
                //MessageBox.Show("Length: " + tmpstring.Length);
                //MessageBox.Show("tmpstring encoded: " + Uri.EscapeUriString(tmpstring));
                if(tmpstring.Length>0)
                { //Küldje el a lekérést
                    string[] response = Networking.SendRequest("updatemessages", tmpstring, 2, true).Split('ͦ');
                    if (response == null || response.Length == 0 || response[0] == "Fail")
                        MessageBox.Show("Az üzenetek frissitése sikertelen.");
                    //0 - Frissitési idő; 1 - Üzenetküldő; 2 - Üzenet; 3 - Üzenetküldés időpontja
                    if (response[0] != "NoChange")
                    {
                        if (Int32.TryParse(response[0], out LastCheck) == false)
                        {
                            MessageBox.Show("Hiba:\n" + response[0]);
                            continue;
                        }
                        //LastCheck = Int32.Parse(response[0]);
                        recentMsgTextBox.AppendText(response[1] + " üzenete (" + response[3] + "):\n" + response[2]);
                    }
                }
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            for(int i=0; i<ChatWindows.Count; i++)
            {
                if (ChatWindows[i].Equals(this))
                    //ChatWindows[i] = null;
                    ;
            }
        }
    }
}
