using PhantomsForever_GRP.Core.Discord;
using PhantomsForever_GRP.Core.Extensions;
using PhantomsForever_GRP.Core.PRUdp;
using PhantomsForever_GRP.Core.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP
{
    class Program
    {
        static void Main(string[] args)
        {
            var wserver = new WebServer();
            wserver.Start();
            var pserver = new PRUdpServer();
            pserver.Listen(10264);
            var bot = new PhantomsForeverBot();
            bot.Start();
            Console.ReadLine();
        }
    }
}
