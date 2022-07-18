using System.Diagnostics;
using System.Runtime.InteropServices;

using Flux.Utilities;
using Flux.WoW.Native;
using Flux.WoW.Patchables;

namespace Flux.WoW
{
    /// <summary>
    /// This class just allows us to do some shit!
    /// </summary>
    public static class Pulsator
    {
        private static readonly ChecksumDelegate ChecksumHandler = CRCHook;

        /// <summary>
        /// Gets called once, and only once. Use this for startup shit.
        /// </summary>
        public static void PulseStartup()
        {
            Process.EnterDebugMode();
            Win32.MemoryOpen();

            //Logging.Write("Calling startup functions from Pulsator");
            EventLoader.Register();
            Lua.Init();

            //WoWPackets.Initialize();

            //Lua.CreateEventFrame();

            Detours.Add(Utilities.RegisterDelegate<ChecksumDelegate>(GlobalOffsets.Checksum), ChecksumHandler, "ChecksumHook").Apply();
        }

        public static void PulseFrame()
        {
            if (FluxWoW.IsInGame && !Lua.IsEventFrameAvailable)
            {
                Lua.CreateEventFrame();
            }

            ObjMgr.Pulse();
            WoWFacer.Pulse();
            //Logging.WriteDebug("Pulsing OnFrame...");
            FluxWoW.InvokeOnFrame();
        }

        public static void PulseShutdown()
        {
            Lua.Shutdown();
            Detours.RemoveAll();
            Patcher.RemoveAll();
        }

        private static void CRCHook(uint a, uint b, uint c)
        {
            Detours.RemoveAll();
            Patcher.RemoveAll();
            Detours.Get("ChecksumHook").CallOriginal(a, b, c);
            Detours.ApplyAll();
            Patcher.ApplyAll();
        }

        #region Nested type: ChecksumDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ChecksumDelegate(uint a, uint b, uint c);

        #endregion
    }
}