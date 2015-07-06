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
using System.Net.Sockets;

namespace MSGer.tk
{
    public partial class LoginForm : ThemedForms
    {
        public static string UserCode = "";
        public static Thread LThread;
        private void LoginForm_Load(object sender, EventArgs e)
        {
            List<string> tmp; //E-mail - 2014.04.02.
            if (Storage.Settings[SettingType.Email].Length != 0)
                tmp = Storage.Settings[SettingType.Email].Split(',').ToList<string>();
            else tmp = new List<string>();
            tmp.Add("");
            textBox1.Text = tmp[Int32.Parse(Storage.Settings[SettingType.LastUsedEmail])]; //2014.07.08.
            textBox1.AutoCompleteCustomSource.AddRange(tmp.ToArray());
            this.Activate();
            this.Activate();
        }
        public LoginForm()
        {
            InitializeComponent();
            this.Text = Language.Translate(Language.StringID.Login);
            label1.Text = Language.Translate(Language.StringID.Login);
            label3.Text = Language.Translate(Language.StringID.Password);
            button1.Text = Language.Translate(Language.StringID.Login);
            linkLabel1.Text = Language.Translate(Language.StringID.Registration);
            linkLabel2.Text = Language.Translate(Language.StringID.ForgotPassword);
            textBox3.Text = "";
            List<string> lines = new List<string>();
            lines.Add(Language.Translate(Language.StringID.Login_Desc1));
            lines.Add("");
            lines.Add(Language.Translate(Language.StringID.Login_Desc2));
            textBox3.Lines = lines.ToArray();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == Language.Translate(Language.StringID.Button_Cancel))
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

                textBox1.ReadOnly = true; //2015.04.08.
                textBox2.ReadOnly = true; //2015.04.08.

                // Create the thread object, passing in the Alpha.Beta method
                // via a ThreadStart delegate. This does not start the thread.
                LThread = new Thread(new ThreadStart(LoginUser));
                LThread.Name = "Login Thread";

                // Start the thread
                LThread.Start();
            }
        }
        /// <summary>
        /// A login threadból (stoplogint=false) hívja Invoke-kal
        /// </summary>
        /// <param name="stoplogint"></param>
        public void ResetAfterLogin(bool stoplogint)
        {
            var a = new Action(delegate //2015.04.03.
            {
                button1.Enabled = false;
                Request.Abort();
                if (stoplogint) //2014.09.01.
                    LThread.Abort(); //2014.09.01.
                button1.Text = Language.Translate(Language.StringID.Login);
                button1.Enabled = true;
                linkLabel1.Enabled = true;
                textBox1.Enabled = true; //2014.09.01.
                textBox2.Enabled = true; //2014.09.01.
                textBox1.ReadOnly = false; //2015.04.08.
                textBox2.ReadOnly = false; //2015.04.08.
            });
            if (stoplogint) //2015.04.03.
                a();
            else
                this.Invoke(a);
        }

        public enum LoginInfo
        {
            UserID,
            UserName,
            IPs,
            Password
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
            { //TODO: Fordítás
                MessageBox.Show("Nincs megadva felhasználónév."); //Translate!
                return;
            }
            if (MessageBox.Show("Új jelszót kérsz a megadott névhez?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string ret = Networking.SendRequest(Networking.RequestType.ResetPass, textBox1.Text, 0, false);
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
