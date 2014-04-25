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
        public ChatForm ChatWindow { get; set; }
        public int UserID { get; set; }
        public int Number { get; set; } //Nem tudom egyszerűen meghatározni a tömbbeli helyét
        public static UserInfo[] Partners = new UserInfo[1024];
        public string Name { get; set; }
        public string Message { get; set; }
        public string State { get; set; }
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
            string str = Networking.SendRequest("getimage", Number + "ͦ" + PicUpdateTime, 2, true); //SetVars
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
                File.WriteAllText(tmp + "\\MSGer.tk\\pictures\\" + Number + ".png", str);
            }
            return "noimage.png";
        }
        /*public static void InitVars()                 //Az adott felhasználó profilozásakor adja át a helyileg tárolt információkat
        {
        }*/
        /*public static void SetVars()
        {
            for (int x = 0; x < PicUpdateTime.Length; x++)
            {
                if (Settings.Default.picupdatetime.Length == 0)
                    PicUpdateTime[x] = 0; //Ha nem nyúlunk hozzá, alapesetben régebbinek veszi a dátumot, ezáltal letölti
                else
                {
                    Settings.Default.picupdatetime = String.Join(",", PicUpdateTime.Select(i => i.ToString()).ToArray());
                }
            }
        }*/
    }
}
