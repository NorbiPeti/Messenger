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
            //while (MainForm.PartnerListThreadActive && MainForm.MainThread.IsAlive)
            while (MainForm.LThread != null && MainForm.MainThread.IsAlive)
            //{ Az egészet ellenőrizni, akár egyszerűsíteni a pozíciókat
            {
                do
                {
                    //string[] row = Networking.SendRequest("getlist", "", 0, true).Split('ͦ'); //Lekéri a listát, és különválogatja egyben - 2014.02.28.
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
                    //if (resp[1] == (byte)Networking.UpdateType.ListUpdate) //0x01: getlist
                    if (updatetype == Networking.UpdateType.ListUpdate)
                    {
                        //throw new NotImplementedException(); //2014.08.28. - Elkezdtem magamtól használni ezeket :P
                        /*var tmp = new byte[resp.Length - 6]; //2014.08.30.
                        Array.Copy(resp, 6, tmp, 0, tmp.Length); //2014.08.30.
                        Networking.ParseUpdateInfo(new byte[][] { tmp }); //2014.08.30.*/
                        Networking.ParseUpdateInfo(new byte[][] { data }); //2014.09.15.
                    }
                    //Thread.Sleep(5000);
                    //else if (resp[1] == (byte)Networking.UpdateType.UpdateMessages) //0x02: updatemessages
                    else if (updatetype == Networking.UpdateType.UpdateMessages)
                    {
                        //string[] response = Networking.GetStrings(resp, 2); //0: response, 1: action
                        string[] response = Networking.GetStrings(data, 0);
                        string[] tmp = response[0].Split(',');
                        //int[] tmp2 = new int[tmp.Length];
                        List<int> tmp2 = new List<int>();
                        /*for (int i = 0; i < tmp.Length; i++)
                        {
                            tmp2[i] = Int32.Parse(tmp[i]);
                        }*/
                        //tmp2 = tmp.Select(entry => Int32.Parse(entry)).ToArray(); //2014.09.22.
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
                        //for (int x = 0; x + 2 < response.Length; x += 3)
                        //{
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
                        //}
                    }
                    //else if(resp[5]==0x00) //Nem adatküldés, kérelem
                    /*else if (resp[1] == (byte)Networking.UpdateType.SetState)
                    {
                        //byte[] tmpuid = new byte[4];
                        //Array.Copy(resp, 1, tmpuid, 0, 4); //UserID
                        //int uid = BitConverter.ToInt32(tmpuid, 0);
                        int uid = BitConverter.ToInt32(resp, 1);
                        if (uid == 0)
                            break;
                        for (int i = 0; i < UserInfo.KnownUsers.Count; i++)
                        {
                            if (UserInfo.KnownUsers[i].UserID == uid)
                            {
                                //byte[] tmparr = new byte[4];
                                //Array.Copy(resp, 6, tmparr, 0, 4);
                                //UserInfo.Partners[i].State = BitConverter.ToInt32(tmparr, 0);
                                UserInfo.KnownUsers[i].State = BitConverter.ToInt32(resp, 6);
                            }
                        }
                        Networking.SendUpdate(Networking.UpdateType.SetState, new byte[] { 0x01 }, true);
                    }
                    else if (resp[1] == (byte)Networking.UpdateType.GetImage)
                    {
                        int senderuid = BitConverter.ToInt32(resp, 6);
                        int imguid = BitConverter.ToInt32(resp, 10);
                        int picupdatetime = BitConverter.ToInt32(resp, 14);
                        int i;
                        for (i = 0; i < UserInfo.KnownUsers.Count; i++)
                        {
                            if (UserInfo.KnownUsers[i].UserID == imguid)
                            {
                                if (UserInfo.KnownUsers[i].PicUpdateTime >= picupdatetime)
                                {
                                    string tmp = Path.GetTempPath();
                                    List<byte> tmptmp = new List<byte>();
                                    tmptmp.AddRange(BitConverter.GetBytes(UserInfo.KnownUsers[i].PicUpdateTime));
                                    tmptmp.AddRange(File.ReadAllBytes(tmp + "\\MSGer.tk\\pictures\\" + imguid + ".png"));
                                    Networking.SendUpdate(Networking.UpdateType.GetImage, tmptmp.ToArray(), true);
                                }
                                break;
                            }
                        }
                        if (i == UserInfo.KnownUsers.Count)
                            Networking.SendUpdate(Networking.UpdateType.GetImage, new byte[] { 0x00 }, true);
                    }*/
                    /*else if (resp[1] == (byte)Networking.UpdateType.FindPeople)
                    {
                        //List<string> retstrs = new List<string>();
                        string retstr = "";
                        string name = Encoding.Unicode.GetString(resp, 5, resp.Length - 5);
                        if (CurrentUser.UserName.Contains(name))
                        {
                            //retstrs.Add(CurrentUser.UserName);
                            retstr += CurrentUser.UserName + 'ͦ';
                            retstr += CurrentUser.UserID + 'ͦ';
                            retstr += CurrentUser.Message + 'ͦ';
                            retstr += CurrentUser.State + 'ͦ';
                            retstr += CurrentUser.Name + 'ͦ';
                            retstr += CurrentUser.Email + 'ͦ';
                        }
                        for (int i = 0; i < UserInfo.KnownUsers.Count; i++)
                        {
                            if (UserInfo.KnownUsers[i].UserName.Contains(name))
                            {
                                retstr += UserInfo.KnownUsers[i].UserName + 'ͦ';
                                retstr += UserInfo.KnownUsers[i].UserID + 'ͦ';
                                retstr += UserInfo.KnownUsers[i].Message + 'ͦ';
                                retstr += UserInfo.KnownUsers[i].State + 'ͦ';
                                retstr += UserInfo.KnownUsers[i].Name + 'ͦ';
                                retstr += UserInfo.KnownUsers[i].Email + 'ͦ';
                            }
                        }
                        retstr.TrimEnd('ͦ');
                        Networking.SendUpdate(Networking.UpdateType.FindPeople, Encoding.Unicode.GetBytes(retstr), true);
                    }*/
                    /*else if (resp[5] == (byte)Networking.UpdateType.UpdateSettings)
                    {
                        string[] tmpstr = Encoding.Unicode.GetString(resp, 6, resp.Length - 5).Split('ͦ');
                        for (int i = 0; i < UserInfo.KnownUsers.Count; i++)
                        {
                            if (UserInfo.KnownUsers[i].UserID == Int32.Parse(tmpstr[0]))
                            {
                                UserInfo.KnownUsers[i].Name = tmpstr[1];
                                UserInfo.KnownUsers[i].Message = tmpstr[2];
                                break;
                            }
                        }
                    }*/
                    //else if (resp[1] == (byte)Networking.UpdateType.LoginUser)
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

                        //string str = "";
                        /*foreach (var entry in Storage.LoggedInSettings)
                        {
                            Storage.LoggedInSettings[entry.Key.Remove("userinfo_".Length)+"_"+"_updated"]
                            string tmp = entry.Key.Remove(0, "userinfo_".Length);
                            //Int32.Parse(tmp.Split('_')[0])
                            str += tmp;
                        }*/
                        //int userid = BitConverter.ToInt32(resp, 6);
                        //int userid = BitConverter.ToInt32(resp, 2);
                        //int iplen = BitConverter.ToInt32(resp, 6);
                        int iplen = BitConverter.ToInt32(data, 0);
                        string ip = Encoding.Unicode.GetString(data, 4, iplen);
                        if (!Storage.Settings["ips"].Contains(ip))
                            Storage.Settings["ips"] += ";" + ip;
                        string retstr = "";
                        //for (int i = 0; i < (resp.Length - 9) / 8; i++)
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
                        /*foreach (var entry in UserInfo.KnownUsers)
                        {
                            if (entry.UserID == userid)
                                entry.State = 1;
                            else
                            {
                                //entry.LastUpdate>
                            }
                        }*/
                        UserInfo.Select(userid).State = 1;
                        //Networking.SendUpdate(Networking.UpdateType.LoginUser, Storage.Encrypt(retstr), true);
                        Networking.SendUpdate(Networking.UpdateType.LoginUser, Encoding.Unicode.GetBytes(retstr), true);
                    }
                    //else if (resp[1] == (byte)Networking.UpdateType.LogoutUser)
                    else if (updatetype == Networking.UpdateType.LogoutUser)
                    { //2014.08.31. 0:32
                        //int len = BitConverter.ToInt32(resp, 6);
                        int len = BitConverter.ToInt32(data, 0);
                        string ipstr = Encoding.Unicode.GetString(data, 4, len);
                        //Storage.Settings["ips"].Substring(Storage.Settings["ips"].IndexOf(ipstr), ipstr.Length);
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
