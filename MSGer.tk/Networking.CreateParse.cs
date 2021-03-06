﻿using System;
using System.Collections.Generic;
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

        public static void ParsePacket(byte[] bytes, out byte response, out UpdateType updatetype, out int keyversion, out int port, out int userid, out byte[] data)
        { //2014.09.15.
            /*if (CurrentUser.Keys[CurrentUser.KeyIndex] != null)
                bytes = Storage.Decrypt(bytes, true, CurrentUser.Keys[CurrentUser.KeyIndex]);
            else
                bytes = Storage.Decrypt(bytes, true, "ihavenokeys");*/
            response = bytes[0];
            updatetype = (UpdateType)bytes[1];
            int pos = 2;
            //keyversion = BitConverter.ToInt32(bytes, 1 + 1);
            keyversion = BitConverter.ToInt32(bytes, pos);
            pos += sizeof(int);
            port = BitConverter.ToInt32(bytes, pos); //2014.12.19.
            pos += sizeof(int);
            var encryptedBytes = new byte[bytes.Length - pos];
            //Array.Copy(bytes, 6, encryptedBytes, 0, encryptedBytes.Length);
            Array.Copy(bytes, pos, encryptedBytes, 0, encryptedBytes.Length);
            if (CurrentUser.Keys[CurrentUser.KeyIndex] != null)
                bytes = Storage.Decrypt(encryptedBytes, true, CurrentUser.Keys[CurrentUser.KeyIndex]);
            else
                bytes = Storage.Decrypt(encryptedBytes, true, "ihavenokeys");
            /*userid = BitConverter.ToInt32(bytes, 1 + 1 + 4);
            data = new byte[bytes.Length - 1 - 1 - 4 - 4];
            Array.Copy(bytes, 2 + 4 + 4, data, 0, data.Length);*/
            userid = BitConverter.ToInt32(bytes, 0); //2014.12.18.
            data = new byte[bytes.Length - 4];
            Array.Copy(bytes, 4, data, 0, data.Length);
        }

        public static void ParseUpdateInfo(byte[][] bytes)
        {
            if (bytes == null)
                return;
            for (int i = 0; i < bytes.Length; i++)
            {
                byte[] data = ParsePacket(bytes[i]).Data;
                string[] strs = Encoding.Unicode.GetString(data).Split(new string[] { "\n", "\n\r" }, StringSplitOptions.RemoveEmptyEntries); //2014.09.19.
                string str = "";
                for (int j = 0; j < strs.Length; j++)
                {
                    string[] spl = strs[j].Split('_'); //2014.08.30.
                    int uid = Int32.Parse(spl[0]); //2014.08.30.
                    string[] keyvalue = spl[1].Split('='); //2014.08.30.
                    UserInfo user = UserInfo.Select(uid); //2014.12.31.
                    if (keyvalue[0] == "ispartner")
                    { //2014.08.30.
                        string resp = Networking.SendRequest("ispartner", uid + "", 0, true);
                        if (resp == "yes")
                            str += "userinfo_" + uid + "_ispartner=True";
                        else if (resp == "no")
                            str += "userinfo_" + uid + "_ispartner=False";
                        else
                            MessageBox.Show("ispartner:\n" + resp);
                    }
                    else if (keyvalue[0] == "picupdatetime")
                    { //2014.12.31.
                        user.GetImage(Int32.Parse(keyvalue[1])); //Megvizsgálja, hogy kell-e frissítés és ha kell, letölti
                    }
                    else
                        str += "userinfo_" + strs[j];
                    if (j + 1 != strs.Length)
                        str += "\n";
                    user.Update(); //2014.12.31.
                }
                Storage.Parse(str);
            }
        }

        public static PacketParts ParsePacket(byte[] bytes)
        { //2014.09.15.
            /*if (CurrentUser.Keys[CurrentUser.KeyIndex] != null)
                bytes = Storage.Encrypt(bytes, CurrentUser.Keys[CurrentUser.KeyIndex]);
            else
                bytes = Storage.Encrypt(bytes, "ihavenokeys");*/
            var ret = new PacketParts();
            ret.Response = (bytes[0] == 0x01) ? true : false;
            ret.UpdateType = (UpdateType)bytes[1];
            int pos = 2; //2014.12.19.
            //ret.KeyVersion = BitConverter.ToInt32(bytes, 1 + 1);
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
            //ret.UserID = BitConverter.ToInt32(bytes, 1 + 1 + 4);
            pos = 0; //2014.12.22. - Új tömb lett a visszafejtés után
            ret.UserID = BitConverter.ToInt32(bytes, pos);
            pos += sizeof(int);
            //ret.Data = new byte[bytes.Length - 1 - 1 - 4 - 4];
            ret.Data = new byte[bytes.Length - pos];
            //Array.Copy(bytes, 2 + 4 + 4, ret.Data, 0, ret.Data.Length);
            Array.Copy(bytes, pos, ret.Data, 0, ret.Data.Length);
            return ret;
        }

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
        }
    }
}
