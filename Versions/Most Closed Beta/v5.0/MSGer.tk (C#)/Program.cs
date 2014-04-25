using System;
using System.Collections.Generic;
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
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new MainForm());
            }
            catch
            {
            }
        }
    }
    static class Networking
    {
        public static string SendRequest(string action, string data, int remnum) //2014.02.18.
        {
            HttpWebRequest httpWReq =
                (HttpWebRequest)WebRequest.Create("http://msger.tk/client.php");

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "uid=" + CurrentUser.UserID;
            postData += "&action=" + action;
            postData += "&key=cas1fe4a6feFEFEFE1616CE8099VFE1444cdasf48c1ase5dg";
            postData += "&code=" + LoginForm.UserCode; //2014.02.13.
            postData += "&data=" + data; //2014.02.13.
            byte[] datax = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = datax.Length;

            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(datax, 0, datax.Length);
            }

            //Lista betöltése folyamatban...

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            try
            {
                responseString = responseString.Remove(responseString.IndexOf('<'));
            }
            catch
            {
            }

            responseString = responseString.Remove(responseString.Length - remnum); //A -1 nem elég

            return responseString;
        }
    }
}
