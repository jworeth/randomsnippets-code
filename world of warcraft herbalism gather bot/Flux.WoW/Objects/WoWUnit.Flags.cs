using System;
using System.Collections.Specialized;
using Flux.WoW.Patchables;

namespace Flux.WoW.Objects
{
    public partial class WoWUnit
    {
        #region Bytes0
        private byte[] Bytes0 { get { return BitConverter.GetBytes(GetStorageField<uint>(UnitFields.UNIT_FIELD_BYTES_0)); } }
        public WoWClass Class { get { return (WoWClass)Bytes0[1]; } }
        public WoWRace Race { get { return (WoWRace)Bytes0[0]; } }
        public WoWPowerType PowerType { get { return (WoWPowerType)Bytes0[3]; } }
        public WoWGender Gender { get { return (WoWGender)Bytes0[2]; } }

        #endregion

        #region Bytes1

        private byte[] Bytes1 { get { return BitConverter.GetBytes(GetStorageField<uint>(UnitFields.UNIT_FIELD_BYTES_1)); } }
        public bool Stealthed { get { return Flag((uint)StateFlag.Sneaking, Bytes1[2]); } }
        public bool CanTrack { get { return !Flag((uint)StateFlag.UnTrackable, Bytes1[2]); } }
        public StandState StandState { get { return (StandState)Bytes1[1]; } }

        #endregion

        #region Bytes2

        private byte[] Bytes2 { get { return BitConverter.GetBytes(GetStorageField<uint>(UnitFields.UNIT_FIELD_BYTES_2)); } }
        public PvPState PvPState { get { return (PvPState)Bytes2[1]; } }
        public SheathType SheathType { get { return (SheathType)(sbyte)Bytes2[0]; } }
        public ShapeshiftForm Shapeshift { get { return (ShapeshiftForm)Bytes2[3]; } }

        #endregion

        #region Flags

        private BitVector32 Flags { get { return new BitVector32(GetStorageField<int>(UnitFields.UNIT_FIELD_FLAGS)); } }
        public bool InCombat { get { return Flag(UnitFlags.Combat); } }
        public bool Skinnable { get { return Flag(UnitFlags.Skinnable); } }
        public bool Dazed { get { return Flag(UnitFlags.Dazed); } }
        public bool Disarmed { get { return Flag(UnitFlags.Disarmed); } }
        public bool Attackable { get { return !Flag(UnitFlags.NotAttackable); } }
        public bool PvpFlagged { get { return Flag(UnitFlags.PvPFlagged); } }
        public bool Silenced { get { return Flag(UnitFlags.Silenced); } }
        public bool Fleeing { get { return Flag(UnitFlags.Fleeing); } }
        public bool Pacified { get { return Flag(UnitFlags.Pacified); } }
        public bool Stunned { get { return Flag(UnitFlags.Stunned); } }
        public bool Rooted { get { return Flag(UnitFlags.Influenced); } }
        public bool CanSelect { get { return !Flag(UnitFlags.NotSelectable); } }
        public bool Aggro { get { return Flag((UnitFlags)0x2000); } }
        public bool Possessed { get { return Flag(UnitFlags.Possessed); } }
        public bool Elite { get { return Flag(UnitFlags.PlusMob); } }
        public bool Looting { get { return Flag(UnitFlags.Looting); } }
        public bool PetInCombat { get { return Flag(UnitFlags.PetInCombat); } }
        public bool Mounted { get { return Flag(UnitFlags.Mounted); } }
        public bool IsTotem { get { return Flag(UnitFlags.Totem); } }

        /// <summary>
        /// Taxi flights. Not flying mounts!!
        /// </summary>
        public bool OnTaxi { get { return Flag(UnitFlags.TaxiFlight); } }

        #endregion

        #region FlagsNpc

        private BitVector32 FlagsNpc { get { return new BitVector32(GetStorageField<int>(UnitFields.UNIT_NPC_FLAGS)); } }
        public bool CanGossip { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_GOSSIP); } }
        public bool IsQuestGiver { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_QUESTGIVER); } }
        public bool IsTrainer { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_TRAINER); } }
        public bool IsClassTrainer { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_TRAINER_CLASS); } }
        public bool IsProfessionTrainer { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_TRAINER_PROFESSION); } }
        public bool IsVendor { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_VENDOR); } }
        public bool IsAmmoVendor { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_VENDOR_AMMO); } }
        public bool IsFoodVendor { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_VENDOR_FOOD); } }
        public bool IsPoisonVendor { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_VENDOR_POISON); } }
        public bool IsReagentVendor { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_VENDOR_REAGENT); } }
        public bool IsRepairMerchant { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_REPAIR); } }
        public bool IsFlightMaster { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_FLIGHTMASTER); } }
        public bool IsSpiritHealer { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_SPIRITHEALER); } }
        public bool IsInnKeeper { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_INNKEEPER); } }
        public bool IsBanker { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_BANKER); } }
        public bool IsTabardDesigner { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_TABARDDESIGNER); } }
        public bool IsBattleMaster { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_BATTLEMASTER); } }
        public bool IsAuctioneer { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_AUCTIONEER); } }
        public bool IsStableMaster { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_STABLEMASTER); } }
        public bool IsGuard { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_GUARD); } }
        public bool IsGuildBanker { get { return Flag(UnitNPCFlags.UNIT_NPC_FLAG_GUILD_BANKER); } }

        #endregion

        #region State

        private uint Flags2 { get { return GetStorageField<uint>(UnitFields.UNIT_FIELD_FLAGS_2); } }
        public bool FeignDeathed { get { return Flag(Flags2, (uint)UnitFlags2.FeignDeath); } }

        #endregion

        #region FlagsDynamic

        private uint FlagsDynamic { get { return GetStorageField<uint>(UnitFields.UNIT_DYNAMIC_FLAGS); } }
        public bool Tapped { get { return Flag(FlagsDynamic, (uint)UnitDynamicFlags.TaggedByOther); } }
        public bool TappedByMe { get { return Flag(FlagsDynamic, (uint)UnitDynamicFlags.TaggedByMe); } }
        public bool CanLoot { get { return Flag(FlagsDynamic, (uint)UnitDynamicFlags.Lootable); } }
        public bool Dead { get { return CurrentHealth == 0; } }
        public bool Tracked { get { return Flag(FlagsDynamic, (uint)UnitDynamicFlags.TrackUnit); } }

        #endregion
    }
}