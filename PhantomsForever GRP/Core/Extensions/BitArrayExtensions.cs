using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Extensions
{
    public static class BitArrayExtensions
    {
        public static string ToHex(this BitArray bits)
        {
            StringBuilder sb = new StringBuilder(bits.Length / 4);
            for (int i = 0; i < bits.Length; i += 4)
            {
                int v = (bits[i] ? 8 : 0) | (bits[i + 1] ? 4 : 0) | (bits[i + 2] ? 2 : 0) | (bits[i + 3] ? 1 : 0);
                sb.Append(v.ToString("x1")); // Or "X1"
            }
            return sb.ToString();
        }
    }
}