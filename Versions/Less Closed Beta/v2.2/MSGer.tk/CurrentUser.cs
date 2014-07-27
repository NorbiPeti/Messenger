using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSGer.tk
{
    public static class CurrentUser
    {
        /*
         * 2014.03.05.
         * Információátrendezés: Property-k használata; Minden felhasználóhoz egy-egy User class
         * Ez a class használható lenne az aktuális felhsaználó információinak tárolására
         */
        public static int UserID = 0;
        //public static int[] PartnerIDs = new int[1024];
        public static string Name = "";
        //public static Language Language = Language.English; //2014.04.19.
        public static Language Language;
        public static string Message = "";
        public static string State = "";
        public static string UserName = "";
        public static string Email = "";
        /// <summary>
        /// Átmásolja-e a memóriába az egész fájlt a küldés előtt.
        /// Nagy méretű fájloknál nem ajánlott, különben igen a fájl esetleges elérhetetlensége miatt.
        /// 2014.06.15.
        /// </summary>
        public static bool CopyToMemoryOnFileSend = true;
    }
}
