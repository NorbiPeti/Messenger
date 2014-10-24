//#define LOCAL_SERVER

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Configuration;
using System.Threading;

namespace MSGer.tk
{
    public partial class LoginForm : Form
    {
        public static string UserCode = "";
        public static Thread LThread;
        private void LoginForm_Load(object sender, EventArgs e)
        {
            List<string> tmp; //E-mail - 2014.04.02.
            if (Storage.Settings["email"].Length != 0)
                tmp = Storage.Settings["email"].Split(',').ToList<string>();
            else tmp = new List<string>();
            tmp.Add("");
            textBox1.Text = tmp[Int32.Parse(Storage.Settings["lastusedemail"])]; //2014.07.08.
            textBox1.AutoCompleteCustomSource.AddRange(tmp.ToArray());
            this.Activate();
            this.Activate();
        }
        public LoginForm()
        {
            InitializeComponent();
            this.Text = Language.Translate("login");
            label1.Text = Language.Translate("login");
            label3.Text = Language.Translate("login_password");
            button1.Text = Language.Translate("login");
            linkLabel1.Text = Language.Translate("registration");
            linkLabel2.Text = Language.Translate("forgotpassword");
            textBox3.Text = "";
            List<string> lines = new List<string>();
            lines.Add(Language.Translate("login_desc1"));
            lines.Add("");
            lines.Add(Language.Translate("login_desc2"));
            textBox3.Lines = lines.ToArray();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == Language.Translate("button_cancel"))
            {
                ResetAfterLogin(true);
            }
            else
            {
                try
                {
                    if (LThread.IsAlive)
                    {
                        //2014.03.27. - Ne csináljon semmit - Elvégre ilyen nem fordulhat elő sokáig most már
                        return;
                    }
                }
                catch
                {
                }

                //Ellenőrizzen le néhány dolgot helyileg - 2014.04.28.
                if (textBox2.Text.Length == 0)
                    return;

                // Create the thread object, passing in the Alpha.Beta method
                // via a ThreadStart delegate. This does not start the thread.
                LThread = new Thread(new ThreadStart(LoginUser));
                LThread.Name = "Login Thread";

                // Start the thread
                LThread.Start();

                // Spin for a while waiting for the started thread to become
                // alive:
                while (!LThread.IsAlive) ;
            }
        }
        public delegate int MyDelegate();
        public int SetLoginValues()
        {
            if (UserText.Length == 0)
                UserText = textBox1.Text;
            else
                textBox1.Text = UserText;

            if (PassText.Length == 0)
                PassText = textBox2.Text;
            else
                textBox2.Text = PassText;

            button1.Text = LButtonText;
            linkLabel1.Enabled = RegistLinkEn; //2014.03.27.
            if (Closeable)
            {
                Closeable = false;
                Dispose(); //2014.04.04.
            }
            textBox1.Enabled = false; //2014.09.01.
            textBox2.Enabled = false; //2014.09.01.
            return 0;
        }
        public int ResetAfterLogin(bool stoplogint)
        {
            button1.Enabled = false;
            Request.Abort();
            if (stoplogint) //2014.09.01.
                LThread.Abort(); //2014.09.01.
            button1.Text = Language.Translate("login");
            button1.Enabled = true;
            linkLabel1.Enabled = true;
            textBox1.Enabled = true; //2014.09.01.
            textBox2.Enabled = true; //2014.09.01.
            return 0;
        }
        public int ResetAfterLogin()
        { //2014.09.01.
            return ResetAfterLogin(false); //Ha a thread hívja meg, ne állítsa le a thread-et
        }
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
            postData += "&isserver=" + Storage.Settings["isserver"]; //2014.09.26.
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
                string[] respstr = responseString.Split('ͦ');

                if (respstr[3].Contains("Fail"))
                {
                    this.Invoke(new MyDelegate(ResetAfterLogin));
                    MessageBox.Show(respstr[3]);
                    return;
                }
                string[] entries = respstr[3].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                IEnumerable<IPEndPoint> ips = entries.Select(entry => ((entry != ":") ? new IPEndPoint(IPAddress.Parse(entry.Split(':')[0]), Int32.Parse(entry.Split(':')[1])) : new IPEndPoint(IPAddress.Loopback, 0)));
                UserInfo.IPs = new HashSet<IPEndPoint>(ips); //2014.08.30.
                CurrentUser.IP = IPAddress.Parse(respstr[2]); //2014.10.24. - Most már csak ott lehet rá hivatkozni, felesleges eltárolni
                //2014.09.19. - Bejelentkezés elküldése áthelyezve a MainForm-ba

                CurrentUser.UserID = Int32.Parse(respstr[0]); //2014.09.01. - Áthelyeztem, hogy addig ne higgye bejelentkezettnek, amíg el nem küldi a többieknek

                Storage.SaltKey = CalculateMD5Hash(PassText); //2014.08.07.
                Storage.FileName = respstr[0] + ".db"; //2014.09.01. - Felesleges számmá alakítani, majd vissza

                CurrentUser.UserName = UserText; //2014.09.01. - Ha semmit nem tud saját magáról, és más sem, de nem ismerőse saját magának, akkor az itt beállított felhasználónév érvényesül
                CurrentUser.Name = UserText; //2014.09.01.
                string ReceivedPass = respstr[4]; //2014.10.24. 1:39
                LoginForm.UserCode = CalculateMD5Hash(ReceivedPass + " Some text because why not " + CurrentUser.UserID).ToLower();

                Closeable = true;
                this.Invoke(new MyDelegate(SetLoginValues));
            }
        }

        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private void RegistrateLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            (new LoginForm_RegistrationForm()).ShowDialog();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (LThread != null && LThread.IsAlive)
            {
                LThread.Abort(); //2014.03.27. - Na vajon kell-e más
                Request.Abort(); //2014.03.27. - Kell... Ez
            }
            if (CurrentUser.UserID == 0)
                Program.Exit();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Nincs megadva felhasználónév."); //Translate!
                return;
            }
            if (MessageBox.Show("Új jelszót kérsz a megadott névhez?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string ret = Networking.SendRequest("resetpass", textBox1.Text, 0, false);
                if (ret == "nouser")
                    MessageBox.Show("A megadott felhasználó nem létezik.");
                else if (ret == "already")
                    MessageBox.Show("A link MÁR el lett küldve az E-mail címedre.");
                else if (ret == "sent")
                    MessageBox.Show("A link elküldve az E-mail címedre.");
                else if (ret.Contains("notsent"))
                    MessageBox.Show("A link NEM lett elküldve az E-mail címedre.\nHiba: " + ret.Remove(ret.IndexOf("notsent"), "notsent".Length + 1));
                else
                    MessageBox.Show("Ismeretlen hiba:\n" + ret);
            }
        }
    }
}
