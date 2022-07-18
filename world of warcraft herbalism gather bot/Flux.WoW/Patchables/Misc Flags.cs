using System;

namespace Flux.WoW.Patchables
{
    internal enum Gossip_Guard
    {
        GOSSIP_GUARD_BANK = 32,
        GOSSIP_GUARD_RIDE = 33,
        GOSSIP_GUARD_GUILD = 34,
        GOSSIP_GUARD_INN = 35,
        GOSSIP_GUARD_MAIL = 36,
        GOSSIP_GUARD_AUCTION = 37,
        GOSSIP_GUARD_WEAPON = 38,
        GOSSIP_GUARD_STABLE = 39,
        GOSSIP_GUARD_BATTLE = 40,
        GOSSIP_GUARD_SPELLTRAINER = 41,
        GOSSIP_GUARD_SKILLTRAINER = 42
    }

    internal enum Gossip_Guard_Spell
    {
        GOSSIP_GUARD_SPELL_WARRIOR = 64,
        GOSSIP_GUARD_SPELL_PALADIN = 65,
        GOSSIP_GUARD_SPELL_HUNTER = 66,
        GOSSIP_GUARD_SPELL_ROGUE = 67,
        GOSSIP_GUARD_SPELL_PRIEST = 68,
        GOSSIP_GUARD_SPELL_UNKNOWN1 = 69,
        GOSSIP_GUARD_SPELL_SHAMAN = 70,
        GOSSIP_GUARD_SPELL_MAGE = 71,
        GOSSIP_GUARD_SPELL_WARLOCK = 72,
        GOSSIP_GUARD_SPELL_UNKNOWN2 = 73,
        GOSSIP_GUARD_SPELL_DRUID = 74
    }

    internal enum Gossip_Guard_Skill
    {
        GOSSIP_GUARD_SKILL_ALCHEMY = 80,
        GOSSIP_GUARD_SKILL_BLACKSMITH = 81,
        GOSSIP_GUARD_SKILL_COOKING = 82,
        GOSSIP_GUARD_SKILL_ENCHANT = 83,
        GOSSIP_GUARD_SKILL_FIRSTAID = 84,
        GOSSIP_GUARD_SKILL_FISHING = 85,
        GOSSIP_GUARD_SKILL_HERBALISM = 86,
        GOSSIP_GUARD_SKILL_LEATHER = 87,
        GOSSIP_GUARD_SKILL_MINING = 88,
        GOSSIP_GUARD_SKILL_SKINNING = 89,
        GOSSIP_GUARD_SKILL_TAILORING = 90,
        GOSSIP_GUARD_SKILL_ENGINERING = 91
    }

    internal enum CreatureFlagsExtra
    {
        CREATURE_FLAG_EXTRA_INSTANCE_BIND = 0x00000001, // creature kill bind instance with killer and killer's group
        CREATURE_FLAG_EXTRA_CIVILIAN = 0x00000002, // not aggro (ignore faction/reputation hostility)
        CREATURE_FLAG_EXTRA_NO_PARRY = 0x00000004, // creature can't parry
        CREATURE_FLAG_EXTRA_NO_PARRY_HASTEN = 0x00000008, // creature can't counter-attack at parry
        CREATURE_FLAG_EXTRA_NO_BLOCK = 0x00000010, // creature can't block
        CREATURE_FLAG_EXTRA_NO_CRUSH = 0x00000020, // creature can't do crush attacks
        CREATURE_FLAG_EXTRA_NO_XP_AT_KILL = 0x00000040, // creature kill not provide XP
        CREATURE_FLAG_EXTRA_INVISIBLE = 0x00000080,
        // creature is always invisible for player (mostly trigger creatures)
    }

    [Flags]
    internal enum MovementFlags
    {
        Forward = 0x1,
        Backward = 0x2,
        StrafeLeft = 0x4,
        StrafeRight = 0x8,

        StrafeMask = StrafeLeft | StrafeRight,

        Left = 0x10,
        Right = 0x20,

        TurnMask = Left | Right,

        MoveMask = Forward | Backward | StrafeMask | TurnMask,

        PitchUp = 0x40,
        PitchDown = 0x80,
        Walk = 0x100,
        TimeValid = 0x200,
        Immobilized = 0x400,
        DontCollide = 0x800,
        // JUMPING
        Redirected = 0x1000,
        Rooted = 0x2000,
        Falling = 0x4000,
        FallenFar = 0x8000,
        PendingStop = 0x10000,
        Pendingunstrafe = 0x20000,
        Pendingfall = 0x40000,
        Pendingforward = 0x80000,
        PendingBackward = 0x100000,
        PendingStrafeLeft = 0x200000,
        PendingStrafeRght = 0x400000,
        PendMoveMask = 0x180000,
        PendStrafeMask = 0x600000,
        PendingMask = 0x7f0000,
        Moved = 0x800000,
        Sliding = 0x1000000,
        Swimming = 0x2000000,
        SplineMover = 0x4000000,
        SpeedDirty = 0x8000000,
        Halted = 0x10000000,
        Nudge = 0x20000000,

        FallMask = 0x100c000,
        Local = 0x500f400,
        PitchMask = 0xc0,
        MotionMask = 0xff,
        StoppedMask = 0x3100f,
    }
}