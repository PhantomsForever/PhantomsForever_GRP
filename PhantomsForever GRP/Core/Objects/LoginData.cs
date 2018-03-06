using PhantomsForever_GRP.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Objects
{
    public class LoginData
    {
        public string TypeName { get; set; }
        public string Data { get; set; }
        public static LoginData Decode(string hex)
        {
            var data = new LoginData();
            int typesize = int.Parse(hex.Substring(0, 4).FromHex().Reverse().ToArray().ToHex().FromHexToDecimal().ToString());
            data.TypeName = Encoding.UTF8.GetString(hex.Substring(4, typesize).FromHex());
            int datalennsize = int.Parse(hex.Substring(4 + typesize, 8).FromHex().Reverse().ToArray().ToHex().FromHexToDecimal().ToString());
            int datalen = int.Parse(hex.Substring(12 + typesize, 8).FromHex().Reverse().ToArray().ToHex().FromHexToDecimal().ToString());
            data.Data = hex.Substring(20 + typesize, datalen);
            return data;
        }
    }
}