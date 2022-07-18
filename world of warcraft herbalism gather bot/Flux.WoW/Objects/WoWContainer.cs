using System;
using System.Runtime.InteropServices;

using Flux.WoW.Patchables;

namespace Flux.WoW.Objects
{
    public class WoWContainer : WoWItem
    {
        private static GetBagAtIndexDelegate _getBagAtIndex;
        private readonly int _bagNumber;
        private readonly IntPtr _containerBase;
        private GetBagItemDelegate _getBagItem;

        public WoWContainer(IntPtr objectPtr) : base(objectPtr)
        {
            if (IsValid)
            {
                _containerBase = GetBagPtr();
            }
        }

        public WoWContainer(IntPtr objectPtr, int bagNumber) : this(objectPtr)
        {
            _bagNumber = bagNumber;
        }

        /// <summary>
        /// Slots in the bag
        /// </summary>
        public int Slots
        {
            get
            {
                int ret = Marshal.ReadInt32(_containerBase);

                if (ret == 150) //player Default bag is always 150! :S
                {
                    return 16;
                }

                return ret;
            }
        }

        /// <summary>
        /// The bag number
        /// </summary>
        public int Number { get { return _bagNumber; } }

        /// <summary>
        /// Empty slots in the bag
        /// </summary>
        public int EmptySlots
        {
            get
            {
                int freeSlots = Slots;

                for (int i = 0; i < Slots; ++i)
                {
                    if (GetItem(i).IsValid)
                    {
                        --freeSlots;
                    }
                }

                return freeSlots;
            }
        }

        /// <summary>
        /// Filled slots in the bag
        /// </summary>
        public int FilledSlots { get { return (Slots - EmptySlots); } }

        /// <summary>
        /// Gets a WoWContainer 
        /// </summary>
        /// <param name="index">0-4, 0 being default player bag </param>
        /// <returns></returns>
        public static WoWContainer GetBagSlot(int index)
        {
            if (index == 0) //default player bag logic...
            {
                return new WoWContainer(FluxWoW.Me, 0);
            }

            if (_getBagAtIndex == null)
            {
                _getBagAtIndex = Utilities.RegisterDelegate<GetBagAtIndexDelegate>((uint) GlobalOffsets.GetBagAtIndex);
            }

            ulong contGuid = _getBagAtIndex(index - 1); //-1 because WoW only accepts 0-3

            return new WoWContainer(ObjMgr.GetObjectByGuid(contGuid), index);
        }

        /// <summary>
        /// Get an items from he bag
        /// </summary>
        /// <param name="slot">the slot index (0 based)</param>
        /// <returns>WoWItem</returns>
        public WoWItem GetItem(int slot)
        {
            if (_getBagItem == null)
            {
                _getBagItem = Utilities.RegisterDelegate<GetBagItemDelegate>((uint) GlobalOffsets.GetBagItem);
            }

            if (FluxWoW.Me == ObjPtr)
            {
                slot += 0x16;
            }

            return new WoWItem(_getBagItem(_containerBase, slot));
        }

        private T GetStorageField<T>(ContainerFields fields) where T : struct
        {
            return GetStorageField<T>((uint) fields);
        }

        #region GetBagItemDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate IntPtr GetBagItemDelegate(IntPtr instance, int index);

        #endregion

        #region Nested type: GetBagAtIndexDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong GetBagAtIndexDelegate(int index);

        #endregion
    }
}