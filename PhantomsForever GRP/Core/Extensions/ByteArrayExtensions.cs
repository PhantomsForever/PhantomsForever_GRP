using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Extensions
{
    public static class ByteArrayExtensions
    {
        public static String ToHex(this byte[] array)
        {
            StringBuilder hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}