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
            if (UserInfo.UserID == 0)
                Close();
            this.Show();
            //-----------------------------------------------------------------------
            HttpWebRequest httpWReq =
                (HttpWebRequest)WebRequest.Create("http://msger.tk/client.php");

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "uid=" + UserInfo.UserID;
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

            MessageBox.Show(responseString);
            responseString = responseString.Remove(responseString.Length-3); //-1-gyel nem változik láthatóan (szókőz?) - 2014.02.18.
            MessageBox.Show(responseString);
            //-----------------------------------------------------------------------
            //2014.02.13. - Kiváncsi vagyok, mit ir, hátha megtudom, mi a hiba --> Program: Uppercase - Weboldal: Lowercase
            //MessageBox.Show(UserInfo.UserID + "");
            //MessageBox.Show(responseString);
            //Application.Exit();


            string[] row = responseString.Split('-');
//            var listViewItem = new ListViewItem(row);
            for (int x = 0; x < row.Length; x+=3)
            {
                string state = "";
                if (row[x + 2] == "1")
                    state = " (Elérhető)";
                if (row[x + 2] == "2")
                    state = " (Elfoglalt)";
                if (row[x + 2] == "3")
                    state = " (Nincs a gépnél)";
                var listViewItem = new ListViewItem(row[x]+state);
//                MessageBox.Show("Item létrehozva. Hozzáadás..."); //A sok ellenőrzés itt volt, és közben pedig a PHP-ban volt a hiba
                listViewItem.SubItems.Add(row[x + 1]);
                if (Convert.ToInt32(row[x + 2]) != 0 && Convert.ToInt32(row[x + 2]) != 4) //2014.02.17. - Előtte: ==1 -- 2014.02.18. - !=4
                    listViewItem.Group = listView1.Groups["listViewGroup1"];
                else
                    listViewItem.Group = listView1.Groups["listViewGroup2"];
                listView1.Items.Add(listViewItem);
//                MessageBox.Show("Hozzáadva.");

                /*MessageBox.Show("Row len: " + row.Length);
                MessageBox.Show("Row: " + row[x]);
                MessageBox.Show("ResponseString: " + responseString);*/
//                MessageBox.Show("User ID: " + UserInfo.UserID);
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
            UserInfo.UserID = 0;
            LoginForm.LButtonText = "Bejelentkezés";
            LoginForm.PassText = "";
            this.Invoke(new LoginForm.MyDelegate(LoginDialog.SetLoginValues));
            LoginDialog.ShowDialog();
            //Nézzük, sikerült-e
            if (UserInfo.UserID == 0)
                Close();
            this.Show();
        }

        private void LoginNewUser(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("MSGer.tk.exe");
        }

        private void SetOnlineState(object sender, EventArgs e)
        {
            //MessageBox.Show(sender.ToString()); - 2014.02.17.
            int state = 0;
            if (sender == elérhetőToolStripMenuItem)
                state = 1;
            if (sender == elfoglaltToolStripMenuItem)
                state = 2;
            if (sender == nincsAGépnélToolStripMenuItem)
                state = 3;
            if (sender == rejtveKapcsolódikToolStripMenuItem)
                state = 4;
            //HTTP
            //Networking.SendRequest("setstate", state + ""); - Csináltam neki funkciót, ezért az if részhez felesleges új változó, berakom oda - 2014.02.18. 21:18

            //MessageBox.Show(responseString);
            //MessageBox.Show(responseString.Length + " vs " + "Success".Length);
            if (Networking.SendRequest("setstate", state + "") != "Success")
                MessageBox.Show("Hiba történt az állapot beállitása során."); //2014.02.17.
        }
    }
}
