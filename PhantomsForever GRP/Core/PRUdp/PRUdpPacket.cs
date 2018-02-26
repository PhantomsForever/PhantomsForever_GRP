using PhantomsForever_GRP.Core.Extensions;
using PhantomsForever_GRP.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public byte[] Encode()
        {
            var packet = "313f";
            string s = Convert.ToString((int)Type, 2); //Convert to binary in a string
            int[] bits = s.PadLeft(8, '0') // Add 0's from left
                         .Select(c => int.Parse(c.ToString())) // convert each char to int
                         .ToArray(); // Convert IEnumerable from select to Array
            var iii = 0;
            foreach(var fl in Flags)
            {
                iii += (int)fl;
            }
            string s1 = Convert.ToString(iii, 2); //Convert to binary in a string
            int[] bits1 = s.PadLeft(8, '0') // Add 0's from left
                         .Select(c => int.Parse(c.ToString())) // convert each char to int
                         .ToArray(); // Convert IEnumerable from select to Array
            return null;
        }
        public static PRUdpPacket Decode(byte[] bytes)
        {
            var hex = bytes.ToHex();
            var str = hex.Substring(4, 2);
            var sessionid = hex.Substring(6, 2);
            var sig = hex.Substring(8, 8);
            var seqnum = hex.Substring(16, 4);
            var typenflags = str.FromHexToBits();
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
            packet.SessionId = sessionid;
            packet.Signature = sig;
            return packet;
        }
    }
}