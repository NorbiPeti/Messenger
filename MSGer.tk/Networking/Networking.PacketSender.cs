using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    partial class Networking
    {
        public class PacketSender
        { //2015.05.14.
            private static int NextID = 0;
            private object Lock = new object();
            private PacketFormat Response;
            private IPEndPoint RemoteEP;

            public int ID { get; private set; }
            public PacketFormat Packet { get; private set; }
            /// <summary>
            /// PacketSender
            /// </summary>
            /// <param name="data">PacketData</param>
            /// <param name="respid">Response ID</param>
            public PacketSender(PacketData data, int respid = -1)
            {
                if (NextID >= Int32.MaxValue)
                    NextID = 0;
                if (respid == -1)
                    ID = NextID++;
                else
                    ID = respid;
                Packet = new PacketFormat(respid != -1, data, ID);
                if (respid == -1)
                    Senders.Add(ID, this);
            }

            public void OnReceiveResponse(PacketFormat response, IPEndPoint remoteep)
            {
                lock (Lock)
                {
                    Response = response;
                    RemoteEP = remoteep;
                    Monitor.Pulse(Lock);
                }
            }

            public PacketFormat[] Send(IPEndPoint onlythisep = null)
            {
                return SendUpdate(onlythisep);
            }

            public async Task<PacketFormat[]> SendAsync(IPEndPoint onlythisep = null)
            {
                return await Task.Run(() => Send(onlythisep));
            }

            private PacketFormat[] SendUpdate(IPEndPoint onlythisep = null)
            {
                if (Program.MainF != null && Program.MainF.IsHandleCreated)
                {
                    if (Program.MainF.InvokeRequired)
                    {
                        Program.MainF.Invoke(new Action<MainForm.StatType, int>(Program.MainF.UpdateStats),
                            MainForm.StatType.Servers, UserInfo.IPs.Count);
                        Program.MainF.Invoke(new Action<MainForm.StatType, int>(Program.MainF.UpdateStats),
                            MainForm.StatType.OnlineServers, UserInfo.IPs.Count - UserInfo.BannedIPs.Count);
                    }
                    else
                    {
                        MessageBox.Show("Internal error: Network call was made in the same thread as the UI. This could lead to freezes.\nNetwork update canceled.");
                        return null;
                    }
                }
                if ((UserInfo.IPs.Count == 0
                    || (RemoteEP == null) ? false : !UserInfo.IPs.Any(entry => entry.Address.Equals(RemoteEP.Address))) //<-- 2015.06.16.
                        && Packet.PacketType != Networking.UpdateType.LoginUser)
                    return null;
                byte[] senddata = Packet.ToBytes();
                if (UserInfo.BanTime < Environment.TickCount - 1000 * 10)
                    UserInfo.BannedIPs = new List<IPEndPoint>();
                if (onlythisep == null)
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
                {
                    try
                    {
                        SenderConnection.Send(senddata, senddata.Length, onlythisep);
                    }
                    catch (ObjectDisposedException)
                    {
                        return null;
                    }
                }

                if (!Packet.Response)
                {
                    int lasttick = Environment.TickCount;
                    List<PacketFormat> Ret = new List<PacketFormat>();
                    List<IPEndPoint> ResponsedIPs = new List<IPEndPoint>();
                    int count = 1;
                    while (Environment.TickCount - 1000 * 10 < lasttick && (
                        (onlythisep == null) ?
                        ResponsedIPs.Count < UserInfo.IPs.Count - UserInfo.BannedIPs.Count :
                        ResponsedIPs.Count == 0))
                    {
                        lock (Lock)
                        {
                            while (Response == null && Monitor.Wait(Lock, 1000))
                                ;
                        }
                        if (Response == null)
                        {
                            foreach (var item in UserInfo.IPs.Except(UserInfo.BannedIPs).Except(ResponsedIPs))
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
                            && Packet.PacketType != Networking.UpdateType.LoginUser))
                        {
                            foreach (var item in UserInfo.IPs.Where(entry => entry.Address.Equals(RemoteEP.Address) && entry.Port == Response.Port))
                                ResponsedIPs.Add(item);
                            if (Response.KeyIndex != CurrentUser.KeyIndex && Response.PacketType != UpdateType.SetKey)
                            {
                                Response = null;
                                continue;
                            }

                            Ret.Add(Response);
                        }
                        Response = null;
                    }
                    Response = null;
                    RemoteEP = null;
                    return Ret.ToArray();
                }
                return null;
            }
        }
    }
}
