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
            label1.Text = Language.Translate("registration");
            label2.Text = Language.Translate("reg_code");
            label3.Text = Language.Translate("reg_username");
            label4.Text = Language.Translate("login_password");
            registerButton.Text = Language.Translate("registration");
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            registerButton.Enabled = false;
            if (codeText.TextLength == 0 || userText.TextLength == 0 || passText.TextLength == 0 || emailText.TextLength == 0)
            {
                MessageBox.Show(Language.Translate("reg_emptyfield"), Language.Translate("error"));
                registerButton.Enabled = true;
                return;
            }
            string response = Networking.SendRequest("register", codeText.Text + "ͦ" + userText.Text + "ͦ" + LoginForm.CalculateMD5Hash(passText.Text) + "ͦ" + emailText.Text, 2, false);
            if(response=="code")
            {
                MessageBox.Show(Language.Translate("reg_codeerr"), Language.Translate("error"));
                registerButton.Enabled = true;
            }
            else if (response == "uname")
            {
                MessageBox.Show(Language.Translate("reg_nameerr"), Language.Translate("error"));
                registerButton.Enabled = true;
            }
            else if (response == "ulen")
            {
                MessageBox.Show(Language.Translate("reg_namelen"), Language.Translate("error"));
                registerButton.Enabled = true;
            }
            else if (response == "plen")
            {
                MessageBox.Show(Language.Translate("reg_passlen"), Language.Translate("error"));
                registerButton.Enabled = true;
            }
            else if (response == "email")
            {
                MessageBox.Show(Language.Translate("reg_email"), Language.Translate("error"));
                registerButton.Enabled = true;
            }
            else if (response == "Success!")
            {
                MessageBox.Show(Language.Translate("reg_success"));
                Close();
            }
            else
            {
                MessageBox.Show(Language.Translate("unknown_error") + ":\n" + response);
                registerButton.Enabled = true;
            }
        }
    }
}
