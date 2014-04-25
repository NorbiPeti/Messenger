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
        public UserInfo()
        {
            ChatWindow = null;
        }
    }
}
