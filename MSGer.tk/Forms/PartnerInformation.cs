using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
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
    public partial class PartnerInformation : ThemedForms
    {
        public PartnerInformation(UserInfo uinfo)
        {
            InitializeComponent();

            nameTextBox.BackColor = this.BackColor;
            messageTextBox.BackColor = this.BackColor;
            pictureBox1.Image = uinfo.Image; //2015.05.30.
            nameTextBox.Text = uinfo.Name;
            if (uinfo.State == 1)
                statusLabel.Text = Language.Translate(Language.StringID.Menu_File_Status_Online);
            else if (uinfo.State == 2)
                statusLabel.Text = Language.Translate(Language.StringID.Menu_File_Status_Busy);
            else if (uinfo.State == 3)
                statusLabel.Text = Language.Translate(Language.StringID.Menu_File_Status_Away);
            else
                statusLabel.Text = "";
            messageTextBox.Text = uinfo.Message;
            userName1.Text = Language.Translate(Language.StringID.UserName) + ":";
            userName2.Text = uinfo.UserName;
            userID1.Text = Language.Translate(Language.StringID.UserID);
            userID2.Text = uinfo.UserID.ToString();
            email1.Text = "E-mail:";
            email2.Text = uinfo.Email;
        }
    }
}
