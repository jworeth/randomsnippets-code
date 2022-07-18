using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Flux.WoW.Native;
using Flux.WoW.Objects;
using Flux.WoW.Patchables;

namespace Flux.WoW
{
    public static class FluxWoW
    {
        #region Delegates

        public delegate void OnFrameDelegate();

        #endregion

        private static readonly WoWCache WowCache = new WoWCache();
        private static readonly WoWDB WowDb = new WoWDB();
        private static readonly WoWMovement WowMovement = new WoWMovement();

        public static WoWActivePlayer Me { get { return ObjMgr.Me; } }
        public static WoWMovement Movement { get { return WowMovement; } }
        public static WoWCache Cache { get { return WowCache; } }
        public static string RealZoneText { get { return Reader.Read<string>(Marshal.ReadIntPtr((IntPtr) GlobalOffsets.RealZoneText)); } }
        public static string SubZoneText { get { return Reader.Read<string>(Marshal.ReadIntPtr((IntPtr) ((uint) GlobalOffsets.RealZoneText + 0x4))); } }
        public static string ZoneText { get { return Reader.Read<string>(Marshal.ReadIntPtr((IntPtr) ((uint) GlobalOffsets.RealZoneText + 0x8))); } }
        // TODO: Update WoWBuild to 3.2.2a
        //public static string WoWBuild { get { return Reader.Read<string>(0x009E51EC); } }
        public static int ContinentId { get { return Reader.Read<int>((uint) GlobalOffsets.LocalPlayerCurrentContinentId); } }
        public static bool IsInGame { get { return Reader.Read<uint>((uint) GlobalOffsets.IsInGame) == 1; } }
        public static WoWDB Db { get { return WowDb; } }
        public static List<WoWObject> ObjectList { get { return ObjMgr.ObjectList; } }

        public static bool GlobalCooldown
        {
            // Kudos to Nesox for this code. (Which he ripped from Gorzul on MMOwned)
            get
            {
                //long frequency = 0;
                //long perfCount = 0;
                //Win32.QueryPerformanceFrequency(ref frequency);
                //Win32.QueryPerformanceCounter(ref perfCount);

                //Current time in ms
                ulong currentTime = TimeStamp;

                //Get first list object
                var currentListObject = Reader.Read<uint>((uint) GlobalOffsets.LocalPlayerSpellsOnCooldown + 0x8);

                while ((currentListObject != 0) && ((currentListObject & 1) == 0))
                {
                    //Start time of the spell cooldown in ms
                    var startTime = Reader.Read<uint>(currentListObject + 0x10);

                    //Absolute gcd of the spell in ms
                    var globalCooldown = Reader.Read<uint>((currentListObject + 0x2C));

                    //Spell on gcd?
                    if ((startTime + globalCooldown) > currentTime)
                    {
                        return true;
                    }

                    //Get next list object
                    currentListObject = Reader.Read<uint>(currentListObject + 4);
                }

                return false;
            }
        }

        /// <summary>
        /// This is WoW's own internal perf count. (It picks QPF/QPC or TGT based on user config)
        /// </summary>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong PerformanceCounterDelegate();

        private static PerformanceCounterDelegate _performanceCount;
        public static ulong TimeStamp
        {
            get
            {
                if (_performanceCount == null)
                {
                    _performanceCount = Utilities.RegisterDelegate<PerformanceCounterDelegate>(/*0x820430*/ GlobalOffsets.PerformanceCounter);
                }
                return _performanceCount();
            }
        }

        public static event OnFrameDelegate OnFrame;

        internal static void InvokeOnFrame()
        {
            OnFrameDelegate d = OnFrame;
            if (d != null)
            {
                d();
            }
        }
    }
}