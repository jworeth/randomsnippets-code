using System;
using System.Drawing;

namespace Flux.WoW
{
    public enum ShapeshiftForm
    {
        Normal = 0,
        Cat = 1,
        TreeOfLife = 2,
        Travel = 3,
        Aqua = 4,
        Bear = 5,
        Ambient = 6,
        Ghoul = 7,
        DireBear = 8,
        CreatureBear = 14,
        CreatureCat = 15,
        GhostWolf = 16,
        BattleStance = 17,
        DefensiveStance = 18,
        BerserkerStance = 19,
        EpicFlightForm = 27,
        Shadow = 28,
        Stealth = 30,
        Moonkin = 31,
        SpiritOfRedemption = 32
    }

    public enum StandState : byte
    {
        Stand = 0,
        Sit = 1,
        SittingInChair = 2,
        Sleeping = 3,
        SittingInLowChair = 4,
        SittingInMediumChair = 5,
        SittingInHighChair = 6,
        Dead = 7,
        Kneeling = 8,
        Type9 = 9,
    }

    [Flags]
    public enum PvPState
    {
        None = 0,
        PVP = 0x1,
        FFAPVP = 0x4,
        InPvPSanctuary = 0x8,
    }

    public enum WoWItemType
    {
        Consumable,
        Container,
        Weapon,
        Gem,
        Armor,
        Reagent,
        Projectile,
        TradeGoods,
        Generic,
        Recipe,
        Money,
        Quiver,
        QUest,
        Key,
        Permanent,
        Misc
    }

    public enum WoWPowerType
    {
        Health=-2,
        None,
        Mana,
        Rage,
        Focus,
        Energy,
        Happiness,
        Runes,
        RunicPower,
    }

    [Flags]
    public enum WoWObjectTypeFlag
    {
        Object = 0x1,
        Item = 0x2,
        Container = 0x4,
        Unit = 0x8,
        Player = 0x10,
        GameObject = 0x20,
        DynamicObject = 0x40,
        Corpse = 0x80,
        AiGroup = 0x100,
        AreaTrigger = 0x200,
    }

    public static class WoWObjectTypeFlagExtension
    {
        public static bool HasFlag(this WoWObjectTypeFlag item, WoWObjectTypeFlag flag)
        {
            return (item & flag) != 0;
        }
    }

    public enum WoWObjectType : uint
    {
        Object = 0,
        Item = 1,
        Container = 2,
        Unit = 3,
        Player = 4,
        GameObject = 5,
        DynamicObject = 6,
        Corpse = 7,
        AiGroup = 8,
        AreaTrigger = 9
    }

    public enum WoWGameObjectType : uint
    {
        Door = 0,
        Button = 1,
        QuestGiver = 2,
        Chest = 3,
        Binder = 4,
        Generic = 5,
        Trap = 6,
        Chair = 7,
        SpellFocus = 8,
        Text = 9,
        Goober = 0xa,
        Transport = 0xb,
        AreaDamage = 0xc,
        Camera = 0xd,
        WorldObj = 0xe,
        MapObjTransport = 0xf,
        DuelArbiter = 0x10,
        FishingNode = 0x11,
        Ritual = 0x12,
        Mailbox = 0x13,
        AuctionHouse = 0x14,
        SpellCaster = 0x16,
        MeetingStone = 0x17,
        FlagStand = 0x18,
        FishingPool = 0x19,
        FlagDrop = 0x1A,
        MiniGame = 0x1B,
        LotterKiosk = 28,
        CapturePoint = 29,
        AuraGenerator = 30,
        DungeonDifficulty = 31,
        BarberChair = 32,
        DestructibleBuilding = 33,
        GuildBank = 34,
        FORCEDWORD = 0xFFFFFFFF,
    }

    public enum WoWEquipSlot
    {
        Head = 0,
        Neck,
        Shoulders,
        Body,
        Chest,
        Waist,
        Legs,
        Feet,
        Wrists,
        Hands,
        Finger1,
        Finger2,
        Trinket1,
        Trinket2,
        Back,
        MainHand,
        OffHand,
        Ranged,
        Tabard,
        Bag1,
        Bag2,
        Bag3,
        Bag4,
        NonEquipSlot
    }

    public enum WoWClass : uint
    {
        None = 0,
        Warrior = 1,
        Paladin = 2,
        Hunter = 3,
        Rogue = 4,
        Priest = 5,
        DeathKnight = 6,
        Shaman = 7,
        Mage = 8,
        Warlock = 9,
        Druid = 11,
    }

    public enum WoWClassification
    {
        Normal = 0,
        Elite = 1,
        RareElite = 2,
        WorldBoss = 3,
        Rare = 4
    }

    public enum WoWRace
    {
        Human = 1,
        Orc,
        Dwarf,
        NightElf,
        Undead,
        Tauren,
        Gnome,
        Troll,
        Goblin,
        BloodElf,
        Draenei,
        FelOrc,
        Naga,
        Broken,
        Skeleton = 15,
    }

    public enum WoWCreatureType
    {
        Unknown = 0,
        Beast,
        Dragon,
        Demon,
        Elemental,
        Giant,
        Undead,
        Humanoid,
        Critter,
        Mechanical,
        NotSpecified,
        Totem,
        NonCombatPet,
        GasCloud
    }

    public enum WoWGender
    {
        Male,
        Female,
        Unknown
    }

    public enum WoWUnitRelation : uint
    {
        Hated = 0,
        Hostile = 1,
        Unfriendly = 2,
        Neutral = 3,
        Friendly = 4,
    }

    public static class WoWUnitRelationExtensions
    {
        public static Color GetColor(this WoWUnitRelation r)
        {
            switch (r)
            {
                case WoWUnitRelation.Hated:
                    return Color.Red;
                case WoWUnitRelation.Hostile:
                    return Color.Red;
                case WoWUnitRelation.Unfriendly:
                    return Color.Orange;
                case WoWUnitRelation.Neutral:
                    return Color.Yellow;
                case WoWUnitRelation.Friendly:
                    return Color.LimeGreen;
                default:
                    throw new ArgumentOutOfRangeException("r");
            }
        }
    }

    public enum WoWDispelType : uint
    {
        None = 0,
        Magic = 1,
        Curse = 2,
        Disease = 3,
        Poison = 4,
        Stealth = 5,
        Invisibility = 6,
        All = 7,
        //Special_NpcOnly=8,
        Enrage = 9,
        //ZgTrinkets=10
    }

    public enum WoWQuestType : uint
    {
        Group = 1,
        Life = 21,
        PvP = 41,
        Raid = 62,
        Dungeon = 81,
        WorldEvent = 82,
        Legendary = 83,
        Escort = 84,
        Heroic = 85,
        Raid_10 = 88,
        Raid_25 = 89,
    }

    public enum SheathType : sbyte
    {
        Undetermined = -1,
        None = 0,
        Melee = 1,
        Ranged = 2,

        Shield = 4,
        Rod = 5,
        Light = 7
    }

    public enum BattlegroundStatus
    {
        None,
        Enqueued = 1,
        Preparing = 2,
        Active = 3,
        Finished = 4,
    }

    public enum BattlegroundJoinError
    {
        None = 0,
        Nothing = -1,

        /// <summary>
        /// You or one of your party members is a deserter
        /// </summary>
        Deserter = -2,

        /// <summary>
        /// Your group is not in the same team
        /// </summary>
        NotSameTeam = -3,

        /// <summary>
        /// Can only be enqueued for 3 battles at once
        /// </summary>
        Max3Battles = -4,

        /// <summary>
        /// You cannot queue for a rated match while enqueued for other battles
        /// </summary>
        StillEnqueued = -5,

        /// <summary>
        /// You cannot queue for another battle while queued for a rated arena match
        /// </summary>
        InRatedMatch = -6,

        /// <summary>
        /// Your team has left the arena queue
        /// </summary>
        TeamLeftQueue = -7,

        /// <summary>
        /// Your group has joined a battleground queue but you are not eglible.
        /// (This is the same error message for all other numbers but valid BattlegroundIds)
        /// </summary>
        GroupJoinedNotEligible = - 8
    }

    public enum BattlegroundSide
    {
        Alliance = 0,
        Horde = 1,
        End
    }

    public enum ArenaType
    {
        None = 0,
        TwoVsTwo = 2,
        ThreeVsThree = 3,
        FiveVsFive = 5
    }
}