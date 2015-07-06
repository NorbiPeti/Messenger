using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    partial class Networking
    {
        /*public static UdpClient SenderConnection = new UdpClient(AddressFamily.InterNetworkV6); //2014.09.04. - Ezt ne társítsa egy porthoz, hogy működjön az udp hole punching - IPv6: 2015.04.03.
        public static UdpClient ReceiverConnection; //2014.09.04. - Társítsa egy porthoz - IPv6: 2015.04.03. - 2015.04.03. - A MainForm-nál úgyis létrehozza
        public static byte[] DataBuffer;
        public static IPEndPoint RemoteEP;
        public static bool WaitingOnResponse = false; //2014.08.16.
        public static UpdateType WaitingOnPacket = 0x00; //2014.08.16. - 0x00: Nincs; UpdateType: 2015.05.10.
        //public static byte[][] SendUpdate(UpdateType ut, byte[] data, bool response, IPEndPoint onlythisep = null)

        private static object Lock = new object(); //2015.05.10.
        public static PacketFormat[] SendUpdate(PacketFormat packet, IPEndPoint onlythisep = null)
        {
            if (Program.MainF != null && Program.MainF.IsHandleCreated) //IsHandleCreated: 2014.12.31.
            { //2014.11.21.
                if (Program.MainF.InvokeRequired) //2014.11.21.
                { //2014.11.21.
                    Program.MainF.Invoke(new Action<MainForm.StatType, int>(Program.MainF.UpdateStats), //2014.11.21.
                        MainForm.StatType.Servers, UserInfo.IPs.Count); //2014.11.21.
                    Program.MainF.Invoke(new Action<MainForm.StatType, int>(Program.MainF.UpdateStats), //2014.11.21.
                        MainForm.StatType.OnlineServers, UserInfo.IPs.Count - UserInfo.BannedIPs.Count); //2014.11.21.
                }
                else //2014.11.21.
                { //2014.11.21.
                    //2014.12.31.
                    MessageBox.Show("Internal error: Network call was made in the same thread as the UI. This could lead to freezes.\nNetwork update canceled.");
                    return null;
                }
            }

            lock (Lock)
            {
                var ut = packet.PacketType;

                if (UserInfo.IPs.Count == 0
                    && !UserInfo.IPs.Any(entry => entry.Address.Equals(RemoteEP.Address))
                        && ut != Networking.UpdateType.LoginUser)
                    return null;
                //byte[] senddata = CreatePacket(response, (byte)ut, data);
                byte[] senddata = packet.ToBytes(false);
                if (UserInfo.BanTime < Environment.TickCount - 1000 * 10) //2014.08.30. - 2014.10.09. - 10 percről 1-re csökkentve - 2014.11.22. - 1 percről 10 mp-re csökkentve
                    UserInfo.BannedIPs = new List<IPEndPoint>(); //2014.11.23.
                //if (!response)
                if (!packet.Response)
                { //2014.08.30. - Azelőtt állítsa be, hogy elküldené a lekéréseket, hogy biztosan reagáljon a válaszra
                    WaitingOnResponse = true; //2014.08.16.
                    //WaitingOnPacket = (byte)ut; //2014.08.16.
                    WaitingOnPacket = ut; //2015.05.10.
                }
                if (onlythisep == null) //2014.11.22. - != helyett ==: 2014.12.18.
                {
                    foreach (var item in UserInfo.IPs)
                    { //Elküldi az összes ismert címre
                        try
                        {
                            if (!UserInfo.BannedIPs.Contains(item))
                                SenderConnection.Send(senddata, senddata.Length, item);
                        }
                        catch (ObjectDisposedException)
                        {
                            return null;
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Network error:\n" + e.Message + "\n\n" + e.StackTrace);
                        }
                    }
                }
                else
                { //2014.11.22.
                    try
                    {
                        SenderConnection.Send(senddata, senddata.Length, onlythisep);
                    }
                    catch (ObjectDisposedException)
                    {
                        return null;
                    }
                }

                //if (!response)
                if (!packet.Response)
                {
                    int lasttick = Environment.TickCount;
                    List<byte[]> Ret = new List<byte[]>();
                    List<IPEndPoint> ResponsedIPs = new List<IPEndPoint>();
                    int count = 1;
                    while (Environment.TickCount - 1000 * 10 < lasttick && (
                        (onlythisep == null) ? //2014.11.22.
                        ResponsedIPs.Count < UserInfo.IPs.Count - UserInfo.BannedIPs.Count :
                        ResponsedIPs.Count == 0 //2014.11.22.
                        )) //2014.09.09. - 2014.10.09. - 60 mp --> 10 mp
                    { //2014.08.19. - Ret.Count == tmp.Length
                        if (MainForm.LThread != null) //2014.09.06.
                        {
                            while (DataBuffer == null && Environment.TickCount - 1000 * count * 2 < lasttick) ; //Várakozik, amíg a másik thread át nem adja a választ - 2014.10.09. - 10 mp --> 2 mp
                        }
                        if (DataBuffer == null) //2014.08.30. - Az idő telt le
                        {
                            Console.WriteLine("Didn't get a response in time. Retrying...");
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
                            continue;
                        }
                        if (!(!UserInfo.IPs.Any(entry => entry.Address.Equals(RemoteEP.Address))
                            && ut != Networking.UpdateType.LoginUser))
                        {
                            foreach (var item in UserInfo.IPs.Where(entry => entry == RemoteEP))
                                ResponsedIPs.Add(item);
                            Debug.WriteLine("Received response. (" + packet.PacketType + ")");
                            //var pparts = ParsePacket(DataBuffer);
                            var rpacket = PacketFormat.FromBytes(DataBuffer);
                            //if (pparts.KeyVersion != CurrentUser.KeyIndex && pparts.UpdateType != UpdateType.SetKey)
                            if (rpacket.KeyIndex != CurrentUser.KeyIndex && rpacket.PacketType != UpdateType.SetKey)
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
                    return Ret.Select(entry => PacketFormat.FromBytes(entry)).ToArray();
                }
                return null;
            }
        }*/
    }
}
