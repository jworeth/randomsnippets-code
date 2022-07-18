using System;
using System.Runtime.InteropServices;

namespace Flux.WoW
{
    public class WoWCVar
    {
        #region Nested type: WCVAR

        [StructLayout(LayoutKind.Sequential)]
        private struct WCVAR
        {
            public IntPtr VTable;
            public uint Hash;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            private byte[] Unk04;

            public IntPtr Next;
            public string Name;
            public int Category;
            public int Flags;
            private int Unk20;
            private int Unk24;
            public string StringValue;
            public float FloatValue;
            public int IntValue;
            public int NumMods;
            private int Unk38;
            private int Unk3C;
            public string DefaultValue;
        }

        #endregion
    }
}