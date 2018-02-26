using PhantomsForever_GRP.Core.Extensions;
using System;
using System.Collections.Generic;
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
            _server.Listen(1500);
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
        }
        public void Send(byte[] packet, IPEndPoint endpoint)
        {
            _server.SendTo(packet, endpoint);
        }
    }
}