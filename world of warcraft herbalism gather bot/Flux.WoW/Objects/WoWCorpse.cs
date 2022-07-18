using System;

using Flux.WoW.Patchables;

namespace Flux.WoW.Objects
{
    public class WoWCorpse : WoWObject
    {
        public WoWCorpse(IntPtr objectPtr) : base(objectPtr)
        {
        }

        public WoWCorpse(ulong guid) : base(guid)
        {
        }

        public CorpseType CorpseType { get { return (CorpseType) GetStorageField<uint>(CorpseFields.CORPSE_FIELD_BYTES_1); } }

        private uint Flags { get { return GetStorageField<uint>(CorpseFields.CORPSE_FIELD_FLAGS); } }

        public bool Lootable { get { return HasFlag(Flags, (uint) CorpseFlags.CORPSE_FLAG_LOOTABLE); } }

        public bool IsOnlyBones { get { return HasFlag(Flags, (uint) CorpseFlags.CORPSE_FLAG_BONES); } }

        private T GetStorageField<T>(CorpseFields field) where T : struct
        {
            return GetStorageField<T>((uint) field);
        }

        #region Nested type: CorpseFlags

        private enum CorpseFlags
        {
            CORPSE_FLAG_NONE = 0x00,
            CORPSE_FLAG_BONES = 0x01,
            CORPSE_FLAG_UNK1 = 0x02,
            CORPSE_FLAG_UNK2 = 0x04,
            CORPSE_FLAG_HIDE_HELM = 0x08,
            CORPSE_FLAG_HIDE_CLOAK = 0x10,
            CORPSE_FLAG_LOOTABLE = 0x20
        }

        #endregion
    }

    public enum CorpseType
    {
        CORPSE_BONES = 0,
        CORPSE_RESURRECTABLE_PVE = 1,
        CORPSE_RESURRECTABLE_PVP = 2
    }
}