using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Khendys.Controls;
using SzNPProjects;
using System.Net;

namespace MSGer.tk
{
    public partial class UserInfo
    {
        /*
         * 2014.03.07.
         * Az összes szükséges felhasználó szükséges adatai
         */
        //public int UserID { get; set; } //Az egész rendszerben egyedi azonosítója
        //public int ListID { get; set; } //A listabeli azonosítója
        //public static List<UserInfo> Partners = new List<UserInfo>();
        public static List<UserInfo> KnownUsers = new List<UserInfo>(); //2014.08.28.
        public int UserID //Az egész rendszerben egyedi azonosítója
        {
            get;
            set;
        }
        public int ListID //A listabeli azonosítója
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_listid"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_listid", "-1");
                return Int32.Parse(Storage.LoggedInSettings["userinfo_" + UserID + "_listid"]);
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_listid"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_listid", "-1");
                Storage.LoggedInSettings["userinfo_" + UserID + "_listid"] = value.ToString();
                //UpdateListID - 2014.08.30.
            }
        }
        //private string name;
        public string Name
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_name"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_name", "");
                return Storage.LoggedInSettings["userinfo_" + UserID + "_name"];
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_name"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_name", "");
                Storage.LoggedInSettings["userinfo_" + UserID + "_name"] = value;
                /*List<int> list=GetChatWindows();
                for (int i = 0; i < list.Count; i++)
                {
                    if (ChatForm.ChatWindows != null && ChatForm.ChatWindows[list[i]] != null && !ChatForm.ChatWindows[list[i]].IsDisposed)
                    { //ChatForm
                    }
                }*/
                Update();
            }
        }
        //private string message;
        public string Message
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_message"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_message", "");
                return Storage.LoggedInSettings["userinfo_" + UserID + "_message"];
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_message"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_message", "");
                Storage.LoggedInSettings["userinfo_" + UserID + "_message"] = value;
                /*List<int> list = GetChatWindows();
                for (int i = 0; i < list.Count; i++)
                {
                    if (ChatForm.ChatWindows != null && ChatForm.ChatWindows[list[i]] != null && !ChatForm.ChatWindows[list[i]].IsDisposed)
                    { //ChatForm
                    }
                }*/
                Update();
            }
        }
        //private int state;
        public int State
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_state"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_state", "-1");
                return Int32.Parse(Storage.LoggedInSettings["userinfo_" + UserID + "_state"]);
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_state"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_state", "-1");
                Storage.LoggedInSettings["userinfo_" + UserID + "_state"] = value.ToString();
                /*List<int> list = GetChatWindows();
                for (int i = 0; i < list.Count; i++)
                {
                    if (ChatForm.ChatWindows != null && ChatForm.ChatWindows[list[i]] != null && !ChatForm.ChatWindows[list[i]].IsDisposed)
                    { //ChatForm
                        string tmp = "Hiba";
                        switch (value)
                        {
                            case 0:
                                tmp = Language.Translate("offline");
                                break;
                            case 1:
                                tmp = Language.Translate("menu_file_status_online");
                                break;
                            case 2:
                                tmp = Language.Translate("menu_file_status_busy");
                                break;
                            case 3:
                                tmp = Language.Translate("menu_file_status_away");
                                break;
                        }
                    }
                }*/
                Update();
            }
        }
        //public string UserName { get; set; }
        //public string Email { get; set; }
        public string UserName
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_username"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_username", "");
                return Storage.LoggedInSettings["userinfo_" + UserID + "_username"];
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_username"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_username", "");
                Storage.LoggedInSettings["userinfo_" + UserID + "_username"] = value;
                Update();
            }
        }
        public string Email
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_email"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_email", "");
                return Storage.LoggedInSettings["userinfo_" + UserID + "_email"];
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_email"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_email", "");
                Storage.LoggedInSettings["userinfo_" + UserID + "_email"] = value;
                Update();
            }
        }
        public bool IsPartner
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_ispartner"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_ispartner", "False");
                return bool.Parse(Storage.LoggedInSettings["userinfo_" + UserID + "_ispartner"]);
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_ispartner"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_ispartner", "False");
                Storage.LoggedInSettings["userinfo_" + UserID + "_ispartner"] = value.ToString();
                Update();
            }
        }
        public int LastUpdate
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_lastupdate"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_lastupdate", "0");
                return Int32.Parse(Storage.LoggedInSettings["userinfo_" + UserID + "_lastupdate"]);
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_lastupdate"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_lastupdate", "0");
                Storage.LoggedInSettings["userinfo_" + UserID + "_lastupdate"] = value.ToString();
                Update();
            }
        }
        public string LoginCode
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_logincode"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_logincode", "0");
                return Storage.LoggedInSettings["userinfo_" + UserID + "_logincode"];
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_logincode"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_logincode", "0");
                Storage.LoggedInSettings["userinfo_" + UserID + "_logincode"] = value;
                Update();
            }
        }
        private static HashSet<IPEndPoint> ips = new HashSet<IPEndPoint>();
        public static HashSet<IPEndPoint> IPs
        {
            get
            {
                return ips;
            }
            set
            {
                ips = value;
            }
        }
        private static List<IPEndPoint> bannedips = new List<IPEndPoint>();
        public static List<IPEndPoint> BannedIPs
        {
            get
            {
                return bannedips;
            }
            set
            {
                bannedips = value;
            }
        }
        public static int BanTime { get; set; }


        public UserInfo()
        {
            //Console.WriteLine("Creating UserInfo."); //2014.10.09.
        }
        ~UserInfo() //2014.10.09.
        {
            //Console.WriteLine("UserInfo destroying."); //2014.10.09.
        }
        public int PicUpdateTime = 0;
        public string GetImage()
        {
            /*
             * Szükséges információk az adatbázisban:
             * - Felhasználó képe a users táblában
             * - A legutóbbi képváltás dátuma
             * Ebből megállapitja a program, hogy le kell-e tölteni.
             * Eltárol helyileg is egy dátumot, és ha már frissitette egyszer a képet (újabb a helyi dátum, mint az adatbázisban),
             * akkor nem csinál semmit. Ha régebbi, akkor a partner azóta frissitette, tehát szükséges a letöltés.
             */
            string tmp = Path.GetTempPath();
            if (!Directory.Exists(tmp + "\\MSGer.tk\\pictures")) //2014.08.16. - Áthelyezve, hogy mindig létrehozza, ha kell, és letöltse a képeket
                Directory.CreateDirectory(tmp + "\\MSGer.tk\\pictures");

            //2014.08.16. - A képeket azért nem menti felhasználónként, mert úgyis le tudja tölteni mindenkinek a képét szinte bárki, és amúgy is UserID-val van azonosítva
            List<byte> sendb = new List<byte>();
            sendb.AddRange(BitConverter.GetBytes(CurrentUser.UserID));
            sendb.AddRange(BitConverter.GetBytes(UserID));
            sendb.AddRange(BitConverter.GetBytes((File.Exists(tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png")) ? PicUpdateTime : 0));
            byte[][] bytesb = Networking.SendUpdate(Networking.UpdateType.GetImage, sendb.ToArray(), false);
            if (bytesb == null || bytesb.All(entry => entry.Length == 0)) //bytesb.All(...): 2014.09.01.
            {
                if (File.Exists(tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png"))
                    return tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png";
                else
                    return "noimage.png";
            }
            bytesb = bytesb.Select(entry => Networking.ParsePacket(entry).Data).ToArray();
            //int[] picupdatetimes = bytesb.Select(b => BitConverter.ToInt32(b, 4)).ToArray(); //Az első 4 byte a UserID
            int[] picupdatetimes = bytesb.Select(b => BitConverter.ToInt32(b, 0)).ToArray();
            int maxIndex = Array.IndexOf<int>(picupdatetimes, picupdatetimes.Max());
            byte[] bytes = bytesb[maxIndex]; //Attól tölti le a képet, akinek a legfrissebb

            /*
             * Ez a funkció automatikusan elküldi a bejelentkezett felhasználó azonositóját,
             * a PHP szkript pedig leellenőrzi, hogy egymásnak partnerei-e, ezáltal nem nézheti meg akárki akárkinek a profilképét
             * (pedig a legtöbb helyen igy van, de szerintem jobb igy; lehet, hogy beállithatóvá teszem)
             */
            //if (str == "Fail" || str.Contains("NoChange"))
            if (bytes[0] == 0x00) //Nincs kép, vagy hiba történt
            {
                return "noimage.png";
            }
            else if (bytes[0] == 0x01)
            {
                return tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png";
            }
            else
            { //Mentse el a képet
                //string tmp = Path.GetTempPath();
                //if (!Directory.Exists(tmp + "\\MSGer.tk\\pictures"))
                //Directory.CreateDirectory(tmp + "\\MSGer.tk\\pictures");
                //File.WriteAllText(tmp + "\\MSGer.tk\\pictures\\" + ListID + ".png", str);
                File.WriteAllBytes(tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png", bytes);
                return tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png"; //2014.08.16.
            }
            //return "noimage.png";
        }
        public List<int> GetChatWindows()
        {
            List<int> retlist = new List<int>();
            for (int x = 0; x < ChatForm.ChatWindows.Count; x++)
            {
                if (ChatForm.ChatWindows[x].ChatPartners.Contains(UserID))
                {
                    retlist.Add(x);
                }
            }
            return retlist;
            //return ChatForm.ChatWindows.FindAll(entry => entry.ChatPartners.Contains(UserID)).Select(cform => (ChatForm.ChatWindows.IndexOf(cform))); - 2014.09.19.
        }
        /*public static int GetListIDFromUserID(int UserID)
        {
            for (int i = 0; i < UserInfo.KnownUsers.Count; i++)
            {
                if (UserInfo.KnownUsers[i].IsPartner && UserInfo.KnownUsers[i].UserID == UserID)
                    return UserInfo.KnownUsers[i].ListID;
            }
            return 0;
        }*/
        public static void Load()
        {
            foreach (var entry in Storage.LoggedInSettings)
            {
                string[] tmp = entry.Key.Split('_');
                if (tmp[0] != "userinfo")
                    continue;
                var tmp2 = new UserInfo();
                tmp2.UserID = Int32.Parse(tmp[1]);
                if (!IDIsInList(KnownUsers, tmp2.UserID))
                    KnownUsers.Add(tmp2);
            }
        }
        public static bool IDIsInList(List<UserInfo> list, int userid)
        {
            /*foreach(var entry in list)
            {
                if (entry.UserID == userid)
                    return true;
            }
            return false;*/
            return (list.Count(entry => entry.UserID == userid) > 0); //2014.09.19.
        }
        public static UserInfo Select(int userid)
        {
            /*for (int i = 0; i < KnownUsers.Count; i++)
            {
                if (KnownUsers[i].UserID == userid)
                {
                    return KnownUsers[i];
                }
            }
            return null;*/
            try
            {
                return KnownUsers.Single(entry => entry.UserID == userid); //2014.09.19.
            }
            catch
            {
                return null;
            }
        }
        public static int GetUserIDFromListID(int ListID)
        {
            for (int i = 0; i < UserInfo.KnownUsers.Count; i++)
            {
                if (UserInfo.KnownUsers[i].IsPartner && UserInfo.KnownUsers[i].ListID == ListID)
                    return UserInfo.KnownUsers[i].UserID;
            }
            return 0;
        }
        public static bool AutoUpdate { get; set; }
        private Timer UpdateTimer = new Timer(); //2014.09.26. - Csak másodpercenként frissíti az ismerőslistát
        public void Update()
        {
            if (!IsPartner || !AutoUpdate)
                return;
            if (!UpdateTimer.Enabled)
            {
                UpdateTimer.Interval = 500;
                UpdateTimer.Tick += UpdateTimerTick;
                UpdateTimer.Start();
            }
            //Partnerlista frissítése
            /*if (Program.MainF.contactList.Items.Count >= ListID)
                Program.MainF.contactList.Items.Add(new RichListViewItem());
            var item = Program.MainF.contactList.Items[ListID];*/
            /*var pictb = new PictureBox();
            string imgpath = this.GetImage();
            if (imgpath != "noimage.png" || File.Exists("noimage.png")) //2014.03.13.
                pictb.ImageLocation = imgpath;
            else
                MessageBox.Show(Language.Translate("noimage_notfound"), "Hiba");
            pictb.SizeMode = PictureBoxSizeMode.Zoom; //Megváltoztatva ScretchImage-ről
            var listtext = new ExRichTextBox();
            string state = "";
            if (this.State == 1)
                state = " (" + Language.Translate("menu_file_status_online") + ")";
            else if (this.State == 2)
                state = " (" + Language.Translate("menu_file_status_busy") + ")";
            else if (this.State == 3)
                state = " (" + Language.Translate("menu_file_status_away") + ")";
            else if (this.State == 0)
                state = " (" + Language.Translate("offline") + ")";
            else
                state = " (" + Language.Translate("networking_alone") + ")";
            listtext.Text = this.Name + state + "\n" + this.Message;
            listtext = TextFormat.Parse(listtext);
            //item.SubItems = new Control[] { pictb, listtext };

            //if (ListID != -1)
            //{
            if (ListID == -1)
                ListID = Program.MainF.contactList.Items.Count;
            if (Program.MainF.contactList.Items.Count > ListID)
                Program.MainF.contactList.Items.RemoveAt(ListID);
            Program.MainF.contactList.Items.Insert(ListID, new RichListViewItem(new Control[] { pictb, listtext }));*/
            /*if (Program.MainF.contactList.Items.Count <= ListID) //2014.09.26.
            { - Rájöttem, hogy amit eddig csináltam, az volt a legjobb - Ha egy elem változik, azt nem tudja érzékelni, csak a törlést és a hozzáadást
                Program.MainF.contactList.Items.Insert(ListID, new RichListViewItem(new Control[] { pictb, listtext }));
            }*/
            //Program.MainF.contactList.Items[ListID].SubItems[0] = pictb;
            //Program.MainF.contactList.Items[ListID].SubItems[1] = listtext;
            //Program.MainF.contactList.Items[ListID] = new RichListViewItem(new Control[] { pictb, listtext });
            //}
        }

        private void UpdateTimerTick(object sender, EventArgs e)
        {
            UpdateTimer.Stop();
            //Partnerlista frissítése
            /*var pictb = new PictureBox();
            string imgpath = this.GetImage();
            if (imgpath != "noimage.png" || File.Exists("noimage.png")) //2014.03.13.
                pictb.ImageLocation = imgpath;
            else
                MessageBox.Show(Language.Translate("noimage_notfound"), "Hiba");
            pictb.SizeMode = PictureBoxSizeMode.Zoom; //Megváltoztatva ScretchImage-ről
            var listtext = new ExRichTextBox();
            string state = "";
            if (this.State == 1)
                state = " (" + Language.Translate("menu_file_status_online") + ")";
            else if (this.State == 2)
                state = " (" + Language.Translate("menu_file_status_busy") + ")";
            else if (this.State == 3)
                state = " (" + Language.Translate("menu_file_status_away") + ")";
            else //if (this.State == 0)
                state = " (" + Language.Translate("offline") + ")";
            *else
                state = " (" + Language.Translate("networking_alone") + ")";*
            listtext.Text = this.Name + state + "\n" + this.Message;
            listtext = TextFormat.Parse(listtext);*/
            //2014.10.09.
            string imgpath = this.GetImage();
            if (!(imgpath != "noimage.png" || File.Exists("noimage.png"))) //2014.03.13. - 2014.10.09.
            {
                imgpath = "";
                MessageBox.Show(Language.Translate("noimage_notfound"), Language.Translate("error"));
            }
            string state = "";
            if (this.State == 1)
                state = " (" + Language.Translate("menu_file_status_online") + ")";
            else if (this.State == 2)
                state = " (" + Language.Translate("menu_file_status_busy") + ")";
            else if (this.State == 3)
                state = " (" + Language.Translate("menu_file_status_away") + ")";
            else //if (this.State == 0)
                state = " (" + Language.Translate("offline") + ")";
            string text = this.Name + state + "\n" + this.Message;

            if (ListID == -1)
            {
                //ListID = Program.MainF.contactList.Items.Except(Program.MainF.contactList.Items.TakeWhile(entry => entry.SubItems[1].Text != "")).TakeWhile(entry => entry.SubItems[1].Text == "").Count();
                int i;
                for (i = 0; i < Program.MainF.contactList.Items.Count; i++)
                {
                    if (Program.MainF.contactList.Items[i].SubItems[1].Text == "")
                    {
                        break;
                    }
                }
                ListID = i;
            }
            /*if (Program.MainF.contactList.Items.Count > ListID)
                Program.MainF.contactList.Items.RemoveAt(ListID);
            else //Ha nincs elég eleme a listának
            {
                int i = Program.MainF.contactList.Items.Count;
                do
                {
                    Program.MainF.contactList.Items.Insert(i, new RichListViewItem());
                } while (Program.MainF.contactList.Items.Count <= ListID);
            }
            Program.MainF.contactList.Items.Insert(ListID, new RichListViewItem(new Control[] { pictb, listtext }));*/
            //2014.10.09.
            bool tmp = Program.MainF.contactList.AutoUpdate;
            Program.MainF.contactList.AutoUpdate = false;
            while (Program.MainF.contactList.Items.Count <= ListID) //Azt is adja hozzá, ami a kész listaelem lesz
            {
                var pictb = new PictureBox();
                pictb.SizeMode = PictureBoxSizeMode.Zoom;
                pictb.ImageLocation = imgpath;
                var listtext = new ExRichTextBox();
                listtext.Text = text;
                listtext = TextFormat.Parse(listtext);
                Program.MainF.contactList.Items.Add(new RichListViewItem(new Control[] { pictb, listtext }));
                //System.Threading.Thread.Sleep(1000);
            }
            ((PictureBox)Program.MainF.contactList.Items[ListID].SubItems[0]).ImageLocation = imgpath;
            Program.MainF.contactList.Items[ListID].SubItems[1].Text = text;
            Program.MainF.contactList.Items[ListID].SubItems[1] = TextFormat.Parse((ExRichTextBox)Program.MainF.contactList.Items[ListID].SubItems[1]);
            Program.MainF.contactList.AutoUpdate = tmp;
        }
        /*public static void AddCurrentUser()
        { //2014.09.01.
            if (UserInfo.Select(CurrentUser.UserID) != null)
                return;
            var tmp = new UserInfo();
            tmp.Email = CurrentUser.Email;
            tmp.IsPartner = false; //Ha még nem adta hozzá, nem ismerősök
            tmp.LastUpdate = Int32.Parse(Program.DateTimeToUnixTime(DateTime.Now));
            tmp.Message = CurrentUser.Message;
            tmp.Name = CurrentUser.Name;
            tmp.PicUpdateTime = Int32.Parse(Program.DateTimeToUnixTime(DateTime.Now));
            tmp.State = CurrentUser.State;
            tmp.UserID = CurrentUser.UserID;
            tmp.UserName = CurrentUser.UserName;
            KnownUsers.Add(tmp);
        }*/
    }
}
