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
        public enum UpdateType : byte
        {
            ListUpdate = 0x01, //0x01: OK
            UpdateMessages, //0x01: OK
            GetImage, //(int)0: Hiba, egyébként a kép hossza, majd a kép
            LoginUser, //0x00: Hiba - Egyébként minden fontos információ, amiről "lemaradt"
            LogoutUser, //0x01: OK
            SetKey, //0x01: OK
            KeepAlive,
            /*CheckConn, //0x01: Csatlakozva
            RequestConn, //
            MakeConn,
            MakeConn2*/
        };

        private static void Log(string message) //2014.12.31.
        {
            Logging.Log(message, Logging.LogType.Network);
        }

        /*public static byte[][] SendUpdate(UpdateType ut, byte[] data, bool response) - Opcionális paraméterre változtatva: 2014.12.22.
        { //2014.11.22.
            return SendUpdate(ut, data, response, null);
        }*/
        /*private static void CheckNPunchHole() //Még előtte tudja meg a belső portot
        { //2014.11.22.
            try
            {
                foreach (var item in UserInfo.IPs.Except(UserInfo.BannedIPs))
                {
                    if (Thread.CurrentThread == MainForm.MainThread)
                    {
                        SendUpdateInThread(UpdateType.CheckConn, new byte[] { }, new EventHandler<byte[][]>(
                            (e, resp) =>
                            {
                                if (resp != null && resp.Length != 0 && ParsePacket(resp[0]).Data[0] == 0x01)
                                    return; //Minden rendben, válaszolt
                                UserInfo.BannedIPs.Remove(item); //2014.12.19.
                                //if (Storage.Settings["isserver"] == "1") - Ha szerver, ha nem, kérje a többi szervert, hogy csatlakoztassa össze
                                resp = SendUpdate(UpdateType.RequestConn, Encoding.Unicode.GetBytes(item.ToString()), false); //Mindenkitől kéri, akivel van kapcsolata
                                if (resp != null && resp.Length != 0 && ParsePacket(resp[0]).Data[0] == 0x01) //2014.11.23. - Ilyenkor már tudnia kellene a portot, és hasonlókat
                                {
                                    Networking.SendUpdate(UpdateType.MakeConn2, new byte[] { 0x01 }, false); //Elvileg akkor végez ez az utasítás, ha létrejött a kapcsolat, vagy letelt a határidő
                                }
                            }
                            ), item);
                    }
                    else
                    {
                        var resp = SendUpdate(UpdateType.CheckConn, new byte[] { }, false, item);
                        if (resp != null && resp.Length != 0 && ParsePacket(resp[0]).Data[0] == 0x01)
                            continue; //Minden rendben, válaszolt
                        UserInfo.BannedIPs.Remove(item); //2014.12.19.
                        //if (Storage.Settings["isserver"] == "1") - Ha szerver, ha nem, kérje a többi szervert, hogy csatlakoztassa össze
                        resp = SendUpdate(UpdateType.RequestConn, Encoding.Unicode.GetBytes(item.ToString()), false); //Mindenkitől kéri, akivel van kapcsolata
                        if (resp != null && resp.Length != 0 && ParsePacket(resp[0]).Data[0] == 0x01) //2014.11.23. - Ilyenkor már tudnia kellene a portot, és hasonlókat
                        {
                            Networking.SendUpdate(UpdateType.MakeConn2, new byte[] { 0x01 }, false); //Elvileg akkor végez ez az utasítás, ha létrejött a kapcsolat, vagy letelt a határidő
                        }
                    }
                }
            }
            catch (InvalidOperationException) { }
        }*/

        public static bool SendChatMessage(ChatPanel chat, string message)
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

        public struct PacketParts
        {
            public bool Response;
            public UpdateType UpdateType;
            public int KeyVersion;
            public int Port; //2014.12.19.
            public int UserID;
            public byte[] Data;
        };
    }
}
