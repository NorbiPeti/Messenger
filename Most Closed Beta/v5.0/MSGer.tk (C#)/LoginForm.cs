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
        public LoginForm()
        {
            InitializeComponent();
            Thread.CurrentThread.Name = "Main Thread";
                        
            textBox1.Text = Settings.Default.email;
            if (textBox1.TextLength != 0)
            {
                textBox2.Focus();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(button1.Text=="Mégse")
            {
                ResetAfterLogin();
            }
            else
            {
                try
                {
                    if (LThread.IsAlive)
                    {
                        MessageBox.Show("Még fut a bejelentkező folyamat. Próbáld újra kb. 10 másodperc múlva.\nEzt a hibát általában szerverhiba okozza. Ilyenkor hiába állitod le a bejelentkezést, az tovább próbálkozik.", "Hiba");
                        return;
                    }
                }
                catch
                {
                }
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
            if (Closeable)
            {
                Closeable = false;
                Close();
            }
            return 0;
        }
        public int ResetAfterLogin()
        {
            StopLogin = true;
            button1.Enabled = false;
            /*int x = 0;
            while (LThread.IsAlive)
            {
                if (x == 5000)
                    LThread.Abort();
                else
                    x++;
            }*/
            //StopLogin = false;
            button1.Text = "Bejelentkezés";
            button1.Enabled = true;
            return 0;
        }
        public static string UserText = ""; //2014.02.14.
        public static string PassText = "";
        public static string LButtonText = "";
        public static bool Closeable = false;
        public static bool StopLogin = false;

        public void LoginUser()
        {
            //Állitson vissza minden változót, hogy újra belerakja az értekeket - 2014.02.28.
            UserText = "";
            PassText = "";
            //
            LButtonText = "Mégse";
            this.Invoke(new MyDelegate(SetLoginValues));
            
            HttpWebRequest httpWReq =
                (HttpWebRequest)WebRequest.Create("http://msger.tk/client.php");

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "username=" + UserText;
            postData += "&password=" + CalculateMD5Hash(PassText);
            postData += "&key=cas1fe4a6feFEFEFE1616CE8099VFE1444cdasf48c1ase5dg";
            byte[] data = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = data.Length;

            if (StopLogin)
            {
                StopLogin = false;
                return;
            }
            try
            {
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            catch
            {
                if (!StopLogin)
                {
                    MessageBox.Show("Nem sikerült csatlakozni a szerverhez.", "Hiba");
                    this.Invoke(new MyDelegate(ResetAfterLogin));
                }
            }
            if (StopLogin)
            {
                StopLogin = false;
                return;
            }

            //Bejelentkezés folyamatban...
            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
            if (StopLogin)
            {
                StopLogin = false;
                return;
            }
            
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            try
            {
                responseString = responseString.Remove(responseString.IndexOf('<'));
            }
            catch
            {
            }

            if (String.Compare(responseString, "Fail") == 0)
            {
                LButtonText = "Bejelentkezés";
                this.Invoke(new MyDelegate(SetLoginValues));
                MessageBox.Show("Hiba: Helytelen felhasználónév, vagy jelszó.", "Hiba");
            }
            else
            {
                //Elmenti az E-mail-t
                Settings.Default.email = UserText;
                Settings.Default.Save();
                //Bejelentkezés
                CurrentUser.UserID = Convert.ToInt32(responseString);
                LoginForm.UserCode = CalculateMD5Hash(CalculateMD5Hash(PassText).ToLower() + " Some text because why not " + CurrentUser.UserID).ToLower();
                Closeable = true;
                this.Invoke(new MyDelegate(SetLoginValues));
            }
        }
        public string CalculateMD5Hash(string input)
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
    }
}
