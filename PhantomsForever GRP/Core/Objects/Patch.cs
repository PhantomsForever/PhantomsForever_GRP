using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Objects
{
    [DataContract]
    public class Patch
    {
        [DataMember]
        public string Version
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
        public PatchType Type
        {
            get
            {
                return this.patchType_0;
            }
            set
            {
                this.patchType_0 = value;
            }
        }
        [DataMember]
        public List<string> Http
        {
            get
            {
                return this.list_0;
            }
            set
            {
                this.list_0 = value;
            }
        }
        [DataMember]
        public List<string> Torrent
        {
            get
            {
                return this.list_1;
            }
            set
            {
                this.list_1 = value;
            }
        }
        [DataMember]
        public string Param
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
        [DataMember]
        public string NextVersion
        {
            get
            {
                return this.string_2;
            }
            set
            {
                this.string_2 = value;
            }
        }
        private List<string> list_0;
        internal string string_0;
        internal PatchType patchType_0;
        internal List<string> list_1;
        internal string string_1;
        internal string string_2;
    }
}