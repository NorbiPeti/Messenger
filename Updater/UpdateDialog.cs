using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
    public partial class UpdateDialog : Form
    {
        public UpdateDialog(string[] args)
        { //2014.12.13.
            InitializeComponent();
            if (args.Length < 2)
                Environment.Exit(-1);
            //this.Text = Language.Translate("updater");
            this.Text = args[0];
            label1.Text = this.Text;
            label2.Text = args[1];

            //MessageBox.Show(Directory.GetCurrentDirectory());
            Thread t = new Thread(new ThreadStart(UpdateThread));
            t.Start();
        }

        private void UpdateThread()
        {
            var info = new ProcessStartInfo("SVN\\svn.exe", "export http://msger-tk.googlecode.com/svn/trunk/MSGer.tk/bin/Release/ \"" + Directory.GetCurrentDirectory() + "\" --force");
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            var p = Process.Start(info);
            this.Invoke(new Action(() => textBox1.AppendText("Using PortableSVN which uses Subversion." + Environment.NewLine +
            "http://sourceforge.net/projects/svnportable/" + Environment.NewLine + Environment.NewLine)));
            while (!p.StandardOutput.EndOfStream || !p.StandardError.EndOfStream)
            {
                string s;
                if (!p.StandardOutput.EndOfStream)
                    s = p.StandardOutput.ReadLine();
                else
                    s = p.StandardError.ReadLine();
                this.Invoke(new Action(() => textBox1.AppendText(s + "\n")));
            }
            if (p.ExitCode == 0) //Ha minden rendben, csak akkor folytatja
            {
                Process.Start("MSGer.tk.exe");
                Environment.Exit(0);
            }
        }

        private void UpdateDialog_Load(object sender, EventArgs e)
        {
            this.Select();
        }
    }
}
