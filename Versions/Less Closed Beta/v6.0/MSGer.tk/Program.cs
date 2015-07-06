//#define LOCAL_SERVER

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
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
        public static SettingsForm SettingsF;
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
            /*Application.Run(new FloatingChatIcon(null)); //2015.05.21.
            return;*/
            BeforeLogin.Create();
            Console.WriteLine("Starting MSGer.tk...");
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Load(); //2015.05.23.
            //throw new Exception(); //TMP
            //try
            //{
            //Application.Run(new MainForm()); //2014.11.21.
            Application.Run(MainF); //2015.05.23.
            //}
            /*catch (FileNotFoundException e)
            {
                MessageBox.Show("Egy fájl nem található.\nA fájl neve:\n" + e.FileName + "\nEnnél a műveletnél: " + e.TargetSite);

            }*/
        }

        public static readonly bool ShowFirstChangeExceptions = false;
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            /*if (MessageBox.Show("An unhandled error occured\n" + ((Exception)e.ExceptionObject).Message + "\nHere: " + ((Exception)e.ExceptionObject).TargetSite + ((e.IsTerminating) ? "\nThe program will exit(?)." : "") + "\n\nExit?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                Program.Exit(CurrentUser.UserID != 0); //2014.09.26. - Ha be van jelentkezve, elmenti a beállításokat - Csak mert így tán a legegyszerűbb*/
            new ErrorHandler(ErrorType.Unknown, e.ExceptionObject as Exception); //2015.05.04.
        }

        static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            if (ShowFirstChangeExceptions)
                MessageBox.Show("An error occured (a first change exception):\n" + e.Exception.Message + "\nHere: " + e.Exception.TargetSite);
        }

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0,
                                                      DateTimeKind.Utc);

        //public static DateTime UnixTimeToDateTime(string text)
        public static DateTime UnixTimeToDateTime(double seconds)
        {
            //double seconds = double.Parse(text, CultureInfo.InvariantCulture);
            //2014.04.25.
            DateTime time = Epoch.AddSeconds(seconds);
            time = DateTime.SpecifyKind(time, DateTimeKind.Utc);
            return time.ToLocalTime();
        }

        //public static string DateTimeToUnixTime(DateTime time) //2014.08.28.
        public static double DateTimeToUnixTime(DateTime time)
        {
            time = time.ToUniversalTime(); //2015.07.04.
            return ((time.Ticks - 621355968000000000) / 10000000);
        }

        //private static void BeforeExit(bool savesettings)
        private static void BeforeExit()
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
                        Storage.Settings[SettingType.WindowState] = "1";
                    else if (MainF.WindowState == FormWindowState.Minimized)
                        Storage.Settings[SettingType.WindowState] = "2";
                    else if (MainF.WindowState == FormWindowState.Normal)
                        Storage.Settings[SettingType.WindowState] = "3";
                    Storage.Save(true); //2014.08.07.
                }
            }
            /*if (savesettings) //2014.08.28.
                Storage.Save(false); //2014.08.08.*/
            if (Storage.IsLoaded)
                Storage.Save(false);
            if (Networking.ReceiverConnection != null)
                Networking.ReceiverConnection.Close();
            if (Networking.SenderConnection != null)
                Networking.SenderConnection.Close();
        }
        //public static void Exit(bool savesettings)
        public static void Exit() //2015.06.04.
        { //2014.04.12.
            //BeforeExit(savesettings);
            BeforeExit();
            Environment.Exit(0); //Hatásosabb
            MessageBox.Show("Hiba: Nem sikerült leállítani a programot.");
        }
        /*public static void Exit()
        { //2014.08.28. - Csak akkor ne mentse el a beállításokat, ha nem is töltötte még be őket
            Exit(true);
        }*/
        //public static void Restart(bool savesettings)
        public static void Restart()
        { //2014.09.01.
            //BeforeExit(savesettings);
            BeforeExit();
            Process.Start(((ProcessName.Contains("vshost")) ? ProcessName.Replace(".vshost", "") : ProcessName) + ".exe");
            Environment.Exit(0);
            MessageBox.Show("Hiba: Nem sikerült leállítani a programot.");
        }
        public static Thread MainThread = null;
        public static void Load()
        {
            BeforeLogin.SetText("Starting...");
            //Program.MainF = this;
            /*ScriptLoader script = new ScriptLoader("Test.cs");
            if (!script.Load())
                return;
            script.Unload();
            return;*/
            Thread.CurrentThread.Name = "Main Thread";

            BeforeLogin.SetText("Loading program settings...");
            Storage.Load(false); //Töltse be a nyelvet, legutóbb használt E-mail-t...

            BeforeLogin.SetText("Checking available ports...");
            //2014.09.04. - Amint lehet állítsa be a helyes IP-t, majd azt hagyja úgy, akármi történjék
            //TO!DO (2015.05.23.) - A port mindig 4510-től, vagy a beállított értéktől induljon
            //TO!DO (2015.05.23.) - Ténylegesen be lehessen állítani a portot...
            SetPort();

            //BeforeLogin.SetText("Loading languages...");
            //new Language();

            BeforeLogin.SetText("Loading packs...");
            PackManager.LoadAll(); //2015.05.16.

            MainThread = Thread.CurrentThread; //2015.05.23.
            MainF = new MainForm(); //2015.05.23.

            //BeforeLogin.SetText(Language.Translate(Language.StringID.BeforeLogin_LoadTextFormat));
            //2014.05.16.
            //new TextFormat();

            //BeforeLogin.SetText(Language.Translate(Language.StringID.BeforeLogin_LoadScripts)); //2015.04.05.
            //2015.04.05.
            //new ScriptLoader();

            BeforeLogin.SetText(Language.Translate(Language.StringID.BeforeLogin_CheckForUpdates));
            //2014.04.25.
            string response = Networking.SendRequest(Networking.RequestType.CheckForUpdates,
                Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""),
                0, false);
            //response = "lol"; //TMP
            if (response == "outofdate")
            {
                var res = MessageBox.Show(Language.Translate(Language.StringID.OutOfDate), Language.Translate(Language.StringID.OutOfDate_Caption), MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                { //2014.12.13.
                    //Process.Start("Updater.exe", "\"" + Language.Translate(Language.StringID.Updater) + "\" \"" + Language.Translate(Language.StringID.Updater_Info) + "\"");
                    Process.Start("Updater.exe", "\"" + Language.Translate(Language.StringID.Updater) + "\""); //2015.06.14.
                    //Program.Exit(false);
                    Program.Exit();
                }
            }
            else if (response != "fine")
                //MessageBox.Show(Language.Translate(Language.StringID.Error) + ": " + response);
                new ErrorHandler(ErrorType.ServerError, new Exception(response)); //2015.06.04.

            BeforeLogin.SetText(Language.Translate(Language.StringID.BeforeLogin_LoginForm));
            //try
            //{
            //LoginDialog = new LoginForm();
            var LoginDialog = new LoginForm(); //2015.05.23.
            BeforeLogin.Destroy();
            LoginDialog.ShowDialog();
            /*}
            catch (Exception ex)
            {
                ErrorHandling.FormError(LoginDialog, ex);
            }*/
            //Nézzük, sikerült-e
            if (CurrentUser.UserID == 0)
                return; //2014.09.06.

            // Start the thread
            MainForm.PartnerListUpdateThread.Start(); //Áthelyezve: 2015.06.30.

            Storage.Load(true); //2014.08.07.
        }

        /// <summary>
        /// Beállítja a portot a Storage.Settings-ben található értéktől kezdve a legelső elérhető portra
        /// </summary>
        public static void SetPort()
        {
            if (Storage.Settings[SettingType.Port] == "")
                Storage.Settings[SettingType.Port] = "4510"; //2015.05.21.
            CurrentUser.Port = int.Parse(Storage.Settings[SettingType.Port]); //2015.05.24.
            while (true)
            {
                /*if (IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners().Select(entry => entry.Port).Contains(Int32.Parse(Storage.Settings[SettingType.Port])))
                    Storage.Settings[SettingType.Port] = (Int32.Parse(Storage.Settings[SettingType.Port]) + 1).ToString();*/
                if (IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners().Select(entry => entry.Port).Contains(CurrentUser.Port)) //2015.05.24.
                    CurrentUser.Port++; //2015.05.24.
                else
                    break;
            }
            //Networking.ReceiverConnection = new UdpClient(Int32.Parse(Storage.Settings[SettingType.Port]), AddressFamily.InterNetworkV6); //2014.09.04.
            Networking.ReceiverConnection = new UdpClient(CurrentUser.Port, AddressFamily.InterNetworkV6); //2015.05.24.
            Networking.SenderConnection.AllowNatTraversal(true); //2014.09.04.
        }

        public static Image LoadImageFromFile(string path)
        { //2015.06.06.
            //TODO: Ellenőrizni és használni mindenhol, ahol kell - http://stackoverflow.com/questions/12680618/exception-parameter-is-not-valid-on-passing-new-image-to-picturebox
            using (Image sourceImg = Image.FromFile(path))
            {
                Image clonedImg = new Bitmap(sourceImg.Width, sourceImg.Height, PixelFormat.Format32bppArgb);
                using (var copy = Graphics.FromImage(clonedImg))
                {
                    copy.DrawImage(sourceImg, 0, 0);
                }
                return clonedImg;
            }
        }
    }

    static class Extensions
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
        public static object Clone(this object obj)
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
        public static IEnumerable<Control> GetAll(this Control control, Type type = null)
        { //2015.02.26.
            var controls = control.Controls.Cast<Control>();
            var ret = controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls);
            return (type == null) ? ret : ret.Where(c => c.GetType() == type);
        }
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
