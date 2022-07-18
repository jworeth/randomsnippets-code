using System;

using Flux.WoW.Patchables;

namespace Flux.WoW.Objects
{
    public class WoWItem : WoWObject
    {
        public uint ID;

        public WoWItem(IntPtr objectPtr) : base(objectPtr)
        {
        }

        public WoWItem(uint itemId) : base(IntPtr.Zero)
        {
            ID = itemId;
        }

        public ulong OwnerGuid { get { return GetStorageField<ulong>(ItemFields.ITEM_FIELD_OWNER); } }

        public ulong ContainerGuid { get { return GetStorageField<ulong>(ItemFields.ITEM_FIELD_CONTAINED); } }

        public ulong CreatorGuid { get { return GetStorageField<ulong>(ItemFields.ITEM_FIELD_CREATOR); } }

        public ulong GiftCreatorGuid { get { return GetStorageField<ulong>(ItemFields.ITEM_FIELD_GIFTCREATOR); } }

        public uint StackCount { get { return GetStorageField<uint>(ItemFields.ITEM_FIELD_STACK_COUNT); } }

        public uint Duration { get { return GetStorageField<uint>(ItemFields.ITEM_FIELD_DURATION); } }

        public uint SpellCharges { get { return GetStorageField<uint>(ItemFields.ITEM_FIELD_SPELL_CHARGES); } }

        public uint Flags { get { return GetStorageField<uint>(ItemFields.ITEM_FIELD_FLAGS); } }

        public uint PropertySeed { get { return GetStorageField<uint>(ItemFields.ITEM_FIELD_PROPERTY_SEED); } }

        public uint RandomPropertiesId { get { return GetStorageField<uint>(ItemFields.ITEM_FIELD_RANDOM_PROPERTIES_ID); } }

        public uint TextId { get { return GetStorageField<uint>(ItemFields.ITEM_FIELD_ITEM_TEXT_ID); } }

        public uint Durability { get { return GetStorageField<uint>(ItemFields.ITEM_FIELD_DURABILITY); } }

        public uint MaxDurability { get { return GetStorageField<uint>(ItemFields.ITEM_FIELD_MAXDURABILITY); } }

        private T GetStorageField<T>(ItemFields field) where T : struct
        {
            return GetStorageField<T>((uint) field);
        }
    }
}