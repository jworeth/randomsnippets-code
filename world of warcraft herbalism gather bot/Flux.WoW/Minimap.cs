using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Flux.WoW.Objects;
using Flux.WoW.Patchables;

namespace Flux.WoW
{
    /// <summary>
    /// Handles Minimap tracking. Credits to WoWX.
    /// </summary>
    public static class Minimap
    {
        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool CanTrackDelegate(IntPtr objPtr);

        #endregion

        /// <summary>
        /// Our Hook for the CanTrack functions
        /// </summary>
        public static readonly CanTrackDelegate CanTrackHook = HookCanTrack;

        /// <summary>
        /// WoW's CanTrackObject function
        /// </summary>
        public static readonly CanTrackDelegate CanTrackObject;

        /// <summary>
        /// WoW's CanTrackUnit function
        /// </summary>
        public static readonly CanTrackDelegate CanTrackUnit;

        private static readonly bool[] Flags = new bool[Enum.GetNames(typeof (WoWCreatureType)).Length];

        /// <summary>
        /// List of object names. That will be tracked.
        /// </summary>
        public static readonly List<string> ObjectNames = new List<string>();

        static Minimap()
        {
            //Make sure we register the WoW functions!
            CanTrackUnit = Utilities.RegisterDelegate<CanTrackDelegate>((uint) GlobalOffsets.UnitTracking);
            CanTrackObject = Utilities.RegisterDelegate<CanTrackDelegate>((uint) GlobalOffsets.ObjectTracking);
        }

        //The function that handles hook
        private static bool HookCanTrack(IntPtr objPtr)
        {
            try
            {
                if (objPtr == IntPtr.Zero)
                {
                    return false;
                }

                var obj = new WoWObject(objPtr);

                if (ObjectNames.Count != 0)
                {
                    string objName = obj.Name;
                    foreach (string name in ObjectNames)
                    {
                        if (objName == name)
                        {
                            return true;
                        }
                    }
                }

                if (obj.Type == WoWObjectType.Unit || obj.Type == WoWObjectType.Player)
                {
                    var unit = new WoWUnit(objPtr);
                    return Flags[(int) unit.CreatureType];
                }

                return false;
            }
            catch (Exception e)
            {
                //Logging.WriteException(e);
                return false;
            }
        }

        /// <summary>
        /// Sets a tracking flag for a WoW creature.
        /// </summary>
        /// <param name="unitType">The type of creature you want to track</param>
        /// <param name="state">The state you want to set the flag to.</param>
        public static void SetTrackingFlag(WoWCreatureType unitType, bool state)
        {
            Flags[(int) unitType] = state;
        }
    }
}