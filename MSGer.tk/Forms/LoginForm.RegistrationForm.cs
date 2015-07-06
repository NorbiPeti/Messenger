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
    public partial class LoginForm_RegistrationForm : ThemedForms
    {
        public LoginForm_RegistrationForm()
        {
            InitializeComponent();
            label1.Text = Language.Translate(Language.StringID.Registration);
            label2.Text = Language.Translate(Language.StringID.Reg_Code);
            label3.Text = Language.Translate(Language.StringID.UserName);
            label4.Text = Language.Translate(Language.StringID.Password);
            registerButton.Text = Language.Translate(Language.StringID.Registration);
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            registerButton.Enabled = false;
            if (codeText.TextLength == 0 || userText.TextLength == 0 || passText.TextLength == 0 || emailText.TextLength == 0)
            {
                MessageBox.Show(Language.Translate(Language.StringID.Reg_EmptyField), Language.Translate(Language.StringID.Error));
                registerButton.Enabled = true;
                return;
            }
            string response = Networking.SendRequest(Networking.RequestType.Register, codeText.Text + "ͦ" + userText.Text + "ͦ" + LoginForm.CalculateMD5Hash(passText.Text) + "ͦ" + emailText.Text, 2, false);
            if (response == "code")
            {
                MessageBox.Show(Language.Translate(Language.StringID.Reg_CodeErr), Language.Translate(Language.StringID.Error));
                registerButton.Enabled = true;
            }
            else if (response == "uname")
            {
                MessageBox.Show(Language.Translate(Language.StringID.Reg_NameErr), Language.Translate(Language.StringID.Error));
                registerButton.Enabled = true;
            }
            else if (response == "ulen")
            {
                MessageBox.Show(Language.Translate(Language.StringID.Reg_NameLen), Language.Translate(Language.StringID.Error));
                registerButton.Enabled = true;
            }
            else if (response == "plen")
            {
                MessageBox.Show(Language.Translate(Language.StringID.Reg_PassLen), Language.Translate(Language.StringID.Error));
                registerButton.Enabled = true;
            }
            else if (response == "email")
            {
                MessageBox.Show(Language.Translate(Language.StringID.Reg_Email), Language.Translate(Language.StringID.Error));
                registerButton.Enabled = true;
            }
            else if (response == "Success!")
            {
                MessageBox.Show(Language.Translate(Language.StringID.Reg_Success));
                Close();
            }
            else
            {
                new ErrorHandler(ErrorType.ServerError, new Exception(response)); //2015.06.04.
                registerButton.Enabled = true;
            }
        }
    }
}
