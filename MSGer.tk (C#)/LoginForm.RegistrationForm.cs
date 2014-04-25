using System;
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
    public partial class LoginForm_RegistrationForm : Form
    {
        public LoginForm_RegistrationForm()
        {
            InitializeComponent();
            label1.Text = Language.GetCuurentLanguage().Strings["registration"];
            label2.Text = Language.GetCuurentLanguage().Strings["reg_code"];
            label3.Text = Language.GetCuurentLanguage().Strings["reg_username"];
            label4.Text = Language.GetCuurentLanguage().Strings["login_password"];
            registerButton.Text = Language.GetCuurentLanguage().Strings["registration"];
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            registerButton.Enabled = false;
            if (codeText.TextLength == 0 || userText.TextLength == 0 || passText.TextLength == 0 || emailText.TextLength == 0)
            {
                MessageBox.Show(Language.GetCuurentLanguage().Strings["reg_emptyfield"], Language.GetCuurentLanguage().Strings["error"]);
                registerButton.Enabled = true;
                return;
            }
            //MessageBox.Show(codeText.Text + "ͦ" + userText.Text + "ͦ" + LoginForm.CalculateMD5Hash(passText.Text) + "ͦ" + emailText.Text);
            //MessageBox.Show(Uri.EscapeUriString(codeText.Text + "ͦ" + userText.Text + "ͦ" + LoginForm.CalculateMD5Hash(passText.Text) + "ͦ" + emailText.Text));
            string response = Networking.SendRequest("register", codeText.Text + "ͦ" + userText.Text + "ͦ" + LoginForm.CalculateMD5Hash(passText.Text) + "ͦ" + emailText.Text, 2, false);
            if(response=="code")
            {
                MessageBox.Show(Language.GetCuurentLanguage().Strings["reg_codeerr"], Language.GetCuurentLanguage().Strings["error"]);
                registerButton.Enabled = true;
            }
            else if (response == "uname")
            {
                MessageBox.Show(Language.GetCuurentLanguage().Strings["reg_nameerr"], Language.GetCuurentLanguage().Strings["error"]);
                registerButton.Enabled = true;
            }
            else if (response == "ulen")
            {
                MessageBox.Show(Language.GetCuurentLanguage().Strings["reg_namelen"], Language.GetCuurentLanguage().Strings["error"]);
                registerButton.Enabled = true;
            }
            else if (response == "plen")
            {
                MessageBox.Show(Language.GetCuurentLanguage().Strings["reg_passlen"], Language.GetCuurentLanguage().Strings["error"]);
                registerButton.Enabled = true;
            }
            else if (response == "Success!")
            {
                MessageBox.Show(Language.GetCuurentLanguage().Strings["reg_success"]);
                Close();
            }
            else
            {
                MessageBox.Show(Language.GetCuurentLanguage().Strings["unknown_error"] + ":\n" + response);
                registerButton.Enabled = true;
            }
        }
    }
}
