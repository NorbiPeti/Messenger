using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    public class Language
    { //2014.04.19.

        public static Dictionary<string, Language> UsedLangs = new Dictionary<string, Language>();

        public Dictionary<string, string> Strings = new Dictionary<string, string>();

        private static Dictionary<Control, string> Controls = new Dictionary<Control, string>();

        private Language(string lang)
        {
            UsedLangs.Add(lang, this);
        }
        public Language() //2014.09.06.
        {
            if (!Directory.Exists("languages"))
                Directory.CreateDirectory("languages");
            string[] files = Directory.GetFiles("languages");
            if (files.Length == 0)
            {
                MessageBox.Show("Error: No languages found.");
                return; //Még nem jelentkezett be, ezért ki fog lépni
            }
            for (int x = 0; x < files.Length; x++)
            {
                string[] lines = File.ReadAllLines(files[x]);
                var dict = lines.Select(l => l.Split('=')).ToDictionary(a => a[0], a => a[1]);
                var finaldict = new Dictionary<string, string>();
                foreach(var item in dict)
                {
                    var spl = item.Key.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach(var key in spl)
                    {
                        finaldict.Add(key, item.Value); //Hozzáadja az összes felsorolt keyt külön, ugyanazzal az értékkel
                    }
                }
                new Language(new FileInfo(files[x]).Name.Split('.')[0]).Strings = finaldict; //(FileInfo: 2014.09.01.) - Eltárol egy új nyelvet, majd a szövegeket hozzátársítja
            }

            /*if (Language.FromString(Storage.Settings["lang"]) == null)
            {
                MessageBox.Show("Error: Could not find language: " + Storage.Settings["lang"]);
                return;
            }*/
            CurrentUser.Language = Language.FromString(Storage.Settings["lang"]);
            if (CurrentUser.Language == null)
            {
                if (Language.UsedLangs.ContainsKey("en"))
                {
                    MessageBox.Show("Error: The specified language (" + Storage.Settings["lang"] + ") is not found.\nThe program will use english that you can change later.");
                    Storage.Settings["lang"] = "en";
                }
                else
                {
                    MessageBox.Show("Error: The specified language (" + Storage.Settings["lang"] + "), nor enlish are found.\nPlease download translations.");
                    return;
                }
            }
        }
        public override string ToString()
        {
            return UsedLangs.FirstOrDefault(x => x.Value == this).Key;
        }
        public static Language FromString(string value)
        {
            Language tmp = null;
            UsedLangs.TryGetValue(value, out tmp);
            return tmp;
        }
        public static Language GetCurrentLanguage() //Javítva Cuurent-ről Current-re: 2014.12.13. - Már régóta ki akartam javítani
        {
            return Language.FromString(Storage.Settings["lang"]);
        }
        public static string Translate(string id, Control defaultevent = null) //Csak akkor kell az event, ha látszódik az adott ablak, amikor átállítódik - Tehát csak MainForm és ChatForm
        { //2014.08.19.
            Language lang = GetCurrentLanguage();
            if (lang.Strings.ContainsKey(id))
            {
                if (defaultevent != null) //2014.12.22.
                    ReloadEvent += delegate { defaultevent.Text = lang.Strings[id]; }; //2014.12.22.
                return lang.Strings[id];
            }
            else
            {
                MessageBox.Show("Translation string not found: " + id + "\nIn file: " + lang + ".txt");
                return "Str not found";
            }
        }
        public static event EventHandler ReloadEvent;
        public static void ReloadLangs()
        {
            ChatPanel.ReopenChatWindows(false);
            ReloadEvent(null, null);
            Program.MainF.contactList.Items.Clear();
            Program.MainF.LoadPartnerList();
        }
    }
}
