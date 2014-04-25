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
        public static string UserCode=""; //2014.02.13.
        public static Thread LThread; //2014.02.14.
        public LoginForm()
        {
            InitializeComponent();
            Thread.CurrentThread.Name = "Main Thread";

            //LoginThread LoginT = new LoginThread(); //2014.02.14. - Csak szivás külön class-ba rakni
                        
            textBox1.Text = Settings.Default.email;
//            MessageBox.Show("Text: " + textBox1.Text);
//            MessageBox.Show("Length: " + textBox1.TextLength);
            if (textBox1.TextLength != 0)
            {
//                MessageBox.Show("A hossz nem nulla");
                textBox2.Focus();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //Give required info to the LoginThread - 2014.02.13. - http://msdn.microsoft.com/en-us/library/aa645740(v=vs.71).aspx
            /*LoginThread.LoginButton = button1;        - In that tutorial you create a new class... It's not necessary
            LoginThread.UserText = textBox1;
            LoginThread.PassText = textBox2;*/

            if(button1.Text=="Mégse")
            {
                StopLogin = true;
                button1.Enabled = false;
                int x = 0;
                while (LThread.IsAlive)
                {
                    if (x == 5000)
                        LThread.Abort();
                    else
                        x++;
                }
                StopLogin = false;
                button1.Text = "Bejelentkezés";
                button1.Enabled = true;
            }
            else
            {
                // Create the thread object, passing in the Alpha.Beta method
                // via a ThreadStart delegate. This does not start the thread.
                LThread = new Thread(new ThreadStart(LoginUser)); //2014.02.14. - LoginT.LoginUser
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
            //button1.Enabled = LButtonEnabled;
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
                //Dispose();
            }
            return 0;
        }
    /*}
    public class LoginThread - 2014.02.14.
    {*/
        /*public static Button LoginButton;
        public static TextBox UserText;
        public static TextBox PassText;*/
        //public static bool LButtonEnabled = true; //2014.02.14.
        public static string UserText = ""; //2014.02.14.
        public static string PassText = "";
        public static string LButtonText = "";
        public static bool Closeable = false;
        public static bool StopLogin = false;

        //private EventWaitHandle wh = new AutoResetEvent(false);

        public void LoginUser() //2014.02.13. - Előtte a button1_Click-ben volt
        {
            //LButtonEnabled = false;
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
                return;

            try //2014.02.15. 0:46 - Ha itt áll meg, hibát irhat
            {
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            catch
            {
            }
            if (StopLogin)
                return;

            //Bejelentkezés folyamatban...

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
            if (StopLogin)
                return;
            
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            try
            {
                responseString = responseString.Remove(responseString.IndexOf('<'));
            }
            catch
            {
            }
            if (StopLogin)
                return;

            if (String.Compare(responseString, "Fail") == 0)
            {
                //button1.Enabled = true;
                LButtonText = "Bejelentkezés";
                this.Invoke(new MyDelegate(SetLoginValues));
                MessageBox.Show("Hiba: Helytelen felhasználónév, vagy jelszó.", "Hiba");
            }
            else
            {
                //Elmenti az E-mail-t
                Settings.Default.email = UserText;
                //                MessageBox.Show("TextBox: " + textBox1.Text);
                //                MessageBox.Show("Settings: " + Settings.Default.email);
                Settings.Default.Save();
                //Bejelentkezés
                Program.UserID = Convert.ToInt32(responseString);
                //MessageBox.Show(CalculateMD5Hash(textBox2.Text)); //2014.02.13. - Teszt
                LoginForm.UserCode = CalculateMD5Hash(CalculateMD5Hash(PassText).ToLower() + " Some text because why not " + Program.UserID).ToLower(); //2014.02.13.
                //MessageBox.Show(UserCode);
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
        /*public void StartWorking()
        {
            wh.Set();
        }*/
    }
}
