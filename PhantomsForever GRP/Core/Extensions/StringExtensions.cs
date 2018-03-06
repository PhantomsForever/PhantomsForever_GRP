using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Extensions
{
    public static class StringExtensions
    {
        public static byte[] FromHex(this String str)
        {
            return Enumerable.Range(0, str.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(str.Substring(x, 2), 16))
                     .ToArray();
        }
        public static BitArray FromHexToBits(this String str)
        {
            BitArray ba = new BitArray(4 * str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                byte b = byte.Parse(str[i].ToString(), NumberStyles.HexNumber);
                for (int j = 0; j < 4; j++)
                {
                    ba.Set(i * 4 + j, (b & (1 << (3 - j))) != 0);
                }
            }
            return ba;
        }
        public static Decimal FromHexToDecimal(this String str)
        {
            str = str.Replace("x", string.Empty);
            long result = 0;
            long.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out result);
            return result;
        }
    }
}