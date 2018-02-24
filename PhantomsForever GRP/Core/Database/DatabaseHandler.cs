using Nancy.Json;
using PhantomsForever_GRP.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Database
{
    public class DatabaseHandler
    {
        public static string LoginGameUser(string u, string p)
        {
            //do actual login
            var cr = new CredentialResponse();
            cr.UserName = u;
            cr.Token = new List<int>(new int[] { 1, 5, 6, 7, 8, 9, 5 });//little static currently
            var jss = new JavaScriptSerializer();
            return jss.Serialize(cr);
        }
    }
}