using System;

using Flux.WoW.Native;

namespace Flux.WoW.Patchables
{
    public static class Offsets
    {
        //static FindPattern _patterns = new FindPattern(Logging.ApplicationPath + "//Patterns.xml", 0x401000, 0x800000);
        private static readonly Random Rand = new Random();

        public static IntPtr GetRandomSixByteCave
        {
            get
            {
                // Skip past all the 'bad' addresses in our find.
                uint[] available = FindPatterns.FindAll(0x4010FF, 0x800000, "CC CC CC CC CC CC", "xxxxxx");
                ;
                uint ret = available[Rand.Next(0, available.Length)];
                return (IntPtr) ret;
            }
        }

        //public static uint FrameScript_Execute
        //{
        //    get
        //    {
        //        return _patterns.Get("FrameScript_Execute");
        //    }
        //}
    }
}