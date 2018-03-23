using PhantomsForever_GRP.Core.Cryptography;
using PhantomsForever_GRP.Core.Data;
using PhantomsForever_GRP.Core.Discord;
using PhantomsForever_GRP.Core.Extensions;
using PhantomsForever_GRP.Core.PRUdp;
using PhantomsForever_GRP.Core.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhantomsForever_GRP
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if(!File.Exists(Settings.Python36Path))
            {
                MessageBox.Show("Could not find Python 3.6, please install it and point towards it");
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Python 3.6|python.exe";
                    if(ofd.ShowDialog() == DialogResult.OK)
                    {
                        if(ofd.CheckFileExists)
                        {
                            Settings.Python36Path = ofd.FileName;
                        }
                    }
                }
            }
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
