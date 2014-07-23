using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSGer.tk
{
    public class Language
    { //2014.04.19.

        public static Dictionary<string, Language> UsedLangs = new Dictionary<string, Language>();

        public Dictionary<string, string> Strings = new Dictionary<string, string>();

        public Language(string lang)
        {
            UsedLangs.Add(lang, this);
        }
        public override string ToString()
        {
            return UsedLangs.FirstOrDefault(x => x.Value == this).Key;
        }
        public static Language FromString(string value)
        {
            Language tmp = null;
            try
            {
                tmp = UsedLangs[value];
            }
            catch
            {
            }
            return tmp;
        }
        public static Language GetCuurentLanguage()
        {
            return Language.FromString(Settings.Default.lang);
        }
    }
}
