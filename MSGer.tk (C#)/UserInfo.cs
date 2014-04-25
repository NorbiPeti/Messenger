using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MSGer.tk
{
    public partial class UserInfo
    {
        /*
         * 2014.03.07.
         * Az összes szükséges felhasználó szükséges adatai
         */
        //public int ChatID { get; set; } //A chatablakának azonosítója
        public int UserID { get; set; } //Az egész rendszerben egyedi azonosítója
        public int ListID { get; set; } //A listabeli azonosítója
        public static List<UserInfo> Partners = new List<UserInfo>();
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                List<int> list=GetChatWindows();
                for (int i = 0; i < list.Count; i++)
                {
                    //if (ChatForm.ChatWindows != null && ChatID < ChatForm.ChatWindows.Count && ChatForm.ChatWindows[ChatID] != null && !ChatForm.ChatWindows[ChatID].IsDisposed)
                    if (ChatForm.ChatWindows != null && ChatForm.ChatWindows[list[i]] != null && !ChatForm.ChatWindows[list[i]].IsDisposed)
                    { //ChatForm
                        new ThreadSetVar(ChatForm.ChatWindows[list[i]].partnerName, value, Program.MainF);
                    }
                }
            }
        }
        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                List<int> list = GetChatWindows();
                for (int i = 0; i < list.Count; i++)
                {
                    //if (ChatForm.ChatWindows != null && ChatID < ChatForm.ChatWindows.Count && ChatForm.ChatWindows[ChatID] != null && !ChatForm.ChatWindows[ChatID].IsDisposed)
                    if (ChatForm.ChatWindows != null && ChatForm.ChatWindows[list[i]] != null && !ChatForm.ChatWindows[list[i]].IsDisposed)
                    { //ChatForm
                        new ThreadSetVar(ChatForm.ChatWindows[list[i]].partnerMsg, value, Program.MainF);
                    }
                }
            }
        }
        private string state;
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                List<int> list = GetChatWindows();
                for (int i = 0; i < list.Count; i++)
                {
                    //if (ChatForm.ChatWindows != null && ChatID < ChatForm.ChatWindows.Count && ChatForm.ChatWindows[ChatID] != null && !ChatForm.ChatWindows[ChatID].IsDisposed)
                    if (ChatForm.ChatWindows != null && ChatForm.ChatWindows[list[i]] != null && !ChatForm.ChatWindows[list[i]].IsDisposed)
                    { //ChatForm
                        string tmp = "Hiba";
                        switch (value)
                        {
                            case "0":
                                tmp = Language.GetCuurentLanguage().Strings["offline"];
                                break;
                            case "1":
                                tmp = Language.GetCuurentLanguage().Strings["menu_file_status_online"];
                                break;
                            case "2":
                                tmp = Language.GetCuurentLanguage().Strings["menu_file_status_busy"];
                                break;
                            case "3":
                                tmp = Language.GetCuurentLanguage().Strings["menu_file_status_away"];
                                break;
                        }
                        new ThreadSetVar(ChatForm.ChatWindows[list[i]].statusLabel, tmp, Program.MainF);
                    }
                }
            }
        }
        public string UserName { get; set; }
        public string Email { get; set; }

        public UserInfo()
        {

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
            string str = Networking.SendRequest("getimage", UserID + "ͦ" + PicUpdateTime, 2, true); //SetVars
            /*
             * Ez a funkció automatikusan elküldi a bejelentkezett felhasználó azonositóját,
             * a PHP szkript pedig leellenőrzi, hogy egymásnak partnerei-e, ezáltal nem nézheti meg akárki akárkinek a profilképét
             * (pedig a legtöbb helyen igy van, de szerintem jobb igy; lehet, hogy beállithatóvá teszem)
             */
            if (str == "Fail")
            {
                return "noimage.png";
            }
            else
            { //Mentse el a képet
                string tmp = Path.GetTempPath();
                if (!Directory.Exists(tmp + "\\MSGer.tk\\pictures"))
                    Directory.CreateDirectory(tmp + "\\MSGer.tk\\pictures");
                File.WriteAllText(tmp + "\\MSGer.tk\\pictures\\" + ListID + ".png", str);
            }
            return "noimage.png";
        }
        public List<int> GetChatWindows()
        {
            List<int> retlist = new List<int>();
            for(int x=0; x<ChatForm.ChatWindows.Count; x++)
            {
                if(ChatForm.ChatWindows[x].ChatPartners.Contains(ListID))
                {
                    retlist.Add(x);
                }
            }
            return retlist;
        }
    }
}
