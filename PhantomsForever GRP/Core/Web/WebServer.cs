using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Web
{
    public class WebServer
    {
        private NancyHost host;
        public WebServer()
        {
            host = new NancyHost(new Uri[] { new Uri("") });
        }
        public void Start()
        {
            host.Start();
        }
        public void Stop()
        {
            host.Stop();
        }
    }
}