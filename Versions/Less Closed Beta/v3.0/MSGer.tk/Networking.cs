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
            //CheckForUpdates = 0x01, //0x00: outofdate, 0x01: fine -- Ezt is a weboldalról ellenőrizze
            //SetState = 0x01, //0x01: Success
            //GetList - Változás esetén küldjék el, ne kelljen letölteni
            ListUpdate = 0x01, //0x01: OK
            UpdateMessages, //0x01: OK
            GetImage, //(int)0: Hiba, egyébként a kép hossza, majd a kép
            //FindPeople,
            //UpdateSettings, - Küldje el a ListUpdate-tel
            LoginUser, //0x00: Hiba - Egyébként minden fontos információ, amiről "lemaradt"
            LogoutUser, //0x01: OK
            SetKey, //0x01: OK
            KeepAlive,
        };
        //public static UdpClient Connection = new UdpClient();
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
            /*try - A Connection MINDIG be legyen állítva
            {
                if (Connection == null || Connection.Available >= 0) //Az utóbbi csak arra szolgál, hogy ObjectDisposedException történjen, ha kell
                {
                    while (true) //<-- 2014.08.30.
                    {
                        var remoteEP = new IPEndPoint(IPAddress.Any, Int32.Parse(Storage.Settings["port"]));
                        if (IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners().Select(entry => entry.Port).Contains(remoteEP.Port))
                            Storage.Settings["port"] = (Int32.Parse(Storage.Settings["port"]) + 1).ToString();
                        else
                            break;
                    }
                    Connection = new UdpClient(Int32.Parse(Storage.Settings["port"]));
                }
            }
            catch (ObjectDisposedException)
            {
                Connection = new UdpClient(Int32.Parse(Storage.Settings["port"]));
            }*/
            //List<byte> senddata = new List<byte>();
            //senddata.Add((response) ? (byte)0x01 : (byte)0x00); //0x00: Kérelem/Adatküldés, 0x01: Válasz
            //senddata.Add((byte)ut);
            //senddata.AddRange(BitConverter.GetBytes(CurrentUser.UserID)); //4 bájt; ellenőrizze le, hogy az IP is megegyezik-e, hacsak nem IP-változást jelez
            //senddata.AddRange(data);
            byte[] senddata = CreatePacket(response, (byte)ut, data);
            if (UserInfo.BanTime < Environment.TickCount - 1000 * 60 * 1) //2014.08.30. - 2014.10.09. - 10 percről 1-re csökkentve
                UserInfo.BannedIPs = new List<IPEndPoint>(); //2014.08.30.
            //string[] tmp = UserInfo.IPs.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries); //RemoveEmptyEntries: 2014.08.29. - UserInfo... szintén...
            //for (int i = 0; i < tmp.Length; i++)
            //for (int i = 0; i < UserInfo.IPs.Count; i++)
            if (!response)
            { //2014.08.30. - Azelőtt állítsa be, hogy elküldené a lekéréseket, hogy biztosan reagáljon a válaszra
                WaitingOnResponse = true; //2014.08.16.
                WaitingOnPacket = (byte)ut; //2014.08.16.
            }
            foreach (var item in UserInfo.IPs)
            { //Elküldi az összes ismert címre
                //int port=
                //Connection.Send(Storage.Encrypt(senddata.ToArray()), senddata.Count, new IPEndPoint(IPAddress.Parse(tmp2[0]), Int32.Parse(tmp2[1])));
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
            /*if (!loggedin)
                ReceiveUpdates();
            else
                while (Networking.DataBuffer == null) ;*/

            if (!response)
            {
                //WaitingOnResponse = true; //2014.08.16.
                //WaitingOnPacket = (byte)ut; //2014.08.16.
                int lasttick = Environment.TickCount;
                List<byte[]> Ret = new List<byte[]>();
                List<IPEndPoint> ResponsedIPs = new List<IPEndPoint>();
                //while (Environment.TickCount - 1000 * 60 > lasttick || Ret.Count == tmp.Length) //2014.08.16. - Visszatér, ha kapott választ egy ismert IP-ről, de ha máshonnan kap, újra várakozik, legfeljebb egy percig
                //while (Environment.TickCount - 1000 * 60 < lasttick && Ret.Count < UserInfo.IPs.Count - UserInfo.BannedIPs.Count) //2014.08.29.
                int count = 0;
                while (Environment.TickCount - 1000 * 10 < lasttick && ResponsedIPs.Count < UserInfo.IPs.Count - UserInfo.BannedIPs.Count) //2014.09.09. - 2014.10.09. - 60 mp --> 10 mp
                { //2014.08.19. - Ret.Count == tmp.Length
                    //if (MainForm.PartnerListThreadActive) //2014.09.06.
                    if (MainForm.LThread != null) //2014.09.06.
                    {
                        while (DataBuffer == null && Environment.TickCount - 1000 * count * 2 < lasttick) ; //Várakozik, amíg a másik thread át nem adja a választ - 2014.10.09. - 10 mp --> 2 mp

                        foreach (var item in UserInfo.IPs.Except(UserInfo.BannedIPs).Except(ResponsedIPs)) //2014.09.22.
                        { //Elküldi az összes ismert címre
                            try
                            {
                                //if (!UserInfo.BannedIPs.Contains(item))
                                SenderConnection.Send(senddata, senddata.Length, item);
                            }
                            catch (ObjectDisposedException)
                            {
                                return null;
                            }
                        }
                        count++;
                    }
                    //else //2014.09.06.
                    //ReceiveUpdates(); //2014.09.06.
                    if (DataBuffer == null) //2014.08.30. - Az idő telt le
                    {
                        //UserInfo.IPs.Select(entry => (ResponsedIPs.Contains(entry)) ? (UserInfo.IPs.Remove(entry)) : entry)
                        UserInfo.BannedIPs = UserInfo.IPs.Except(ResponsedIPs).ToList(); //2014.08.30. - Ideiglenesen kitilt minden IP-t, ahonnan nem érkezett válasz
                        UserInfo.BanTime = Environment.TickCount;
                        break;
                    }
                    //if (tmp.Contains(RemoteEP.ToString() + ":" + RemoteEP.Port))
                    if (UserInfo.IPs.Contains(RemoteEP) && !UserInfo.BannedIPs.Contains(RemoteEP))
                    {
                        ResponsedIPs.Add(RemoteEP);
                        //byte[] ret = Storage.Decrypt(DataBuffer, true, "sendupdatestringencrypted");
                        var pparts = ParsePacket(DataBuffer);
                        //int keyindex = BitConverter.ToInt32(DataBuffer, 0);
                        /*DataBuffer = null;
                        RemoteEP = null;
                        WaitingOnResponse = false;
                        WaitingOnPacket = 0x00;*/
                        if (pparts.KeyVersion != CurrentUser.KeyIndex && pparts.UpdateType != UpdateType.SetKey)
                        {
                            DataBuffer = null; //2014.09.22. - Mindig adja meg a lehetőséget, hogy újra beállítsa
                            continue;
                        }
                        //byte[] ret = new byte[DataBuffer.Length - 4];
                        //Array.Copy(DataBuffer, 4, ret, 0, ret.Length);

                        int i;
                        for (i = 0; i < Ret.Count; i++)
                        {
                            if (DataBuffer.SequenceEqual(Ret[i]))
                                break;
                        }
                        if (i == Ret.Count)
                            Ret.Add(DataBuffer);
                        //return ret;
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
            //foreach(var pID in chat.ChatPartners)
            //{
            //var pInfo = UserInfo.Select(pID);
            //TCP közvetlen kapcsolat nem feltétlenül lehetséges, mert a felhasználók router mögött lehetnek...
            //Ezért ugyanazt a rendszert kell használni itt is...
            //}
            List<byte> bytes = new List<byte>();
            string sendstr = "";
            foreach (var pID in chat.ChatPartners)
            {
                sendstr += pID + ",";
            }
            //sendstr += "ͦ";
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
            /*if (ReceiverConnection != null)
            {
                if (Connection.Client != null) //2014.09.01.
                {
                    Connection.Client.Close(); //2014.08.30.
                    //while (Connection.Client.IsBound) ; //2014.08.30.
                }
                if (Connection != null) //2014.09.01. - Még mindig nem null
                    Connection.Close();
            }*/
            IPEndPoint remoteEP;
            /*while (true) //<-- 2014.08.30. -- 2014.09.01. - Ne ellenőrizze le minden alkalommal, csak a program indulásakor
            {
                remoteEP = new IPEndPoint(IPAddress.Any, Int32.Parse(Storage.Settings["port"]));
                if (IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners().Select(entry => entry.Port).Contains(remoteEP.Port))
                    Storage.Settings["port"] = (Int32.Parse(Storage.Settings["port"]) + 1).ToString();
                else
                    break;
            }*/
            remoteEP = new IPEndPoint(IPAddress.Any, Int32.Parse(Storage.Settings["port"])); //2014.09.04. - A port beállítása már megtörtént
            //Connection = new UdpClient(Int32.Parse(Storage.Settings["port"]));
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
                //DataBuffer = buf;
                //Array.Copy(buf, 2, DataBuffer = new byte[buf.Length - 2], 0, buf.Length - 2); //2014.08.16. - Ne rakja bele a packet ID-t, és hogy válasz
                DataBuffer = buf; //2014.09.19. - Küldön el mindenhova mindent, és egységesen egy funkcióval dolgozza fel
                RemoteEP = remoteEP;
                return null;
            }
            else
                //return Storage.Decrypt(buf, true);
                return new object[] { buf, remoteEP };
        }
        /*private static void SendTo(byte[] data, IPEndPoint ip)
        {
            if (!HttpListener.IsSupported)
            {
                MessageBox.Show("Windows XP or newer required.");
                return;
            }
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://" + CurrentUser.IP.ToString() + ":" + Storage.Settings["port"] + "/");
            listener.Start();
            Console.WriteLine("Listening for incoming connections...");
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            request
        }*/
        public static string[] GetStrings(byte[] bytes, int startIndex)
        {
            List<string> strs = new List<string>();
            //byte[] tmparr;
            int pos = startIndex;
            while (pos < bytes.Length)
            {
                //tmparr = new byte[4];
                //pos = 2;
                //Array.Copy(bytes, pos, tmparr, 0, 4);
                //int len = BitConverter.ToInt32(tmparr, 0); //A hosszúság megállapítása
                int len = BitConverter.ToInt32(bytes, pos);
                pos += 4;
                //tmparr = new byte[len];
                //Array.Copy(bytes, pos, tmparr, 0, len);
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
                //byte[] tmp = new byte[bytes[i].Length - 4];
                //Array.Copy(bytes[i], 4, tmp, 0, tmp.Length); //Az első 4 bájt a UserID
                byte[] data = ParsePacket(bytes[i]).Data;
                //string[] strs = Storage.Decrypt(tmp).Split(new string[] { "\n", "\n\r" }, StringSplitOptions.RemoveEmptyEntries);
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
                        //strs[j] = "userinfo_" + strs[j];
                        //str += strs[j];
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

            //Lista betöltése folyamatban...

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
            //if (!binary)
            //{
            string responseString;
            responseString = Uri.UnescapeDataString(new StreamReader(response.GetResponseStream()).ReadToEnd());
            return responseString;
            //}
            /*else
            {
                byte[] responseBinary = new byte[1024 * 1024 * 60];
                response.GetResponseStream().Read(responseBinary, 0, 1024 * 1024 * 60); //Nem vagyok még biztos benne, hogy jó ötlet-e
                return responseBinary;
            }*/
        }
        /*public static string SendRequest(string action, string data, int remnum, bool loggedin) //2014.08.02.
        {
            return (string)SendRequest(action, data, remnum, loggedin, false);
        }*/
        //public static Socket SendConnection;
        /*public static object SendSocket(string action, string data, int remnum, bool loggedin, bool binary) //2014.02.18.
        {
            Socket SendConnection;
#if LOCAL_SERVER //2014.07.07. 22:00
            SendConnection = new Socket(SocketType.Stream, ProtocolType.Tcp);
            SendConnection.Connect("localhost", Settings.Default.port + 1); //2014.08.02.
#else
                Connection = new Socket(SocketType.Stream, ProtocolType.Tcp);
                Connection.Connect("localhost", Settings.Default.port + 1); //2014.08.02.
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

            SendConnection.Send(BitConverter.GetBytes(datax.Length));
            SendConnection.Send(datax);

            //Lista betöltése folyamatban...

            byte[] response = new byte[4];
            SendConnection.Receive(response, 4, SocketFlags.None);
            int len = BitConverter.ToInt32(response, 0);

            response = new byte[len];
            try
            {
                SendConnection.Receive(response);
            }
            catch (Exception e)
            {
                if (Tries > 10)
                {
                    MessageBox.Show(Language.Translate("error_network"] + ":\n" + e.Message + "\n\n" + Language.Translate("error_network2"], Language.Translate("error"]); //2014.04.25.
                    Tries = 0;
                }
                Tries++;
                return SendRequest(action, data, remnum, loggedin); //Újrapróbálkozik
            }
            if (!binary)
            {
                string responseString;
                responseString = encoding.GetString(response);
                return responseString;
            }
            else
            {
                return response;
            }
        }*/
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
