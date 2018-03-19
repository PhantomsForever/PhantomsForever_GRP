using PhantomsForever_GRP.Core.Cryptography;
using PhantomsForever_GRP.Core.Extensions;
using PhantomsForever_GRP.Core.Python;
using PhantomsForever_GRP.Core.RMC;
using PhantomsForever_GRP.Core.Utilities;
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
        public byte[] PacketId { get; set; }
        public byte[] FragmentId { get; set; }
        public byte[] SessionId { get; set; }
        public byte[] Payload { get; set; }
        public RMCPayload RMCPayload { get; set; }
        public byte[] Method { get; set; }
        public byte[] Signature { get; set; }
        public byte[] Checksum { get; set; }
        public byte[] ConnectionSignature { get; set; }
        public byte[] Encode()
        {
            using(var mem = new MemoryStream())
            using (var bw = new BinaryWriter(mem))
            {
                bw.Write("313f".FromHex());
                var s = Convert.ToString((int)Type, 2);
                if (s.Length != 3)
                    s = s.PadLeft(3 - s.Length + 1, '0');
                var iii = 0;
                foreach (var fl in Flags)
                    iii += (int)fl;
                var s1 = Convert.ToString(iii, 2);
                if (s1.Length != 5)
                    s1 = s1.PadLeft(5 - s1.Length + 1, '0');
                var typenflags = s1 + s;
                int[] bits = typenflags.PadLeft(8, '0').Select(c => int.Parse(c.ToString())).ToArray();
                var b = bits.ToBitArray().ToHex();
                bw.Write(b.FromHex());
                bw.Write(SessionId);
                if (Type != PacketTypes.DATA)
                    bw.Write(ConnectionSignature);
                else
                    if (!string.IsNullOrEmpty(ConnectionSignature.ToHex()))
                        bw.Write(ConnectionSignature.ToHex().Substring(0, ConnectionSignature.Length - 2).FromHex().Reverse().ToArray());//a2c1e170
                bw.Write(PacketId);
                if (Type == PacketTypes.DATA)
                    bw.Write(FragmentId);
                bw.Write("db44870f".FromHex());
                if (Payload != null)
                {
                    bw.Write("00".FromHex());
                    Payload = PythonScript.CompressPacketPayload(RC4.Encrypt(Encoding.ASCII.GetBytes("CD&ML"), Payload).ToHex()).Result.FromHex();
                    bw.Write("02".FromHex());
                    bw.Write(Payload);
                }
                bw.Write(CalculateChecksum(mem.ToArray().ToHex()));
                return mem.ToArray();
            }
        }
        public static string CalculateChecksum(string hex, string accesskey = "cH0on9AsIXx7")
        {
            return PythonScript.GetPacketChecksum(hex).Result;
        }
        public static PRUdpPacket Decode(byte[] bytes)
        {
            var packet = new PRUdpPacket();
            using (var br = new BinaryReader(new MemoryStream(bytes)))
            {
                var ports = br.ReadBytes(4);
                PacketTypes t;
                PacketFlags[] f;
                ParseTypenFlags(br.ReadBytes(1), out f, out t);
                packet.Type = t;
                packet.Flags = f;
                var sessid = br.ReadBytes(1);
                var sig = br.ReadBytes(4);
                var seqn = br.ReadBytes(2);
                var connsig = new byte[4];
                var fragid = new byte[1];
                var payload = "";
                if (packet.Type == PacketTypes.SYN || packet.Type == PacketTypes.CONNECT)
                {
                    connsig = br.ReadBytes(4);
                }
                else if (packet.Type == PacketTypes.DATA)
                {
                    fragid = br.ReadBytes(1);
                    var hex = bytes.ToHex();
                    payload = PythonScript.DecompressPacketPayload(RC4.Decrypt(Encoding.ASCII.GetBytes("CD&ML"), hex.Substring(22, hex.Length - 30).FromHex()).ToHex().Substring(2)).Result;
                }
                packet.SessionId = sessid;
                packet.Signature = sig;
                packet.ConnectionSignature = connsig;
                packet.FragmentId = fragid;
                packet.Payload = payload.FromHex();
            }
            return packet;
        }
        private static void ParseTypenFlags(byte[] arr, out PacketFlags[] flags, out PacketTypes types)
        {
            var typenflags = new BitArray(arr);
            int[] data = new int[typenflags.Count];
            for (int i = 0; i < typenflags.Count; i++)
            {
                if ((bool)typenflags[i])
                    data[i] = 1;
                else
                    data[i] = 0;
            }
            var flag = Convert.ToString(data[0]) + Convert.ToString(data[1]) + Convert.ToString(data[2]) + Convert.ToString(data[3]) + Convert.ToString(data[4]);
            var type = Convert.ToString(data[5]) + Convert.ToString(data[6]) + Convert.ToString(data[7]);
            types = (PacketTypes)Convert.ToInt32(type, 2);
            flags = PFlags.ParseFlags(flag);
        }
    }
}