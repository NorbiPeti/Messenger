using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSGer.tk
{
    public class Logging
    {
        public enum LogType
        {
            Network
        }
        public static void Log(string message, LogType logtype) //2014.12.31.
        {
            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");
            string path;
            switch (logtype)
            {
                case LogType.Network:
                    path = "network";
                    break;
                default:
                    throw new NotImplementedException("Log type not implemented.");
            }
            string finaltext = "[" + Process.GetCurrentProcess().Id + ": " + Thread.CurrentThread.Name + " | " + DateTime.Now.ToString("yyyy.MM.dd. HH:mm:ss") + "] " + message + Environment.NewLine;
            Console.WriteLine(logtype.ToString() + " - " + finaltext);
            while (true)
            {
                bool retry = false;
                try
                {
                    File.AppendAllText("logs\\" + path + ".txt", finaltext);
                }
                catch (IOException)
                {
                    retry = true;
                }
                if (!retry)
                    break;
            }
        }
    }
}
