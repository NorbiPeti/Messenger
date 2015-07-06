using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSGer.tk
{
    partial class Networking
    {
        public class PacketFormat
        {
            public int ID; //2015.05.15.
            public bool Response;
            public UpdateType PacketType
            {
                get
                {
                    return EData.PacketType;
                }
            }
            public int KeyIndex;
            public int Port;
            public int EUserID;
            public PacketData EData;

            internal PacketFormat(bool response, PacketData data, int id)
            {
                _PacketFormat(response, CurrentUser.KeyIndex, CurrentUser.Port, CurrentUser.UserID, data, id); //2015.05.24.
            }

            internal PacketFormat(bool response, int keyindex, int port, int userid, PacketData data, int id)
            {
                _PacketFormat(response, keyindex, port, userid, data, id);
            }

            private void _PacketFormat(bool response, int keyindex, int port, int userid, PacketData data, int id)
            {
                Response = response;
                KeyIndex = keyindex;
                Port = port;
                EUserID = userid;
                EData = data;
                EData.Response = Response; //2015.04.03.
                ID = id; //2015.05.15.
            }

            public byte[] ToBytes()
            {
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(ID);
                bw.Write(Response);
                bw.Write((byte)PacketType);
                bw.Write(CurrentUser.KeyIndex);
                bw.Write(Port);
                MemoryStream ems = new MemoryStream();
                BinaryWriter ebw = new BinaryWriter(ems); //2015.04.03.
                ebw.Write(EUserID);
                ebw.Write(EData.ToBytes());
                ebw.Flush();
                if (CurrentUser.Keys[KeyIndex] != null)
                    bw.Write(Storage.Encrypt(ems.ToArray(), CurrentUser.Keys[KeyIndex]));
                else
                    bw.Write(Storage.Encrypt(ems.ToArray(), "ihavenokeys"));
                bw.Flush();
                return ms.ToArray();
            }

            public static PacketFormat FromBytes(byte[] bytes)
            {
                MemoryStream ms = new MemoryStream(bytes);
                BinaryReader br = new BinaryReader(ms);
                int id = br.ReadInt32();
                bool response = br.ReadBoolean();
                UpdateType packettype = (UpdateType)br.ReadByte();
                int keyindex = br.ReadInt32();
                int port = br.ReadInt32();
                byte[] ebytes = new byte[ms.Length - ms.Position];
                br.Read(ebytes, 0, ebytes.Length);
                byte[] uebytes;
                if (CurrentUser.Keys.Length > keyindex && CurrentUser.Keys[keyindex] != null)
                    uebytes = Storage.Decrypt(ebytes, true, CurrentUser.Keys[keyindex]);
                else
                    uebytes = Storage.Decrypt(ebytes, true, "ihavenokeys");
                MemoryStream ems = new MemoryStream(uebytes);
                BinaryReader ebr = new BinaryReader(ems);
                int userid = ebr.ReadInt32();
                PacketData data = (PacketData)Activator.CreateInstance(PacketData.PacketDataTypes[packettype], true);
                data.Response = response; //2015.04.03.
                data.FromBytes(uebytes);
                PacketFormat pf = new PacketFormat(response, keyindex, port, userid, data, id);
                return pf;
            }
        }
    }
}
