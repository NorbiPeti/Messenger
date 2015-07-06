using System;
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

        /*[Obsolete("Networking.PacketFormat.FromBytes()")]
        public static void ParsePacket(byte[] bytes, out byte response, out UpdateType updatetype, out int keyversion, out int port, out int userid, out byte[] data)
        { //2014.09.15.
            response = bytes[0];
            updatetype = (UpdateType)bytes[1];
            int pos = 2;
            keyversion = BitConverter.ToInt32(bytes, pos);
            pos += sizeof(int);
            port = BitConverter.ToInt32(bytes, pos); //2014.12.19.
            pos += sizeof(int);
            var encryptedBytes = new byte[bytes.Length - pos];
            Array.Copy(bytes, pos, encryptedBytes, 0, encryptedBytes.Length);
            if (CurrentUser.Keys[CurrentUser.KeyIndex] != null)
                bytes = Storage.Decrypt(encryptedBytes, true, CurrentUser.Keys[CurrentUser.KeyIndex]);
            else
                bytes = Storage.Decrypt(encryptedBytes, true, "ihavenokeys");
            userid = BitConverter.ToInt32(bytes, 0); //2014.12.18.
            data = new byte[bytes.Length - 4];
            Array.Copy(bytes, 4, data, 0, data.Length);
        }*/

        //public static void ParseUpdateInfo(byte[][] bytes)
        public static void ParseUpdateInfo(IEnumerable<string[]> strings)
        {
            if (strings == null)
                return;
            //for (int i = 0; i < packets.Length; i++)
            //foreach(PacketFormat packet in packets)
            foreach (string[] strs in strings)
            {
                //byte[] data = ParsePacket(bytes[i]).Data;
                //string[] strs = ((PDListUpdate)packet.EData).Strings;
                //string[] strs = Encoding.Unicode.GetString(data).Split(new string[] { "\n", "\n\r" }, StringSplitOptions.RemoveEmptyEntries); //2014.09.19.
                string str = "";
                for (int j = 0; j < strs.Length; j++)
                {
                    /*if (strs[j][0] == 2)
                        strs[j] = strs[j].Remove(0, 1);
                    while (strs[j].Contains('\0'))
                        strs[j] = strs[j].Remove(strs[j].IndexOf('\0'), 1);*/
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
                    //Debug.WriteLine("Updated " + strs[j]);
                    //user.Update(); //2014.12.31.
                }
                Storage.Parse(str);
                foreach (UserInfo user in UserInfo.KnownUsers)
                    user.Update(); //2015.05.10.
            }
        }

        /*[Obsolete("Networking.PacketFormat.FromBytes()")]
        public static PacketParts ParsePacket(byte[] bytes)
        { //2014.09.15.
            var ret = new PacketParts();
            ret.Response = (bytes[0] == 0x01) ? true : false;
            ret.UpdateType = (UpdateType)bytes[1];
            int pos = 2; //2014.12.19.
            ret.KeyVersion = BitConverter.ToInt32(bytes, pos);
            pos += sizeof(int);
            ret.Port = BitConverter.ToInt32(bytes, pos); //2014.12.19.
            pos += sizeof(int);
            byte[] encryptedBytes = new byte[bytes.Length - pos]; //2014.12.22. - A hátralévő rész titkosított
            Array.Copy(bytes, pos, encryptedBytes, 0, encryptedBytes.Length); //2014.12.22.
            if (CurrentUser.Keys[CurrentUser.KeyIndex] != null)
                bytes = Storage.Decrypt(encryptedBytes, true, CurrentUser.Keys[CurrentUser.KeyIndex]);
            else
                bytes = Storage.Decrypt(encryptedBytes, true, "ihavenokeys");
            pos = 0; //2014.12.22. - Új tömb lett a visszafejtés után
            ret.UserID = BitConverter.ToInt32(bytes, pos);
            pos += sizeof(int);
            ret.Data = new byte[bytes.Length - pos];
            Array.Copy(bytes, pos, ret.Data, 0, ret.Data.Length);
            return ret;
        }

        [Obsolete("Networking.PacketFormat.ToBytes()")]
        public static byte[] CreatePacket(bool response, byte updatetype, byte[] data)
        { //2014.09.15.
            List<byte> senddata = new List<byte>();
            senddata.Add((response) ? (byte)0x01 : (byte)0x00); //0x00: Kérelem/Adatküldés, 0x01: Válasz
            senddata.Add(updatetype);
            senddata.AddRange(BitConverter.GetBytes(CurrentUser.KeyIndex));
            senddata.AddRange(BitConverter.GetBytes(Int32.Parse(Storage.Settings["port"]))); //2014.12.19.
            List<byte> sendd = new List<byte>();
            sendd.AddRange(BitConverter.GetBytes(CurrentUser.UserID));
            sendd.AddRange(data);
            if (CurrentUser.Keys[CurrentUser.KeyIndex] != null)
                senddata.AddRange(Storage.Encrypt(sendd.ToArray(), CurrentUser.Keys[CurrentUser.KeyIndex]));
            else
                senddata.AddRange(Storage.Encrypt(sendd.ToArray(), "ihavenokeys"));
            return senddata.ToArray();
        }*/
    }
}
