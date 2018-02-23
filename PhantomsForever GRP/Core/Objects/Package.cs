using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Objects
{
    [DataContract]
    public class Package
    {
        [DataMember]
        public string Name
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
        public PackageType Type
        {
            get
            {
                return this.packageType_0;
            }
            set
            {
                this.packageType_0 = value;
            }
        }
        [DataMember]
        public List<Patch> Patches
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
        private string string_0;
        private List<Patch> list_0;
        internal PackageType packageType_0;
    }
}