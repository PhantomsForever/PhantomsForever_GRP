using PhantomsForever_GRP.Core.Extensions;
using PhantomsForever_GRP.Core.RMC.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.RMC
{
    public class RMCPayload
    {
        public int Size { get; set; }
        public string ProtocolId { get; set; }
        public string CallId { get; set; }
        public string MethodId { get; set; }
        public string Payload { get; set; }
        public static RMCPayload Decode(string hex)
        {
            var payload = new RMCPayload();
            payload.Size = int.Parse(hex.Substring(0, 8).FromHex().Reverse().ToArray().ToHex().FromHexToDecimal().ToString());
            payload.ProtocolId = hex.Substring(8, 2);
            payload.CallId = hex.Substring(10, 8);
            payload.MethodId = hex.Substring(18, 8);
            payload.Payload = hex.Substring(26, hex.Length - 26);
            var a = Simple_Authentication.LoginWithTokenEx.Decode(payload.Payload);
            return payload;
        }
    }
}