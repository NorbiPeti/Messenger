using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSGer.tk
{
    class CurrentUser
    {
        /*
         * 2014.03.05.
         * Információátrendezés: Property-k használata; Minden felhasználóhoz egy-egy User class
         * Ez a class használható lenne az aktuális felhsaználó információinak tárolására
         */
        public static int UserID = 0;
        public static int[] PartnerIDs = new int[1024];
        public static int[] PicUpdateTime = new int[1024];
        public static string GetPartnerImage(int id)
        {
            /*
             * Szükséges információk az adatbázisban:
             * - Felhasználó képe a users táblában
             * - A legutóbbi képváltás dátuma
             * Ebből megállapitja a program, hogy le kell-e tölteni.
             * Eltárol helyileg is egy dátumot, és ha már frissitette egyszer a képet (újabb a helyi dátum, mint az adatbázisban),
             * akkor nem csinál semmit. Ha régebbi, akkor a partner azóta frissitette, tehát szükséges a letöltés.
             */
            string str = Networking.SendRequest("getimage", PartnerIDs[id] + "+" + PicUpdateTime[PartnerIDs[id]], 2); //SetVars
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
                File.WriteAllText(tmp + "\\MSGer.tk\\pictures\\" + PartnerIDs[id] + ".png", str);
            }
            return "noimage.png";
        }
        public static void InitVars()
        {
            if (Settings.Default.picupdatetime.Length == 0)
            {
                for (int x = 0; x < 1024; x++)
                    PicUpdateTime[x] = 0; //Ha nem nyúlunk hozzá, alapesetben régebbinek veszi a dátumot, ezáltal letölti
            }
            else
            {
                PicUpdateTime = Settings.Default.picupdatetime.Split(',').Select(s => Int32.Parse(s)).ToArray();
            }
        }
        public static void SetVars()
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
        }
    }
}
