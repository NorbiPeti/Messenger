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
    }
}
