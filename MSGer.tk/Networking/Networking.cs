using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
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
            SendImage,
            SendFile,
        };

        private static void Log(string message) //2014.12.31.
        {
            Logging.Log(message, Logging.LogType.Network);
        }

        public static async Task<bool> SendChatMessage(ChatPanel chat, string message, double time)
        { //2014.09.22.
            var tmp1 = chat.ChatPartners.Select(entry => entry.UserID).ToArray();
            int[] tmp2 = new int[tmp1.Length + 1];
            tmp1.CopyTo(tmp2, 0);
            tmp2[tmp2.Length - 1] = CurrentUser.UserID;
            var resp = await new PacketSender(new PDUpdateMessages(tmp2, message, time)).SendAsync(); //2015.05.15.
            if (resp == null || resp.Length == 0 || resp.All(entry => !((PDUpdateMessages)entry.EData).RSuccess))
                return false;
            else //Ha válaszoltak, és senki sem válaszolt nem oké jelzéssel, akkor rendben van
                return true;
        }
        public static bool ReceiveUpdates(out PacketFormat packet, out IPEndPoint remoteep)
        {
            //buffer = null; //2015.03.28.
            packet = null;
            remoteep = null; //2015.03.28.

            IPEndPoint remoteEP;
            remoteEP = new IPEndPoint(IPAddress.Any, CurrentUser.Port); //2015.05.24.
            byte[] buf;
            try
            {
                buf = ReceiverConnection.Receive(ref remoteEP);
            }
            catch
            {
                return false; //2015.03.28.
            }
            PacketFormat pf = PacketFormat.FromBytes(buf);
            if (pf.Response)
            {
                if (Senders.ContainsKey(pf.ID))
                    Senders[pf.ID].OnReceiveResponse(pf, remoteEP);
                return false;
            }
            else
            {
                packet = pf;
                remoteep = remoteEP;
                return true;
            }
        }
    }
}
