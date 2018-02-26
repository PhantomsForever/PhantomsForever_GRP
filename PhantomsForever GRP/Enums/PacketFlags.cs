using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Enums
{
    public enum PacketFlags
    {
        FLAG_ACK = 1,
        FLAG_RELIABLE = 2,
        FLAG_NEED_ACK = 4,
        FLAG_HAS_SIZE = 8,
        FLAG_MULTI_ACK = 200
    }
    public static class PFlags
    {
        public static PacketFlags[] ParseFlags(string s)
        {
            var i = Convert.ToInt32(s, 2);
            switch(i)
            {
                case 1:
                    return new PacketFlags[] { PacketFlags.FLAG_ACK };
                case 2:
                    return new PacketFlags[] { PacketFlags.FLAG_RELIABLE };
                case 3:
                    return new PacketFlags[] { PacketFlags.FLAG_ACK, PacketFlags.FLAG_RELIABLE };
                case 4:
                    return new PacketFlags[] { PacketFlags.FLAG_NEED_ACK };
                case 6:
                    return new PacketFlags[] { PacketFlags.FLAG_RELIABLE, PacketFlags.FLAG_NEED_ACK };
                case 7:
                    return new PacketFlags[] { PacketFlags.FLAG_ACK, PacketFlags.FLAG_RELIABLE, PacketFlags.FLAG_NEED_ACK };
                case 8:
                    return new PacketFlags[] { PacketFlags.FLAG_HAS_SIZE };
                case 9:
                    return new PacketFlags[] { PacketFlags.FLAG_ACK, PacketFlags.FLAG_HAS_SIZE };
                case 10:
                    return new PacketFlags[] { PacketFlags.FLAG_RELIABLE, PacketFlags.FLAG_HAS_SIZE };
                case 11:
                    return new PacketFlags[] { PacketFlags.FLAG_ACK, PacketFlags.FLAG_RELIABLE, PacketFlags.FLAG_HAS_SIZE };//todo, add others
                case 200:
                    return new PacketFlags[] { PacketFlags.FLAG_MULTI_ACK };
                default:
                    return null;
            }
        }
    }
}