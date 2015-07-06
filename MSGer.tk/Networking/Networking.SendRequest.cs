using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    partial class Networking
    {
        public enum RequestType
        { //2015.04.05.
            AddUser,
            AddCode,
            GetCodes,
            RemCode,
            ResetPass,
            Register,
            CheckForUpdates,
            GetList,
            SetState,
            IsPartner,
            KeepActive,
            CheckUser
        }
        public const string WebAddress = "http://msger.url.ph"; //TODO: HTTPS
        private static int Tries = 0;
        public static string SendRequest(RequestType action, string data, int remnum, bool loggedin) //2014.02.18.
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
                return "Fail: " + Language.Translate(Language.StringID.Error_No_Network);
            }
            HttpWebRequest httpWReq =
                (HttpWebRequest)WebRequest.Create(WebAddress + "/client.php"); //WebAddress: 2015.06.14.

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "action=" + action.ToString().ToLower(); //ToString().ToLower(): 2015.04.05.
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

            try
            {
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(datax, 0, datax.Length);
                }
            }
            catch (WebException e)
            { //2015.04.08.
                new ErrorHandler(ErrorType.ServerConnectError, e); //2015.06.04.
                return "Fail";
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
                    new ErrorHandler(ErrorType.ServerConnectError, e); //2015.06.04.
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
