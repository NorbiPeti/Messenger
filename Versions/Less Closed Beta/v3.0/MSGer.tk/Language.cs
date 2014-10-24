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
                //(new Language(files[x].Split('\\')[files[x].Split('\\').Length - 1].Split('.')[0])).Strings = dict; //Eltárol egy új nyelvet, majd a szövegeket hozzátársítja
                new Language(new FileInfo(files[x]).Name.Split('.')[0]).Strings = dict; //(FileInfo: 2014.09.01.) - Eltárol egy új nyelvet, majd a szövegeket hozzátársítja
            }

            if (Language.FromString(Storage.Settings["lang"]) == null)
            {
                MessageBox.Show("Error: Could not find language: " + Storage.Settings["lang"]);
                return;
            }
            CurrentUser.Language = Language.FromString(Storage.Settings["lang"]);
            if (CurrentUser.Language == null)
            {
                //MessageBox.Show("Error: The specified language is not found.\nTo quickly solve this, copy the preffered language file in languages folder to the same place with the name of \"" + Storage.Settings["lang"] + "\"\nYou can then change the language in your preferences later.");
                //return;
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
            /*try
            {
                tmp = UsedLangs[value];
            }
            catch
            {
            }*/
            UsedLangs.TryGetValue(value, out tmp);
            return tmp;
        }
        public static Language GetCuurentLanguage()
        {
            return Language.FromString(Storage.Settings["lang"]);
        }
        public static string Translate(string id)
        { //2014.08.19.
            Language lang = GetCuurentLanguage();
            if (lang.Strings.ContainsKey(id))
                return lang.Strings[id];
            else
            {
                MessageBox.Show("Translation string not found: " + id + "\nIn file: " + lang + ".txt");
                return "Str not found";
            }
        }
    }
}
