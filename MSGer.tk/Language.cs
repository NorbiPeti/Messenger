using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    public class Language : IPackable, IEquatable<Language>, IPackWithDefaults
    { //2014.04.19.
        public Dictionary<string, string> Strings = new Dictionary<string, string>();

        private static Dictionary<Control, string> Controls = new Dictionary<Control, string>();
        private Language()
        {
        }

        public static List<Language> Languages = new List<Language>(); //2015.05.16.

        private string LanguageKey = ""; //2015.04.03.
        public override string ToString()
        {
            return LanguageKey;
        }
        public static Language FromString(string value)
        {
            Language tmp = null;
            tmp = Languages.FirstOrDefault(entry => entry.LanguageKey == value); //Single-->FirstOrDefault: 2015.06.06.
            return tmp;
        }
        private static Language currentlanguage;
        public static Language CurrentLanguage
        { //2015.04.03.
            get
            {
                if (currentlanguage == null) //2015.05.16.
                {
                    if (Storage.Settings[SettingType.Lang] == "")
                        Storage.Settings[SettingType.Lang] = "en"; //2015.05.21.
                    currentlanguage = Language.FromString(Storage.Settings[SettingType.Lang]); //2015.05.16.
                }
                return currentlanguage; //2015.05.16.
            }
            set
            {
                Storage.Settings[SettingType.Lang] = value.ToString();
                currentlanguage = value; //2015.05.16.
                value.LoadAllStrings(); //2015.05.21.
            }
        }
        public static string Translate(StringID id, Control defaultevent = null) //Csak akkor kell az event, ha látszódik az adott ablak, amikor átállítódik - Tehát csak MainForm és ChatForm
        { //2014.08.19.
            if (CurrentLanguage == null)
            { //2015.05.22.
                if (Language.Languages.Any(entry => entry.LanguageKey == "en"))
                {
                    MessageBox.Show("Error: The specified language (" + Storage.Settings[SettingType.Lang] + ") is not found.\nThe program will use english that you can change later.");
                    CurrentLanguage = Languages.FirstOrDefault(entry => entry.LanguageKey == "en"); //Single-->FirstOrDefault: 2015.06.06.
                }
                else
                {
                    throw new FileNotFoundException("This is just so it will use English when downloading languages...");
                }
            }
            string strid = id.ToString().ToLower();
            if (CurrentLanguage.Strings.ContainsKey(strid))
            {
                if (defaultevent != null) //2014.12.22.
                    ReloadEvent += delegate { defaultevent.Text = CurrentLanguage.Strings[strid]; }; //2014.12.22.
                return CurrentLanguage.Strings[strid];
            }
            else
            {
                MessageBox.Show("Translation string not found: " + strid + "\nIn file: " + CurrentLanguage.FileName); //CurrentLanguage.FileName: 2015.05.22.
                return "Str not found (" + strid + ")"; //id: 2015.04.03.
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
        #region StringID
        public enum StringID
        { //2015.04.05.
            About,
            About_Version,
            About_Programmer,
            About_SpecialThanks,
            About_SpecThanks1,
            About_SpecThanks2,
            About_SpecThanks3,
            About_SpecThanks4,
            AddContact,
            AddContact_NameEmail,
            AddContact_Search,
            UserName,
            Chat_Title,
            Sendbtn_Send,
            Chat_NoWindow,
            Networking_Alone,
            Login,
            Password,
            Registration,
            ForgotPassword,
            Login_Desc1,
            Login_Desc2,
            Button_Cancel,
            ConnectError,
            Reg_Code,
            Reg_EmptyField,
            Reg_CodeErr,
            Reg_NameErr,
            Reg_NameLen,
            Reg_PassLen,
            Reg_Email,
            Reg_Success,
            Menu_Contacts_MakeCategory,
            Menu_Contacts_EditCategory,
            Menu_Contacts_RemoveCategory,
            Menu_Operations,
            Sendmsg,
            Menu_Operations_SendOther,
            Menu_Operations_SendMail,
            Menu_File_SendFile,
            Menu_Operations_CallContact,
            Menu_Operations_VideoCall,
            Menu_Operations_ShowOnlineFiles,
            Menu_Operations_PlayGame,
            Menu_Operations_AskForHelp,
            Menu_Tools,
            Menu_Tools_AlwaysOnTop,
            Menu_Tools_ChangeImage,
            Menu_Tools_ChangeBackground,
            Menu_Tools_VoiceVideoSettings,
            Menu_Tools_Settings,
            Menu_Help,
            Menu_Help_Contents,
            Menu_Help_Status,
            Menu_Help_PrivacyPolicy,
            Menu_Help_TermsOfUse,
            Menu_Help_Report,
            Menu_Help_ImproveProgram,
            Menu_Help_About,
            SearchBar,
            IconMenu_Show,
            Menu_File_Logout,
            Menu_File_Exit,
            BeforeLogin_CheckForUpdates,
            OutOfDate,
            OutOfDate_Caption,
            Updater,
            Error,
            BeforeLogin_LoginForm,
            Close,
            Contact_SendEmail,
            Contact_Info,
            Contact_Block,
            Contact_Remove,
            Contact_EditName,
            Contact_EventNotifications,
            Contact_OpenChatLog,
            Stats_MainServer,
            Stats_NoNetwork,
            Stats_Retrying,
            Stats_Connected,
            Stats_Servers,
            Stats_OnlineServers,
            BeforeLogin_TranslateMainF,
            Menu_File,
            Menu_File_LoginNewUser,
            Menu_File_Status,
            Menu_File_Status_Online,
            Menu_File_Status_Busy,
            Menu_File_Status_Away,
            Menu_File_Status_Hidden,
            Menu_File_OpenReceivedFiles,
            Menu_File_OpenRecentmsgs,
            Menu_File_Close,
            Menu_Contacts,
            Menu_Contacts_Add,
            Menu_Contacts_Edit,
            Menu_Contacts_Remove,
            Menu_Contacts_Invite,
            Menu_Contacts_MakeGroup,
            Error_No_Network,
            UserID,
            Settings,
            Settings_Personal,
            Settings_Layout,
            Name,
            Message,
            Language,
            Settings_ChatWindow,
            Settings_ChatWindowTabs,
            Offline,
            Contact_CopyEmail,
            Login_BadNamePass,
            Said,
            CurrentLang,
            ScriptNotFound,
            ScriptError,
            ScriptUnloadRequired,
            Scripter,
            Scripter_New,
            Scripter_Open,
            Scripter_Save,
            Scripter_Exit,
            Error_Network_NoInternet,
            ReceivedFiles,
            Settings_Packs,
            Settings_Network,
            Error_Unknown1,
            Error_Unknown2,
            Error_ServerError,
            FatalError,
            RestartButton,
            SelectImage,
            Modify,
            Emoticons,
            Add,
            Remove,
            Text,
            AreYouSure,
            Handwriting,
            FileReceived
        } //TODO: Egyszerűsíteni az azonosítókat, nagyjából jelentsék azt, amit, és semmi több (pl. ne Menu_File_...)
        #endregion

        private string FileName;
        public bool LoadFromPack(string filename) //2015.05.16.
        {
            FileName = filename; //2015.05.21.
            LanguageKey = Path.GetFileNameWithoutExtension(filename);
            if (this != CurrentLanguage) //<-- Ellenőrzés: 2015.05.21.
            {
                foreach (string line in File.ReadLines(filename))
                {
                    string[] strs = line.Split('=');
                    if (strs.Length != 2)
                        continue;
                    if (strs[0] == "currentlang")
                    {
                        Strings.Add(strs[0], strs[1]);
                        break;
                    }
                }
            }
            Languages.Add(this);

            if (this == CurrentLanguage)
            {
                LoadAllStrings(); //2015.05.21.
            }
            return true; //2015.05.24.
        }

        private void LoadAllStrings() //Külön metódus: 2015.05.21.
        {
            string filename = FileName;
            string[] stringids = Enum.GetNames(typeof(StringID)).Select(entry => entry.ToLower()).ToArray();
            bool[] stringidsused = new bool[stringids.Length];
            string[] lines = File.ReadAllLines(filename);
            var split = lines.Select(l => l.Split('=')); //2015.06.06.
            List<String> duplicates = split.GroupBy(x => x[0]) //2015.06.06.
                             .Where(g => g.Count() > 1) //2015.06.06.
                             .Select(g => g.Key) //2015.06.06.
                             .ToList(); //2015.06.06.
            if (duplicates.Count > 0)
            { //2015.06.06.
                MessageBox.Show("Duplicate found (in a different line) in \"" + filename + "\":\n" + duplicates.Aggregate((entry1, entry2) => entry1 += ", " + entry2));
                return;
            }
            var dict = split.ToDictionary(a => a[0], a => a[1]);
            var finaldict = new Dictionary<string, string>();
            foreach (var item in dict)
            {
                var spl = item.Key.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                duplicates = spl.GroupBy(x => x) //2015.06.06.
                                 .Where(g => g.Count() > 1) //2015.06.06.
                                 .Select(g => g.Key) //2015.06.06.
                                 .ToList(); //2015.06.06.
                if (duplicates.Count > 0)
                { //2015.06.06.
                    MessageBox.Show("Duplicate found (in the same line) in \"" + filename + "\":\n" + duplicates.Aggregate((entry1, entry2) => entry1 += ", " + entry2));
                    return;
                }
                foreach (var key in spl)
                {
                    finaldict.Add(key, item.Value); //Hozzáadja az összes felsorolt keyt külön, ugyanazzal az értékkel
                    int pos = Array.IndexOf<string>(stringids, key);
                    if (pos == -1)
                    {
                        MessageBox.Show("Warning: The translation ID \"" + key + "\" in " + filename + " is not in use. Please remove it or correct the ID.");
                    }
                    else
                        stringidsused[pos] = true;
                }
            }
            for (int i = 0; i < stringids.Length; i++)
            {
                if (!stringidsused[i])
                {
                    MessageBox.Show("Warning: The translation for ID \"" + stringids[i] + "\" in " + filename + " is missing. Please add it to the file.");
                }
            }
            this.LanguageKey = Path.GetFileNameWithoutExtension(filename);
            this.Strings = finaldict;
        }

        public void UnloadFromPack() //2015.05.16.
        {

        }

        public bool Equals(Language other)
        { //2015.05.16.
            return this.LanguageKey == other.LanguageKey;
        }
    }
}
