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
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            registerButton.Enabled = false;
            if (codeText.TextLength == 0 || userText.TextLength == 0 || passText.TextLength == 0 || emailText.TextLength == 0)
            {
                MessageBox.Show("Minden mezőt tölts ki.", "Hiba");
                registerButton.Enabled = true;
                return;
            }
            string response = Networking.SendRequest("register", codeText.Text + "ͦ" + userText.Text + "ͦ" + LoginForm.CalculateMD5Hash(passText.Text) + "ͦ" + emailText.Text, 2, false);
            if(response=="code")
            {
                MessageBox.Show("A megadott kód nem létezik vagy már felhasználták.", "Hiba");
                registerButton.Enabled = true;
            }
            else if (response == "uname")
            {
                MessageBox.Show("A felhasználónév már foglalt.", "Hiba");
                registerButton.Enabled = true;
            }
            else if (response == "ulen")
            {
                MessageBox.Show("A felhasználónév hossza nem megfelelő. (Min. 4 karakter)", "Hiba");
                registerButton.Enabled = true;
            }
            else if (response == "plen")
            {
                MessageBox.Show("A jelszó hossza nem megfelelő. (Min. 6 karakter)", "Hiba");
                registerButton.Enabled = true;
            }
            else if (response == "Success!")
            {
                MessageBox.Show("Sikeres regisztráció.\nÜdv a közösségben!");
                Close();
            }
            else
            {
                MessageBox.Show("Ismeretlen hiba:\n" + response);
                registerButton.Enabled = true;
            }
        }
    }
}
