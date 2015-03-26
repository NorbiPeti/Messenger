using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MSGer.tk
{
    public static class ICMPPacket
    {
        public const byte Type = ICMP_ECHO;
        public const byte SubCode = 0;
        public const byte RespType = ICMP_TIME_EXCEEDED;
        public const byte RespCode = 1;
        public const UInt16 CheckSum = 0;
        public const UInt16 Identifier = 52;
        public const UInt16 SequenceNumber = 84;
        public static readonly byte[] Data = Encoding.Unicode.GetBytes("MSGer.tk connection");

        public const byte ICMP_ECHO = 8;
        public const byte ICMP_REPLY = 0;
        public const byte ICMP_TIME_EXCEEDED = 11;

        public static byte[] CreateRequest() //2015.02.05.
        {
            var data = new List<byte>();
            data.Add(Type);
            data.Add(SubCode);
            data.AddRange(BitConverter.GetBytes(CheckSum));
            data.AddRange(BitConverter.GetBytes(Identifier));
            data.AddRange(BitConverter.GetBytes(SequenceNumber));
            data.AddRange(Data);
            return data.ToArray();
        }

        public static byte[] CreateReply()
        {
            //ICMPPacket packet = new ICMPPacket();
            var data = new List<byte>();
            data.Add(RespType);
            data.Add(RespCode);
            data.AddRange(BitConverter.GetBytes(CheckSum));
            data.AddRange(BitConverter.GetBytes((int)0)); //4 bytes unused
            data.AddRange(CreateRequest()); //Original packet
            return data.ToArray();
        }

        public static bool IsPacketGood(byte[] bytes)
        {
            return (bytes[0] == ICMP_TIME_EXCEEDED);
        }
    }
}
