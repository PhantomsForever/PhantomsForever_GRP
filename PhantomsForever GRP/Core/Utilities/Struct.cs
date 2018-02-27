using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Utilities
{
    public class Struct
    {
        private static byte[] TypeAgnosticGetBytes(object o)
        {
            if (o is int) return BitConverter.GetBytes((int)o);
            if (o is uint) return BitConverter.GetBytes((uint)o);
            if (o is long) return BitConverter.GetBytes((long)o);
            if (o is ulong) return BitConverter.GetBytes((ulong)o);
            if (o is short) return BitConverter.GetBytes((short)o);
            if (o is ushort) return BitConverter.GetBytes((ushort)o);
            if (o is byte || o is sbyte) return new byte[] { (byte)o };
            throw new ArgumentException("Unsupported object type found");
        }

        private static string GetFormatSpecifierFor(object o)
        {
            if (o is int) return "i";
            if (o is uint) return "I";
            if (o is long) return "q";
            if (o is ulong) return "Q";
            if (o is short) return "h";
            if (o is ushort) return "H";
            if (o is byte) return "B";
            if (o is sbyte) return "b";
            throw new ArgumentException("Unsupported object type found");
        }
        public static object[] Unpack(string fmt, byte[] bytes)
        {
            if (fmt.Length < 1) throw new ArgumentException("Format string cannot be empty.");
            bool endianFlip = false;
            if (fmt.Substring(0, 1) == "<")
            {
                if (BitConverter.IsLittleEndian == false) endianFlip = true;
                fmt = fmt.Substring(1);
            }
            else if (fmt.Substring(0, 1) == ">")
            {
                if (BitConverter.IsLittleEndian == true) endianFlip = true;
                fmt = fmt.Substring(1);
            }
            int totalByteLength = 0;
            foreach (char c in fmt.ToCharArray())
            {
                switch (c)
                {
                    case 'q':
                    case 'Q':
                        totalByteLength += 8;
                        break;
                    case 'i':
                    case 'I':
                        totalByteLength += 4;
                        break;
                    case 'h':
                    case 'H':
                        totalByteLength += 2;
                        break;
                    case 'b':
                    case 'B':
                    case 'x':
                        totalByteLength += 1;
                        break;
                    default:
                        throw new ArgumentException("Invalid character found in format string.");
                }
            }
            if (bytes.Length != totalByteLength) throw new ArgumentException("The number of bytes provided does not match the total length of the format string.");
            int byteArrayPosition = 0;
            List<object> outputList = new List<object>();
            byte[] buf;
            foreach (char c in fmt.ToCharArray())
            {
                switch (c)
                {
                    case 'q':
                        outputList.Add((object)(long)BitConverter.ToInt64(bytes, byteArrayPosition));
                        byteArrayPosition += 8;
                        break;
                    case 'Q':
                        outputList.Add((object)(ulong)BitConverter.ToUInt64(bytes, byteArrayPosition));
                        byteArrayPosition += 8;
                        break;
                    case 'l':
                        outputList.Add((object)(int)BitConverter.ToInt32(bytes, byteArrayPosition));
                        byteArrayPosition += 4;
                        break;
                    case 'L':
                        outputList.Add((object)(uint)BitConverter.ToUInt32(bytes, byteArrayPosition));
                        byteArrayPosition += 4;
                        break;
                    case 'h':
                        outputList.Add((object)(short)BitConverter.ToInt16(bytes, byteArrayPosition));
                        byteArrayPosition += 2;
                        break;
                    case 'H':
                        outputList.Add((object)(ushort)BitConverter.ToUInt16(bytes, byteArrayPosition));
                        byteArrayPosition += 2;
                        break;
                    case 'b':
                        buf = new byte[1];
                        Array.Copy(bytes, byteArrayPosition, buf, 0, 1);
                        outputList.Add((object)(sbyte)buf[0]);
                        byteArrayPosition++;
                        break;
                    case 'B':
                        buf = new byte[1];
                        Array.Copy(bytes, byteArrayPosition, buf, 0, 1);
                        outputList.Add((object)(byte)buf[0]);
                        byteArrayPosition++;
                        break;
                    case 'x':
                        byteArrayPosition++;
                        break;
                    default:
                        throw new ArgumentException("You should not be here.");
                }
            }
            return outputList.ToArray();
        }
        public static byte[] Pack(object[] items, bool LittleEndian, out string NeededFormatStringToRecover)
        {
            List<byte> outputBytes = new List<byte>();
            bool endianFlip = (LittleEndian != BitConverter.IsLittleEndian);
            string outString = (LittleEndian == false ? ">" : "<");
            foreach (object o in items)
            {
                byte[] theseBytes = TypeAgnosticGetBytes(o);
                if (endianFlip == true) theseBytes = (byte[])theseBytes.Reverse();
                outString += GetFormatSpecifierFor(o);
                outputBytes.AddRange(theseBytes);
            }
            NeededFormatStringToRecover = outString;
            return outputBytes.ToArray();
        }

        public static byte[] Pack(object[] items)
        {
            string dummy = "";
            return Pack(items, true, out dummy);
        }
    }
}