//#define LOCAL_SERVER

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SzNPProjects;

namespace MSGer.tk
{
    static class Program
    {
        public static MainForm MainF;
        public static string ProcessName = Process.GetCurrentProcess().ProcessName;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Process[] pname = Process.GetProcessesByName(ProcessName);
            if (pname.Length > 1 && !(args.Length > 0 && args[0] == "multi"))
                Environment.Exit(0); //2014.09.26. - Felesleges bármi más műveletet végrehajtani, még nem is töltött be semmit

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BeforeLogin.Create();
            Console.WriteLine("Starting MSGer.tk...");
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            try
            {
                MainF = new MainForm();
                Application.Run(MainF);
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("Egy fájl nem található.\nA fájl neve:\n" + e.FileName + "\nEnnél a műveletnél: " + e.TargetSite);

            }
        }

        public static readonly bool ShowFirstChangeExceptions = false;
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (MessageBox.Show("An unhandled error occured\n" + ((Exception)e.ExceptionObject).Message + "\nHere: " + ((Exception)e.ExceptionObject).TargetSite + ((e.IsTerminating) ? "\nThe program will exit(?)." : "") + "\n\nExit?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                Program.Exit(CurrentUser.UserID != 0); //2014.09.26. - Ha be van jelentkezve, elmenti a beállításokat - Csak mert így tán a legegyszerűbb
        }

        static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            if (ShowFirstChangeExceptions)
                MessageBox.Show("An error occured (a first change exception):\n" + e.Exception.Message + "\nHere: " + e.Exception.TargetSite);
        }

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0,
                                                      DateTimeKind.Utc);

        public static DateTime UnixTimeToDateTime(string text)
        {
            double seconds = double.Parse(text, CultureInfo.InvariantCulture);
            //2014.04.25.
            DateTime time = Epoch.AddSeconds(seconds);
            time = DateTime.SpecifyKind(time, DateTimeKind.Utc);
            return time.ToLocalTime();
        }

        public static string DateTimeToUnixTime(DateTime time) //2014.08.28.
        {
            return ((time.Ticks - 621355968000000000) / 10000000).ToString();
        }

        private static void BeforeExit(bool savesettings)
        { //2014.09.01.
            if (MainF != null)
            {
                MainF.Hide();
                MainF.toolStripMenuItem4.Enabled = false; //2014.04.12.
                MainF.toolStripMenuItem8.Enabled = false; //2014.04.12.
                MainF.notifyIcon1.Dispose(); //2014.08.28.
                if (CurrentUser.UserID != 0) //2014.04.18.
                {
                    CurrentUser.SendChanges = false; //2014.08.30.
                    MainF.SetOnlineState(null, null); //2014.04.12.)
                    if (MainF.WindowState == FormWindowState.Maximized) //2014.04.18. - 2014.08.08.
                        Storage.Settings["windowstate"] = "1";
                    else if (MainF.WindowState == FormWindowState.Minimized)
                        Storage.Settings["windowstate"] = "2";
                    else if (MainF.WindowState == FormWindowState.Normal)
                        Storage.Settings["windowstate"] = "3";
                    Storage.Save(true); //2014.08.07.
                }
            }
            if (savesettings) //2014.08.28.
                Storage.Save(false); //2014.08.08.
            if (Networking.ReceiverConnection != null)
                Networking.ReceiverConnection.Close();
            if (Networking.SenderConnection != null)
                Networking.SenderConnection.Close();
        }
        public static void Exit(bool savesettings)
        { //2014.04.12.
            BeforeExit(savesettings);
            Environment.Exit(0); //Hatásosabb
            MessageBox.Show("Hiba: Nem sikerült leállítani a programot.");
        }
        public static void Exit()
        { //2014.08.28. - Csak akkor ne mentse el a beállításokat, ha nem is töltötte még be őket
            Exit(true);
        }
        public static void Restart(bool savesettings)
        { //2014.09.01.
            BeforeExit(savesettings);
            Process.Start(((ProcessName.Contains("vshost")) ? ProcessName.Replace(".vshost", "") : ProcessName) + ".exe");
            Environment.Exit(0);
            MessageBox.Show("Hiba: Nem sikerült leállítani a programot.");
        }
    }

    static class EnumerableExtensions
    {
        public static bool HasSameElementsAs<T>(
            this IEnumerable<T> first,
            IEnumerable<T> second
        )
        {
            var firstMap = first
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
            var secondMap = second
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
            return
                firstMap.Keys.All(x =>
                    secondMap.Keys.Contains(x) && firstMap[x] == secondMap[x]
                ) &&
                secondMap.Keys.All(x =>
                    firstMap.Keys.Contains(x) && secondMap[x] == firstMap[x]
                );
        }

        public static bool Test(this object A, object B)
        {
            return object.ReferenceEquals(A, B);
        }

        /// 
        /// Clones an object by using the .
        /// 
        /// The object to clone.
        /// 
        /// The object to be cloned must be serializable.
        /// 
        public static object Clone(object obj)
        {
            using (MemoryStream buffer = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(buffer, obj);
                buffer.Position = 0;
                object temp = formatter.Deserialize(buffer);
                return temp;
            }
        }
    }
}
