﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Enums
{
    [DataContract]
    public enum PackageType
    {
        [EnumMember]
        Required,
        [EnumMember]
        Deferred,
        [EnumMember]
        OnDemand
    }
}