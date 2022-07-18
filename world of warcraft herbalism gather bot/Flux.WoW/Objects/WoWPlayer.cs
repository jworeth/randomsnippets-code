using System;
using System.Collections.Specialized;

using Flux.WoW.Patchables;

namespace Flux.WoW.Objects
{
    public class WoWPlayer : WoWUnit
    {
        #region Stuff

        public WoWPlayer(IntPtr objectPtr) : base(objectPtr)
        {
        }

        private BitVector32 Flags { get { return new BitVector32(GetStorageField<int>(PlayerFields.PLAYER_FLAGS)); } }

        public bool IsGhost { get { return Flags[(int) PlayerFlags.Ghost]; } }
        public bool AFK { get { return Flags[(int) PlayerFlags.AFK]; } }
        public bool DND { get { return Flags[(int) PlayerFlags.DND]; } }
        public bool Resting { get { return Flags[(int) PlayerFlags.Resting]; } }
        public bool IsGroupLeader { get { return Flags[(int) PlayerFlags.GroupLeader]; } }

        public WoWItem GetEquip(int slot)
        {
            return GetEquip((WoWEquipSlot) slot);
        }

        public WoWItem GetEquip(WoWEquipSlot slot)
        {
            uint field = (uint) PlayerFields.PLAYER_VISIBLE_ITEM_1_ENTRYID + ((uint) slot * 2);

            return new WoWItem(GetStorageField<uint>(field));
        }

        private T GetStorageField<T>(PlayerFields field) where T : struct
        {
            return GetStorageField<T>((uint) field);
        }

        #endregion

        private byte[] Bytes { get { return BitConverter.GetBytes(GetStorageField<uint>(PlayerFields.PLAYER_BYTES)); } }
        private byte[] Bytes2 { get { return BitConverter.GetBytes(GetStorageField<uint>(PlayerFields.PLAYER_BYTES_2)); } }
        private byte[] Bytes3 { get { return BitConverter.GetBytes(GetStorageField<uint>(PlayerFields.PLAYER_BYTES_3)); } }

        #region Nested type: PlayerFlags

        [Flags]
        private enum PlayerFlags : uint
        {
            None = 0,
            GroupLeader = 0x1, //2.4.2
            AFK = 0x2, // 2.4.2
            DND = 0x4, // 2.4.2
            GM = 0x8,
            Ghost = 0x10,
            Resting = 0x20,
            Flag_0x40 = 0x40,
            FreeForAllPVP = 0x80, // 2.4.2
            ContestedPvP = 0x100, // 2.4.2
            PVP = 0x200,
            HideHelm = 0x400,
            HideCloak = 0x800,
            PartialPlayTime = 0x1000, //played long time
            NoPlayTime = 0x2000, //played too long time
            OutOfBounds = 0x4000, // Lua_IsOutOfBounds
            Flag_0x8000 = 0x8000,
            InPvPSanctuary = 0x10000,
            Flag_0x20000 = 0x20000, // Taxi Time Test
            PVPTimerActive = 0x40000,
        }

        #endregion

        #region Descriptor Wrappers

        public uint GuildId { get { return GetStorageField<uint>(PlayerFields.PLAYER_GUILDID); } }
        public uint GuildRank { get { return GetStorageField<uint>(PlayerFields.PLAYER_GUILDRANK); } }
        public uint GuildTimestamp { get { return GetStorageField<uint>(PlayerFields.PLAYER_GUILD_TIMESTAMP); } }

        public uint DuelTeam { get { return GetStorageField<uint>(PlayerFields.PLAYER_DUEL_TEAM); } }

        public string ChosenTitle
        {
            get
            {
                return
                    FluxWoW.Db[ClientDb.CharTitles].GetRow(GetStorageField<int>(PlayerFields.PLAYER_CHOSEN_TITLE)).GetField
                        <string>(3);
            }
        }

        public uint HonorToday { get { return GetStorageField<uint>(PlayerFields.PLAYER_FIELD_TODAY_CONTRIBUTION); } }
        public uint HonorYesterday { get { return GetStorageField<uint>(PlayerFields.PLAYER_FIELD_YESTERDAY_CONTRIBUTION); } }
        public uint LifetimeHonorableKills { get { return GetStorageField<uint>(PlayerFields.PLAYER_FIELD_LIFETIME_HONORBALE_KILLS); } }

        #endregion
    }
}