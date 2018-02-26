using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Extensions
{
    public static class IntegerExtensions
    {
        public static BitArray ToBitArray(this Int32[] ints)
        {
            var ba = new BitArray(8);
            for (int i = 0; i < 8; i++)
            {
                byte b = byte.Parse(ints[i].ToString(), NumberStyles.Integer);
                ba.Set(i, b != 0);
            }
            return ba;
        }
    }
}