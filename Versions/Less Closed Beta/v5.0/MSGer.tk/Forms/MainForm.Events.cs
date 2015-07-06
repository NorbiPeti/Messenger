using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    partial class MainForm
    {
        private void PopupCloseClick(object sender, EventArgs e)
        {
            MessageBox.Show("Close");
        }

        private void PopupClick(object sender, EventArgs e)
        {
            MessageBox.Show("Click");
        }

        private void LoginNewUser(object sender, EventArgs e)
        {
            Storage.Save(true); //2014.09.19.
            Process.Start(((Program.ProcessName.Contains("vshost")) ? Program.ProcessName.Replace(".vshost", "") : Program.ProcessName) + ".exe", "multi");
        }

        public void SetOnlineState(object sender, EventArgs e)
        {
            int state = 0;
            if (sender == elérhetőToolStripMenuItem)
                state = 1;
            if (sender == elfoglaltToolStripMenuItem)
                state = 2;
            if (sender == nincsAGépnélToolStripMenuItem)
                state = 3;
            if (sender == null) //2014.08.30. - Erre nagyon sokáig nem volt felkészítve, és ezt kihasználtam a kijelentkezéshez
            {
                if (Networking.SendRequest(Networking.RequestType.SetState, 0 + "", 0, true).Contains("Fail")) //Kijelentkezés - if: 2014.11.15.
                    MessageBox.Show(Language.Translate(Language.StringID.Error));
                /*string str = "";
                foreach (var ip in CurrentUser.IPs)
                    str += ip + ";";*/
                /*byte[] tmpb = Encoding.Unicode.GetBytes(str);
                byte[] sendb = new byte[4 + tmpb.Length];
                Array.Copy(BitConverter.GetBytes(tmpb.Length), sendb, 4);
                Array.Copy(tmpb, 0, sendb, 4, tmpb.Length);*/
                //Networking.SendUpdateInThread(Networking.UpdateType.LogoutUser, sendb, null);
                //Networking.SendUpdateInThread(new Networking.PacketFormat(false, new Networking.PDLogoutUser()), null).ContinueWith((t) => //2015.04.03.
                new Networking.PacketSender(new Networking.PDLogoutUser()).SendAsync().ContinueWith((t) =>
                {
                    CurrentUser.SendChanges = false; //2014.12.31.
                    CurrentUser.State = state; //2014.08.28.
                });
            }
            else //else: 2015.04.03.
                CurrentUser.State = state; //2014.08.28.
        }

        private void SelectPartner(object sender, EventArgs e)
        {
            var form = new SelectPartnerForm((ToolStripMenuItem)sender);
            DialogResult dr = form.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //2014.04.25.
                string[] partners = form.Partners;
                //ChatPanel tmpchat = new ChatPanel();
                List<UserInfo> tmplist = new List<UserInfo>(); //2015.05.15.
                for (int i = 0; i < partners.Length; i++)
                {
                    if (partners[i] != "") //2014.04.17.
                    {
                        for (int j = 0; j < UserInfo.KnownUsers.Count; j++)
                        {
                            if (!UserInfo.KnownUsers[j].IsPartner)
                                continue;
                            int tmp; //2014.04.17.
                            if (!Int32.TryParse(partners[i], out tmp))
                                tmp = -1;
                            if (UserInfo.KnownUsers[j].UserName == partners[i] || UserInfo.KnownUsers[j].Email == partners[i] || UserInfo.KnownUsers[j].UserID == tmp)
                            { //Egyezik a név, E-mail vagy ID - UserName: 2014.04.17.
                                //tmpchat.ChatPartners.Add(UserInfo.KnownUsers[j]); //2014.08.28.
                                tmplist.Add(UserInfo.KnownUsers[j]); //2015.05.15.
                            }
                        }
                    }
                }
                //if (tmpchat.ChatPartners.Count != 0)
                if(tmplist.Count!=0)
                {
                    //ChatPanel.ChatWindows.Add(tmpchat);
                    if (sender == fájlKüldéseToolStripMenuItem)
                    {
                        //tmpchat.Init();
                        ChatPanel tmpchat = ChatPanel.Create(tmplist);
                        tmpchat.OpenSendFile(form);
                    }
                    if (sender == azonnaliÜzenetKüldéseToolStripMenuItem)
                    {
                        //tmpchat.Init();
                        ChatPanel.Create(tmplist);
                    }
                }
            }
        }

        private void ClearSearchBar(object sender, EventArgs e)
        {
            if (textBox1.Text == Language.Translate(Language.StringID.SearchBar))
                textBox1.Clear();
        }

        private void PutTextInSearchBar(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = Language.Translate(Language.StringID.SearchBar);
        }

        public static void OpenSendMessage(int uid)
        {
            var uinfo = UserInfo.Select(uid);
            //Üzenetküldő form
            int ChatNum = -1;
            for (int i = 0; i < ChatPanel.ChatWindows.Count; i++)
            {
                if (ChatPanel.ChatWindows[i].ChatPartners.Count == 1 && ChatPanel.ChatWindows[i].ChatPartners.Contains(uinfo))
                { //Vele, és csak vele beszél
                    ChatNum = i;
                    break;
                }
            }
            if (ChatNum == -1)
            { //Nincs még chatablaka
                /*ChatPanel.ChatWindows.Add(new ChatPanel());
                var cf = ChatPanel.ChatWindows[ChatPanel.ChatWindows.Count - 1];
                cf.ChatPartners.Add(uinfo);
                cf.Init();*/
                ChatPanel.Create(new UserInfo[1] { uinfo });

            }
            else
            {
                ChatPanel.ChatWindows[ChatNum].Show();
                ChatPanel.ChatWindows[ChatNum].Focus();
            }
        }

        private void OnMainFormLoad(object sender, EventArgs e)
        {
            //2015.05.23. - Áthelyeztem a betöltőkódot a ThemeDesigner miatt
            //TO!DO (2015.05.23.) - Ezeket a kódokat átrakni valami betöltőfüggvénybe, a MainForm csak a LoginForm után jelenjen meg

            if (CurrentUser.UserID == 0)
                Program.Exit();

            LoadPartnerList(); //Be kell töltenie a MainForm-nak, hogy hivatkozhasson rá

            CurrentUser.SendChanges = true; //2014.08.30.
        }

        private void InvitePartner(object sender, EventArgs e)
        {
            (new InvitePartner()).ShowDialog();
        }

        private void BeforeExit(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (CurrentUser.UserID != 0) //2014.04.18.
            {
                this.Show();
                this.Focus();
            }
        }

        private void ExitProgram(object sender, EventArgs e)
        {
            Program.Exit();
        }

        private void ismerősFelvételeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AddPartner()).ShowDialog();
        }

        private void névjegyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AboutBox1()).ShowDialog();
        }

        private void mindigLegfelülToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = mindigLegfelülToolStripMenuItem.Checked;
        }

        private void beállitásokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.SettingsF == null)
            {
                Program.SettingsF = new SettingsForm();
                Program.SettingsF.Show();
            }
        }

        private void contactList_ItemDoubleClicked(object sender, int e)
        {
            int uid = UserInfo.GetUserIDFromListID(e);
            OpenSendMessage(uid);
        }

        private void bezárásToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void információToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < UserInfo.KnownUsers.Count; i++)
            {
                if (UserInfo.KnownUsers[i].TMPListID != (int)((ToolStripMenuItem)sender).GetCurrentParent().Tag)
                    continue;
                (new PartnerInformation(UserInfo.KnownUsers[i])).ShowDialog();
                break;
            }
        }

        private void contactList_ItemRightClicked(object sender, int e)
        {
            contactList.Items[e].Selected = true;
            listPartnerMenu.Tag = e;
            listPartnerMenu.Show(Cursor.Position);
        }

        private void PartnerMenu_SendMessage(object sender, EventArgs e)
        {
            int uid = UserInfo.GetUserIDFromListID((int)((ToolStripMenuItem)sender).GetCurrentParent().Tag); //Erre kattintott jobb gombbal
            OpenSendMessage(uid);
        }

        private void panel2_Click(object sender, EventArgs e)
        { //2014.10.31.
            textBox1.Focus();
        }

        private System.Windows.Forms.Timer searchbartimer = new System.Windows.Forms.Timer();
        private void textBox1_TextChanged(object sender, EventArgs e)
        { //2014.11.30. 1:33
            var searchlist = searchListView; //2014.12.14. 1:46
            if (searchbartimer.Enabled)
                return;
            searchbartimer.Interval = 1000;
            searchbartimer.Tick += delegate
            {
                searchbartimer.Stop();
                if (textBox1.Text.Length == 0 || textBox1.Text == Language.Translate(Language.StringID.SearchBar)) //text==translate...: 2014.12.05.
                {
                    searchlist.Visible = false;
                    contactList.Visible = true;
                    return;
                }
                contactList.Visible = false;
                searchlist.Parent = contactList.Parent; //2014.12.05.
                searchlist.Bounds = contactList.Bounds;
                searchlist.Items.Clear(); //2014.12.05.
                searchlist.AutoUpdate = false;
                foreach (var item in UserInfo.KnownUsers)
                {
                    if (!item.IsPartner)
                        continue;
                    if (item.Name.ToLower().Contains(textBox1.Text.ToLower()) || item.Email.ToLower().Contains(textBox1.Text.ToLower()) || item.UserName.ToLower().Contains(textBox1.Text.ToLower())) //ToLower: 2014.12.14. 1:53
                    {
                        item.CreateListItem(searchlist, searchlist.Items.Count);
                    }
                }
                searchlist.AutoUpdate = true;
                searchlist.Visible = true;
            };
            searchbartimer.Start();
        }

        private void flowLayoutPanel1_Click(object sender, EventArgs e)
        {
            foreach (var item in ChatPanel.ChatWindows)
            {
                item.Hide();
            }
        }

        private void szkriptíróToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ScripterWindow().Show();
        }

        private void beérkezettFájlokMappájánakMegnyitásaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + Language.Translate(Language.StringID.ReceivedFiles);
            if (Directory.Exists(path))
                Process.Start(path);
        }

        private void megjelenitendőKépVáltásaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
