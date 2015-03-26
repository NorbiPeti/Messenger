using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    partial class Networking
    {

        private static int Tries = 0;
        public static string SendRequest(string action, string data, int remnum, bool loggedin) //2014.02.18.
        {
            if (!IsNetworkAvailable()) //2014.11.15.
            {
                if (Program.MainF != null)
                { //2014.11.21.
                    if (Program.MainF.InvokeRequired) //2014.11.21.
                    { //2014.11.21.
                        Program.MainF.Invoke(new Action<MainForm.StatType, int>(Program.MainF.UpdateStats), //2014.11.21.
                            MainForm.StatType.MainServer, 0); //2014.11.21.
                    }
                    else //2014.11.21.
                    { //2014.11.21.
                        Program.MainF.UpdateStats(MainForm.StatType.MainServer, 0); //2014.11.21.
                    }
                }
                return "Fail: " + Language.Translate("error_no_network");
            }

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

            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse)httpWReq.GetResponse();
            }
            catch (Exception e)
            {
                if (Tries > 10)
                {
                    MessageBox.Show(Language.Translate("error_network") + ":\n" + e.Message + "\n\n" + Language.Translate("error_network2"), Language.Translate("error")); //2014.04.25.
                    Tries = 0;
                }
                if (Program.MainF != null)
                { //2014.11.21.
                    if (Program.MainF.InvokeRequired) //2014.11.21.
                    { //2014.11.21.
                        Program.MainF.Invoke(new Action<MainForm.StatType, int>(Program.MainF.UpdateStats), //2014.11.21.
                            MainForm.StatType.MainServer, 1); //2014.11.21.
                    }
                    else //2014.11.21.
                    { //2014.11.21.
                        Program.MainF.UpdateStats(MainForm.StatType.MainServer, 1); //2014.11.21.
                    }
                }
                Tries++;
                return SendRequest(action, data, remnum, loggedin); //Újrapróbálkozik
            }
            string responseString;
            responseString = Uri.UnescapeDataString(new StreamReader(response.GetResponseStream()).ReadToEnd());
            if (Program.MainF != null)
            { //2014.11.21.
                if (Program.MainF.InvokeRequired) //2014.11.21.
                { //2014.11.21.
                    Program.MainF.Invoke(new Action<MainForm.StatType, int>(Program.MainF.UpdateStats), //2014.11.21.
                        MainForm.StatType.MainServer, 2); //2014.11.21.
                }
                else //2014.11.21.
                { //2014.11.21.
                    Program.MainF.UpdateStats(MainForm.StatType.MainServer, 2); //2014.11.21.
                }
            }
            return responseString;
        }

        private static bool IsNetworkAvailable()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface nic in nics)
            {
                if (
                    (nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel) &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
