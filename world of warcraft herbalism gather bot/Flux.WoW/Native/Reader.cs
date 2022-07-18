using System;
using System.Runtime.InteropServices;

namespace Flux.WoW.Native
{
    public static class Reader
    {
        public static T Read<T>(IntPtr address)
        {
            object ret;

            // Handle types that don't have a real typecode
            // and/or can be done without the ReadByte bullshit
            if (typeof (T) == typeof (IntPtr))
            {
                ret = Marshal.ReadIntPtr(address);
                return (T) ret;
            }

            if (typeof (T) == typeof (string))
            {
                ret = Marshal.PtrToStringAnsi(address);
                return (T) ret;
            }

            int size = Marshal.SizeOf(typeof (T));
            var ba = new byte[size];
            for (int i = 0; i < size; i++)
            {
                ba[i] = Marshal.ReadByte(address, i);
            }

            switch (Type.GetTypeCode(typeof (T)))
            {
                case TypeCode.Boolean:
                    ret = BitConverter.ToBoolean(ba, 0);
                    break;
                case TypeCode.Char:
                    ret = BitConverter.ToChar(ba, 0);
                    break;
                case TypeCode.Byte:
                    ret = ba[0];
                    break;
                case TypeCode.Int16:
                    ret = BitConverter.ToInt16(ba, 0);
                    break;
                case TypeCode.UInt16:
                    ret = BitConverter.ToUInt16(ba, 0);
                    break;
                case TypeCode.Int32:
                    ret = BitConverter.ToInt32(ba, 0);
                    break;
                case TypeCode.UInt32:
                    ret = BitConverter.ToUInt32(ba, 0);
                    break;
                case TypeCode.Int64:
                    ret = BitConverter.ToInt64(ba, 0);
                    break;
                case TypeCode.UInt64:
                    ret = BitConverter.ToUInt64(ba, 0);
                    break;
                case TypeCode.Single:
                    ret = BitConverter.ToSingle(ba, 0);
                    break;
                case TypeCode.Double:
                    ret = BitConverter.ToDouble(ba, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return (T) ret;
        }

        public static T Read<T>(params uint[] addresses)
        {
            if (addresses.Length == 0)
            {
                return default(T);
            }
            if (addresses.Length == 1)
            {
                return Read<T>((IntPtr) addresses[0]);
            }

            uint last = 0;
            for (int i = 0; i < addresses.Length; i++)
            {
                if (i == addresses.Length - 1)
                {
                    return Read<T>((IntPtr) (addresses[i] + last));
                }
                last = Read<uint>(new IntPtr(last + addresses[i]));
            }

            // Should never hit this.
            // The compiler just bitches.
            return default(T);
        }

        public static T ReadStruct<T>(IntPtr address) where T : struct
        {
            return (T) Marshal.PtrToStructure(address, typeof (T));
        }
    }
}