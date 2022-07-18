using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

using Flux.Utilities;
using Flux.WoW.Patchables;

namespace Flux.WoW
{
    public class Utilities
    {
        private static TraceLine _traceLineHandler;

        public static bool IsInLineOfSite(Point from, Point to)
        {
            from.Z += 1.3f;
            to.Z += 1.3f;

            float junk = 1f;
            var junk2 = new Point();
            // Known flags:

            // 0xFFEFFFFF

            // 0x40F3010F
            // 0x40F300FF
            // 0x40F0000F
            // 0x40F00000

            // 0x00100111
            // 0x00100151
            // 0x00100171
            // 0x00120171 // 0x00020000 is set in some sub-function call during the flag checks for units.
            // 0x00120111
            // 0x00200112
            // 0x00020000

            // Value at 0x1000124 or 0x1020124

            // 0x00000110
            // 0x00000151
            return DoTraceLine(from, to, ref junk, ref junk2, 0x00100171) == CGWorldFrameHitType.None;
        }

        private static CGWorldFrameHitType DoTraceLine(Point from, Point to, ref float distance, ref Point result, uint flags)
        {
            if (_traceLineHandler == null)
            {
                _traceLineHandler = RegisterDelegate<TraceLine>((uint) GlobalOffsets.TraceLine);
            }
            return _traceLineHandler(ref to, ref from, ref result, ref distance, flags, 0);
        }

        #region Nested type: CGWorldFrameHitType

        private enum CGWorldFrameHitType : byte
        {
            None = 0,
            Ground = 1,
            Object = 2
        }

        #endregion

        #region Nested type: TraceLine

        /// <summary>
        /// pOptional is apparently another WOWPOS pointer. But is always passed as 0.
        /// (WOWPOS according to WoWX, 0 according to... everyone else...)
        /// </summary>
        /// <param name="pEnd"></param>
        /// <param name="pStart"></param>
        /// <param name="pResult"></param>
        /// <param name="pDistance"></param>
        /// <param name="dwFlag"></param>
        /// <param name="pOptional"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate CGWorldFrameHitType TraceLine(
            ref Point pEnd, ref Point pStart, ref Point pResult, ref float pDistance, uint dwFlag, uint pOptional);

        #endregion

        #region RegisterDelegate

        /// <summary>
        /// Registers the delegate from an unmanaged function address.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static T RegisterDelegate<T>(int address) where T : class
        {
            return RegisterDelegate<T>(new IntPtr(address));
        }

        /// <summary>
        /// Registers the delegate from an unmanaged function address. This is here to handle addresses of type uint.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static T RegisterDelegate<T>(long address) where T : class
        {
            return RegisterDelegate<T>(new IntPtr(address));
        }

        /// <summary>
        /// Registers the delegate from an unmanaged function address.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static T RegisterDelegate<T>(IntPtr address) where T : class
        {
            //Logging.WriteDebug("Registering delegate " + typeof (T).Name + " at 0x" + address.ToString("X8"));
            return Marshal.GetDelegateForFunctionPointer(address, typeof (T)) as T;
        }

        public static T RegisterDelegate<T>(GlobalOffsets address) where T : class
        {
            return RegisterDelegate<T>((IntPtr) address);
        }

        #endregion
    }

    public static class Extensions
    {
        public static float ToFloat(this IntPtr val)
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(val.ToInt32()), 0);
        }

        public static uint ToUInt32(this IntPtr val)
        {
            return BitConverter.ToUInt32(BitConverter.GetBytes(val.ToInt32()), 0);
        }

        public static ulong ToUInt64(this IntPtr val)
        {
            return BitConverter.ToUInt64(BitConverter.GetBytes(val.ToInt64()), 0);
        }

        public static string ToRealString(this IList lst)
        {
            var tmp = new List<string>();
            if (lst == null)
            {
                return "NULL";
            }
            if (lst.Count == 0)
            {
                return "NULL";
            }
            foreach (object s in lst)
            {
                try
                {
                    if (s.Equals(null))
                    {
                        tmp.Add("NULL");
                        continue;
                    }
                }
                catch
                {
                    tmp.Add("NULL");
                }
                tmp.Add(s.ToString());
            }
            return string.Join(", ", tmp.ToArray());
        }

        public static string ToRealString<T>(this T[] lst)
        {
            var tmp = new List<T>();
            if (lst == null)
            {
                return "NULL";
            }
            tmp.AddRange(lst);
            return tmp.ToRealString();
        }

        public static string ToRealString(this BitArray arr)
        {
            string ret = "";

            foreach (bool b in arr)
            {
                if (b)
                {
                    ret += "1";
                }
                else
                {
                    ret += "0";
                }
            }
            return ret;
        }

        public static string ToRealString(this BitVector32 vec)
        {
            string ret = "";

            for (int i = 0; i < 32; i++)
            {
                if (vec[i])
                {
                    ret += "1";
                }
                else
                {
                    ret += "0";
                }
            }
            return ret;
        }
    }
}