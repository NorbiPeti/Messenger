using System;
using System.Collections.Generic;
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
        public static UdpClient SenderConnection = new UdpClient(); //2014.09.04. - Ezt ne társítsa egy porthoz, hogy működjön az udp hole punching
        public static UdpClient ReceiverConnection = new UdpClient(); //2014.09.04. - Társítsa egy porthoz
        public static byte[] DataBuffer;
        public static IPEndPoint RemoteEP;
        public static bool WaitingOnResponse = false; //2014.08.16.
        public static byte WaitingOnPacket = 0x00; //2014.08.16. - 0x00: Nincs
        public static byte[][] SendUpdate(UpdateType ut, byte[] data, bool response, IPEndPoint onlythisep = null)
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
                    /*Program.MainF.UpdateStats(MainForm.StatType.Servers, UserInfo.IPs.Count); //2014.11.21.
                    Program.MainF.UpdateStats(MainForm.StatType.OnlineServers, UserInfo.IPs.Count - UserInfo.BannedIPs.Count); //2014.11.21. - 2014.12.05. - Servers --> OnlineServers*/
                    //2014.12.31.
                    MessageBox.Show("Internal error: Network call was made in the same thread as the UI. This could lead to freezes.\nNetwork update canceled.");
                    return null;
                }
            }

            if (UserInfo.IPs.Count == 0
                && !UserInfo.IPs.Any(entry => entry.Address.Equals(RemoteEP.Address))
                    && ut != Networking.UpdateType.LoginUser
                    /*&& ut != Networking.UpdateType.CheckConn
                    && ut != Networking.UpdateType.RequestConn
                    && ut != Networking.UpdateType.MakeConn
                    && ut != Networking.UpdateType.MakeConn2*/) //2014.12.19.
                return null;
            //Log("Packet sending begins: there are users online and/or it is an update type that does not require that.");
            byte[] senddata = CreatePacket(response, (byte)ut, data);
            //Log("Packet created successfully (" + ((response) ? "" : "not ") + "response) with update type " + ut.ToString());
            if (UserInfo.BanTime < Environment.TickCount - 1000 * 10) //2014.08.30. - 2014.10.09. - 10 percről 1-re csökkentve - 2014.11.22. - 1 percről 10 mp-re csökkentve
                //UserInfo.BannedIPs = new List<IPEndPoint>(); //2014.08.30.
                UserInfo.BannedIPs = new List<IPEndPoint>(); //2014.11.23.
            if (!response)
            { //2014.08.30. - Azelőtt állítsa be, hogy elküldené a lekéréseket, hogy biztosan reagáljon a válaszra
                WaitingOnResponse = true; //2014.08.16.
                WaitingOnPacket = (byte)ut; //2014.08.16.
            }
            //Log("Hole punching begins.");
            /*if (ut != UpdateType.CheckConn && ut != UpdateType.RequestConn)
                CheckNPunchHole(); //2014.11.22.*/
            //Log("Hole punching done.");
            if (onlythisep == null) //2014.11.22. - != helyett ==: 2014.12.18.
            {
                //Log("Sending to every known user...");
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
            }
            else
            { //2014.11.22.
                //Log("Sending to single ip: " + onlythisep.Address + ":" + onlythisep.Port);
                try
                {
                    //if (UserInfo.IPs.Any(entry=>entry.IP==onlythisep) && !UserInfo.BannedIPs.Any(entry=>entry.IP==onlythisep))
                    //if (UserInfo.IPs.Any(entry => entry.IP == onlythisep) && !UserInfo.BannedIPs.Any(entry => entry.IP == onlythisep))
                    SenderConnection.Send(senddata, senddata.Length, onlythisep);
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
                int count = 1;
                //Log("Starting to wait on response...");
                while (Environment.TickCount - 1000 * 10 < lasttick && (
                    (onlythisep == null) ? //2014.11.22.
                    ResponsedIPs.Count < UserInfo.IPs.Count - UserInfo.BannedIPs.Count :
                    ResponsedIPs.Count == 0 //2014.11.22.
                    )) //2014.09.09. - 2014.10.09. - 60 mp --> 10 mp
                { //2014.08.19. - Ret.Count == tmp.Length
                    if (MainForm.LThread != null) //2014.09.06.
                    {
                        //Log("Waiting to get response... Wait time: " + count * 2 + "seconds");
                        while (DataBuffer == null && Environment.TickCount - 1000 * count * 2 < lasttick) ; //Várakozik, amíg a másik thread át nem adja a választ - 2014.10.09. - 10 mp --> 2 mp

                        /*foreach (var item in UserInfo.IPs.Except(UserInfo.BannedIPs).Except(ResponsedIPs)) //2014.09.22.
                        { //Elküldi az összes ismert címre
                            try
                            {
                                SenderConnection.Send(senddata, senddata.Length, item.IP);
                            }
                            catch (ObjectDisposedException)
                            {
                                return null;
                            }
                        }
                        count++;*/
                    }
                    if (DataBuffer == null) //2014.08.30. - Az idő telt le
                    {
                        //Log("No response received. Retrying...");
                        //UserInfo.BannedIPs = UserInfo.IPs.Except(ResponsedIPs).ToList(); //2014.08.30. - Ideiglenesen kitilt minden IP-t, ahonnan nem érkezett válasz
                        //UserInfo.BanTime = Environment.TickCount;

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
                        //break;
                        continue;
                    }
                    //Log("A response received.");
                    //if (UserInfo.IPs.Any(entry=>entry.IP==RemoteEP) && !UserInfo.BannedIPs.Any(entry=>entry.IP==RemoteEP))
                    if (!(!UserInfo.IPs.Any(entry => entry.Address.Equals(RemoteEP.Address))
                        && ut != Networking.UpdateType.LoginUser
                        /*&& ut != Networking.UpdateType.CheckConn
                        && ut != Networking.UpdateType.RequestConn
                        && ut != Networking.UpdateType.MakeConn
                        && ut != Networking.UpdateType.MakeConn2*/)) //2014.12.22. - Ha az egész feltétel nem teljesül, akkor sikerült
                    {
                        //Log("Response is from known ip or it's not required to be that.");
                        //ResponsedIPs.Add(RemoteEP);
                        foreach (var item in UserInfo.IPs.Where(entry => entry == RemoteEP))
                            ResponsedIPs.Add(item);
                        var pparts = ParsePacket(DataBuffer);
                        //Log("Packet parsed.");
                        if (pparts.KeyVersion != CurrentUser.KeyIndex && pparts.UpdateType != UpdateType.SetKey)
                        {
                            //Log("Key version mismatch.");
                            DataBuffer = null; //2014.09.22. - Mindig adja meg a lehetőséget, hogy újra beállítsa
                            continue;
                        }
                        //Log("Complete success!");

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
                //Log("Resetting everything.");
                DataBuffer = null;
                RemoteEP = null;
                WaitingOnResponse = false;
                WaitingOnPacket = 0x00;
                //Log("Returning response if there was any.");
                return Ret.ToArray();
            }
            return null;
        }
    }
}
