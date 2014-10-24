using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MSGer.tk
{
    public static class CurrentUser
    {
        /*
         * 2014.03.05.
         * Információátrendezés: Property-k használata; Minden felhasználóhoz egy-egy User class
         * Ez a class használható lenne az aktuális felhsaználó információinak tárolására
         */
        //public static int UserID = 0;
        public static int UserID
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_userid"))
                    Storage.LoggedInSettings.Add("currentuser_userid", "0");
                return Int32.Parse(Storage.LoggedInSettings["currentuser_userid"]);
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_userid"))
                    Storage.LoggedInSettings.Add("currentuser_userid", "0");
                Storage.LoggedInSettings["currentuser_userid"] = value.ToString();
            }
        }
        //public static int[] PartnerIDs = new int[1024];
        //public static string Name = "";
        public static string Name
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_name"))
                    Storage.LoggedInSettings.Add("currentuser_name", "");
                return Storage.LoggedInSettings["currentuser_name"];
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_name"))
                    Storage.LoggedInSettings.Add("currentuser_name", "");
                var tmp = UserInfo.Select(CurrentUser.UserID);
                if (tmp != null)
                    tmp.Name = value;
                Storage.LoggedInSettings["currentuser_name"] = value;
                SendUpdate();
            }
        }
        //public static Language Language = Language.English; //2014.04.19.
        //public static Language Language;
        public static Language Language
        {
            get
            {
                return Language.FromString(Storage.Settings["lang"]);
            }
            set
            {
                Storage.Settings["lang"] = value.ToString();
            }
        }
        //public static string Message = "";
        public static string Message
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_message"))
                    Storage.LoggedInSettings.Add("currentuser_message", "");
                return Storage.LoggedInSettings["currentuser_message"];
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_message"))
                    Storage.LoggedInSettings.Add("currentuser_message", "");
                var tmp = UserInfo.Select(CurrentUser.UserID);
                if (tmp != null)
                    tmp.Message = value;
                Storage.LoggedInSettings["currentuser_message"] = value;
                SendUpdate();
            }
        }
        //public static string State = "";
        public static int State
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_state"))
                    Storage.LoggedInSettings.Add("currentuser_state", "0");
                return Int32.Parse(Storage.LoggedInSettings["currentuser_state"]);
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_state"))
                    Storage.LoggedInSettings.Add("currentuser_state", "0");
                var tmp = UserInfo.Select(CurrentUser.UserID);
                if (tmp != null)
                    tmp.State = value;
                Storage.LoggedInSettings["currentuser_state"] = value.ToString();
                SendUpdate();
            }
        }
        //public static string UserName = "";
        public static string UserName
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_username"))
                    Storage.LoggedInSettings.Add("currentuser_username", "");
                return Storage.LoggedInSettings["currentuser_username"];
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_username"))
                    Storage.LoggedInSettings.Add("currentuser_username", "");
                var tmp = UserInfo.Select(CurrentUser.UserID);
                if (tmp != null)
                    tmp.UserName = value;
                Storage.LoggedInSettings["currentuser_username"] = value;
                SendUpdate();
            }
        }
        //public static string Email = "";
        public static string Email
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_email"))
                    Storage.LoggedInSettings.Add("currentuser_email", "");
                return Storage.LoggedInSettings["currentuser_email"];
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_email"))
                    Storage.LoggedInSettings.Add("currentuser_email", "");
                var tmp = UserInfo.Select(CurrentUser.UserID);
                if (tmp != null)
                    tmp.Email = value;
                Storage.LoggedInSettings["currentuser_email"] = value;
                SendUpdate();
            }
        }
        //private static IPAddress[] ips = new IPAddress[2];
        /*public static IPAddress[] IPs //2014.08.29.
        {
            get
            {
                if (!Storage.Settings.ContainsKey("myip"))
                    Storage.LoggedInSettings.Add("myip", "127.0.0.1");
                string[] strs = Storage.LoggedInSettings["myip"].Split(';');
                *var ips = new IPAddress[strs.Length];
                for (int i = 0; i < ips.Length; i++)
                {
                    ips[i] = IPAddress.Parse(strs[i]);
                }*
                IPAddress[] ips = strs.Select(entry => IPAddress.Parse(entry)).ToArray();
                return ips;
            }
            set
            {
                if (!Storage.Settings.ContainsKey("myip"))
                    Storage.Settings.Add("myip", "");
                string[] strs = value.Select(entry => entry.ToString()).ToArray();
                Storage.Settings["myip"] = String.Join(";", strs);
            }
        }*/
        //public static IPAddress[] IPs { get; set; }
        //private static List<IPAddress> ips = new List<IPAddress>();
        /*public static List<IPAddress> IPs
        {
            get
            {
                return ips;
            }
            set
            {
                ips = value;
            }
        }*/
        public static IPAddress IP;
        /*public static IPAddress IP
        {
            get
            {
                return IPAddress.Parse(Storage.Settings["myip"]);
            }
            set
            {
                Storage.Settings["myip"] = value.ToString();
            }
        }*/
        public static string[] Keys
        { //2014.09.08-09.
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_keys"))
                    Storage.LoggedInSettings.Add("currentuser_keys", "");
                var tmp = new string[100]; //Mindig 100 elemű tömb legyen
                Storage.LoggedInSettings["currentuser_keys"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).CopyTo(tmp, 0);
                return tmp;
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_keys"))
                    Storage.LoggedInSettings.Add("currentuser_keys", "");
                string x = "";
                //Storage.LoggedInSettings["currentuser_keys"] = value.Select(new Func<string, string>(delegate { var x = ""; foreach (var item in value) { x += item + ";"; } return x; }));
                foreach (var item in value)
                {
                    x += item;
                    x += ";";
                }
                Storage.LoggedInSettings["currentuser_keys"] = x;
            }
        }
        public static int KeyIndex
        { //2014.09.09.
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_keyindex"))
                    Storage.LoggedInSettings.Add("currentuser_keyindex", "0");
                return Int32.Parse(Storage.LoggedInSettings["currentuser_keyindex"]);
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_keyindex"))
                    Storage.LoggedInSettings.Add("currentuser_keyindex", "0");
                Storage.LoggedInSettings["currentuser_keys"] = value.ToString();
            }
        }
        public static bool SendChanges = false;
        ///// <summary>
        ///// Átmásolja-e a memóriába az egész fájlt a küldés előtt.
        ///// Nagy méretű fájloknál nem ajánlott, különben igen a fájl esetleges elérhetetlensége miatt.
        ///// 2014.06.15.
        ///// </summary>
        //public static bool CopyToMemoryOnFileSend = true; - Automatikusan érzékelje (2014.08.18.)

        public static void SendUpdate()
        { //2014.08.30.
            /*
             * CurrentUser.SendUpdate()
             * Bármi változás történik, elküldi mindenkinek
             */
            if (!SendChanges)
                return;
            string retstr = "";
            retstr += UserID + "_name=" + Name + "\n";
            retstr += UserID + "_message=" + Message + "\n";
            retstr += UserID + "_state=" + State + "\n";
            retstr += UserID + "_username=" + UserName + "\n";
            retstr += UserID + "_email=" + Email + "\n";
            retstr += UserID + "_ispartner=" + false + "\n"; //Ellenőrizze le, amikor megkapja
            retstr += UserID + "_lastupdate=" + Program.DateTimeToUnixTime(DateTime.Now);
            while (true)
            {
                byte[][] resp = Networking.SendUpdate(Networking.UpdateType.ListUpdate, Encoding.Unicode.GetBytes(retstr), false);
                bool fine = false;
                if (resp == null || resp.Length == 0)
                    break;
                //foreach (var item in resp) //Ha sehonnan nem kapott választ (egy perc után), újrapróbálkozik
                foreach (var item in resp) //Ha sehonnan nem kapott választ (egy perc után), újrapróbálkozik
                {
                    //if (item[4] == 0x01) //Az első 4 byte a UserID
                    if (Networking.ParsePacket(item).Data[0] == 0x01) //2014.09.19.
                        fine = true;
                }
                if (fine)
                    break;
            }
        }
    }
}
