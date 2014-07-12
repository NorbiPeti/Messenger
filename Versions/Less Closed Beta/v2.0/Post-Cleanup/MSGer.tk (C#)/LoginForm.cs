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
        public static string UserCode="";
        public static Thread LThread;
        private void LoginForm_Load(object sender, EventArgs e)
        {
            List<string> tmp; //E-mail - 2014.04.02.
            if (Settings.Default.email.Length != 0)
                tmp = Settings.Default.email.Split(',').ToList<string>();
            else tmp = new List<string>();
            //textBox1.Text = tmp[0];
            //textBox1.Text = tmp[tmp.Count - 1]; //2014.07.08.
            textBox1.Text = tmp[Settings.Default.lastusedemail]; //2014.07.08.
            textBox1.AutoCompleteCustomSource.AddRange(tmp.ToArray());
        }
        public LoginForm()
        {
            InitializeComponent();
            this.Text = Language.GetCuurentLanguage().Strings["login"];
            label1.Text = Language.GetCuurentLanguage().Strings["login"];
            label3.Text = Language.GetCuurentLanguage().Strings["login_password"];
            button1.Text = Language.GetCuurentLanguage().Strings["login"];
            linkLabel1.Text = Language.GetCuurentLanguage().Strings["registration"];
            textBox3.Text = "";
            List<string> lines = new List<string>();
            lines.Add(Language.GetCuurentLanguage().Strings["login_desc1"]);
            lines.Add("");
            lines.Add(Language.GetCuurentLanguage().Strings["login_desc2"]);
            textBox3.Lines = lines.ToArray();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == Language.GetCuurentLanguage().Strings["button_cancel"])
            {
                ResetAfterLogin();
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

                // Put the Main thread to sleep for 1 millisecond to allow oThread
                // to do some work:
                Thread.Sleep(1);
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
            return 0;
        }
        public int ResetAfterLogin()
        {
            Request.Abort();
            button1.Enabled = false;
            button1.Text = Language.GetCuurentLanguage().Strings["login"];
            button1.Enabled = true;
            linkLabel1.Enabled = true;
            return 0;
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
            LButtonText = Language.GetCuurentLanguage().Strings["button_cancel"];
            this.Invoke(new MyDelegate(SetLoginValues));

            //HttpWebRequest httpWReq =
            //    (HttpWebRequest)WebRequest.Create("http://msger.tk/client.php");
            HttpWebRequest httpWReq =
                (HttpWebRequest)WebRequest.Create("http://msger.url.ph/client.php");

            Request = httpWReq; //2014.03.27.

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "username=" + UserText;
            postData += "&password=" + CalculateMD5Hash(PassText);
            postData += "&key=cas1fe4a6feFEFEFE1616CE8099VFE1444cdasf48c1ase5dg";
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
                    MessageBox.Show(Language.GetCuurentLanguage().Strings["connecterror"] + "\n" + e.Message, Language.GetCuurentLanguage().Strings["error"]);
                    this.Invoke(new MyDelegate(ResetAfterLogin));
                    return;
                }
                else
                {
                    return;
                }
            }

            //Bejelentkezés folyamatban...
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)httpWReq.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Status != WebExceptionStatus.RequestCanceled)
                {
                    MessageBox.Show(Language.GetCuurentLanguage().Strings["connecterror"] + "\n" + e.Message, Language.GetCuurentLanguage().Strings["error"]);
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
                    MessageBox.Show(Language.GetCuurentLanguage().Strings["error"] + ":\n" + responseString);
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
                MessageBox.Show(Language.GetCuurentLanguage().Strings["error"] + ": " + Language.GetCuurentLanguage().Strings["login_badnamepass"], Language.GetCuurentLanguage().Strings["error"]);
            }
            else
            {


                //Elmenti az E-mail-t
                if (!Settings.Default.email.Contains(UserText))
                {
                    if (Settings.Default.email.Length != 0) //2014.07.08.
                        Settings.Default.email += ",";
                    Settings.Default.email += UserText;
                }
                else
                    Settings.Default.lastusedemail = Settings.Default.email.Split(',').ToList<string>().IndexOf(UserText);
                Settings.Default.Save();
                //Bejelentkezés
                string[] respstr = responseString.Split('ͦ');
                CurrentUser.UserID = Convert.ToInt32(respstr[0]); //Régebben ezt találtam, most meg az Int32.Parse-t... (2014.04.02.)
                CurrentUser.Name = respstr[1]; //2014.04.04.
                LoginForm.UserCode = CalculateMD5Hash(CalculateMD5Hash(PassText) + " Some text because why not " + CurrentUser.UserID).ToLower();
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
    }
}
