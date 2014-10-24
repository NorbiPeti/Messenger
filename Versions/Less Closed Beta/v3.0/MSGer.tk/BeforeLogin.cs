using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    public partial class BeforeLogin : Form
    {
        private static BeforeLogin mInstance;
        private static bool done = false;
        private static string ttext { get; set; }
        public static void Create()
        {
            var t = new System.Threading.Thread(() =>
            {
                Thread.Sleep(1000);
                if (done)
                    return;
                mInstance = new BeforeLogin();
                mInstance.FormClosed += (s, e) => mInstance = null;
                Application.Run(mInstance);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
        }

        public static void Destroy()
        {
            done = true;
            if (mInstance != null) mInstance.Invoke(new Action(() => mInstance.Close()));
        }

        public static void SetText(string text)
        {
            if (mInstance != null) mInstance.Invoke((MethodInvoker)delegate { mInstance.Text = text; });
            ttext = text;
        }

        private BeforeLogin()
        { //2014.09.06.
            InitializeComponent();
            Text = ttext;
        }

        private void BeforeLogin_TextChanged(object sender, EventArgs e)
        {
            label1.Text = this.Text;
        }

        private void BeforeLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!done)
                Program.Exit(false);
        }
    }
}
