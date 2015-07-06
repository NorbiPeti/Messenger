using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
                    return 0; //2015.06.25. - Nincs bejelentkezve
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
                return Language.CurrentLanguage; //2015.05.16.
            }
            set
            {
                Language.CurrentLanguage = value; //2015.05.16.
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
        public static double PicUpdateTime //2015.06.06. - int-->double
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

        public static int Port; //2015.05.24.

        public static bool SendChanges = false;

        public static void SendUpdate()
        { //2014.08.30.
            if (!SendChanges)
                return;
            List<string> retstr = new List<string>();
            retstr.Add(UserID + "_name=" + Name);
            retstr.Add(UserID + "_message=" + Message);
            retstr.Add(UserID + "_state=" + State);
            retstr.Add(UserID + "_username=" + UserName);
            retstr.Add(UserID + "_email=" + Email);
            retstr.Add(UserID + "_ispartner=" + false); //Ellenőrizze le, amikor megkapja
            retstr.Add(UserID + "_lastupdate=" + Program.DateTimeToUnixTime(DateTime.Now));
            retstr.Add(UserID + "_picupdatetime=" + PicUpdateTime);
            var task = new Networking.PacketSender(new Networking.PDListUpdate(retstr.ToArray())).SendAsync();
        }

        private static Image image;
        public static Image Image
        { //2015.06.06.
            get
            {
                if (image == null)
                {
                    if (!Directory.Exists("pictures"))
                        Directory.CreateDirectory("pictures");
                    string[] files = Directory.GetFiles("pictures", UserID + ".*");
                    if (files.Length > 0)
                        image = Program.LoadImageFromFile(files[0]);
                    else
                        image = UserInfo.NoImage.Clone() as Image; //Ugyanaz, mint a Properties.Resources.noimage
                }
                return image;
            }
            set
            {
                if (image != null)
                    image.Dispose();
                image = value;
                string[] files = Directory.GetFiles("pictures", UserID + ".*");
                foreach (string file in files)
                {
                    if (Path.GetExtension(file) != ".png")
                        File.Delete(file);
                }
                image.Save("pictures\\" + UserID + ".png", ImageFormat.Png);
                PicUpdateTime = Program.DateTimeToUnixTime(DateTime.Now);
                var tmp = UserInfo.Select(CurrentUser.UserID);
                if (tmp != null)
                {
                    tmp.Image = value;
                    tmp.PicUpdateTime = PicUpdateTime;
                }
                SendUpdate();
            }
        }
    }
}
