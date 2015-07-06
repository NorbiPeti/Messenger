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

        public static void KeepUpThread() //TODO: KeepUpThread
        { //2014.08.28.
            while (true) //while: 2014.12.19.
            {
                Thread.Sleep(59 * 60 * 1000); //59 percenként frissíti a jelenlétét, így biztosan nem jelenti offline-nak a PHP (elvileg)
                Console.WriteLine("KeepUpThread: " + Networking.SendRequest(Networking.RequestType.KeepActive, "", 0, true));
            }
        }
    }
}
