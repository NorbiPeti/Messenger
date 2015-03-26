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
        //public static IPAddress IP;
        public static List<IPAddress> IPs;
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
        public static int PicUpdateTime
        {
            get
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_picupdatetime"))
                    Storage.LoggedInSettings.Add("currentuser_picupdatetime", "0");
                return Int32.Parse(Storage.LoggedInSettings["currentuser_picupdatetime"]);
            }
            set
            {
                if (!Storage.LoggedInSettings.ContainsKey("currentuser_picupdatetime"))
                    Storage.LoggedInSettings.Add("currentuser_picupdatetime", "0");
                Storage.LoggedInSettings["currentuser_picupdatetime"] = value.ToString();
            }
        }
        public static bool SendChanges = false;

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
            retstr += UserID + "_picupdatetime=" + PicUpdateTime;
            //while (true)
            //{
                //byte[][] resp = Networking.SendUpdate(Networking.UpdateType.ListUpdate, Encoding.Unicode.GetBytes(retstr), false);
                //bool fine = false; //Elvileg így az event működésekor is tudja használni, és utána ha minden rendben, akkor törli az objectet
                Networking.SendUpdateInThread(Networking.UpdateType.ListUpdate, Encoding.Unicode.GetBytes(retstr), null);
                    /*(e, resp) =>
                    {
                        if (resp == null || resp.Length == 0)
                            fine = true;
                        foreach (var item in resp) //Ha sehonnan nem kapott választ (egy perc után), újrapróbálkozik
                        {
                            if (Networking.ParsePacket(item).Data[0] == 0x01) //2014.09.19.
                                fine = true;
                        }*/
                /*if (fine)
                    break;*/
            //}
        }
    }
}
