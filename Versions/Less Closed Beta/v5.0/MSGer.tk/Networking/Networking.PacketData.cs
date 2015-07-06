using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MSGer.tk
{
    partial class Networking
    { //2015.03.28-29.
        /// <summary>
        /// Prefixes:
        /// R - Used in response
        /// </summary>
        public abstract class PacketData
        {
            public abstract byte[] ToBytes();
            /// <summary>
            /// Load the object from bytes
            /// </summary>
            /// <param name="bytes"></param>
            /// <returns>Should always return current instance (this)</returns>
            public abstract PacketData FromBytes(byte[] bytes);
            public abstract UpdateType PacketType { get; }

            public static readonly Dictionary<UpdateType, Type> PacketDataTypes = new Dictionary<UpdateType, Type>(Enum.GetNames(typeof(UpdateType)).Length);
            static PacketData()
            {
                PacketDataTypes.Add(UpdateType.ListUpdate, typeof(PDListUpdate));
                PacketDataTypes.Add(UpdateType.LoginUser, typeof(PDLoginUser));
                PacketDataTypes.Add(UpdateType.LogoutUser, typeof(PDLogoutUser));
                PacketDataTypes.Add(UpdateType.UpdateMessages, typeof(PDUpdateMessages));
                PacketDataTypes.Add(UpdateType.GetImage, typeof(PDGetImage));
                PacketDataTypes.Add(UpdateType.SetKey, typeof(PDSetKey));
            }

            public bool Response; //2015.04.03.
        }
        public class PDListUpdate : PacketData
        {
            /// <summary>
            /// Same as PDLoginUser.RStrings
            /// </summary>
            public string[] Strings { get; private set; }
            public bool RSuccess { get; private set; }
            public PDListUpdate(string[] strings)
            {
                Strings = strings;
            }
            public PDListUpdate(bool success)
            {
                RSuccess = success;
            }
            /// <summary>
            /// Csak a FromBytes miatt
            /// </summary>
            private PDListUpdate()
            {
            }
            public override byte[] ToBytes()
            {
                if (!Response)
                {
                    string str = "";
                    for (int i = 0; i < Strings.Length; i++)
                        str += Strings[i] + "\n";
                    return Encoding.Unicode.GetBytes(str);
                }
                else
                    return BitConverter.GetBytes(RSuccess);
            }

            public override PacketData FromBytes(byte[] bytes)
            {
                if (!Response)
                    Strings = Encoding.Unicode.GetString(bytes).Split(new string[] { "\n", "\n\r" }, StringSplitOptions.RemoveEmptyEntries);
                else
                    RSuccess = BitConverter.ToBoolean(bytes, 0);
                return this;
            }

            public override UpdateType PacketType
            {
                get
                {
                    return UpdateType.ListUpdate;
                }
            }
        }
        public class PDLoginUser : PacketData
        {
            /// <summary>
            /// Same as PDListUpdate.Strings
            /// </summary>
            public string[] RStrings { get; private set; }
            //public IEnumerable<KeyValuePair<int, int>> UserInfos { get; private set; }
            public Dictionary<int, int> UserInfos { get; private set; }
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="userinfos">Key: UserID, Value: LastUpdate</param>
            public PDLoginUser(IEnumerable<KeyValuePair<int, int>> userinfos)
            {
                UserInfos = userinfos.ToDictionary(entry => entry.Key, entry => entry.Value);
            }
            public PDLoginUser(string[] strings)
            {
                RStrings = strings;
            }
            /// <summary>
            /// Csak a FromBytes miatt
            /// </summary>
            private PDLoginUser()
            {
            }

            public override byte[] ToBytes()
            {
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                if (!Response)
                {
                    foreach (var userinfo in UserInfos)
                    {
                        bw.Write(userinfo.Key);
                        bw.Write(userinfo.Value);
                    }
                }
                else
                {
                    string str = "";
                    for (int i = 0; i < RStrings.Length; i++)
                    {
                        str += RStrings[i] + "\n";
                    }
                    bw.Write(str);
                }
                return ms.ToArray();
            }

            public override PacketData FromBytes(byte[] bytes)
            {
                MemoryStream ms = new MemoryStream(bytes);
                BinaryReader br = new BinaryReader(ms);
                if(!Response)
                {
                    UserInfos = new Dictionary<int, int>();
                    while (ms.Position < ms.Length)
                    {
                        //UserInfos.Add(br.ReadInt32(), br.ReadInt32());
                        int userid = br.ReadInt32();
                        int lastupdate = br.ReadInt32();
                        if (!UserInfos.ContainsKey(userid))
                            UserInfos.Add(userid, lastupdate);
                    }
                }
                else
                {
                    RStrings = br.ReadString().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                }
                return this;
            }

            public override UpdateType PacketType
            {
                get
                {
                    return UpdateType.LoginUser;
                }
            }
        }
        public class PDLogoutUser : PacketData
        {
            //public string IPs { get; private set; }
            //public IPEndPoint[] IPs { get; private set; } - Nem is kell, bejelentkezéskor sem kellett

            public PDLogoutUser()
            {
                //IPs=ips;
            }
            /*/// <summary>
            /// Csak a FromBytes miatt
            /// </summary>
            private PDLogoutUser()
            {
            }*/

            public override byte[] ToBytes()
            {
                /*if (!Response)
                    //return Encoding.Unicode.GetBytes(IPs);
                    *{
                        var ms = new MemoryStream();
                        var bw = new BinaryWriter(ms);
                        bw.Write(IPs.Length);
                        foreach (IPEndPoint ip in IPs)
                        {
                            bw.Write(ip.Address.GetAddressBytes());
                            bw.Write(ip.Port);
                        }
                        return ms.ToArray();
                    }*
                else
                    return new byte[] { 0x01 };*/
                return new byte[] { 0x01 };
            }

            public override PacketData FromBytes(byte[] bytes)
            {
                /*if (!Response)
                //IPs = Encoding.Unicode.GetString(bytes);
                {
                    var ms = new MemoryStream(bytes);
                    var br = new BinaryReader(ms);
                    int len = br.ReadInt32();
                    IPs = new IPEndPoint[len];
                    for (int i = 0; i < len; i++)
                    {
                        IPAddress ip = new IPAddress(br.ReadBytes(4));
                        int port = br.ReadInt32();
                        IPs[i] = new IPEndPoint(ip, port);
                    }
                }*/
                return this;
            }

            public override UpdateType PacketType
            {
                get { return UpdateType.LogoutUser; }
            }
        }
        public class PDGetImage : PacketData
        {
            public int UserID { get; private set; }
            public int PicUpdateTime { get; private set; }
            public bool RSuccess { get; private set; }
            public int RPicUpdateTime { get; private set; }
            public byte[] RImageData { get; private set; }
            public PDGetImage(int userid, int picupdatetime)
            {
                UserID = userid;
                PicUpdateTime = picupdatetime;
            }
            /// <summary>
            /// Csak a FromBytes miatt
            /// </summary>
            private PDGetImage()
            {
            }
            public PDGetImage(bool success, int picupdatetime, byte[] imagedata)
            {
                RSuccess = success;
                RPicUpdateTime = picupdatetime;
                RImageData = imagedata;
            }
            
            public override byte[] ToBytes()
            {
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                if(!Response)
                {
                    bw.Write(UserID);
                    bw.Write(PicUpdateTime);
                }
                else
                {
                    bw.Write(RSuccess);
                    bw.Write(RPicUpdateTime);
                    bw.Write(RImageData);
                }
                return ms.ToArray();
            }

            public override PacketData FromBytes(byte[] bytes)
            {
                MemoryStream ms = new MemoryStream(bytes);
                BinaryReader br = new BinaryReader(ms);
                if(!Response)
                {
                    UserID = br.ReadInt32();
                    PicUpdateTime = br.ReadInt32();
                }
                else
                {
                    RSuccess = br.ReadBoolean();
                    RPicUpdateTime = br.ReadInt32();
                    br.Read(RImageData, 0, (int)(ms.Length - ms.Position));
                }
                return this;
            }

            public override UpdateType PacketType
            {
                get { return UpdateType.GetImage; }
            }
        }
        public class PDUpdateMessages : PacketData
        {
            //public ChatPanel Chat { get; private set; }
            public int[] Users { get; private set; }
            public string Message { get; private set; }
            public double Time { get; private set; } //2015.05.15.
            public bool RSuccess { get; private set; }
            public PDUpdateMessages(int[] users, string message, double time)
            {
                //Chat = chat;
                Users = users;
                Message = message;
                Time = time;
            }
            public PDUpdateMessages(bool success)
            {
                RSuccess = success;
            }
            /// <summary>
            /// Csak a FromBytes miatt
            /// </summary>
            private PDUpdateMessages()
            {
            }

            /*
            public UpdateType get_PacketType()
            {
                return null;
            }
            ^^
             Type 'MSGer.tk.Networking.PDUpdateMessages' already reserves a member called 'get_PacketType' with the same parameter types
            */

            public override byte[] ToBytes()
            {
                if (!Response)
                {
                    string sendstr = "";
                    foreach (int user in Users)
                    {
                        sendstr += user + ",";
                    }
                    sendstr = sendstr.Remove(sendstr.Length - 1);
                    sendstr += ";" + Time; //2015.05.15.
                    sendstr += ";" + Message;
                    return Encoding.Unicode.GetBytes(sendstr);
                }
                else
                    return BitConverter.GetBytes(RSuccess);
            }

            public override PacketData FromBytes(byte[] bytes)
            {
                if (!Response)
                {
                    string str = Encoding.Unicode.GetString(bytes);
                    int index = str.IndexOf('\0');
                    if (index != -1)
                        str = str.Remove(0, index + 1);
                    index = str.IndexOf('\0');
                    if (index != -1)
                        str = str.Remove(index, str.Length - index);
                    string[] strs = str.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    Users = strs[0].Split(',').Select(entry => int.Parse(entry)).ToArray();
                    Time = double.Parse(strs[1]);
                    Message = strs[2];
                }
                else
                    RSuccess = BitConverter.ToBoolean(bytes, 0);
                return this;
            }

            public override UpdateType PacketType
            {
                get { return UpdateType.UpdateMessages; }
            }
        }
        /*public class PDKeepAlive : PacketData
        {
            public override byte[] ToBytes(bool response)
            {
                throw new NotImplementedException();
            }

            public override PacketData FromBytes(byte[] bytes, bool response)
            {
                throw new NotImplementedException();
            }

            public override UpdateType PacketType
            {
                get { return UpdateType.KeepAlive; }
            }
        }*/
        public class PDSetKey : PacketData
        {
            public int KeyIndex { get; private set; }
            public PDSetKey(int keyindex)
            {
                KeyIndex = keyindex;
            }
            public PDSetKey()
            {

            }

            public override byte[] ToBytes()
            {
                if (!Response)
                    return BitConverter.GetBytes(KeyIndex);
                else
                    return new byte[1] { 0x01 };
            }

            public override PacketData FromBytes(byte[] bytes)
            {
                if (!Response)
                    KeyIndex = BitConverter.ToInt32(bytes, 0);
                return this;
            }

            public override UpdateType PacketType
            {
                get { return UpdateType.SetKey; }
            }
        }
    }
}
