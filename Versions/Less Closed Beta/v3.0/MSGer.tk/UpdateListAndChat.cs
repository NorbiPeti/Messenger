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
                    byte[] resp = (byte[])retobj[0]; //2014.10.24.
                    IPEndPoint remoteEP = (IPEndPoint)retobj[1]; //2014.10.24.
                    if (resp == null)
                        break;
                    byte isresponse; //2014.09.15.
                    Networking.UpdateType updatetype; //2014.09.15.
                    int keyversion; //2014.09.15.
                    int userid; //2014.09.15.
                    byte[] data; //2014.09.15.
                    Networking.ParsePacket(resp, out isresponse, out updatetype, out keyversion, out userid, out data); //2014.09.15.
                    if (!UserInfo.IPs.Contains(remoteEP) && updatetype != Networking.UpdateType.LoginUser) //2014.10.24.
                        break;
                    if (updatetype == Networking.UpdateType.ListUpdate)
                    {
                        Networking.ParseUpdateInfo(new byte[][] { data }); //2014.09.15.
                    }
                    else if (updatetype == Networking.UpdateType.UpdateMessages)
                    {
                        string[] response = Networking.GetStrings(data, 0);
                        string[] tmp = response[0].Split(',');
                        List<int> tmp2 = new List<int>();
                        tmp2.Add(CurrentUser.UserID);
                        tmp2.AddRange(tmp.Select(entry => Int32.Parse(entry))); //2014.10.24.
                        if (tmp2.All(entry => !UserInfo.Select(entry).IsPartner)) //2014.10.24.
                            break; //Ha a beszélgetésben nincs ismerőse, akkor nem foglalkozik vele
                        var cf = ChatForm.GetChatFormByUsers(tmp2);
                        if (cf == null)
                        {
                            Program.MainF.Invoke((MethodInvoker)delegate
                            {
                                ChatForm.ChatWindows.Add(new ChatForm());
                                cf = ChatForm.ChatWindows[ChatForm.ChatWindows.Count - 1];
                                cf.ChatPartners.AddRange(tmp2);
                                cf.Show();
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

                        int iplen = BitConverter.ToInt32(data, 0);
                        string ip = Encoding.Unicode.GetString(data, 4, iplen);
                        if (!Storage.Settings["ips"].Contains(ip))
                            Storage.Settings["ips"] += ";" + ip;
                        string retstr = "";
                        for (int i = 4 + iplen; i + 8 < resp.Length; i += 8)
                        {
                            int uid = BitConverter.ToInt32(resp, i);
                            int utime = BitConverter.ToInt32(resp, i + 4);
                            if (Int32.Parse(Storage.LoggedInSettings["userinfo_" + uid + "_updatetime"]) > utime)
                            {
                                retstr += uid + "_name=" + Storage.LoggedInSettings["userinfo_" + uid + "_name"] + "\n";
                                retstr += uid + "_message=" + Storage.LoggedInSettings["userinfo_" + uid + "_message"] + "\n";
                                retstr += uid + "_state=" + Storage.LoggedInSettings["userinfo_" + uid + "_state"] + "\n";
                                retstr += uid + "_username=" + Storage.LoggedInSettings["userinfo_" + uid + "_username"] + "\n";
                                retstr += uid + "_email=" + Storage.LoggedInSettings["userinfo_" + uid + "_email"] + "\n";
                                retstr += uid + "_ispartner=" + Storage.LoggedInSettings["userinfo_" + uid + "_ispartner"] + "\n";
                                retstr += uid + "_lastupdate=" + Program.DateTimeToUnixTime(DateTime.Now);
                                //if (i + 1 != (resp.Length - 9) / 8)
                                if (i + 16 < resp.Length)
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
                        Storage.Settings["ips"] = Storage.Settings["ips"].Remove(Storage.Settings["ips"].IndexOf(ipstr), ipstr.Length); //2014.09.22.
                    }
                    else if (updatetype == Networking.UpdateType.SetKey) //2014.09.09.
                    { //2014.09.22.
                        CurrentUser.KeyIndex = BitConverter.ToInt32(data, 0);
                    }
                } while (false);
            }
        }
    }
}
