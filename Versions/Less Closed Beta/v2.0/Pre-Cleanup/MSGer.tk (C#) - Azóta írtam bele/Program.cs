//#define LOCAL_SERVER

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SzNPProjects;

namespace MSGer.tk
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static MainForm MainF;
        //public static ThreadSetVarTarget ThreadTarget;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Console.WriteLine("Starting MSGer.tk...");
            //ThreadTarget = new ThreadSetVarTarget();
            /*ThreadSetVar.RunnerForm = new Form();
            ThreadSetVar.RunnerForm.WindowState = FormWindowState.Minimized;
            ThreadSetVar.RunnerForm.ShowInTaskbar = false;
            ThreadSetVar.RunnerForm.Show();
            ThreadSetVar.RunnerForm.Hide();*/
            try
            {
                MainF = new MainForm();
                Application.Run(MainF);
            }
            catch(FileNotFoundException e)
            {
                MessageBox.Show("Egy fájl nem található.\nA fájl neve:\n" + e.FileName+"\nEnnél a műveletnél: "+e.TargetSite);

            }
            catch(Exception e)
            {
                ErrorHandling.FormError(MainF, e);
            }
        }

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0,
                                                      DateTimeKind.Utc);

        public static DateTime UnixTimeToDateTime(string text)
        {
            double seconds = double.Parse(text, CultureInfo.InvariantCulture);
            //return Epoch.AddSeconds(seconds);
            //2014.04.25.
            DateTime time = Epoch.AddSeconds(seconds);
            time = DateTime.SpecifyKind(time, DateTimeKind.Utc);
            return time.ToLocalTime();
        }

        public static void Exit()
        { //2014.04.12.
            if (MainF != null)
            {
                MainF.Hide();
                MainF.toolStripMenuItem4.Enabled = false; //2014.04.12.
                MainF.toolStripMenuItem8.Enabled = false; //2014.04.12.
                if (CurrentUser.UserID != 0) //2014.04.18.
                {
                    MainF.SetOnlineState(null, null); //2014.04.12.)
                    if (MainF.WindowState == FormWindowState.Maximized) //2014.04.18.
                        Settings.Default.windowstate = 1;
                    else if (MainF.WindowState == FormWindowState.Minimized)
                        Settings.Default.windowstate = 2;
                    else if (MainF.WindowState == FormWindowState.Normal)
                        Settings.Default.windowstate = 3;
                    Settings.Default.Save();
                }
            }
            CurrentUser.UserID = 0;
            Application.Exit();
            Application.ExitThread();
            MessageBox.Show("Hiba: Nem sikerült leállítani a programot.");
        }
    }
    static class Networking
    {
        private static int Tries = 0;
        public static string SendRequest(string action, string data, int remnum, bool loggedin) //2014.02.18.
        {
            //HttpWebRequest httpWReq =
            //    (HttpWebRequest)WebRequest.Create("http://msger.tk/client.php");
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

            //Lista betöltése folyamatban...

            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse)httpWReq.GetResponse();
            }
            catch (Exception e)
            {
                //MessageBox.Show("Hiba az ismerőslista frissítésekor:\n", e.Message);
                if (Tries > 10)
                {
                    MessageBox.Show(Language.GetCuurentLanguage().Strings["error_network"] + ":\n" + e.Message + "\n\n" + Language.GetCuurentLanguage().Strings["error_network2"], Language.GetCuurentLanguage().Strings["error"]); //2014.04.25.
                    Tries = 0;
                }
                Tries++;
                return SendRequest(action, data, remnum, loggedin); //Újrapróbálkozik
            }
            string responseString;
            responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }
    }
    /*class ThreadSetVarTarget
    {
        public object SetVarTarget;
        public object SetVarValue;
        public ThreadSetVarTarget()
        {
        }
    }*/
#if THREAD_SET_VAR
    class ThreadSetVar
    { //2014.04.11.
        private delegate void SetVarDelegate();
        //private ThreadSetVarTarget Target;
        internal object SetVarTarget;
        internal object SetVarValue;
        internal string Mode;
        /*public ThreadSetVar(Control variable, object value, Control sender, ThreadSetVarTarget target)
        {
            if (value is String)
            {
                target.SetVarTarget = variable;
                target.SetVarValue = (string)value;
                Target = target;
                sender.Invoke(new SetVarDelegate(IntThreadSetVar));
            }
        }*//*
        public ThreadSetVar(object variable, object value, Control sender, string mode)
        {
            if (sender.IsDisposed)
                return;
            //Target = target;
            SetVarTarget = variable;
            SetVarValue = value;
            Mode = mode;
            sender.Invoke(new SetVarDelegate(IntThreadSetVar));
        }
        private void IntThreadSetVar()
        { //Belső meghívás
            switch (Mode)
            {
                case "control":
                    if (SetVarValue is String && SetVarTarget.GetType().IsSubclassOf(typeof(Control)))
                    {
                        ((Control)SetVarTarget).Text = (string)SetVarValue;
                    }
                    break;
            }*/

            /*if (Target.SetVarTarget is RichListView && Target.SetVarValue is RichListViewItem)
            {
                var item = new RichListViewItem(((RichListView)Target.SetVarTarget).Columns.Length);
                item.SubItems = ((RichListViewItem)Target.SetVarValue).SubItems;
                ((RichListView)Target.SetVarTarget).Items.Add(item);
            }
            if (Target.SetVarTarget is RichListView && Target.SetVarValue is String)
            {
                if ((String)Target.SetVarValue == "clear")
                {
                    ((RichListView)Target.SetVarTarget).Items.Clear();
                }
            }*/

        public const int
            UpdateMode_AddRichListViewItem = 0,
            UpdateMode_BindingListClear = 1,
            UpdateMode_SetText = 2;

        public delegate void ObjectDelegate(object target, object value, int method);
        public ObjectDelegate Del;
        //public static Form RunnerForm;

        public ThreadSetVar(object target, object value, int method)
        {
            /*RunnerForm = new Form();
            RunnerForm.WindowState = FormWindowState.Minimized;
            RunnerForm.ShowInTaskbar = false;
            RunnerForm.Show();
            RunnerForm.Hide();*/
            // first thing we do is create a delegate pointing to update method
            Del = new ObjectDelegate(UpdateObject);

            // then simply enough, we invoke it
            Del.Invoke(target, value, method);

            // if we wanted to create a thread to use it, we'd use a
            // params threadstart and pass the delegate as an object
            //Thread th = new Thread(new ParameterizedThreadStart(WorkThread));
            //th.Start(del);
        }

        /*private void WorkThread(object obj)
        {
            // we would then unbox the obj into the delegate
            ObjectDelegate del = (ObjectDelegate)obj;

            // and invoke it like before
            del.Invoke("Hello from WorkThread!");
        }*/

        private void UpdateObject(object target, object value, int method)
        {
            // do we need to switch threads?
            if (Program.MainF.InvokeRequired)
            {
                // we then create the delegate again
                // if you've made it global then you won't need to do this
                //ObjectDelegate method = new ObjectDelegate(UpdateTextBox);

                // we then simply invoke it and return
                Program.MainF.Invoke(Del, target, value, method);
                return;
            }

            // ok so now we're here, this means we're able to update the control
            // so we unbox the object into a string
            //string text = (string)obj;

            // and update
            //((Control)obj).Text += text + "\r\n"; //-.lo

            switch(method)
            {
                case UpdateMode_AddRichListViewItem:
                    var tmp = new Control[((Object[])value).Length];
                    for (int i = 0; i < ((Object[])value).Length; i++)
                    {
                        tmp[i] = (Control)(((Object[])value)[i]); //Egy kevés zárójelezés
                    }
                    var item = new RichListViewItem(((Object[])value).Length);
                    item.SubItems = tmp;
                    ((BindingList<RichListViewItem>)target).Add(item);
                    //((BindingList<RichListViewItem>)target).Add((RichListViewItem)value);

                    break;
                case UpdateMode_BindingListClear:
                    ((BindingList<RichListViewItem>)target).Clear();
                    break;
                case UpdateMode_SetText:
                    ((Control)target).Text = (string)value;
                    break;
            }
        }
    }
#endif
}
