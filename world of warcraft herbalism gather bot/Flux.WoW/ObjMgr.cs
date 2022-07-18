using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Flux.Utilities;
using Flux.WoW.Objects;
using Flux.WoW.Patchables;

namespace Flux.WoW
{
    internal static class ObjMgr
    {
        private static readonly EnumObjectsCallback Callback = EnumObjectsCallbackHandler;
        private static readonly IntPtr CallbackPtr;
        private static readonly ClntObjMgrGetObjectPtrDelegate ClntObjMgrGetObjectPtrWoW;
        private static readonly EnumVisibleObjectsDelegate EnumObjs;
        private static readonly ClntObjMgrGetActivePlayer GetActivePlayer;
        private static readonly Dictionary<ulong, WoWObject> RealObjects = new Dictionary<ulong, WoWObject>();
        private static ulong _frameCounter = 1;

        static ObjMgr()
        {
            //Logging.WriteDebug("Object manager initializing...");
            Me = new WoWActivePlayer(IntPtr.Zero);
            CallbackPtr = Marshal.GetFunctionPointerForDelegate(Callback);
            GetActivePlayer = Utilities.RegisterDelegate<ClntObjMgrGetActivePlayer>((uint) GlobalOffsets.ClntObjMgrGetActivePlayer);
            EnumObjs = Utilities.RegisterDelegate<EnumVisibleObjectsDelegate>((uint) GlobalOffsets.EnumVisibleObjects);
            ClntObjMgrGetObjectPtrWoW = Utilities.RegisterDelegate<ClntObjMgrGetObjectPtrDelegate>((uint) GlobalOffsets.ClntObjMgrObjectPtr);
        }

        internal static WoWActivePlayer Me { get; private set; }

        internal static List<WoWObject> ObjectList { get { return RealObjects.Values.Where(o => o.IsValid).ToList(); } }

        /// <summary>
        /// Called each frame. This 'pulses' our object manager.
        /// </summary>
        internal static void Pulse()
        {
            // Reset all the pointers.
            // This is faster than instantiating the class hundreds of times...
            // Make sure this is called BEFORE RemoveInvalidEntries!!!
            ClearPtrs();

            // Yep, that's it. Populate the current object list; and/or update pointers.
            // Note: Using 0 as the filter, as we want everything!
            // Can use more specific filters if needed.
            EnumObjs(CallbackPtr, 0);

            // Every 10th frame, lets clear out the invalid entries in the object list
            // to conserve resources
            if (_frameCounter++ % 10 == 0)
            {
                RemoveInvalidEntries();
            }

            // Make sure 'Me' is correct.
            Me.UpdatePointer( InternalGetObjectByGuid( GetActivePlayer() ) );
        }

        /// <summary>
        /// Clears out any invalid objects from our list.
        /// </summary>
        private static void RemoveInvalidEntries()
        {
            // Yes, this is pretty damned simple. Just clear out any invalid
            // objects. (IsValid is just ObjPtr != IntPtr.Zero)
            IEnumerable<KeyValuePair<ulong, WoWObject>> r = from o in RealObjects
                                                            where !o.Value.IsValid
                                                            select o;

            foreach (var pair in r)
            {
                RealObjects.Remove(pair.Key);
            }
        }

        /// <summary>
        /// Update ALL pointers and set them to IntPtr.Zero. Making every object !IsValid
        /// </summary>
        private static void ClearPtrs()
        {
            foreach (WoWObject o in RealObjects.Values)
            {
                o.UpdatePointer(IntPtr.Zero);
            }
        }

        /// <summary>
        /// The EVO callback handler. Simple. But effective.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static int EnumObjectsCallbackHandler(ulong guid, int filter)
        {
            // Note: Filter is actually a WoWObjectType enum value.
            // If we decide to call EVO with a filter set; we will only receive objects of that type!

            IntPtr objPtr = InternalGetObjectByGuid(guid);

            if (!RealObjects.ContainsKey(guid))
            {
                var tmp = new WoWObject(objPtr);

                switch (tmp.Type)
                {
                    case WoWObjectType.Item:
                        tmp = new WoWItem(objPtr);
                        break;
                    case WoWObjectType.Container:
                        tmp = new WoWContainer(objPtr);
                        break;
                    case WoWObjectType.Unit:
                        tmp = new WoWUnit(objPtr);
                        break;
                    case WoWObjectType.Player:
                        // Skip ourselves
                        //if (tmp.Guid == Me.Guid)
                        //    return 1;
                        tmp = new WoWPlayer(objPtr);
                        break;
                    case WoWObjectType.GameObject:
                        tmp = new WoWGameObject(objPtr);
                        break;
                    case WoWObjectType.Corpse:
                        tmp = new WoWCorpse(objPtr);
                        break;
                    default:
                        // Maybe in the future we'll support the rest of the types
                        // for now however, we'll just skip over them completely.
                        return 1;
                }

                RealObjects.Add(guid, tmp);
            }
            else if (RealObjects[guid] != objPtr)
            {
                RealObjects[guid].UpdatePointer(objPtr);
            }

            // 1 == continue to next object. Will continue to call this function if more objects exist.
            // 0 == stop. This function will not be called regardless of if another object exists.
            return 1;
        }

        /// <summary>
        /// Uses our current internal list of objects to retrieve a WoWObject. This is much faster
        /// if the object is known to be in the current object list. USE THIS FUNCTION!
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        internal static WoWObject GetObjectByGuid(ulong guid)
        {
            WoWObject ret;
            RealObjects.TryGetValue(guid, out ret);

            return ret ?? new WoWObject(IntPtr.Zero);
        }

        /// <summary>
        /// Calls WoW's internal ClntObjPtr (or whatever it's called) to retrieve an object pointer
        /// based on the specified GUID.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        internal static IntPtr InternalGetObjectByGuid(ulong guid)
        {
            return ClntObjMgrGetObjectPtrWoW(guid, -1);
        }

        #region Nested type: ClntObjMgrGetActivePlayer

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong ClntObjMgrGetActivePlayer();

        #endregion

        #region Nested type: ClntObjMgrGetObjectPtrDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr ClntObjMgrGetObjectPtrDelegate(ulong guid, int filter);

        #endregion

        #region Nested type: EnumObjectsCallback

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int EnumObjectsCallback(ulong guid, int filter);

        #endregion

        #region Nested type: EnumVisibleObjectsDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int EnumVisibleObjectsDelegate(IntPtr callback, int filter);

        #endregion
    }
}