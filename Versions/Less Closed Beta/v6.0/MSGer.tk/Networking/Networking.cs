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
    static partial class Networking
    {
        public static UdpClient SenderConnection = new UdpClient(AddressFamily.InterNetworkV6);
        public static UdpClient ReceiverConnection;
        public static Dictionary<int, PacketSender> Senders = new Dictionary<int, PacketSender>();

        public enum UpdateType : byte
        {
            ListUpdate = 0x01, //0x01: OK
            UpdateMessages, //0x01: OK
            GetImage, //(int)0: Hiba, egyébként a kép hossza, majd a kép
            LoginUser, //0x00: Hiba - Egyébként minden fontos információ, amiről "lemaradt"
            LogoutUser, //0x01: OK
            SetKey, //0x01: OK
            //KeepAlive,
            SendImage,
            SendFile,
        };

        private static void Log(string message) //2014.12.31.
        {
            Logging.Log(message, Logging.LogType.Network);
        }

        public static async Task<bool> SendChatMessage(ChatPanel chat, string message, double time)
        { //2014.09.22.
            /*List<byte> bytes = new List<byte>();
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
            bytes.AddRange(Encoding.Unicode.GetBytes(sendstr));*/
            //byte[][] resp = SendUpdate(UpdateType.UpdateMessages, Encoding.Unicode.GetBytes(sendstr), false);
            //var resp = await SendUpdateInThread(new PacketFormat(false, new PDUpdateMessages(chat.ChatPartners.Select(entry => entry.UserID).ToArray(), message)));
            var tmp1 = chat.ChatPartners.Select(entry => entry.UserID).ToArray();
            int[] tmp2 = new int[tmp1.Length + 1];
            tmp1.CopyTo(tmp2, 0);
            tmp2[tmp2.Length - 1] = CurrentUser.UserID;
            var resp = await new PacketSender(new PDUpdateMessages(tmp2, message, time)).SendAsync(); //2015.05.15.
            if (resp == null || resp.Length == 0 || resp.All(entry => !((PDUpdateMessages)entry.EData).RSuccess))
                return false;
            else //Ha válaszoltak, és senki sem válaszolt nem oké jelzéssel, akkor rendben van
                return true;
            //if (resp == null || resp.Length == 0 || resp.All(bytesb => bytesb[0] != 0x01))
        }
        //public static object[] ReceiveUpdates() //Thread function
        //public static bool ReceiveUpdates(out byte[] buffer, out IPEndPoint remoteep) //<-- 2015.03.28.
        public static bool ReceiveUpdates(out PacketFormat packet, out IPEndPoint remoteep)
        {
            //buffer = null; //2015.03.28.
            packet = null;
            remoteep = null; //2015.03.28.

            IPEndPoint remoteEP;
            //remoteEP = new IPEndPoint(IPAddress.Any, Int32.Parse(Storage.Settings[SettingType.Port])); //2014.09.04. - A port beállítása már megtörtént
            remoteEP = new IPEndPoint(IPAddress.Any, CurrentUser.Port); //2015.05.24.
            byte[] buf;
            try
            {
                buf = ReceiverConnection.Receive(ref remoteEP);
            }
            catch
            {
                //return null;
                return false; //2015.03.28.
            }
            //if (buf[0] == 0x01) //0x01: Válasz egy kérelemre
            PacketFormat pf = PacketFormat.FromBytes(buf);
            if (pf.Response)
            {
                //if (WaitingOnResponse == false || WaitingOnPacket != buf[1])
                /*if (WaitingOnResponse == false || WaitingOnPacket != pf.PacketType) //2015.05.10.
                    return false;
                while (DataBuffer != null) ; //Várja meg, amíg feldolgozza a legutóbbi adatot
                DataBuffer = buf; //2014.09.19. - Küldön el mindenhova mindent, és egységesen egy funkcióval dolgozza fel
                RemoteEP = remoteEP;*/
                if (Senders.ContainsKey(pf.ID))
                    Senders[pf.ID].OnReceiveResponse(pf, remoteEP);
                return false;
            }
            else
            //return new object[] { buf, remoteEP };
            {
                //buffer = buf;
                packet = pf;
                remoteep = remoteEP;
                return true;
            }
        }

        /*[Obsolete]
        public struct PacketParts
        {
            public bool Response;
            public UpdateType UpdateType;
            public int KeyVersion;
            public int Port; //2014.12.19.
            public int UserID;
            public byte[] Data;
        };*/
    }
}
