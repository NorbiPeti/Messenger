using System;
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
    public partial class PartnerInformation : Form
    {
        //public PartnerInformation(string shownname, int status, string message, string username, int userid, string email)
        public PartnerInformation(UserInfo uinfo)
        {
            InitializeComponent();

            nameTextBox.BackColor = this.BackColor;
            messageTextBox.BackColor = this.BackColor;

            string tmp = Path.GetTempPath();
            string pictlocation = tmp + "\\MSGer.tk\\pictures\\" + uinfo.UserID + ".png";
            if (File.Exists(pictlocation))
                pictureBox1.ImageLocation = pictlocation;
            else
                pictureBox1.ImageLocation = "noimage.png";
            nameTextBox.Text = uinfo.Name;
            //statusLabel.Text = uinfo.State.ToString();
            if (uinfo.State == 1)
                statusLabel.Text = Language.Translate("menu_file_status_online");
            else if (uinfo.State == 2)
                statusLabel.Text = Language.Translate("menu_file_status_busy");
            else if (uinfo.State == 3)
                statusLabel.Text = Language.Translate("menu_file_status_away");
            else
                statusLabel.Text = "";
            messageTextBox.Text = uinfo.Message;
            userName1.Text = Language.Translate("reg_username") + ":";
            userName2.Text = uinfo.UserName;
            userID1.Text = Language.Translate("userid");
            userID2.Text = uinfo.UserID.ToString();
            email1.Text = "E-mail:";
            email2.Text = uinfo.Email;
        }
    }
}
