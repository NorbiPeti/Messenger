using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    public partial class ErrorHandler : Form
    { //2015.06.04.
        public ErrorHandler(ErrorType type, Exception e)
        {
            InitializeComponent();
            try
            {
                sendbtn.Text = Language.Translate(Language.StringID.Sendbtn_Send);
                exitbtn.Text = Language.Translate(Language.StringID.Menu_File_Exit);
                restartokbtn.Text = Language.Translate(Language.StringID.RestartButton);
            }
            catch
            {
                sendbtn.Text = "Send";
                exitbtn.Text = "Exit";
                restartokbtn.Text = "Restart";
            }
            switch (type)
            {
                case ErrorType.Unknown:
                    fatalerror = true;
                    int maxvalue = Enum.GetNames(typeof(Language.StringID)).Where(entry => entry.StartsWith("Error_Unknown")).Count();
                    if (maxvalue < 1)
                    {
                        textBox1.Text = "We don't have any error messages.";
                        break;
                    }
                    int index = new Random().Next(1, maxvalue);
                    try
                    {
                        Language.StringID result;
                        if (Enum.TryParse<Language.StringID>("Error_Unknown" + index, out result))
                        {
                            textBox1.Text = Language.Translate(result);
                        }
                        else
                        {
                            textBox1.Lines = new string[]
                            {
                                "Unknown error.",
                                "Also...",
                                "Something interesting happened. I don't have all messages up to the latest message count (" + index  + "). Which means I forgot to do something."
                            };
                        }
                    }
                    catch
                    { //2015.06.14. ˇˇ
                        textBox1.Text = "Unknown error.";
                    }
                    break;
                case ErrorType.ServerError:
                    fatalerror = false;
                    textBox1.Lines = new string[]
                    {
                        Language.Translate(Language.StringID.Error_ServerError)
                    };
                    break;
                case ErrorType.ServerConnectError:
                    fatalerror = false;
                    textBox1.Text = Language.Translate(Language.StringID.ConnectError);
                    break;
                case ErrorType.Chat_NoPartners:
                    fatalerror = false;
                    textBox1.Text = Language.Translate(Language.StringID.Chat_NoWindow);
                    break;
                default:
                    fatalerror = true;
                    textBox1.Text = "I don't have this implemented (" + type + "). I'm really sorry.";
                    break;
            }

            if (!fatalerror)
            {
                exitbtn.Hide();
                restartokbtn.Text = "OK";
            }

            try
            {
                if (fatalerror)
                    titleLabel.Text = Language.Translate(Language.StringID.FatalError);
                else
                    titleLabel.Text = Language.Translate(Language.StringID.Error);
            }
            catch
            {
                if (fatalerror) //<-- 2015.06.14.
                    titleLabel.Text = "A fatal error occured."; //2015.06.14.
                else
                    titleLabel.Text = "An error occured.";
            }
            this.Text = titleLabel.Text;

            textBox1.AppendText(Environment.NewLine + e.Message);
            textBox1.AppendText(Environment.NewLine + e.StackTrace);
            this.ShowDialog();
        }

        private void exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ErrorHandler_FormClosing(object sender, FormClosingEventArgs e)
        {
#if !DEBUG
            if (fatalerror)
                Program.Exit();
#endif
        }

        private bool fatalerror;
        private void restartbtn_Click(object sender, EventArgs e)
        {
            if (fatalerror)
                Program.Restart();
            else
                this.Close();
        }

        private void sendbtn_Click(object sender, EventArgs e)
        {
            //TODO: Hibajelentés
        }
    }
    public enum ErrorType
    {
        Unknown,
        ServerError,
        ServerConnectError,
        Chat_NoPartners
    }
}
