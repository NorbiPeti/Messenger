//#define LOCAL_SERVER

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SzNPProjects;

namespace MSGer.tk
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static MainForm MainF;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Console.WriteLine("Starting MSGer.tk...");
            try
            {
                MainF = new MainForm();
                Application.Run(MainF);
            }
            catch(FileNotFoundException e)
            {
                MessageBox.Show("Egy fájl nem található.\nA fájl neve:\n" + e.FileName+"\nEnnél a műveletnél: "+e.TargetSite);

            }
            catch(Exception e)
            {
                ErrorHandling.FormError(MainF, e);
            }
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

        public static void Exit()
        { //2014.04.12.
            if (MainF != null)
            {
                MainF.Hide();
                MainF.toolStripMenuItem4.Enabled = false; //2014.04.12.
                MainF.toolStripMenuItem8.Enabled = false; //2014.04.12.
                if (CurrentUser.UserID != 0) //2014.04.18.
                {
                    MainF.SetOnlineState(null, null); //2014.04.12.)
                    if (MainF.WindowState == FormWindowState.Maximized) //2014.04.18.
                        Settings.Default.windowstate = 1;
                    else if (MainF.WindowState == FormWindowState.Minimized)
                        Settings.Default.windowstate = 2;
                    else if (MainF.WindowState == FormWindowState.Normal)
                        Settings.Default.windowstate = 3;
                    Settings.Default.Save();
                }
            }
            //CurrentUser.UserID = 0; - Felesleges, néha megáll itt, és úgy hajt végre egy bejelentkezést igénylő lekérést
            //Application.Exit();
            //Application.ExitThread();
            Environment.Exit(0); //Hatásosabb
            MessageBox.Show("Hiba: Nem sikerült leállítani a programot.");
        }
    }
    static class Networking
    {
        private static int Tries = 0;
        public static string SendRequest(string action, string data, int remnum, bool loggedin) //2014.02.18.
        {
            #if LOCAL_SERVER //2014.07.07. 22:00
            HttpWebRequest httpWReq =
                (HttpWebRequest)WebRequest.Create("http://localhost/ChatWithWords/client.php");
            #else
            HttpWebRequest httpWReq =
                (HttpWebRequest)WebRequest.Create("http://msger.url.ph/client.php");
            #endif

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "action=" + action;
            if (loggedin) //2014.03.14.
                postData += "&uid=" + CurrentUser.UserID;
            postData += "&key=cas1fe4a6feFEFEFE1616CE8099VFE1444cdasf48c1ase5dg";
            if (loggedin) //2014.03.14.
                postData += "&code=" + LoginForm.UserCode; //2014.02.13.
            postData += "&data=" + Uri.EscapeUriString(data); //2014.02.13.
            byte[] datax = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = datax.Length;

            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(datax, 0, datax.Length);
            }

            //Lista betöltése folyamatban...

            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse)httpWReq.GetResponse();
            }
            catch (Exception e)
            {
                if (Tries > 10)
                {
                    MessageBox.Show(Language.GetCuurentLanguage().Strings["error_network"] + ":\n" + e.Message + "\n\n" + Language.GetCuurentLanguage().Strings["error_network2"], Language.GetCuurentLanguage().Strings["error"]); //2014.04.25.
                    Tries = 0;
                }
                Tries++;
                return SendRequest(action, data, remnum, loggedin); //Újrapróbálkozik
            }
            string responseString;
            responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }
    }
}
