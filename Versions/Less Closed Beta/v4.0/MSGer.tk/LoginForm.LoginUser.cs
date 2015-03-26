using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    partial class LoginForm
    {
        public static string UserText = ""; //2014.02.14.
        public static string PassText = "";
        public static string LButtonText = "";
        public static bool RegistLinkEn = true;
        public static bool Closeable = false;
        public static HttpWebRequest Request; //2014.03.27. - A megállitáshoz

        public void LoginUser()
        {
            //Állitson vissza minden változót, hogy újra belerakja az értekeket - 2014.02.28.
            UserText = "";
            PassText = "";
            RegistLinkEn = false; //2014.03.27.
            LButtonText = Language.Translate("button_cancel");
            this.Invoke(new MyDelegate(SetLoginValues));

            //HttpWebRequest httpWReq =
            //    (HttpWebRequest)WebRequest.Create("http://msger.tk/client.php");
#if LOCAL_SERVER
            HttpWebRequest httpWReq =
                (HttpWebRequest)WebRequest.Create("http://localhost/ChatWithWords/client.php");
#else
            HttpWebRequest httpWReq =
                (HttpWebRequest)WebRequest.Create("http://msger.url.ph/client.php");
#endif

            Request = httpWReq; //2014.03.27.

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "username=" + UserText;
            postData += "&password=" + CalculateMD5Hash(PassText).ToLower(); //ToLower: 2014.10.24. 1:22 - Most már a PHP-nak is titkosítania kell többek közt MD5-tel
            postData += "&key=cas1fe4a6feFEFEFE1616CE8099VFE1444cdasf48c1ase5dg";
            postData += "&port=" + Storage.Settings["port"]; //2014.08.29.
            //postData += "&isserver=" + Storage.Settings["isserver"]; //2014.09.26.
            /*postData += "&myip=" + Dns.GetHostEntry(Dns.GetHostName()).AddressList.Single(entry =>
                entry.AddressFamily == AddressFamily.InterNetwork
                    && (entry.ToString().Contains("192.168.0.") || entry.ToString().Contains("192.168.1.") || entry.ToString().Contains("10.0.0.") || entry.ToString().Contains("172.16.0.")) //Helyi IP-k
                ); //2014.11.15. - Pontok téve az IP-prefixek után, hogy pontos legyen az egyezés: 2014.12.22.
             */ //Nincs már szükség rá; IPv6
            byte[] data = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = data.Length;

            try
            {
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (WebException e)
            {
                if (e.Status != WebExceptionStatus.RequestCanceled)
                {
                    MessageBox.Show(Language.Translate("connecterror") + "\n" + e.Message, Language.Translate("error"));
                    this.Invoke(new MyDelegate(ResetAfterLogin));
                    return;
                }
                else
                {
                    return;
                }
            }

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)httpWReq.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Status != WebExceptionStatus.RequestCanceled)
                {
                    MessageBox.Show(Language.Translate("connecterror") + "\n" + e.Message, Language.Translate("error"));
                    this.Invoke(new MyDelegate(ResetAfterLogin));
                    return;
                }
                else
                {
                    return;
                }
            }

            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            try
            {
                if (responseString[0] == '<')
                {
                    this.Invoke(new MyDelegate(ResetAfterLogin));
                    MessageBox.Show(Language.Translate("error") + ":\n" + responseString);
                    return;
                }
                else
                    responseString = responseString.Remove(responseString.IndexOf('<'));
            }
            catch
            {
            }

            if (String.Compare(responseString, "Fail") == 0)
            {
                this.Invoke(new MyDelegate(ResetAfterLogin));
                MessageBox.Show(Language.Translate("error") + ": " + Language.Translate("login_badnamepass"), Language.Translate("error"));
            }
            else
            {


                //Elmenti az E-mail-t
                if (!Storage.Settings["email"].Contains(UserText))
                {
                    if (Storage.Settings["email"].Length != 0) //2014.07.08.
                        Storage.Settings["email"] += ",";
                    Storage.Settings["email"] += UserText;
                }
                //else - 2014.10.02. - Egyszer észrevettem a Google Code összehasonlítójával, hogy ez nem kéne ide
                Storage.Settings["lastusedemail"] = Storage.Settings["email"].Split(',').ToList<string>().IndexOf(UserText).ToString();

                /*
                 * respstr:
                 * 0: uid
                 * 1: username
                 * 2: myip
                 * 3: server ips
                 * 4: non-server ips
                 * 5: server same ips
                 * 6: non-server same ips
                 * 7: password
                 */
                string[] respstr = responseString.Split('ͦ');

                //if (respstr[3].Contains("Fail"))
                if(respstr.Any(entry=>entry.Contains("Fail"))) //2014.12.05.
                {
                    this.Invoke(new MyDelegate(ResetAfterLogin));
                    //MessageBox.Show(respstr[3]);
                    try
                    {
                        MessageBox.Show(respstr.Single(entry => entry.Contains("Fail"))); //2014.12.05.
                    }
                    catch { }
                    return;
                }
                //string[] entries = respstr[(int)LoginInfo.ServerIPs].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                string[] entries = respstr[(int)LoginInfo.IPs].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                IEnumerable<IPEndPoint> ips = entries.Select(entry => ((entry != ":") ? new IPEndPoint(IPAddress.Parse(entry.Split(':')[0]), Int32.Parse(entry.Split(':')[1])) : new IPEndPoint(IPAddress.Loopback, 0)));
                UserInfo.IPs = new HashSet<IPEndPoint>(ips); //2014.08.30.
                //UserInfo.IPs = new HashSet<IPEndPoint>(ips.Select(entry => new IPEndPoint(entry, true))); //2014.11.23.
                //CurrentUser.IP = IPAddress.Parse(respstr[(int)LoginInfo.MyIP]); //2014.10.24. - Most már csak ott lehet rá hivatkozni, felesleges eltárolni
                CurrentUser.IPs = new List<IPAddress>(respstr[(int)LoginInfo.MyIP].Split(new char[] { ';' }).Select(entry => IPAddress.Parse(entry)));

                /*entries = respstr[(int)LoginInfo.NonServerIPs].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                ips = entries.Select(entry => ((entry != ":") ? new IPEndPoint(IPAddress.Parse(entry.Split(':')[0]), Int32.Parse(entry.Split(':')[1])) : new IPEndPoint(IPAddress.Loopback, 0)));
                foreach (var item in ips)
                    UserInfo.IPs.Add(new IPEndPoint(item, false)); //2014.11.23.*/

                /*entries = respstr[(int)LoginInfo.ServerIPsOnNAT].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries); //2014.12.18.
                ips = entries.Select(entry => ((entry != ":") ? new IPEndPoint(IPAddress.Parse(entry.Split(':')[0]), Int32.Parse(entry.Split(':')[1])) : new IPEndPoint(IPAddress.Loopback, 0))); //2014.12.18.
                foreach (var item in ips) //2014.12.18.
                    UserInfo.IPs.Add(new IPEndPoint(item, true)); //2014.12.18.*/

                /*entries = respstr[(int)LoginInfo.NonServerIPsOnNat].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries); //2014.12.18.
                ips = entries.Select(entry => ((entry != ":") ? new IPEndPoint(IPAddress.Parse(entry.Split(':')[0]), Int32.Parse(entry.Split(':')[1])) : new IPEndPoint(IPAddress.Loopback, 0))); //2014.12.18.
                foreach (var item in ips) //2014.12.18.
                    UserInfo.IPs.Add(new IPEndPoint(item, false)); //2014.12.18.*/

                /*entries = respstr[(int)LoginInfo.IPs].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                ips = entries.Select(entry => ((entry != ":") ? new IPEndPoint(IPAddress.Parse(entry.Split(':')[0]), Int32.Parse(entry.Split(':')[1])) : new IPEndPoint(IPAddress.Loopback, 0)));
                f*oreach (var item in ips)
                    UserInfo.IPs.Add(item);*/

                //2014.09.19. - Bejelentkezés elküldése áthelyezve a MainForm-ba

                CurrentUser.UserID = Int32.Parse(respstr[(int)LoginInfo.UserID]); //2014.09.01. - Áthelyeztem, hogy addig ne higgye bejelentkezettnek, amíg el nem küldi a többieknek

                Storage.SaltKey = CalculateMD5Hash(PassText); //2014.08.07.
                Storage.FileName = respstr[(int)LoginInfo.UserID] + ".db"; //2014.09.01. - Felesleges számmá alakítani, majd vissza

                CurrentUser.UserName = UserText; //2014.09.01. - Ha semmit nem tud saját magáról, és más sem, de nem ismerőse saját magának, akkor az itt beállított felhasználónév érvényesül
                CurrentUser.Name = UserText; //2014.09.01.
                string ReceivedPass = respstr[(int)LoginInfo.Password]; //2014.10.24. 1:39
                LoginForm.UserCode = CalculateMD5Hash(ReceivedPass + " Some text because why not " + CurrentUser.UserID).ToLower();

                Closeable = true;
                this.Invoke(new MyDelegate(SetLoginValues));
            }
        }
    }
}
