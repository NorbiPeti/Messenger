using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
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
                while (MainForm.PartnerListUpdateThread != null && Program.MainThread.IsAlive)
                {
                    do
                    {
                        PacketFormat pf;
                        IPEndPoint remoteEP;
                        if (!Networking.ReceiveUpdates(out pf, out remoteEP))
                            break;
                        IPEndPoint SendBackEP = new IPEndPoint(remoteEP.Address, pf.Port); //2015.05.10.
                        bool Break = false;
                        switch (pf.PacketType)
                        {
                            case UpdateType.ListUpdate:
                                {
                                    Networking.ParseUpdateInfo(new string[1][] { ((PDListUpdate)pf.EData).Strings });
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
                                    new PacketSender(new PDLoginUser(strs.ToArray()), pf.ID).Send(SendBackEP);
                                    break;
                                }
                            case UpdateType.LogoutUser:
                                { //2015.05.10. - Felesleges elküldenie az IP-címeket, mint a bejelentkezésnél
                                    UserInfo.Select(pf.EUserID).State = 0;
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
                                    var userinfos = data.Users.Select(entry => UserInfo.Select(entry)); //2015.06.16.
                                    if (userinfos.Except(UserInfo.KnownUsers.Where(entry => entry.IsPartner && entry.UserID != CurrentUser.UserID)).Count() == 0)
                                    { //2015.06.16. - Leellenőrzi, hogy van-e ismerőse a listában
                                        success = false; //2015.06.25.
                                    }
                                    else
                                    {
                                        ChatPanel cp = ChatPanel.GetChatPanelByUsers(userinfos);
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
                                    bool success = true;
                                    var userinfos = data.Users.Select(entry => UserInfo.Select(entry));
                                    if (userinfos.Except(UserInfo.KnownUsers.Where(entry => entry.IsPartner && entry.UserID != CurrentUser.UserID)).Count() == 0)
                                    {
                                        success = false;
                                    }
                                    ChatPanel cp = ChatPanel.GetChatPanelByUsers(userinfos);
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
                    } while (false);
                }
            }
        }
    }
}
