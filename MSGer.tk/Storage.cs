using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MSGer.tk
{
    static class Storage
    { //2014.08.07.
        public static string FileName;
        public static Dictionary<string, string> Settings = new Dictionary<string, string>();
        public static Dictionary<string, string> LoggedInSettings = new Dictionary<string, string>();
        public static readonly string PasswordHash = "PWPassword";
        public static string SaltKey; //Bejelentkezéskor kapja meg
        public static readonly string VIKey = "SGf546HEfg56h45K";
        public static void Load(bool loggedin)
        {
            if (!loggedin)
            {
                SaltKey = "nologinnologinnologinnologin";
                FileName = "program.db";
            }
            if (!File.Exists(FileName))
            {
                if (!loggedin)
                {
                    Settings.Add("email", "");
                    Settings.Add("windowstate", "3");
                    Settings.Add("lang", CultureInfo.InstalledUICulture.TwoLetterISOLanguageName);
                    Settings.Add("port", "4510"); //Use this to connect to different users <-- És fogalmam sincs, miért angolul írtam...
                    Settings.Add("lastusedemail", "0");
                    Settings.Add("filelen", "-1"); //(long) Maximum fájlméret, ameddig bemásolhatja a memóriába
                    //Settings.Add("isserver", "");
                    Settings.Add("chatwindow", "0");
                }
            }
            else
            {
                Parse(Decrypt(Read(loggedin)), loggedin);
                if (loggedin)
                    UserInfo.Load();
            }
        }

        public static void Save(bool loggedin)
        {
            if (!loggedin)
                SaltKey = "nologinnologinnologinnologin";
            Write(Encrypt(GetString(loggedin)), loggedin);
        }

        public static string GetString(bool loggedin)
        {
            string s = "";
            if (loggedin)
            {
                foreach (var entry in LoggedInSettings)
                {
                    s += entry.Key;
                    s += "=";
                    s += entry.Value;
                    s += "\n";
                }
            }
            else
            {
                foreach (var entry in Settings)
                {
                    s += entry.Key;
                    s += "=";
                    s += entry.Value;
                    s += "\n";
                }
            }
            return s;
        }

        private static void Parse(string filecontent, bool loggedin)
        {
            string[] splitCache = filecontent.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var tmp = splitCache.ToDictionary(
                                   entry => entry.Substring(0, entry.IndexOf("=")),
                                   entry => entry.Substring(entry.IndexOf("=") + 1));
            if (loggedin)
                LoggedInSettings = tmp;
            else
                Settings = tmp;
        }

        public static void Parse(string filecontent) //Publikus metódus
        {
            string[] splitCache = filecontent.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var tmp = splitCache.ToDictionary(
                                   entry => "userinfo_" + entry.Substring(0, entry.IndexOf("=")),
                                   entry => entry.Substring(entry.IndexOf("=") + 1));
            LoggedInSettings = LoggedInSettings.Concat(tmp)
                         .ToLookup(pair => pair.Key, pair => pair.Value)
                         .ToDictionary(group => group.Key, group => group.Last());
        }
        
        public static byte[] Encrypt(byte[] content, string code)
        {
            //byte[] plainTextBytes = content;

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(code)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] plainTextBytes;
            //if (content.Length + 4 > keyBytes.Length)
                /*plainTextBytes = new byte[content.Length + 4];
            else
                plainTextBytes = new byte[content.Length + 4];*/
            int targetsize = content.Length + 16 - content.Length % 16; //Hozzáadja a hosszához a hossz 16-tal való osztásának maradékát - Tehát 16-tal osztható lesz
            plainTextBytes = new byte[targetsize];
            /*Array.Copy(BitConverter.GetBytes(content.Length), plainTextBytes, 4);
            Array.Copy(content, 0, plainTextBytes, 4, content.Length);*/
            Array.Copy(content, plainTextBytes, content.Length);

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            byte[] final = new byte[cipherTextBytes.Length + 4];
            Array.Copy(BitConverter.GetBytes(cipherTextBytes.Length), final, 4);
            Array.Copy(cipherTextBytes, 0, final, 4, cipherTextBytes.Length);
            return final;
        }
        private static byte[] Encrypt(byte[] filecontent)
        { //2014.09.01.
            return Encrypt(filecontent, SaltKey);
        }
        private static byte[] Encrypt(string filecontent)
        {
            return Encrypt(Encoding.UTF8.GetBytes(filecontent));
        }

        public static byte[] Decrypt(byte[] b, bool tr, string code)
        {
            //byte[] cipherTextBytes = b;
            int len = BitConverter.ToInt32(b, 0);
            byte[] cipherTextBytes = new byte[len];
            Array.Copy(b, 4, cipherTextBytes, 0, b.Length - 4); //Itt még az eredeti, feltöltött hosszal számol
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(code)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            //List<byte> plainTextBytes = new List<byte>(); //2014.12.18.

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            /*int r=0; //2014.12.18.
            while ((r = cryptoStream.ReadByte()) != -1)
            { //2014.12.18.
                plainTextBytes.Add((byte)r);
            }*/
            memoryStream.Close();
            cryptoStream.Close();
            byte[] ret = new byte[len];
            Array.Copy(plainTextBytes, ret, len);
            return ret;
        }
        private static byte[] Decrypt(byte[] b, bool tr)
        { //2014.09.01.
            return Decrypt(b, tr, SaltKey);
        }
        private static string Decrypt(byte[] b)
        {
            byte[] tmp = Decrypt(b, true, SaltKey);
            return Encoding.UTF8.GetString(tmp, 0, tmp.Length).TrimEnd("\0".ToCharArray());
        }

        private static byte[] Read(bool loggedin)
        {
            FileStream fs;
            if (loggedin)
                fs = new FileStream(FileName, FileMode.Open);
            else
                fs = new FileStream("program.db", FileMode.Open);
            byte[] b = new byte[4];
            fs.Read(b, 0, b.Length);
            var file_len = BitConverter.ToInt32(b, 0);
            b = new byte[file_len];
            fs.Read(b, 0, b.Length);
            fs.Close();
            return b;
        }

        private static void Write(byte[] b, bool loggedin)
        {
            var len = BitConverter.GetBytes(b.Length);
            FileStream fs;
            if (loggedin)
                fs = new FileStream(FileName, FileMode.Create);
            else
                fs = new FileStream("program.db", FileMode.Create);
            fs.Write(len, 0, len.Length);
            fs.Write(b, 0, b.Length);
            fs.Close();
        }

        public static void Dispose()
        {
            LoggedInSettings = new Dictionary<string, string>();
        }
    }
}
