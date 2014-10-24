using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    static class Networking
    {
        public enum UpdateType : byte
        {
            ListUpdate = 0x01, //0x01: OK
            UpdateMessages, //0x01: OK
            GetImage, //(int)0: Hiba, egyébként a kép hossza, majd a kép
            LoginUser, //0x00: Hiba - Egyébként minden fontos információ, amiről "lemaradt"
            LogoutUser, //0x01: OK
            SetKey, //0x01: OK
            KeepAlive,
        };
        public static UdpClient SenderConnection = new UdpClient(); //2014.09.04. - Ezt ne társítsa egy porthoz, hogy működjön az udp hole punching
        public static UdpClient ReceiverConnection = new UdpClient(); //2014.09.04. - Társítsa egy porthoz
        public static byte[] DataBuffer;
        public static IPEndPoint RemoteEP;
        public static bool WaitingOnResponse = false; //2014.08.16.
        public static byte WaitingOnPacket = 0x00; //2014.08.16. - 0x00: Nincs
        public static byte[][] SendUpdate(UpdateType ut, byte[] data, bool response)
        {
            if (UserInfo.IPs.Count == 0)
                return null;
            byte[] senddata = CreatePacket(response, (byte)ut, data);
            if (UserInfo.BanTime < Environment.TickCount - 1000 * 60 * 1) //2014.08.30. - 2014.10.09. - 10 percről 1-re csökkentve
                UserInfo.BannedIPs = new List<IPEndPoint>(); //2014.08.30.
            if (!response)
            { //2014.08.30. - Azelőtt állítsa be, hogy elküldené a lekéréseket, hogy biztosan reagáljon a válaszra
                WaitingOnResponse = true; //2014.08.16.
                WaitingOnPacket = (byte)ut; //2014.08.16.
            }
            foreach (var item in UserInfo.IPs)
            { //Elküldi az összes ismert címre
                try
                {
                    if (!UserInfo.BannedIPs.Contains(item))
                        //SenderConnection.Send(Storage.Encrypt(senddata.ToArray(), "sendupdatestringencrypted"), senddata.Count, item);
                        SenderConnection.Send(senddata, senddata.Length, item);
                }
                catch (ObjectDisposedException)
                {
                    return null;
                }
            }

            if (!response)
            {
                int lasttick = Environment.TickCount;
                List<byte[]> Ret = new List<byte[]>();
                List<IPEndPoint> ResponsedIPs = new List<IPEndPoint>();
                int count = 0;
                while (Environment.TickCount - 1000 * 10 < lasttick && ResponsedIPs.Count < UserInfo.IPs.Count - UserInfo.BannedIPs.Count) //2014.09.09. - 2014.10.09. - 60 mp --> 10 mp
                { //2014.08.19. - Ret.Count == tmp.Length
                    if (MainForm.LThread != null) //2014.09.06.
                    {
                        while (DataBuffer == null && Environment.TickCount - 1000 * count * 2 < lasttick) ; //Várakozik, amíg a másik thread át nem adja a választ - 2014.10.09. - 10 mp --> 2 mp

                        foreach (var item in UserInfo.IPs.Except(UserInfo.BannedIPs).Except(ResponsedIPs)) //2014.09.22.
                        { //Elküldi az összes ismert címre
                            try
                            {
                                SenderConnection.Send(senddata, senddata.Length, item);
                            }
                            catch (ObjectDisposedException)
                            {
                                return null;
                            }
                        }
                        count++;
                    }
                    if (DataBuffer == null) //2014.08.30. - Az idő telt le
                    {
                        UserInfo.BannedIPs = UserInfo.IPs.Except(ResponsedIPs).ToList(); //2014.08.30. - Ideiglenesen kitilt minden IP-t, ahonnan nem érkezett válasz
                        UserInfo.BanTime = Environment.TickCount;
                        break;
                    }
                    if (UserInfo.IPs.Contains(RemoteEP) && !UserInfo.BannedIPs.Contains(RemoteEP))
                    {
                        ResponsedIPs.Add(RemoteEP);
                        var pparts = ParsePacket(DataBuffer);
                        if (pparts.KeyVersion != CurrentUser.KeyIndex && pparts.UpdateType != UpdateType.SetKey)
                        {
                            DataBuffer = null; //2014.09.22. - Mindig adja meg a lehetőséget, hogy újra beállítsa
                            continue;
                        }

                        int i;
                        for (i = 0; i < Ret.Count; i++)
                        {
                            if (DataBuffer.SequenceEqual(Ret[i]))
                                break;
                        }
                        if (i == Ret.Count)
                            Ret.Add(DataBuffer);
                    } //(2014.08.17. -->) Várja meg, amíg az összes online(!) ismerőse válaszol - Vagy letelik az egy perc
                    DataBuffer = null; //2014.09.22. - Mindig adja meg a lehetőséget, hogy újra beállítsa
                }
                DataBuffer = null;
                RemoteEP = null;
                WaitingOnResponse = false;
                WaitingOnPacket = 0x00;
                return Ret.ToArray();
            }
            return null;
        }
        public static bool SendChatMessage(ChatForm chat, string message)
        { //2014.09.22.
            List<byte> bytes = new List<byte>();
            string sendstr = "";
            foreach (var pID in chat.ChatPartners)
            {
                sendstr += pID + ",";
            }
            bytes.AddRange(BitConverter.GetBytes(Encoding.Unicode.GetByteCount(sendstr)));
            bytes.AddRange(Encoding.Unicode.GetBytes(sendstr));
            sendstr = message; //Átállítja a sendstr-t az üzenetre, majd újra belerakja
            bytes.AddRange(BitConverter.GetBytes(Encoding.Unicode.GetByteCount(sendstr)));
            bytes.AddRange(Encoding.Unicode.GetBytes(sendstr));
            sendstr = Program.DateTimeToUnixTime(DateTime.Now);
            bytes.AddRange(BitConverter.GetBytes(Encoding.Unicode.GetByteCount(sendstr)));
            bytes.AddRange(Encoding.Unicode.GetBytes(sendstr));
            byte[][] resp = SendUpdate(UpdateType.UpdateMessages, Encoding.Unicode.GetBytes(sendstr), false);
            if (resp == null || resp.Length == 0 || resp.All(bytesb => bytesb[0] != 0x01))
                return false;
            else //Ha válaszoltak, és senki sem válaszolt nem oké jelzéssel, akkor rendben van
                return true;
        }
        public static object[] ReceiveUpdates() //Thread function
        {
            IPEndPoint remoteEP;
            remoteEP = new IPEndPoint(IPAddress.Any, Int32.Parse(Storage.Settings["port"])); //2014.09.04. - A port beállítása már megtörtént
            byte[] buf;
            try
            {
                buf = ReceiverConnection.Receive(ref remoteEP);
            }
            catch
            {
                return null;
            }
            if (buf[0] == 0x01) //0x01: Válasz egy kérelemre
            {
                if (WaitingOnResponse == false || WaitingOnPacket != buf[1])
                    return null;
                while (DataBuffer != null) ; //Várja meg, amíg feldolgozza a legutóbbi adatot
                DataBuffer = buf; //2014.09.19. - Küldön el mindenhova mindent, és egységesen egy funkcióval dolgozza fel
                RemoteEP = remoteEP;
                return null;
            }
            else
                return new object[] { buf, remoteEP };
        }
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
                    else
                        str += "userinfo_" + strs[j];
                    if (j + 1 != strs.Length)
                        str += "\n";
                }
                Storage.Parse(str);
            }
        }

        public static void ParsePacket(byte[] bytes, out byte response, out UpdateType updatetype, out int keyversion, out int userid, out byte[] data)
        { //2014.09.15.
            if (CurrentUser.Keys[CurrentUser.KeyIndex] != null)
                bytes = Storage.Encrypt(bytes, CurrentUser.Keys[CurrentUser.KeyIndex]);
            else
                bytes = Storage.Encrypt(bytes, "ihavenokeys");
            response = bytes[0];
            updatetype = (UpdateType)bytes[1];
            keyversion = BitConverter.ToInt32(bytes, 1 + 1);
            userid = BitConverter.ToInt32(bytes, 1 + 1 + 4);
            data = new byte[bytes.Length - 1 - 1 - 4 - 4];
            Array.Copy(bytes, 2 + 4 + 4, data, 0, data.Length);
        }

        public struct PacketParts
        {
            public bool Response;
            public UpdateType UpdateType;
            public int KeyVersion;
            public int UserID;
            public byte[] Data;
        };

        public static PacketParts ParsePacket(byte[] bytes)
        { //2014.09.15.
            if (CurrentUser.Keys[CurrentUser.KeyIndex] != null)
                bytes = Storage.Encrypt(bytes, CurrentUser.Keys[CurrentUser.KeyIndex]);
            else
                bytes = Storage.Encrypt(bytes, "ihavenokeys");
            var ret = new PacketParts();
            ret.Response = (bytes[0] == 0x01) ? true : false;
            ret.UpdateType = (UpdateType)bytes[1];
            ret.KeyVersion = BitConverter.ToInt32(bytes, 1 + 1);
            ret.UserID = BitConverter.ToInt32(bytes, 1 + 1 + 4);
            ret.Data = new byte[bytes.Length - 1 - 1 - 4 - 4];
            Array.Copy(bytes, 2 + 4 + 4, ret.Data, 0, ret.Data.Length);
            return ret;
        }

        public static byte[] CreatePacket(bool response, byte updatetype, byte[] data)
        { //2014.09.15.
            List<byte> senddata = new List<byte>();
            senddata.Add((response) ? (byte)0x01 : (byte)0x00); //0x00: Kérelem/Adatküldés, 0x01: Válasz
            senddata.Add(updatetype);
            senddata.AddRange(BitConverter.GetBytes(CurrentUser.KeyIndex));
            List<byte> sendd = new List<byte>();
            sendd.AddRange(BitConverter.GetBytes(CurrentUser.UserID));
            sendd.AddRange(data);
            if (CurrentUser.Keys[CurrentUser.KeyIndex] != null)
                senddata.AddRange(Storage.Encrypt(sendd.ToArray(), CurrentUser.Keys[CurrentUser.KeyIndex]));
            else
                senddata.AddRange(Storage.Encrypt(sendd.ToArray(), "ihavenokeys"));
            return senddata.ToArray();
        }

        private static int Tries = 0;
        public static string SendRequest(string action, string data, int remnum, bool loggedin) //2014.02.18.
        {
#if LOCAL_SERVER //2014.07.07. 22:00
            HttpWebRequest httpWReq =
                (HttpWebRequest)WebRequest.Create("http://localhost/ChatWithWords/client.php");
#else
            HttpWebRequest httpWReq =
                (HttpWebRequest)WebRequest.Create("http://msger.url.ph/client.php");
#endif

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "action=" + action;
            if (loggedin) //2014.03.14.
                postData += "&uid=" + CurrentUser.UserID;
            postData += "&key=cas1fe4a6feFEFEFE1616CE8099VFE1444cdasf48c1ase5dg";
            if (loggedin) //2014.03.14.
                postData += "&code=" + LoginForm.UserCode; //2014.02.13.
            postData += "&data=" + Uri.EscapeUriString(data); //2014.02.13.
            byte[] datax = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = datax.Length;

            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(datax, 0, datax.Length);
            }

            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse)httpWReq.GetResponse();
            }
            catch (Exception e)
            {
                if (Tries > 10)
                {
                    MessageBox.Show(Language.Translate("error_network") + ":\n" + e.Message + "\n\n" + Language.Translate("error_network2"), Language.Translate("error")); //2014.04.25.
                    Tries = 0;
                }
                Tries++;
                return SendRequest(action, data, remnum, loggedin); //Újrapróbálkozik
            }
            string responseString;
            responseString = Uri.UnescapeDataString(new StreamReader(response.GetResponseStream()).ReadToEnd());
            return responseString;
        }
        public static void KeepUpThread()
        { //2014.08.28.
            Thread.Sleep(59 * 60 * 1000); //59 percenként frissíti a jelenlétét, így biztosan nem jelenti offline-nak a PHP (elvileg)
            Console.WriteLine("KeepUpThread: " + Networking.SendRequest("keepactive", "", 0, true));
        }
        public static void KeepUpUsersThread() //2014.09.26. - Nehogy bezáruljon a kapcsolat
        {
            Thread.Sleep(1000);
            Networking.SendUpdate(UpdateType.KeepAlive, new byte[] { 0x01 }, false);
        }
    }
}
