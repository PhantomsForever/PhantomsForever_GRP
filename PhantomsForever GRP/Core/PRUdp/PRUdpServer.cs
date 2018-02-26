using PhantomsForever_GRP.Core.Extensions;
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
            if(hex.Equals("3f3120000000000000000000000034322000"))
            {
                var resp = "313f08000000000000000f8744db6a1b1887";
                Send(resp.FromHex(), endpoint);
            }
            else
            {
                var packet = PRUdpPacket.Decode(data);
                if(packet.Type == PacketTypes.CONNECT)
                {
                    var response = new PRUdpPacket()
                    {
                        Flags = new PacketFlags[]{ PacketFlags.FLAG_ACK },
                        Type = PacketTypes.CONNECT,
                        SessionId = packet.SessionId,
                        Signature = packet.Signature
                    };
                    var p = response.Encode();
                    Send(p, endpoint);
                }
                else if(packet.Type == PacketTypes.DATA)
                {
                    //do interesting stuff with data packet
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