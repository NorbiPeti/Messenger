using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    partial class Networking
    {
        public static string[] GetStrings(byte[] bytes, int startIndex)
        {
            List<string> strs = new List<string>();
            int pos = startIndex;
            while (pos < bytes.Length)
            {
                int len = BitConverter.ToInt32(bytes, pos);
                pos += 4;
                strs.Add(Encoding.Unicode.GetString(bytes, pos, len));
            }
            return strs.ToArray();
        }

        public static void ParseUpdateInfo(IEnumerable<string[]> strings)
        {
            if (strings == null)
                return;
            foreach (string[] strs in strings)
            {
                string str = "";
                for (int j = 0; j < strs.Length; j++)
                {
                    int index = strs[j].IndexOfAny("0123456789".ToCharArray());
                    if (index == -1)
                        continue;
                    strs[j] = strs[j].Remove(0, index);
                    if (strs[j].Length == 0)
                        continue;
                    if (!strs[j].Contains('_'))
                        continue;
                    string[] spl = strs[j].Split('_'); //2014.08.30.
                    int uid = Int32.Parse(spl[0]); //2014.08.30.
                    string[] keyvalue = spl[1].Split('='); //2014.08.30.
                    UserInfo user = UserInfo.Select(uid); //2014.12.31.
                    if (keyvalue[0] == "ispartner")
                    { //2014.08.30.
                        string resp = Networking.SendRequest(Networking.RequestType.IsPartner, uid + "", 0, true);
                        if (resp == "yes")
                            str += "userinfo_" + uid + "_ispartner=True";
                        else if (resp == "no")
                            str += "userinfo_" + uid + "_ispartner=False";
                        else
                            MessageBox.Show("ispartner:\n" + resp);
                    }
                    else if (keyvalue[0] == "picupdatetime")
                    { //2014.12.31.
                        user.GetImageFromNetwork(Int32.Parse(keyvalue[1])); //Megvizsgálja, hogy kell-e frissítés és ha kell, letölti
                    }
                    else
                        str += "userinfo_" + strs[j];
                    if (j + 1 != strs.Length)
                        str += "\n";
                }
                Storage.Parse(str);
                foreach (UserInfo user in UserInfo.KnownUsers)
                    user.Update(); //2015.05.10.
            }
        }
    }
}
