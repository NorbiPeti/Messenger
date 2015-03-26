using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    class UpdateListAndChat
    {
        public void Run()
        {
            while (MainForm.LThread != null && MainForm.MainThread.IsAlive)
            {
                do
                {
                    object[] retobj = Networking.ReceiveUpdates();
                    if (retobj == null) //2014.12.05.
                        return; //2014.12.05. - Leállt a program
                    byte[] resp = (byte[])retobj[0]; //2014.10.24.
                    IPEndPoint remoteEP = (IPEndPoint)retobj[1]; //2014.10.24.
                    if (resp == null)
                        break;
                    byte isresponse; //2014.09.15.
                    Networking.UpdateType updatetype; //2014.09.15.
                    int keyversion; //2014.09.15.
                    int port; //2014.12.19.
                    int userid; //2014.09.15.
                    byte[] data; //2014.09.15.
                    Networking.ParsePacket(resp, out isresponse, out updatetype, out keyversion, out port, out userid, out data); //2014.09.15.
                    //if (!UserInfo.IPs.Any(entry=>entry.IP==remoteEP) && updatetype != Networking.UpdateType.LoginUser) //2014.10.24.
                    //if (!UserInfo.IPs.Any(entry => entry.IP == remoteEP) && updatetype != Networking.UpdateType.LoginUser) //2014.11.23
                    if (!UserInfo.IPs.Any(entry => entry.Address.Equals(remoteEP.Address)) //2014.12.19. - A port nem ugyanaz, ráadásul a == nem hívja meg a .Equals metódust
                        && updatetype != Networking.UpdateType.LoginUser
                        /*&& updatetype != Networking.UpdateType.CheckConn
                        && updatetype != Networking.UpdateType.RequestConn
                        && updatetype != Networking.UpdateType.MakeConn
                        && updatetype != Networking.UpdateType.MakeConn2*/)
                        break;
                    //bool x = UserInfo.IPs.Single().IP.Address.Equals(remoteEP.Address);
                    if (updatetype == Networking.UpdateType.ListUpdate)
                    {
                        //Networking.ParseUpdateInfo(new byte[][] { data }); //2014.09.15.
                        Networking.ParseUpdateInfo(new byte[][] { resp }); //2014.12.22. - A funkció az egész packet-re számít, nem csak a data-ra
                    }
                    else if (updatetype == Networking.UpdateType.UpdateMessages)
                    {
                        string[] response = Networking.GetStrings(data, 0);
                        string[] tmp = response[0].Split(',');
                        List<UserInfo> tmp2 = new List<UserInfo>();
                        tmp2.Add(UserInfo.Select(userid)); //Adja hozzá a küldőt is
                        tmp2.AddRange(tmp.Select(entry => UserInfo.Select(Int32.Parse(entry)))); //2014.10.24.
                        //if (tmp2.All(entry => !UserInfo.Select(entry).IsPartner)) //2014.10.24.
                        if (tmp2.All(entry => !entry.IsPartner)) //2014.10.31.
                            break; //Ha a beszélgetésben nincs ismerőse, akkor nem foglalkozik vele
                        var cf = ChatPanel.GetChatFormByUsers(tmp2);
                        if (cf == null)
                        {
                            Program.MainF.Invoke((MethodInvoker)delegate
                            {
                                ChatPanel.ChatWindows.Add(new ChatPanel());
                                cf = ChatPanel.ChatWindows[ChatPanel.ChatWindows.Count - 1];
                                cf.ChatPartners.AddRange(tmp2);
                                cf.Init();
                            });
                        }
                        //0 - Résztvevők; 1 - Üzenet; 2 - Üzenetküldés időpontja
                        string[] cmd = response[1].Split(' ');
                        switch (cmd[0])
                        {
                            case "//sendfile":
                                string[] ipportname = cmd[1].Split(':');
                                IPAddress ipAddr = IPAddress.Parse(ipportname[0]);
                                var permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, "", SocketPermission.AllPorts);
                                var ipEndPoint = new IPEndPoint(ipAddr, Int32.Parse(ipportname[1]));
                                var receiverSock = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                                receiverSock.Connect(ipEndPoint);
                                var ns = new NetworkStream(receiverSock);
                                var fs = new FileStream(ipportname[2], FileMode.Create);
                                break;
                        }
                        cf.TMessage = "\n" + ((userid == CurrentUser.UserID) ? CurrentUser.Name : UserInfo.Select(userid).Name) + " " + Language.Translate("said") + " (" + Program.UnixTimeToDateTime(response[2]).ToString("yyyy.MM.dd. HH:mm:ss") + "):\n" + response[1] + "\n";
                        Program.MainF.Invoke(new LoginForm.MyDelegate(cf.SetThreadValues));
                    }
                    else if (updatetype == Networking.UpdateType.LoginUser)
                    {
                        string tmpresp = Networking.SendRequest("checkuser", userid.ToString(), 0, true); //2014.09.19.
                        if (tmpresp == "Fail")
                        {
                            break; //Nem küld el neki semmit, hanem újra várja a packet-eket
                        }
                        else if (tmpresp != "Success")
                        {
                            MessageBox.Show("LoginUser:\n" + tmpresp);
                            break;
                        }

                        //int iplen = BitConverter.ToInt32(data, 0);
                        //string ip = Encoding.Unicode.GetString(data, 4, iplen);
                        //IPAddress ip = IPAddress.Parse(Encoding.Unicode.GetString(data, 4, iplen));
                        /*if (!Storage.Settings["ips"].Contains(ip))
                            Storage.Settings["ips"] += ";" + ip;*/
                        //var ep = new IPEndPoint(ip, UserInfo.GetPortForIP(ip)); //2014.11.15.
                        //var ep = new IPEndPoint(ip, port); //2014.12.19.
                        var ep = new IPEndPoint(remoteEP.Address, port);
                        //if (!UserInfo.IPs.Any(entry=>entry.IP==ep))
                        if (!UserInfo.IPs.Any(entry=>entry==ep))
                            UserInfo.IPs.Add(ep);
                        string retstr = "";
                        //for (int i = 4 + iplen; i + 8 < resp.Length; i += 8)
                        for (int i = 0; i + 8 < data.Length; i += 8)
                        {
                            int uid = BitConverter.ToInt32(data, i);
                            int utime = BitConverter.ToInt32(data, i + 4);
                            if (Storage.LoggedInSettings.ContainsKey("userinfo_" + uid + "_updatetime") && Int32.Parse(Storage.LoggedInSettings["userinfo_" + uid + "_updatetime"]) > utime)
                            {
                                retstr += uid + "_name=" + Storage.LoggedInSettings["userinfo_" + uid + "_name"] + "\n";
                                retstr += uid + "_message=" + Storage.LoggedInSettings["userinfo_" + uid + "_message"] + "\n";
                                retstr += uid + "_state=" + Storage.LoggedInSettings["userinfo_" + uid + "_state"] + "\n";
                                retstr += uid + "_username=" + Storage.LoggedInSettings["userinfo_" + uid + "_username"] + "\n";
                                retstr += uid + "_email=" + Storage.LoggedInSettings["userinfo_" + uid + "_email"] + "\n";
                                retstr += uid + "_ispartner=" + Storage.LoggedInSettings["userinfo_" + uid + "_ispartner"] + "\n";
                                //retstr += uid + "_lastupdate=" + Program.DateTimeToUnixTime(DateTime.Now);
                                if (uid != CurrentUser.UserID) //2014.11.29.
                                    retstr += uid + "_lastupdate=" + Storage.LoggedInSettings["userinfo_" + uid + "_lastupdate"]; //2014.11.29. - Arra az időpontra állítsa, amikor ő kapta a frissítést, így ez elvileg az eredeti frissítés időpontját mutatja kb. - Ezért a sajátját biztosan frissen kell tartani
                                else
                                    retstr += uid + "_lastupdate=" + Program.DateTimeToUnixTime(DateTime.Now);
                                //if (i + 1 != (resp.Length - 9) / 8)
                                if (i + 16 < data.Length)
                                    retstr += "\n";
                            }
                        }
                        UserInfo.Select(userid).State = 1;
                        Networking.SendUpdate(Networking.UpdateType.LoginUser, Encoding.Unicode.GetBytes(retstr), true);
                    }
                    else if (updatetype == Networking.UpdateType.LogoutUser)
                    { //2014.08.31. 0:32
                        int len = BitConverter.ToInt32(data, 0);
                        string ipstr = Encoding.Unicode.GetString(data, 4, len);
                        //Storage.Settings["ips"] = Storage.Settings["ips"].Remove(Storage.Settings["ips"].IndexOf(ipstr), ipstr.Length); //2014.09.22.
                        //var ip = IPAddress.Parse(ipstr);
                        string[] ips = ipstr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        var ip = ips.Select(entry => IPAddress.Parse(entry));
                        //UserInfo.IPs.RemoveWhere(entry => entry.IP.Address == ip && entry.IP.Port == UserInfo.GetPortForIP(entry.IP.Address)); //2014.11.15.
                        UserInfo.IPs.RemoveWhere(entry => entry.Address == ip && entry.Port == port); //2014.11.15. - Port: 2014.12.19.
                    }
                    else if (updatetype == Networking.UpdateType.SetKey) //2014.09.09.
                    { //2014.09.22.
                        CurrentUser.KeyIndex = BitConverter.ToInt32(data, 0);
                    }
                    else if (updatetype == Networking.UpdateType.GetImage) //2014.11.01. 0:53
                    {
                        string tmp = Path.GetTempPath();
                        List<byte> sendb = new List<byte>();
                        int user = BitConverter.ToInt32(data, 0);
                        int picupdatetime = BitConverter.ToInt32(data, 4);
                        //int thispicupdatetime = UserInfo.Select(user).PicUpdateTime;
                        int thispicupdatetime = 0;
                        UserInfo userinfo = UserInfo.Select(user);
                        if (userinfo != null)
                            thispicupdatetime = UserInfo.Select(user).PicUpdateTime;
                        if (thispicupdatetime > picupdatetime)
                        {
                            sendb.AddRange(BitConverter.GetBytes(thispicupdatetime));
                            sendb.AddRange(File.ReadAllBytes(tmp + "\\MSGer.tk\\pictures\\" + user + ".png"));
                        }
                        Networking.SendUpdate(Networking.UpdateType.GetImage, sendb.ToArray(), true); //2014.11.23.
                    }
                    /*else if (updatetype == Networking.UpdateType.CheckConn)
                    { //2014.11.23.
                        Networking.SendUpdate(Networking.UpdateType.CheckConn, new byte[] { 0x01 }, true, new IPEndPoint(remoteEP.Address, port));
                    }
                    else if (updatetype == Networking.UpdateType.RequestConn)
                    { //2014.11.23.
                        string[] s = Encoding.Unicode.GetString(data).Split(':');
                        var secondEP = new IPEndPoint(IPAddress.Parse(s[0]), Int32.Parse(s[1]));
                        Networking.SendUpdate(Networking.UpdateType.MakeConn, Encoding.Unicode.GetBytes(secondEP.ToString()), false, remoteEP); //Elküldi, hogy próbálkozzon kapcsolódni
                        Networking.SendUpdate(Networking.UpdateType.RequestConn, new byte[] { 0x01 }, true); //Majd válaszol, hogy ő is próbálkozhat
                    }
                    else if (updatetype == Networking.UpdateType.MakeConn)
                    { //2014.11.23.
                        string[] s = Encoding.Unicode.GetString(data).Split(':');
                        var secondEP = new IPEndPoint(IPAddress.Parse(s[0]), Int32.Parse(s[1]));
                        Networking.SendUpdate(Networking.UpdateType.MakeConn2, new byte[] { 0x01 }, false, secondEP); //Próbálkoziks
                        Networking.SendUpdate(Networking.UpdateType.MakeConn, new byte[] { 0x01 }, true); //Válaszol, hogy ne várjon rá a szerver
                    }
                    else if (updatetype == Networking.UpdateType.MakeConn2)
                    { //2014.11.23.
                        Networking.SendUpdate(Networking.UpdateType.MakeConn2, new byte[] { 0x01 }, true); //Végzett
                    }*/
                } while (false);
            }
        }
    }
}
