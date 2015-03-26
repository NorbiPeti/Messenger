using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSGer.tk
{
    partial class Networking
    {
        //private const string ConnID = "MSGer.tk connection";
        /*public static void WaitForNewConnections()
        {
            while (true)
            {
                Ping pingSender = new Ping();
                //byte[] id = Encoding.Unicode.GetBytes(ConnID);
                //PingReply reply = pingSender.Send("3.3.3.3", 30000, id, new PingOptions(512, true));
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
                socket.SendTo(ICMPPacket.CreateRequest(), new IPEndPoint(IPAddress.Parse("3.3.3.3"), 8988));
                byte[] buffer = new byte[128];
                //IPEndPoint ep = new IPEndPoint(IPAddress.Any, 8988);
                EndPoint ep = (EndPoint)new IPEndPoint(IPAddress.Any, 8988);
                socket.ReceiveFrom(buffer, ref ep);
                //if (reply.Status == IPStatus.TimeExceeded)
                if(ICMPPacket.IsPacketGood(buffer))
                {
                    Log("A client is trying to connect...");
                    //MakeConnection(reply.Address);
                    MakeConnection(((IPEndPoint)ep).Address);
                }
                else
                {
                    //Log("An error occured during ICMP... - Status: " + reply.Status);
                    Log("An error occured during ICMP...");
                }
                Thread.Sleep(28 * 1000);
            }
        }
		public static void ConnectToServer(IPEndPoint server)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
            socket.SendTo(ICMPPacket.CreateReply(),server);
        }

        private static void MakeConnection(IPAddress addr)
        {

        }*/

        public static void KeepUpThread()
        { //2014.08.28.
            while (true) //while: 2014.12.19.
            {
                Thread.Sleep(59 * 60 * 1000); //59 percenként frissíti a jelenlétét, így biztosan nem jelenti offline-nak a PHP (elvileg)
                Console.WriteLine("KeepUpThread: " + Networking.SendRequest("keepactive", "", 0, true));
            }
        }
        public static void KeepUpUsersThread() //2014.09.26. - Nehogy bezáruljon a kapcsolat
        {
            while (true) //while: 2014.12.19.
            {
                Thread.Sleep(20 * 1000);
                Networking.SendUpdate(UpdateType.KeepAlive, new byte[] { 0x01 }, false);
            }
        }

        public static void SendUpdateInThread(UpdateType ut, byte[] data, EventHandler<byte[][]> doneevent, IPEndPoint onlythisep = null)
        { //2014.12.31. 0:24 - Válaszadásra van külön thread, itt csak eredeti küldést használhat
            threadobject.Clear();
            threadobject.Add(ut);
            threadobject.Add(data);
            threadobject.Add(doneevent);
            threadobject.Add(onlythisep);
            while (networkthread == null) ;
            networkthread.Interrupt();
        }
        private static List<object> threadobject = new List<object>();
        private static Thread networkthread;
        public static void NetworkThread() //A MainThread ezen keresztül hívja meg, hogy ne fagyjon le
        { //2014.12.30.
            networkthread = Thread.CurrentThread;
            while (true)
            {
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }
                catch (ThreadInterruptedException)
                {
                    var resp = SendUpdate((UpdateType)threadobject[0], (byte[])threadobject[1], false, (IPEndPoint)threadobject[3]);
                    if (threadobject[2] != null)
                        ((EventHandler<byte[][]>)threadobject[2])(null, resp);
                }
            }
        }
    }
}
