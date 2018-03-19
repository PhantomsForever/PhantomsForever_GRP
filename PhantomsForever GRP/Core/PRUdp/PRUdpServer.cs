using PhantomsForever_GRP.Core.Extensions;
using PhantomsForever_GRP.Core.Objects;
using PhantomsForever_GRP.Core.RMC;
using PhantomsForever_GRP.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.PRUdp
{
    public class PRUdpServer
    {
        private Socket _server;
        private byte[] Buffer = new byte[8192];
        public PRUdpServer()
        {
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }
        public void Listen(int port)
        {
            _server.Bind(new IPEndPoint(IPAddress.Any, port));
            EndPoint tempEndpoint = new IPEndPoint(IPAddress.Any, 0x00000000);
            _server.BeginReceiveFrom(Buffer, 0, Buffer.Length, SocketFlags.None, ref tempEndpoint, PRUdpReceiveCallback, null);
        }

        private void PRUdpReceiveCallback(IAsyncResult ar)
        {
            try
            {
                EndPoint tempEndpoint = new IPEndPoint(IPAddress.Any, 0x00000000);
                var transferred = _server.EndReceiveFrom(ar, ref tempEndpoint);
                if (transferred <= 0)
                    throw new Exception();
                var actualbytes = new byte[transferred];
                Array.Copy(Buffer, actualbytes, transferred);
                Handle(actualbytes, (IPEndPoint)tempEndpoint);
            }
            catch(Exception)
            {

            }
            try
            {
                EndPoint tempEndpoint = new IPEndPoint(IPAddress.Any, 0x00000000);
                _server.BeginReceiveFrom(Buffer, 0, Buffer.Length, SocketFlags.None, ref tempEndpoint, PRUdpReceiveCallback, null);
            }
            catch(Exception)
            {

            }
        }
        private void Handle(byte[] data, IPEndPoint endpoint)
        {
            var hex = data.ToHex();
            Console.WriteLine("Received: " + hex);
            if(hex.Equals("3f3120000000000000000000000034322000"))
            {
                //var packet = PRUdpPacket.Decode(data);
                var resp = "313f08000000000000000f8744db6a1b1887";
                //var packet1 = PRUdpPacket.Decode(resp.FromHex());
                Send(resp.FromHex(), endpoint);
            }
            else
            {
                var packet = PRUdpPacket.Decode(data);
                if(packet.Flags.Contains(PacketFlags.FLAG_NEED_ACK))
                {
                    var response = new PRUdpPacket()
                    {
                        Flags = new PacketFlags[] { PacketFlags.FLAG_ACK },
                        Type = PacketTypes.DATA,
                        SessionId = "a1",
                        Signature = packet.Signature,
                        ConnectionSignature = packet.ConnectionSignature,
                        PacketId = packet.PacketId,
                        FragmentId = packet.FragmentId
                    };
                    var p = response.Encode().ToHex();
                    //Console.WriteLine("Sent: " + p);
                    //Send(p.FromHex(), endpoint);
                }
                if(packet.Type == PacketTypes.PING)
                {
                    //todo, ping handling code
                }
                else if(packet.Type == PacketTypes.CONNECT)
                {
                    var response = new PRUdpPacket()
                    {
                        Flags = new PacketFlags[] { PacketFlags.FLAG_ACK },
                        Type = PacketTypes.CONNECT,
                        PacketId = "0000",
                        SessionId = packet.SessionId,
                        Signature = packet.Signature,
                        ConnectionSignature = packet.ConnectionSignature
                    };
                    var p = response.Encode();
                    Console.WriteLine("Sent: " + p.ToHex());
                    Send(p, endpoint);
                }
                else if(packet.Type == PacketTypes.DATA)
                {
                    var rvconndata = new RVConnectionData()
                    {
                        StationUrl = "prudps:/127.0.0.1:10264"
                    };
                    var rmc = new RMCPayload()
                    {
                        CallId = packet.RMCPayload.CallId,
                        MethodId = packet.RMCPayload.MethodId,
                        ProtocolId = packet.RMCPayload.ProtocolId,
                        Payload = rvconndata.Encode()
                    };
                    var response = new PRUdpPacket()
                    {
                        Flags = new PacketFlags[] { PacketFlags.FLAG_RELIABLE, PacketFlags.FLAG_NEED_ACK },
                        Type = PacketTypes.DATA,
                        SessionId = packet.SessionId,
                        Signature = packet.Signature,
                        Payload = rmc.Encode(),
                        ConnectionSignature = packet.ConnectionSignature
                    };
                    var p = response.Encode().ToHex();
                    Console.WriteLine("Sent: " + p);
                    Send(p.FromHex(), endpoint);
                }
                else
                {
                    Console.WriteLine("Unknown packet: " + hex);
                }
            }
        }
        public void Send(byte[] packet, IPEndPoint endpoint)
        {
            _server.SendTo(packet, endpoint);
        }
    }
}