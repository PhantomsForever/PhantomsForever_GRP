using Nancy;
using Nancy.Json;
using PhantomsForever_GRP.Core.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Web.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule() : base("/")
        {
            Get("/OnlineConfigService.svc/GetOnlineConfig", args =>
            {
                Console.WriteLine("Request: " + this.Context.Request.Path);
                //return "[{\"Name\":\"SandboxUrl\",\"Values\":[\"prudp:\\/ address = lb - pdc - 192.168.0.178; port = 22700\"]}]";
                return "[{\"Name\":\"SandboxUrl\",\"Values\":[\"prudp:\\/ address = lb - pdc - 192.168.0.178; port = 22700\"]},{\"Name\":\"SandboxUrlWS\",\"Values\":[\"lb - pdc - 192.168.0.178:22700\"]}]";
            });
            Get("/Version/PDC-Live_Packages.txt", args =>
            {
                Console.WriteLine("Request: " + this.Context.Request.Path);
                return "";
                List<Package> pp = new List<Package>();
                Patch ppp = new Objects.Patch();
                ppp.Http = new List<string>();
                ppp.Http.Add("test");
                ppp.NextVersion = "2";
                ppp.Param = "";
                ppp.Torrent = new List<string>();
                ppp.Torrent.Add("test");
                ppp.Type = Enums.PatchType.Major;
                ppp.Version = "1";
                Package p = new Package();
                p.Name = "test";
                p.Type = Enums.PackageType.Required;
                p.Patches = new List<Objects.Patch>();
                p.Patches.Add(ppp);
                pp.Add(p);
                var ll = new JavaScriptSerializer();
                return ll.Serialize(pp);
            });
            Get("/updater/UtcNow", args =>
            {
                Console.WriteLine("Request: " + this.Context.Request.Path);
                return DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'");
            });
            Get("/updater/GetValues/LauncherConfig@", args =>
            {
                Console.WriteLine("Request: " + this.Context.Request.Path);
                var ll = new JavaScriptSerializer();
                var config = new List<Objects.Configuration>();
                var upload = new List<Objects.UploaderSettings>();
                var u = new UploaderSettings();
                upload.Add(u);
                var c = new Configuration();
                c.Key = "UtcNow";
                c.Value = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss");
                var c1 = new Configuration();
                c1.Key = "RequestStatus";
                c1.Value = "OK";
                config.Add(c);
                config.Add(c1);
                return ll.Serialize(config);
            });
            Get("/updater/GetValues/LauncherConfig_DownloadType", args =>
            {
                Console.WriteLine("Request: " + this.Context.Request.Path);
                return "";
            });
            Get("/updater/GetValues/eula_ubisoft", args =>
            {
                Console.WriteLine("Request: " + this.Context.Request.Path);
                return "[{\"Key\":\"eula_ubisoft\",\"Value\":\"{\\\"Version\\\":1,\\\"LanguageTags\\\":{\\\"english\\\":\\\"ubisoft sucks dicks\\\"}}\"},{\"Key\":\"RequestStatus\",\"Value\":\"OK\"}]";
            });
            Get("/LauncherWeb/main_english.html", args =>
            {
                Console.WriteLine("Request: " + this.Context.Request.Path);
                return "<html><body><h3>LifeCoder is bringing this back</h3></body></html>";
            });
            Get("/updater/GetValues/LauncherConfig_CanSeedInLobby", args =>
            {
                Console.WriteLine("Request: " + this.Context.Request.Path);
                return "[{\"Key\":\"LauncherConfig_CanSeedInLobby\",\"Value\":\"True\"},{\"Key\":\"RequestStatus\",\"Value\":\"OK\"}]";
            });
            Get("/redirect/uat.html", args =>
            {
                Console.WriteLine("Request: " + this.Context.Request.Path);
                return View["grp/uat"];
            });
            Post("/grp-login/json/login", args =>
            {
                Console.WriteLine("Request: " + this.Context.Request.Path);
                var c = this.Context.Request.Body;
                string user = null;
                using (var sr = new StreamReader(c))
                {
                    user = sr.ReadLine();
                }
                JavaScriptSerializer s = new JavaScriptSerializer();
                var u = s.Deserialize<Credentials>(user);
                return DatabaseHandler.LoginGameUser(u.Username, u.Password);
            });
            Post("/loginservice/Login.svc/json/Login", args =>
            {
                Console.WriteLine("Request: " + this.Context.Request.Path);
                var c = this.Context.Request.Body;
                string user = null;
                using (var sr = new StreamReader(c))
                {
                    user = sr.ReadLine();
                }
                var s = new JavaScriptSerializer();
                var u = s.Deserialize<Credentials>(user);
                return DatabaseHandler.LoginGameUser(u.Username, u.Password);
            });
            Get(@"/(.*)", args =>
            {
                Console.WriteLine("Request: " + this.Context.Request.Path);
                return "";
            });
            Post(@"/(.*)", args =>
            {
                Console.WriteLine("Request: " + this.Context.Request.Path);
                return "";
            });
        }
    }
}