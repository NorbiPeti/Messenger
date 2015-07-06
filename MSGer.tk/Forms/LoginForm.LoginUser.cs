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
        public static HttpWebRequest Request; //2014.03.27. - A megállitáshoz

        public void LoginUser()
        {
            string UserText = "";
            string PassText = "";
            this.Invoke(new Action(delegate
            { //2015.04.03.
                UserText = textBox1.Text;
                PassText = textBox2.Text;
                linkLabel1.Enabled = false;
                button1.Text = Language.Translate(Language.StringID.Button_Cancel);
            }));
            HttpWebRequest httpWReq =
                (HttpWebRequest)WebRequest.Create("http://msger.url.ph/client.php");

            Request = httpWReq; //2014.03.27.

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "username=" + UserText;
            postData += "&password=" + CalculateMD5Hash(PassText).ToLower(); //ToLower: 2014.10.24. 1:22 - Most már a PHP-nak is titkosítania kell többek közt MD5-tel
            postData += "&key=cas1fe4a6feFEFEFE1616CE8099VFE1444cdasf48c1ase5dg";
            //postData += "&port=" + Storage.Settings[SettingType.Port]; //2014.08.29.
            postData += "&port=" + CurrentUser.Port; //2015.05.24.
            List<IPAddress> myips = new List<IPAddress>();
            do
            { //Az ips már deklarálva lesz később; megváltoztattam myips-re ezt
                foreach (var ipaddr in Dns.GetHostAddresses(Dns.GetHostName()))
                    if (ipaddr.AddressFamily == AddressFamily.InterNetworkV6)
                        myips.Add(ipaddr);
                string ipstr = "";
                foreach (var ip in myips)
                {
                    ipstr += ip + ";";
                }
                postData += "&ip=" + Uri.EscapeUriString(ipstr);
            } while (false);
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
                    new ErrorHandler(ErrorType.ServerConnectError, e); //2015.06.04.
                    ResetAfterLogin(false); //2015.04.03.
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
                    new ErrorHandler(ErrorType.ServerConnectError, e); //2015.06.04.
                    ResetAfterLogin(false); //2015.04.03.
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
                    ResetAfterLogin(false); //2015.04.03.
                    new ErrorHandler(ErrorType.ServerConnectError, new Exception(responseString));
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
                ResetAfterLogin(false); //2015.04.03.
                MessageBox.Show(Language.Translate(Language.StringID.Error) + ": " + Language.Translate(Language.StringID.Login_BadNamePass), Language.Translate(Language.StringID.Error));
            }
            else if (responseString.Contains("Fail"))
            { //2015.04.03.
                ResetAfterLogin(false);
                new ErrorHandler(ErrorType.ServerConnectError, new Exception(responseString)); //2015.06.04.
            }
            else
            {
                //Elmenti az E-mail-t
                if (!Storage.Settings[SettingType.Email].Contains(UserText))
                {
                    if (Storage.Settings[SettingType.Email].Length != 0) //2014.07.08.
                        Storage.Settings[SettingType.Email] += ",";
                    Storage.Settings[SettingType.Email] += UserText;
                }
                Storage.Settings[SettingType.LastUsedEmail] = Storage.Settings[SettingType.Email].Split(',').ToList<string>().IndexOf(UserText).ToString();
                string[] respstr = responseString.Split('ͦ');

                if (respstr.Any(entry => entry.Contains("Fail"))) //2014.12.05.
                {
                    //this.Invoke(new MyDelegate(ResetAfterLogin));
                    ResetAfterLogin(false); //2015.04.03.
                    /*try
                    {
                        MessageBox.Show(respstr.Single(entry => entry.Contains("Fail"))); //2014.12.05.
                    }
                    catch { }*/
                    MessageBox.Show(respstr.FirstOrDefault(entry => entry.Contains("Fail"))); //2014.12.05. - Single-->FirstOrDefault: 2015.06.06.
                    return;
                }
                string[] entries = respstr[(int)LoginInfo.IPs].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                IEnumerable<IPEndPoint> ips = entries.Select(entry => ((entry != ",") ? new IPEndPoint(IPAddress.Parse(entry.Split(',')[0]), Int32.Parse(entry.Split(',')[1])) : new IPEndPoint(IPAddress.Loopback, 0)));
                UserInfo.IPs = new HashSet<IPEndPoint>(ips); //2014.08.30.
                CurrentUser.IPs = myips;

                CurrentUser.UserID = Int32.Parse(respstr[(int)LoginInfo.UserID]); //2014.09.01. - Áthelyeztem, hogy addig ne higgye bejelentkezettnek, amíg el nem küldi a többieknek

                Storage.SaltKey = CalculateMD5Hash(PassText); //2014.08.07.
                Storage.FileName = respstr[(int)LoginInfo.UserID] + ".db"; //2014.09.01. - Felesleges számmá alakítani, majd vissza

                CurrentUser.UserName = UserText; //2014.09.01. - Ha semmit nem tud saját magáról, és más sem, de nem ismerőse saját magának, akkor az itt beállított felhasználónév érvényesül
                CurrentUser.Name = UserText; //2014.09.01.
                string ReceivedPass = respstr[(int)LoginInfo.Password]; //2014.10.24. 1:39
                LoginForm.UserCode = CalculateMD5Hash(ReceivedPass + " Some text because why not " + CurrentUser.UserID).ToLower();

                this.Invoke(new Action(delegate
                {
                    this.Dispose();
                }));
            }
        }
    }
}
