using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    partial class Networking
    {
        public class UpdateListAndChat
        {
            public void Run()
            {
                //while (MainForm.PartnerListUpdateThread != null && MainForm.MainThread.IsAlive)
                while (MainForm.PartnerListUpdateThread != null && Program.MainThread.IsAlive)
                {
                    do
                    {
                        /*object[] retobj = Networking.ReceiveUpdates();
                        if (retobj == null) //2014.12.05.
                            return; //2014.12.05. - Leállt a program
                        byte[] resp = (byte[])retobj[0]; //2014.10.24.
                        IPEndPoint remoteEP = (IPEndPoint)retobj[1]; //2014.10.24.
                        if (resp == null)
                            break;*/
                        //byte[] resp;
                        PacketFormat pf;
                        IPEndPoint remoteEP;
                        if (!Networking.ReceiveUpdates(out pf, out remoteEP))
                            break;
                        //PacketFormat pf = PacketFormat.FromBytes(resp);
                        IPEndPoint SendBackEP = new IPEndPoint(remoteEP.Address, pf.Port); //2015.05.10.
                        bool Break = false;
                        switch (pf.PacketType)
                        {
                            case UpdateType.ListUpdate:
                                {
                                    Networking.ParseUpdateInfo(new string[1][] { ((PDListUpdate)pf.EData).Strings });
                                    //Networking.SendUpdate(new PacketFormat(true, new PDListUpdate(true)), SendBackEP);
                                    new PacketSender(new PDListUpdate(true), pf.ID).Send(SendBackEP);
                                    break;
                                }
                            case UpdateType.LoginUser:
                                {
                                    PDLoginUser data = (PDLoginUser)pf.EData;
                                    string tmpresp = Networking.SendRequest(Networking.RequestType.CheckUser, pf.EUserID.ToString(), 0, true); //2014.09.19.
                                    if (tmpresp == "Fail")
                                    {
                                        Break = true;
                                        break; //Nem küld el neki semmit, hanem újra várja a packet-eket
                                    }
                                    else if (tmpresp != "Success")
                                    {
                                        MessageBox.Show("LoginUser:\n" + tmpresp);
                                        Break = true;
                                        break;
                                    }

                                    //var ep = new IPEndPoint(remoteEP.Address, pf.Port);
                                    if (!UserInfo.IPs.Any(entry => entry == SendBackEP))
                                        UserInfo.IPs.Add(SendBackEP);
                                    List<string> strs = new List<string>();
                                    foreach (var uinfo in data.UserInfos)
                                    {
                                        int uid = uinfo.Key;
                                        int updatetime = uinfo.Value;
                                        UserInfo user = UserInfo.Select(uid);
                                        if (user == null)
                                            continue;
                                        strs.Add(user.ToString()); //Egybe rakja felhasználónként, de amikor megkapja, ugyanúgy szétválogatja majd
                                    }
                                    //var rpf = new PacketFormat(true, new PDLoginUser(strs.ToArray()));
                                    //Networking.SendUpdate(rpf, SendBackEP); //2015.04.11.
                                    new PacketSender(new PDLoginUser(strs.ToArray()), pf.ID).Send(SendBackEP);
                                    break;
                                }
                            case UpdateType.LogoutUser:
                                { //2015.05.10. - Felesleges elküldenie az IP-címeket, mint a bejelentkezésnél
                                    UserInfo.Select(pf.EUserID).State = 0;
                                    //Networking.SendUpdate(new PacketFormat(true, new PDLogoutUser()), SendBackEP);
                                    new PacketSender(new PDLogoutUser(), pf.ID).Send(SendBackEP);
                                    break;
                                }
                            case UpdateType.GetImage:
                                {
                                    PDGetImage data = (PDGetImage)pf.EData;
                                    UserInfo user = UserInfo.Select(data.UserID);
                                    bool success;
                                    byte[] imagedata;
                                    if (user.PicUpdateTime > data.PicUpdateTime)
                                    {
                                        success = true;
                                        //if (File.Exists(user.ImagePath))
                                        //imagedata = File.ReadAllBytes(user.ImagePath);
                                        var ms = new MemoryStream();
                                        if (user.Image != null)
                                        {
                                            user.Image.Save(ms, ImageFormat.Tiff); //2015.05.30.
                                            imagedata = ms.ToArray(); //2015.05.30.
                                        }
                                        else
                                        {
                                            success = false;
                                            imagedata = new byte[0];
                                        }
                                    }
                                    else
                                    {
                                        success = false;
                                        imagedata = new byte[0];
                                    }
                                    new PacketSender(new PDGetImage(success, user.PicUpdateTime, imagedata), pf.ID).Send(SendBackEP);
                                }
                                break;
                            case UpdateType.UpdateMessages:
                                {
                                    PDUpdateMessages data = (PDUpdateMessages)pf.EData;
                                    bool success = true;
                                    //ChatPanel cp = ChatPanel.GetChatPanelByUsers(data.Users.Select(entry => UserInfo.Select(entry)));
                                    var userinfos = data.Users.Select(entry => UserInfo.Select(entry)); //2015.06.16.
                                    if (userinfos.Except(UserInfo.KnownUsers.Where(entry => entry.IsPartner && entry.UserID != CurrentUser.UserID)).Count() == 0)
                                    { //2015.06.16. - Leellenőrzi, hogy van-e ismerőse a listában
                                        success = false; //2015.06.25.
                                        //break;
                                    }
                                    else
                                    {
                                        ChatPanel cp = ChatPanel.GetChatPanelByUsers(userinfos);
                                        //if (cp != null)
                                        if (cp == null) //2015.05.16.
                                            cp = ChatPanel.Create(data.Users.Select(entry => UserInfo.Select(entry))); //2015.05.16.
                                        cp.ShowReceivedMessageT(UserInfo.Select(pf.EUserID), data.Message, data.Time);
                                    }
                                    new PacketSender(new PDUpdateMessages(success), pf.ID).Send(SendBackEP);
                                }
                                break;
                            case UpdateType.SetKey:
                                break; //TODO
                            case UpdateType.SendImage: //2015.06.25.
                                {
                                    PDSendImage data = (PDSendImage)pf.EData;
                                    //ChatPanel cp = ChatPanel.GetChatPanelByUsers(data.Users.Select(entry => UserInfo.Select(entry)));
                                    bool success = true;
                                    var userinfos = data.Users.Select(entry => UserInfo.Select(entry));
                                    if (userinfos.Except(UserInfo.KnownUsers.Where(entry => entry.IsPartner && entry.UserID != CurrentUser.UserID)).Count() == 0)
                                    {
                                        success = false;
                                        //break; //Leellenőrzi, hogy van-e ismerőse a listában
                                    }
                                    ChatPanel cp = ChatPanel.GetChatPanelByUsers(userinfos);
                                    //if (cp != null)
                                    if (cp == null)
                                        cp = ChatPanel.Create(data.Users.Select(entry => UserInfo.Select(entry)));
                                    cp.ShowReceivedImageT(UserInfo.Select(pf.EUserID), data.Image, data.Time);
                                    new PacketSender(new PDSendImage(success), pf.ID).Send(SendBackEP);
                                }
                                break;
                            case UpdateType.SendFile: //2015.06.30.
                                {
                                    PDSendFile data = (PDSendFile)pf.EData;
                                    bool success = true;
                                    var userinfos = data.Users.Select(entry => UserInfo.Select(entry));
                                    if (userinfos.Except(UserInfo.KnownUsers.Where(entry => entry.IsPartner && entry.UserID != CurrentUser.UserID)).Count() == 0)
                                    {
                                        success = false;
                                    }
                                    ChatPanel cp = ChatPanel.GetChatPanelByUsers(userinfos);
                                    if (cp == null)
                                        cp = ChatPanel.Create(data.Users.Select(entry => UserInfo.Select(entry)));
                                    long rprogress = cp.ShowReceivedFileT(UserInfo.Select(pf.EUserID), data.File, data.Time, data.Progress);
                                    new PacketSender(new PDSendFile(success, rprogress, CurrentUser.IPs, CurrentUser.Port), pf.ID).Send(SendBackEP);
                                }
                                break;
                            default: //2015.06.25.
                                throw new NotImplementedException("This type (" + pf.PacketType + ") is not handled in code."); //2015.06.25.
                        }
                        if (Break)
                            break;
                        /*byte isresponse; //2014.09.15.
                        Networking.UpdateType updatetype; //2014.09.15.
                        int keyversion; //2014.09.15.
                        int port; //2014.12.19.
                        int userid; //2014.09.15.
                        byte[] data; //2014.09.15.
                        Networking.ParsePacket(resp, out isresponse, out updatetype, out keyversion, out port, out userid, out data); //2014.09.15.
                        if (!UserInfo.IPs.Any(entry => entry.Address.Equals(remoteEP.Address)) //2014.12.19. - A port nem ugyanaz, ráadásul a == nem hívja meg a .Equals metódust
                            && updatetype != Networking.UpdateType.LoginUser)
                            break;
                        if (updatetype == Networking.UpdateType.ListUpdate)
                        {
                            Networking.ParseUpdateInfo(new byte[][] { resp }); //2014.12.22. - A funkció az egész packet-re számít, nem csak a data-ra
                        }
                        else if (updatetype == Networking.UpdateType.UpdateMessages)
                        {
                            string[] response = Networking.GetStrings(data, 0);
                            string[] tmp = response[0].Split(',');
                            List<UserInfo> tmp2 = new List<UserInfo>();
                            tmp2.Add(UserInfo.Select(userid)); //Adja hozzá a küldőt is
                            tmp2.AddRange(tmp.Select(entry => UserInfo.Select(Int32.Parse(entry)))); //2014.10.24.
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

                            var ep = new IPEndPoint(remoteEP.Address, port);
                            if (!UserInfo.IPs.Any(entry => entry == ep))
                                UserInfo.IPs.Add(ep);
                            string retstr = "";
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
                                    if (uid != CurrentUser.UserID) //2014.11.29.
                                        retstr += uid + "_lastupdate=" + Storage.LoggedInSettings["userinfo_" + uid + "_lastupdate"]; //2014.11.29. - Arra az időpontra állítsa, amikor ő kapta a frissítést, így ez elvileg az eredeti frissítés időpontját mutatja kb. - Ezért a sajátját biztosan frissen kell tartani
                                    else
                                        retstr += uid + "_lastupdate=" + Program.DateTimeToUnixTime(DateTime.Now);
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
                            string[] ips = ipstr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            var ip = ips.Select(entry => IPAddress.Parse(entry));
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
                        }*/
                    } while (false);
                }
            }
        }
    }
}
