using PhantomsForever_GRP.Core.Extensions;
using PhantomsForever_GRP.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.PRUdp
{
    public class PRUdpPacket
    {
        public PacketTypes Type { get; set; }
        public PacketFlags[] Flags { get; set; }
        public string SessionId { get; set; }
        public string Signature { get; set; }
        public string Checksum { get; set; }
        public byte[] Encode()
        {
            var packet = "313f";
            var s = Convert.ToString((int)Type, 2);
            if (s.Length != 3)
                s = s.PadLeft(3 - s.Length + 1, '0');
            var iii = 0;
            foreach(var fl in Flags)
                iii += (int)fl;
            var s1 = Convert.ToString(iii, 2);
            if(s1.Length != 5)
                s1 = s1.PadLeft(5 - s1.Length + 1, '0');
            var typenflags = s1 + s;
            int[] bits = typenflags.PadLeft(8, '0').Select(c => int.Parse(c.ToString())).ToArray();
            var b = bits.ToBitArray().ToHex();
            packet += b;
            packet += "00";
            packet += Checksum;
            return packet.FromHex();
        }
        public static PRUdpPacket Decode(byte[] bytes)
        {
            var hex = bytes.ToHex();
            var str = hex.Substring(4, 2);
            string sessionid = "", sig = "", seqnum = "";
            var typenflags = str.FromHexToBits();
            var checksum = hex.Substring(hex.Length - 8);
            int[] data = new int[typenflags.Count];
            for(int i = 0; i < typenflags.Count; i++)
            {
                if ((bool)typenflags[i])
                    data[i] = 1;
                else
                    data[i] = 0;
            }
            var flags = Convert.ToString(data[0]) + Convert.ToString(data[1]) + Convert.ToString(data[2]) + Convert.ToString(data[3]) + Convert.ToString(data[4]);
            var type = Convert.ToString(data[5]) + Convert.ToString(data[6]) + Convert.ToString(data[7]);
            var packet = new PRUdpPacket();
            packet.Type = (PacketTypes)Convert.ToInt32(type, 2);
            packet.Flags = PFlags.ParseFlags(flags);
            if(packet.Type == PacketTypes.SYN || packet.Type == PacketTypes.CONNECT)
            {
                sessionid = hex.Substring(6, 2);
                sig = hex.Substring(8, 8);
                seqnum = hex.Substring(16, 4);
            }
            else
            {
                sessionid = hex.Substring(6, 2);
                seqnum = hex.Substring(8, 4);
            }
            packet.SessionId = sessionid;
            packet.Signature = sig;
            packet.Checksum = checksum;
            return packet;
        }
    }
}