using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace MSGer.tk
{
    public partial class MainForm : Form
    {
        private LoginForm LoginDialog;
        public MainForm()
        {
            InitializeComponent();
            this.Hide();
            //(new LoginForm()).ShowDialog();
            LoginDialog = new LoginForm();
            LoginDialog.ShowDialog();
            //LoginForm.Closeable = true; //Akárhogy is áll le (ha X-szel nyomja ki, akkor is), bezárja teljesen
            //Nézzük, sikerült-e
            if (Program.UserID == 0)
                Close();
            //-----------------------------------------------------------------------
            HttpWebRequest httpWReq =
                (HttpWebRequest)WebRequest.Create("http://msger.tk/client.php");

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "uid=" + Program.UserID;
            postData += "&action=getlist";
            postData += "&key=cas1fe4a6feFEFEFE1616CE8099VFE1444cdasf48c1ase5dg";
            postData += "&code="+LoginForm.UserCode; //2014.02.13.
            byte[] data = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = data.Length;

            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            //Lista betöltése folyamatban...

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            try
            {
                responseString = responseString.Remove(responseString.IndexOf('<'));
            }
            catch
            {
            }

            responseString = responseString.Remove(responseString.Length-1);
            //-----------------------------------------------------------------------
            //2014.02.13. - Kiváncsi vagyok, mit ir, hátha megtudom, mi a hiba --> Program: Uppercase - Weboldal: Lowercase
            //MessageBox.Show(Program.UserID + "");
            //MessageBox.Show(responseString);
            //Application.Exit();


            string[] row = responseString.Split('-');
//            var listViewItem = new ListViewItem(row);
            for (int x = 0; x < row.Length; x+=3)
            {
                var listViewItem = new ListViewItem(row[x]);
//                MessageBox.Show("Item létrehozva. Hozzáadás..."); //A sok ellenőrzés itt volt, és közben pedig a PHP-ban volt a hiba
                listViewItem.SubItems.Add(row[x + 1]);
                if (Convert.ToInt32(row[x + 2]) == 1)
                    listViewItem.Group = listView1.Groups["listViewGroup1"];
                else
                    listViewItem.Group = listView1.Groups["listViewGroup2"];
                listView1.Items.Add(listViewItem);
//                MessageBox.Show("Hozzáadva.");

                /*MessageBox.Show("Row len: " + row.Length);
                MessageBox.Show("Row: " + row[x]);
                MessageBox.Show("ResponseString: " + responseString);*/
//                MessageBox.Show("User ID: " + Program.UserID);
                //MessageBox.Show("Row[x]: " + row[x] + " Row[x+1]: " + row[x + 1] + " Row[x+2]: " + row[x + 2]);
                this.Show();
//                MessageBox.Show("Stop! xD");
                if (x + 4 >= row.Length)
//                    MessageBox.Show("Túl sok");
                    break;
            }
//            MessageBox.Show("ASD");
        }

        private void LogoutUser(object sender, EventArgs e)
        {
            this.Hide();
            Program.UserID = 0;
            LoginForm.LButtonText = "Bejelentkezés";
            LoginForm.PassText = "";
            this.Invoke(new LoginForm.MyDelegate(LoginDialog.SetLoginValues));
            LoginDialog.ShowDialog();
            //Nézzük, sikerült-e
            if (Program.UserID == 0)
                Close();
            this.Show();
        }
    }
}
