using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Objects
{
    public class CredentialResponse
    {
        public string UserName { get; set; }
        public List<int> Token { get; set; }
    }
}