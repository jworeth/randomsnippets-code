using System;

namespace Flux.WoW.Objects
{
    public partial class WoWUnit
    {
        #region Nested type: StateFlag

        /// <summary>
        /// Used in UNIT_FIELD_BYTES_1, 3rd byte
        /// </summary>
        [Flags]
        private enum StateFlag
        {
            None = 0,
            AlwaysStand = 0x1,
            Sneaking = 0x2,
            UnTrackable = 0x4,
        }

        #endregion

        #region Nested type: UnitBaseFields

        private enum UnitBaseFields
        {
            MovementInfoBase = 0x788,
            BoundingBoxWidth = MovementInfoBase + 0xC8,
            BoundingBoxHeight = BoundingBoxWidth+0x4,

            IsAutoAttacking = 0xA20,
            IsAutoAttacking2 = 0xA24,

            SpellToCast = 0xA48,
            SpellCasting = 0xA4C,
            SpellTarget = 0xA50,
            SpellTimeStart = 0xA58,
            SpellTimeEnd = 0xA5C,

            SpellChanneling = 0xA60,
            SpellChannelingStart = 0xA64,
            SpellChannelingStop = 0xA68,

            SelectionFlags = 0xA70,

            CurrentTime = 0xA94,

            AuraCount1 = 0xDD8,
            AuraCount2 = 0xC5C
        }

        #endregion

        #region Nested type: UnitDynamicFlags

        [Flags]
        private enum UnitDynamicFlags
        {
            None = 0,
            Lootable = 0x1,
            TrackUnit = 0x2,
            TaggedByOther = 0x4,
            TaggedByMe = 0x8,
            SpecialInfo = 0x10,
            Dead = 0x20,
            ReferAFriendLinked = 0x40,
            IsTappedByAllThreatList = 0x80,
        }

        #endregion

        #region Nested type: UnitFlags

        [Flags]
        private enum UnitFlags : uint
        {
            None = 0,
            Sitting = 0x1,
            //SelectableNotAttackable_1 = 0x2,
            Influenced = 0x4, // Stops movement packets
            PlayerControlled = 0x8, // 2.4.2
            Totem = 0x10,
            Preparation = 0x20, // 3.0.3
            PlusMob = 0x40, // 3.0.2
            //SelectableNotAttackable_2 = 0x80,
            NotAttackable = 0x100,
            //Flag_0x200 = 0x200,
            Looting = 0x400,
            PetInCombat = 0x800, // 3.0.2
            PvPFlagged = 0x1000,
            Silenced = 0x2000, //3.0.3
            //Flag_14_0x4000 = 0x4000,
            //Flag_15_0x8000 = 0x8000,
            //SelectableNotAttackable_3 = 0x10000,
            Pacified = 0x20000, //3.0.3
            Stunned = 0x40000,
            CanPerformAction_Mask1 = 0x60000,
            Combat = 0x80000, // 3.1.1
            TaxiFlight = 0x100000, // 3.1.1
            Disarmed = 0x200000, // 3.1.1
            Confused = 0x400000, //  3.0.3
            Fleeing = 0x800000,
            Possessed = 0x1000000, // 3.1.1
            NotSelectable = 0x2000000,
            Skinnable = 0x4000000,
            Mounted = 0x8000000,
            //Flag_28_0x10000000 = 0x10000000,
            Dazed = 0x20000000,
            Sheathe = 0x40000000,
            //Flag_31_0x80000000 = 0x80000000,
        }

        #endregion

        #region Nested type: UnitFlags2

        [Flags]
        private enum UnitFlags2
        {
            FeignDeath = 0x1,
            NoModel = 0x2,
            Flag_0x4 = 0x4,
            Flag_0x8 = 0x8,
            Flag_0x10 = 0x10,
            Flag_0x20 = 0x20,
            ForceAutoRunForward = 0x40,

            /// <summary>
            /// Treat as disarmed?
            /// Treat main and off hand weapons as not being equipped?
            /// </summary>
            Flag_0x80 = 0x80,

            /// <summary>
            /// Skip checks on ranged weapon?
            /// Treat it as not being equipped?
            /// </summary>
            Flag_0x400 = 0x400,

            Flag_0x800 = 0x800,
            Flag_0x1000 = 0x1000,
        }

        #endregion

        #region Nested type: UnitNPCFlags

        private enum UnitNPCFlags
        {
            UNIT_NPC_FLAG_NONE = 0x00000000,
            UNIT_NPC_FLAG_GOSSIP = 0x00000001, // 100%
            UNIT_NPC_FLAG_QUESTGIVER = 0x00000002, // guessed, probably ok
            UNIT_NPC_FLAG_UNK1 = 0x00000004,
            UNIT_NPC_FLAG_UNK2 = 0x00000008,
            UNIT_NPC_FLAG_TRAINER = 0x00000010, // 100%
            UNIT_NPC_FLAG_TRAINER_CLASS = 0x00000020, // 100%
            UNIT_NPC_FLAG_TRAINER_PROFESSION = 0x00000040, // 100%
            UNIT_NPC_FLAG_VENDOR = 0x00000080, // 100%
            UNIT_NPC_FLAG_VENDOR_AMMO = 0x00000100, // 100%, general goods vendor
            UNIT_NPC_FLAG_VENDOR_FOOD = 0x00000200, // 100%
            UNIT_NPC_FLAG_VENDOR_POISON = 0x00000400, // guessed
            UNIT_NPC_FLAG_VENDOR_REAGENT = 0x00000800, // 100%
            UNIT_NPC_FLAG_REPAIR = 0x00001000, // 100%
            UNIT_NPC_FLAG_FLIGHTMASTER = 0x00002000, // 100%
            UNIT_NPC_FLAG_SPIRITHEALER = 0x00004000, // guessed
            UNIT_NPC_FLAG_SPIRITGUIDE = 0x00008000, // guessed
            UNIT_NPC_FLAG_INNKEEPER = 0x00010000, // 100%
            UNIT_NPC_FLAG_BANKER = 0x00020000, // 100%
            UNIT_NPC_FLAG_PETITIONER = 0x00040000, // 100% 0xC0000 = guild petitions, 0x40000 = arena team petitions
            UNIT_NPC_FLAG_TABARDDESIGNER = 0x00080000, // 100%
            UNIT_NPC_FLAG_BATTLEMASTER = 0x00100000, // 100%
            UNIT_NPC_FLAG_AUCTIONEER = 0x00200000, // 100%
            UNIT_NPC_FLAG_STABLEMASTER = 0x00400000, // 100%
            UNIT_NPC_FLAG_GUILD_BANKER = 0x00800000, // cause client to send 997 opcode
            UNIT_NPC_FLAG_SPELLCLICK = 0x01000000, // cause client to send 1015 opcode (spell click)
            UNIT_NPC_FLAG_GUARD = 0x10000000, // custom flag for guards
        }

        #endregion
    }
}