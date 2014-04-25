﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            try
            {
                Application.Run(MainF = new MainForm());
            }
            catch(FileNotFoundException e)
            {
                //MessageBox.Show(e.GetType().ToString());
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
            return Epoch.AddSeconds(seconds);
        }
    }
    static class Networking
    {
        //public static HttpWebRequest Request; //2014.03.27. - A megállitáshoz
        public static string SendRequest(string action, string data, int remnum, bool loggedin) //2014.02.18.
        {
            //HttpWebRequest httpWReq =
            //    (HttpWebRequest)WebRequest.Create("http://msger.tk/client.php");
            HttpWebRequest httpWReq =
                (HttpWebRequest)WebRequest.Create("http://msger.url.ph/client.php");

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

            //MessageBox.Show(postData);

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
                MessageBox.Show("Hiba az ismerőslista frissítésekor:\n", e.Message);
                //return null;
                return SendRequest(action, data, remnum, loggedin); //Újrapróbálkozik
            }
            string responseString;
            responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            /*try
            {
                responseString = responseString.Remove(responseString.IndexOf('<'));
            }
            catch
            {
            }*/

            //if (remnum>0 && responseString.Length-remnum>0) //Felt. 2014.03.14.
            //    responseString = responseString.Remove(responseString.Length - remnum); //A -1 nem elég

            return responseString;
        }
    }
    class ThreadSetVar
    { //2014.04.11.
        private Control SetVarTarget;
        private string SetVarValue;
        private delegate void SetVarDelegate();
        public ThreadSetVar(Control variable, object value, Control sender)
        {
            if (value is String)
            {
                SetVarTarget = variable;
                SetVarValue = (string)value;
                sender.Invoke(new SetVarDelegate(IntThreadSetVar));
            }
        }
        private void IntThreadSetVar()
        { //Belső meghívás
            if (SetVarValue is String)
            {
                SetVarTarget.Text = (string)SetVarValue;
            }
        }
    }
}
