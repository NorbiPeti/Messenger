using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
            private static bool Loaded = false; //2015.06.25.
            static PacketData()
            {
                if (!Loaded)
                { //2015.06.25.
                    foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                    { //2015.06.25.
                        if (typeof(PacketData).IsAssignableFrom(type) && !type.IsAbstract)
                        {
                            var pd = (PacketData)Activator.CreateInstance(type, true);
                            PacketDataTypes.Add(pd.PacketType, type);
                        }
                    }
                    Loaded = true;
                }
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
                if (!Response)
                {
                    UserInfos = new Dictionary<int, int>();
                    while (ms.Position < ms.Length)
                    {
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
            public PDLogoutUser()
            {
            }
            /*/// <summary>
            /// Csak a FromBytes miatt
            /// </summary>
            private PDLogoutUser()
            {
            }*/

            public override byte[] ToBytes()
            {
                return new byte[] { 0x01 };
            }

            public override PacketData FromBytes(byte[] bytes)
            {
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
            public double PicUpdateTime { get; private set; }
            public bool RSuccess { get; private set; }
            public double RPicUpdateTime { get; private set; }
            public byte[] RImageData { get; private set; }
            public PDGetImage(int userid, double picupdatetime)
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
            public PDGetImage(bool success, double picupdatetime, byte[] imagedata)
            {
                RSuccess = success;
                RPicUpdateTime = picupdatetime;
                RImageData = imagedata;
            }

            public override byte[] ToBytes()
            {
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                if (!Response)
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
                if (!Response)
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
            public int[] Users { get; private set; }
            public string Message { get; private set; }
            public double Time { get; private set; } //2015.05.15.
            public bool RSuccess { get; private set; }
            public PDUpdateMessages(int[] users, string message, double time)
            {
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
        public class PDSendImage : PacketData
        { //2015.06.25.
            public Image Image { get; private set; }
            public int[] Users { get; private set; }
            public double Time { get; private set; }
            public bool RSuccess { get; private set; }
            public PDSendImage(int[] users, Image image, double time) //2015.06.26.
            {
                Image = image;
                Users = users; //2015.07.04.
                Time = time; //2015.07.04.
            }
            public PDSendImage(bool success)
            {
                RSuccess = success;
            }
            private PDSendImage()
            {

            }

            public override byte[] ToBytes()
            {
                if (!Response)
                {
                    using (var ms = new MemoryStream())
                    {
                        BinaryWriter bw = new BinaryWriter(ms);
                        bw.Write(Users.Length);
                        foreach (int user in Users)
                            bw.Write(user);
                        bw.Write(Time);
                        using (var imgms = new MemoryStream())
                        {
                            Image.Save(imgms, ImageFormat.Tiff);
                            byte[] bytes = imgms.ToArray();
                            bw.Write(bytes.Length);
                            bw.Write(bytes);
                        }
                        return ms.ToArray();
                    }
                }
                else
                    return BitConverter.GetBytes(RSuccess);
            }

            public override PacketData FromBytes(byte[] bytes)
            {
                if (!Response)
                {
                    using (var ms = new MemoryStream(bytes))
                    {
                        BinaryReader br = new BinaryReader(ms);
                        Users = new int[br.ReadInt32()];
                        for (int i = 0; i < Users.Length; i++)
                            Users[i] = br.ReadInt32();
                        Time = br.ReadDouble();
                        int imglen = br.ReadInt32();
                        byte[] img = br.ReadBytes(imglen);
                        using (var imgms = new MemoryStream(img))
                        {
                            Image = Image.FromStream(imgms);
                        }
                    }
                }
                else
                    RSuccess = BitConverter.ToBoolean(bytes, 0);
                return this;
            }

            public override UpdateType PacketType
            {
                get { return UpdateType.SendImage; }
            }
        }
        public class PDSendFile : PacketData
        { //2015.06.29.
            public const int BufferLength = 2048;
            public FileInfo File { get; private set; }
            public int[] Users { get; private set; }
            public double Time { get; private set; }
            public long Progress { get; private set; }
            public long RProgress { get; private set; } //2015.06.30.
            public bool RSuccess { get; private set; }
            public IEnumerable<IPAddress> RespIPs { get; private set; } //2015.06.30.
            public int RPort { get; private set; } //2015.06.30.
            public PDSendFile(int[] users, FileInfo file, double time, long progress)
            {
                Users = users;
                File = file;
                Progress = progress;
            }
            public PDSendFile(bool success, long progress, IEnumerable<IPAddress> ip, int port)
            {
                RSuccess = success;
                RProgress = progress; //2015.06.30.
                RespIPs = ip; //2015.06.30.
                RPort = port; //2015.06.30.
            }
            private PDSendFile()
            {

            }

            public override byte[] ToBytes()
            {
                if (!Response)
                {
                    using (var ms = new MemoryStream())
                    {
                        BinaryWriter bw = new BinaryWriter(ms);
                        bw.Write(Users.Length);
                        foreach (int user in Users)
                            bw.Write(user);
                        bw.Write(Time);
                        bw.Write(Progress);
                        bw.Write(File.Name); //2015.06.30.
                        using (var fs = File.OpenRead())
                        {
                            byte[] bytes = new byte[BufferLength];
                            fs.Seek(Progress, SeekOrigin.Begin);
                            fs.Read(bytes, 0, BufferLength);
                            bw.Write(bytes);
                        }
                        return ms.ToArray();
                    }
                }
                else
                    return BitConverter.GetBytes(RSuccess).Concat(BitConverter.GetBytes(RProgress))
                        .Concat(BitConverter.GetBytes(RPort)) //2015.06.30.
                        .Concat(BitConverter.GetBytes(RespIPs.Count())) //2015.06.30.
                        .Concat(BitConverter.GetBytes(RespIPs.First().GetAddressBytes().Length)) //2015.06.30.
                        .Concat(RespIPs.Select(entry => entry.GetAddressBytes().AsEnumerable()) //2015.06.30.
                            .Aggregate((entry1, entry2) => entry1.Concat(entry2))).ToArray(); //2015.06.30.
            }

            public override PacketData FromBytes(byte[] bytes)
            {
                if (!Response)
                {
                    using (var ms = new MemoryStream(bytes))
                    {
                        BinaryReader br = new BinaryReader(ms);
                        Users = new int[br.ReadInt32()];
                        for (int i = 0; i < Users.Length; i++)
                            Users[i] = br.ReadInt32();
                        Time = br.ReadDouble();
                        Progress = br.ReadInt64();
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + Language.Translate(Language.StringID.ReceivedFiles); //2015.06.30.
                        if (!Directory.Exists(path)) //2015.06.30.
                            Directory.CreateDirectory(path); //2015.06.30.
                        string filename = br.ReadString(); //2015.06.30.
                        if (filename.Length == 0)
                            throw new InvalidOperationException("The file name is not sent."); //2015.06.30.
                        filename = Path.GetInvalidFileNameChars().Select(entry => filename.Replace(entry.ToString(), " ")).Last(); //2015.06.30.
                        File = new FileInfo(path + Path.DirectorySeparatorChar + filename); //2015.06.30.
                        using (var fs = File.OpenWrite())
                        {
                            byte[] buf = new byte[BufferLength];
                            fs.Seek(Progress, SeekOrigin.Begin);
                            br.Read(buf, 0, BufferLength);
                            fs.Write(buf, 0, BufferLength);
                        }
                    }
                }
                else
                {
                    RSuccess = BitConverter.ToBoolean(bytes, 0);
                    int x = 1; //2015.06.30.
                    RProgress = BitConverter.ToInt64(bytes, x); //2015.06.30.
                    x += sizeof(long); //2015.06.30.
                    int len = BitConverter.ToInt32(bytes, x); //2015.06.30.
                    x += sizeof(int); //2015.06.30.
                    int port = BitConverter.ToInt32(bytes, x); //2015.06.30.
                    IPAddress[] ips = new IPAddress[len]; //2015.06.30.
                    x += sizeof(int); //2015.06.30.
                    var ipbytes = new byte[BitConverter.ToInt32(bytes, x)]; //2015.06.30.
                    x += sizeof(int); //2015.06.30.
                    for (int i = 0; i < len; i++)
                    { //2015.06.30.
                        Array.Copy(bytes, x, ipbytes, 0, ipbytes.Length); //2015.06.30.
                        ips[i] = new IPAddress(ipbytes); //2015.06.30.
                        x += ipbytes.Length; //2015.06.30.
                    }
                    RespIPs = ips; //2015.06.30.
                    RPort = port;
                }
                return this;
            }

            public override UpdateType PacketType
            {
                get { return UpdateType.SendFile; }
            }
        }
    }
}
