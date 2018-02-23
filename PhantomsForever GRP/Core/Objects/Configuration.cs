using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Objects
{
    [DataContract]
    public class Configuration
    {
        [DataMember]
        public string Key
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }
        [DataMember]
        public string Value
        {
            get
            {
                return this.string_1;
            }
            set
            {
                this.string_1 = value;
            }
        }
        static Configuration()
        {
            char[] array = new char[8];
            array[4] = '➦';
            array[1] = '㰱';
            array[3] = 'け';
            array[6] = '㐾';
            array[2] = '⩄';
            array[7] = '᪢';
            array[0] = '⇠';
            array[5] = 'ᯔ';
            Configuration.object_2 = new string[2];
            Configuration.object_0 = array;
        }
        private string string_0;
        private string string_1;
        private static readonly object object_0;
        private static readonly object object_1 = new char[13];
        private static readonly object object_2;
        internal static byte byte_0;
        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 26)]
        private struct Struct0
        {
        }
    }
}