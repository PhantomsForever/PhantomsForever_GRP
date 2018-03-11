using PhantomsForever_GRP.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Objects
{
    public class RVConnectionData
    {
        public string StationUrl { get; set; }
        public string SpecialProtocols { get; set; }
        public string StationUrl2 { get; set; }
        public string Encode()
        {
            var hex = "";
            var surlhex = Encoding.UTF8.GetBytes(StationUrl).Reverse().ToArray().ToHex();
            var s = surlhex.Length.ToString("X").FromHex().Reverse().ToArray().ToHex();
            hex += "010001";
            hex += "0001";
            hex += s;
            hex += surlhex;
            hex += "00000000";
            hex += s;
            hex += surlhex;
            hex += "00000000";
            return hex;
        }
        public static RVConnectionData Decode(string hex)
        {
            var data = new RVConnectionData();
            int surlsize = int.Parse(hex.Substring(0, 4).FromHex().Reverse().ToArray().ToHex().FromHexToDecimal().ToString());
            data.StationUrl = hex.Substring(4, surlsize);
            int spurlsize = int.Parse(hex.Substring(surlsize + 4, 8).FromHex().Reverse().ToArray().ToHex().FromHexToDecimal().ToString());
            if (spurlsize != 0)
                data.SpecialProtocols = hex.Substring(surlsize + 12, spurlsize);
            int surlsize2 = int.Parse(hex.Substring(0, 4).FromHex().Reverse().ToArray().ToHex().FromHexToDecimal().ToString());
            data.StationUrl2 = hex.Substring(4, surlsize2);
            return data;
        }
    }
}