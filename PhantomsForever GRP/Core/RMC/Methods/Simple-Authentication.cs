using PhantomsForever_GRP.Core.Extensions;
using PhantomsForever_GRP.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.RMC.Methods
{
    public class Simple_Authentication
    {
        public class LoginWithTokenEx
        {
            public string Token { get; set; }
            public RVConnectionData RVConnectionData { get; set; }
            public string LoginData { get; set; }
            public static LoginWithTokenEx Decode(string hex)
            {
                var auth = new LoginWithTokenEx();
                int tokensize = (int.Parse(hex.Substring(0, 4).FromHex().Reverse().ToArray().ToHex(), System.Globalization.NumberStyles.HexNumber)) * 2;
                auth.Token = hex.Substring(4, tokensize);
                var data = new RVConnectionData();
                int surlsize = (int.Parse(hex.Substring(4 + tokensize, 4).FromHex().Reverse().ToArray().ToHex(), System.Globalization.NumberStyles.HexNumber)) * 2;
                data.StationUrl = hex.Substring(8 + tokensize, surlsize);
                int spurlsize = (int.Parse(hex.Substring(surlsize + 8 + tokensize, 8).FromHex().Reverse().ToArray().ToHex(), System.Globalization.NumberStyles.HexNumber)) * 2;
                if (spurlsize != 0)
                    data.SpecialProtocols = hex.Substring(surlsize + 16 + tokensize, spurlsize);
                int surlsize2 = (int.Parse(hex.Substring(16 + surlsize + tokensize + spurlsize, 4).FromHex().Reverse().ToArray().ToHex(), System.Globalization.NumberStyles.HexNumber)) * 2;
                data.StationUrl2 = hex.Substring(20 + surlsize + tokensize + spurlsize, surlsize2);
                auth.RVConnectionData = data;
                int logindatalength = (int.Parse(hex.Substring(20 + surlsize + tokensize + spurlsize + surlsize2, 4).FromHex().Reverse().ToArray().ToHex(), System.Globalization.NumberStyles.HexNumber)) * 2;
                auth.LoginData = hex.Substring(24 + surlsize + tokensize + spurlsize + surlsize2, logindatalength);
                return auth;
            }
        }
    }
}