﻿using System;
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
            }
        }
        public int TMPListID { get; set; } //2014.12.05.
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
                Update();
            }
        }
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
                Update();
            }
        }
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
                Update();
            }
        }
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
        public string LoginCode //
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
        public int PicUpdateTime
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_picupdatetime"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_picupdatetime", "0");
                return Int32.Parse(Storage.LoggedInSettings["userinfo_" + UserID + "_picupdatetime"]);
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("userinfo_" + UserID + "_picupdatetime"))
                    Storage.LoggedInSettings.Add("userinfo_" + UserID + "_picupdatetime", "0");
                Storage.LoggedInSettings["userinfo_" + UserID + "_picupdatetime"] = value.ToString();
                Update();
            }
        }
        public string ImagePath = "noimage.png";

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
        }
        ~UserInfo() //2014.10.09.
        {
        }
        public void GetImageFromNetwork(int receivedupdate)
        { //Most már elvileg csak akkor hívja meg, amikor feldolgozza a kapott adatokat, tehát nem a Main Thread-ban
            string tmp = Path.GetTempPath();
            if (!Directory.Exists(tmp + "\\MSGer.tk\\pictures")) //2014.08.16. - Áthelyezve, hogy mindig létrehozza, ha kell, és letöltse a képeket
                Directory.CreateDirectory(tmp + "\\MSGer.tk\\pictures");

            if (this.PicUpdateTime >= receivedupdate) //> --> >=: 2015.05.10. - Ha mindkettő 0, ne töltse le; meg egyébként is, csak ha újabb
            {
                if (File.Exists(tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png"))
                    this.ImagePath = tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png";
                else
                    this.ImagePath = "noimage.png";
                return;
            }

            //2014.08.16. - A képeket azért nem menti felhasználónként, mert úgyis le tudja tölteni mindenkinek a képét szinte bárki, és amúgy is UserID-val van azonosítva
            /*List<byte> sendb = new List<byte>();
            sendb.AddRange(BitConverter.GetBytes(UserID));
            sendb.AddRange(BitConverter.GetBytes((File.Exists(tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png")) ? PicUpdateTime : 0));*/
            //byte[][] bytesb = Networking.SendUpdate(Networking.UpdateType.GetImage, sendb.ToArray(), false);
            //Networking.PacketFormat[] pfs = Networking.SendUpdate(new Networking.PacketFormat(false,
            var pfs = new Networking.PacketSender(
                new Networking.PDGetImage(UserID, (File.Exists(tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png") ? PicUpdateTime : 0))).Send();
            //if (bytesb == null || bytesb.All(entry => entry.Length == 0)) //bytesb.All(...): 2014.09.01.
            if (pfs == null || pfs.All(entry => !((Networking.PDGetImage)entry.EData).RSuccess))
            {
                if (File.Exists(tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png"))
                    this.ImagePath = tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png";
                else
                    this.ImagePath = "noimage.png";
                return;
            }
            //bytesb = bytesb.Select(entry => Networking.ParsePacket(entry).Data).ToArray();
            //int[] picupdatetimes = bytesb.Select(b => BitConverter.ToInt32(b, 0)).ToArray();
            /*int maxIndex = Array.IndexOf<int>(picupdatetimes, picupdatetimes.Max());
            byte[] bytes = new byte[bytesb[maxIndex].Length];
            Array.Copy(bytesb[maxIndex], 4, bytes, 0, bytes.Length); //Hagyja ki a PicUpdateTime-ot*/
            IEnumerable<int> picupdatetimes = pfs.Select(entry => ((Networking.PDGetImage)entry.EData).RPicUpdateTime);
            byte[] bytes = null;
            foreach (var entry in pfs)
            {
                if (((Networking.PDGetImage)entry.EData).RPicUpdateTime == ((Networking.PDGetImage)entry.EData).RPicUpdateTime)
                {
                    bytes = ((Networking.PDGetImage)entry.EData).RImageData;
                    break;
                }
            }

            if (bytes == null || bytes[0] == 0x00) //Nincs kép, vagy hiba történt
            {
                this.ImagePath = "noimage.png";
            }
            else if (bytes[0] == 0x01)
            {
                //this.ImagePath = tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png";
                if (File.Exists(tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png"))
                    this.ImagePath = tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png";
                else
                    this.ImagePath = "noimage.png";
            }
            else
            { //Mentse el a képet
                File.WriteAllBytes(tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png", bytes);
                this.ImagePath = tmp + "\\MSGer.tk\\pictures\\" + UserID + ".png"; //2014.08.16.
            }
        }
        public List<int> GetChatWindows()
        {
            List<int> retlist = new List<int>();
            for (int x = 0; x < ChatPanel.ChatWindows.Count; x++)
            {
                if (ChatPanel.ChatWindows[x].ChatPartners.Any(entry => entry.UserID == UserID))
                {
                    retlist.Add(x);
                }
            }
            return retlist;
        }
        public static void Load()
        {
            foreach (var entry in Storage.LoggedInSettings)
            {
                try
                {
                    string[] tmp = entry.Key.Split('_');
                    if (tmp[0] != "userinfo")
                        continue;
                    var tmp2 = new UserInfo();
                    tmp2.UserID = Int32.Parse(tmp[1]);
                    if (!IDIsInList(KnownUsers, tmp2.UserID))
                        KnownUsers.Add(tmp2);
                }
                catch
                {
                }
            }
        }
        public static bool IDIsInList(List<UserInfo> list, int userid)
        {
            return (list.Count(entry => entry.UserID == userid) > 0); //2014.09.19.
        }
        public static UserInfo Select(int userid)
        {
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
                if (UserInfo.KnownUsers[i].IsPartner && UserInfo.KnownUsers[i].TMPListID == ListID) //Ahol szükség van rá, ott az aktuális ListID szükséges, nem a beállított
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
                if (UpdateTimer.Tag == null || !(bool)UpdateTimer.Tag)
                {
                    UpdateTimer.Interval = 500;
                    UpdateTimer.Tick += UpdateTimerTick;
                    UpdateTimer.Tag = true;
                }
                if (Program.MainF.InvokeRequired)
                    Program.MainF.Invoke(new Action(() => UpdateTimer.Start()));
                else
                    UpdateTimer.Start();
            }
        }

        private void UpdateTimerTick(object sender, EventArgs e)
        {
            UpdateTimer.Stop();
            if (ListID == -1)
            {
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
            CreateListItem(Program.MainF.contactList, ListID);
        }

        public void CreateListItem(RichListView listView, int pos)
        {
            string imgpath = this.ImagePath; //2014.12.31.
            if (!(imgpath != "noimage.png" || File.Exists("noimage.png"))) //2014.03.13. - 2014.10.09.
            {
                imgpath = "";
                MessageBox.Show(Language.Translate(Language.StringID.NoImage_NotFound), Language.Translate(Language.StringID.Error));
            }
            string state = "";
            if (this.State == 1)
                state = " (" + Language.Translate(Language.StringID.Menu_File_Status_Online) + ")";
            else if (this.State == 2)
                state = " (" + Language.Translate(Language.StringID.Menu_File_Status_Busy) + ")";
            else if (this.State == 3)
                state = " (" + Language.Translate(Language.StringID.Menu_File_Status_Away) + ")";
            else
                state = " (" + Language.Translate(Language.StringID.Offline) + ")";
            string text = this.Name + state + "\n" + this.Message;

            TMPListID = pos;

            bool tmp = listView.AutoUpdate;
            listView.AutoUpdate = false;
            listView.SuspendLayout(); //2014.12.21.
            while (listView.Items.Count <= TMPListID) //Azt is adja hozzá, ami a kész listaelem lesz
            {
                var pictb = new PictureBox();
                pictb.SizeMode = PictureBoxSizeMode.Zoom;
                pictb.ImageLocation = imgpath;
                var listtext = new ExRichTextBox();
                listtext.Text = text;
                listtext = TextFormat.Parse(listtext);
                listView.Items.Add(new RichListViewItem(new Control[] { pictb, listtext }));
            }
            ((PictureBox)listView.Items[TMPListID].SubItems[0]).ImageLocation = imgpath;
            listView.Items[TMPListID].SubItems[1].Text = text;
            listView.Items[TMPListID].SubItems[1] = TextFormat.Parse((ExRichTextBox)listView.Items[TMPListID].SubItems[1]);
            listView.AutoUpdate = tmp;
            listView.ResumeLayout(true); //2014.12.21.
        }

        public override string ToString()
        { //2015.04.03.
            string str = "";
            foreach (var setting in Storage.LoggedInSettings)
                if (setting.Key.StartsWith("userinfo_" + UserID + "_"))
                    str += setting.Key.Substring(setting.Key.IndexOf(UserID.ToString())) + "=" + setting.Value + "\n";
            str = str.Remove(str.Length - 1);
            return str;
        }
    }
}