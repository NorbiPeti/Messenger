using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    public partial class ScripterWindow : ThemedForms
    {
        public ScripterWindow()
        {
            InitializeComponent();
            newToolStripMenuItem.Text = Language.Translate(Language.StringID.Scripter_New);
            Language.ReloadEvent += delegate { newToolStripMenuItem.Text = Language.Translate(Language.StringID.Scripter_New); };
            openToolStripMenuItem.Text = Language.Translate(Language.StringID.Scripter_Open);
            Language.ReloadEvent += delegate { openToolStripMenuItem.Text = Language.Translate(Language.StringID.Scripter_Open); };
            saveToolStripMenuItem.Text = Language.Translate(Language.StringID.Scripter_Save);
            Language.ReloadEvent += delegate { saveToolStripMenuItem.Text = Language.Translate(Language.StringID.Scripter_Save); };
            exitToolStripMenuItem.Text = Language.Translate(Language.StringID.Scripter_Exit);
            Language.ReloadEvent += delegate { exitToolStripMenuItem.Text = Language.Translate(Language.StringID.Scripter_Exit); };

            timer.Tick += timer_Tick; //2015.04.10.
        }
        public string Path;
        public void LoadScript(string path)
        {
            if (!File.Exists(path))
            { //2015.04.06.
                MessageBox.Show(Language.Translate(Language.StringID.ScriptNotFound), Language.Translate(Language.StringID.Error));
                return;
            }
            Path = path;
        }
        public void UpdateMessages()
        { //2015.04.06.
            if (Path == null)
                return;
            File.WriteAllText(Path, codeTextBox.Text); //2015.04.10.
            ScriptLoader script = new ScriptLoader(Path);
            var messages = script.JustCompile();
            if (messages == null)
            {
                MessageBox.Show(Language.Translate(Language.StringID.ScriptUnloadRequired), Language.Translate(Language.StringID.Error));
                return;
            }
            compilerResultTextBox.SuspendLayout(); //2015.04.10.
            compilerResultTextBox.Clear(); //2015.04.10.
            foreach (CompilerError message in messages)
            {
                //compilerResultTextBox.Text += message + "\n";
                Color color; //2015.04.10.
                if (message.IsWarning)
                    color = Color.DarkOrange; //2015.04.10.
                else
                    color = Color.Red;
                compilerResultTextBox.AppendText("(Col:" + message.Column + ",Line:" + message.Line + ") " + message.ErrorNumber + ": " + message.ErrorText + "\n", color); //2015.04.10.
            }
            compilerResultTextBox.ResumeLayout(true); //2015.04.10.
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        { //2015.04.06.
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                saveFileDialog1.OpenFile().Dispose();
                Path = saveFileDialog1.FileName;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        { //2015.04.06.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(openFileDialog1.FileName))
                    return;
                Path = openFileDialog1.FileName;
                codeTextBox.Text = File.ReadAllText(Path);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        { //2015.04.06.
            File.WriteAllText(Path, codeTextBox.Text);
        }

        //private Timer UpdateTimer = new Timer { Enabled = false, Interval = 1000 }; //2015.04.06.
        //private int tickcount = Environment.TickCount; //2015.04.06.
        private Timer timer = new Timer { Enabled = false, Interval = 1000 }; //2015.04.10.
        private void codeTextBox_TextChanged(object sender, EventArgs e)
        { //2015.04.06.
            /*if (Environment.TickCount - tickcount > 1000)
            {
                UpdateMessages();
                tickcount = Environment.TickCount;
            }*/
            if (!timer.Enabled)
            {
                UpdateMessages();
                timer.Start();
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            UpdateMessages();
        }
    }
}
