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

        public static void KeepUpThread()
        { //2014.08.28.
            while (true) //while: 2014.12.19.
            {
                Thread.Sleep(59 * 60 * 1000); //59 percenként frissíti a jelenlétét, így biztosan nem jelenti offline-nak a PHP (elvileg)
                Console.WriteLine("KeepUpThread: " + Networking.SendRequest(Networking.RequestType.KeepActive, "", 0, true));
            }
        }
        /*[Obsolete]
        public static void KeepUpUsersThread() //2014.09.26. - Nehogy bezáruljon a kapcsolat
        {
            *while (true) //while: 2014.12.19.
            {
                Thread.Sleep(20 * 1000);
                //Networking.SendUpdate(UpdateType.KeepAlive, new byte[] { 0x01 }, false);
                Neworking.SendUpdate(new PacketFormat(false, new PDKeepAlive()));
            }*
        }*/

        //public static void SendUpdateInThread(UpdateType ut, byte[] data, EventHandler<byte[][]> doneevent, IPEndPoint onlythisep = null)
        /*public static async Task<PacketFormat[]> SendUpdateInThread(PacketFormat packet, IPEndPoint onlythisep = null)
        { //2014.12.31. 0:24 - Válaszadásra van külön thread, itt csak eredeti küldést használhat
            //threadobject.Clear();
            //threadobject.Add(ut);
            //threadobject.Add(data);
            //threadobject.Add(packet);
            //threadobject.Add(doneevent);
            //threadobject.Add(onlythisep);
            //while (networkthread == null) ;
            //networkthread.Interrupt();
            return await Task.Run(() => SendUpdate(packet, onlythisep)); //2015.04.03.
        }*/
        //private static List<object> threadobject = new List<object>();
        //private static Thread networkthread;
        /*public static void NetworkThread() //A MainThread ezen keresztül hívja meg, hogy ne fagyjon le
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
                    //var resp = SendUpdate((UpdateType)threadobject[0], (byte[])threadobject[1], false, (IPEndPoint)threadobject[3]);
                    var resp = SendUpdate((PacketFormat)threadobject[0], (IPEndPoint)threadobject[2]);
                    if (threadobject[1] != null)
                        ((EventHandler<PacketFormat[]>)threadobject[1])(null, resp);
                }
            }
        }*/
    }
}
