using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Flux.Utilities;
using Flux.WoW.Native;
using Flux.WoW.Patchables;

namespace Flux.WoW.Objects
{
    public class WoWActivePlayer : WoWPlayer
    {
        public WoWActivePlayer(IntPtr objectPtr) : base(objectPtr)
        {
        }

        public uint Experience { get { return GetStorageField<uint>(PlayerFields.PLAYER_XP); } }
        public int ExperienceToNextLevel { get { return GetStorageField<int>(PlayerFields.PLAYER_NEXT_LEVEL_XP); } }
        public int RestedExperience { get { return GetStorageField<int>(PlayerFields.PLAYER_REST_STATE_EXPERIENCE); } }
        public string BindLocation { get { return Lua.GetReturnVal<string>("GetBindLocation()", 0); } }
        public int Coinage { get { return GetStorageField<int>(PlayerFields.PLAYER_FIELD_COINAGE); } }
        public byte ComboPoints { get { return Reader.Read<byte>((uint) GlobalOffsets.LocalPlayerComboPoints); } }
        public int CharacterPoints1 { get { return GetStorageField<int>(PlayerFields.PLAYER_CHARACTER_POINTS1); } }
        public int CharacterPoints2 { get { return GetStorageField<int>(PlayerFields.PLAYER_CHARACTER_POINTS2); } }
        public float BlockPercent { get { return GetStorageField<float>(PlayerFields.PLAYER_BLOCK_PERCENTAGE); } }
        public float DodgePercent { get { return GetStorageField<float>(PlayerFields.PLAYER_DODGE_PERCENTAGE); } }
        public float ParryPercent { get { return GetStorageField<float>(PlayerFields.PLAYER_PARRY_PERCENTAGE); } }
        public uint Expertise { get { return GetStorageField<uint>(PlayerFields.PLAYER_EXPERTISE); } }
        public uint ExpertiseOffhand { get { return GetStorageField<uint>(PlayerFields.PLAYER_OFFHAND_EXPERTISE); } }
        public float CritPercent { get { return GetStorageField<float>(PlayerFields.PLAYER_CRIT_PERCENTAGE); } }
        public float RangedCritPercent { get { return GetStorageField<float>(PlayerFields.PLAYER_RANGED_CRIT_PERCENTAGE); } }
        public float OffhandCritPercent { get { return GetStorageField<float>(PlayerFields.PLAYER_OFFHAND_CRIT_PERCENTAGE); } }
        public uint ShieldBlock { get { return GetStorageField<uint>(PlayerFields.PLAYER_SHIELD_BLOCK); } }
        public float ShieldBlockCritPercent { get { return GetStorageField<float>(PlayerFields.PLAYER_SHIELD_BLOCK_CRIT_PERCENTAGE); } }
        public int AmmoId { get { return GetStorageField<int>(PlayerFields.PLAYER_AMMO_ID); } }
        public uint SelfResSpell { get { return GetStorageField<uint>(PlayerFields.PLAYER_SELF_RES_SPELL); } }
        public uint PvPMedals { get { return GetStorageField<uint>(PlayerFields.PLAYER_FIELD_PVP_MEDALS); } }
        public int Kills { get { return GetStorageField<int>(PlayerFields.PLAYER_FIELD_KILLS); } }
        public uint WatchedFactionIndex { get { return GetStorageField<uint>(PlayerFields.PLAYER_FIELD_WATCHED_FACTION_INDEX); } }
        public uint HonorCurrency { get { return GetStorageField<uint>(PlayerFields.PLAYER_FIELD_HONOR_CURRENCY); } }
        public uint ArenaCurrency { get { return GetStorageField<uint>(PlayerFields.PLAYER_FIELD_ARENA_CURRENCY); } }
        public uint MaxLevel { get { return GetStorageField<uint>(PlayerFields.PLAYER_FIELD_MAX_LEVEL); } }
        public uint GlyphsEnabledCount { get { return GetStorageField<uint>(PlayerFields.PLAYER_GLYPHS_ENABLED); } }
        public Point CorpsePoint { get { return (Point) Marshal.PtrToStructure((IntPtr) GlobalOffsets.CorpsePointStruct, typeof (Point)); } }

        public int EmptyInventorySlots
        {
            get
            {
                int ret = 0;

                for (int i = 0; i < 4; ++i)
                {
                    if (WoWContainer.GetBagSlot(i).IsValid)
                    {
                        ret += WoWContainer.GetBagSlot(i).EmptySlots;
                    }
                }

                return ret;
            }
        }

        public List<WoWSpell> KnownSpells
        {
            get
            {
                var ret = new List<WoWSpell>();
                var addr = (uint) GlobalOffsets.LocalPlayerKnownSpells;
                var tmp = Reader.Read<uint>(addr);
                while (tmp != 0)
                {
                    ret.Add(new WoWSpell(tmp));

                    addr += 0x4;
                    tmp = Reader.Read<uint>(addr);
                }

                return ret;
            }
        }

        public BitArray ExploredZones
        {
            get
            {
                var stored = new List<byte>();
                for (uint i = 0; i < 512; i++)
                {
                    stored.Add(GetStorageField<byte>((uint) PlayerFields.PLAYER_EXPLORED_ZONES_1 + i));
                }
                return new BitArray(stored.ToArray());
            }
        }

        public T GetStorageField<T>(PlayerFields field) where T : struct
        {
            return GetStorageField<T>((uint) field);
        }

        public uint GetQuestId(uint index)
        {
            uint field = (uint) PlayerFields.PLAYER_QUEST_LOG_1_1 + (index * 4);
            return GetStorageField<uint>(field);
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// 0-383
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int SkillInfo(int index)
        {
            var field = (uint) (PlayerFields.PLAYER_SKILL_INFO_1_1 + (index * 4));
            return GetStorageField<int>(field);
        }

        public float SpellCritPercent(int type)
        {
            var field = (uint) (PlayerFields.PLAYER_SPELL_CRIT_PERCENTAGE1 + (type * 4));
            return GetStorageField<float>(field);
        }

        /// <summary>
        /// 0-24
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public uint CombatRating(int index)
        {
            var field = (uint) (PlayerFields.PLAYER_FIELD_COMBAT_RATING_1 + (index * 4));
            return GetStorageField<uint>(field);
        }

        /// <summary>
        /// 0-20
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public uint ArenaTeamInfo(int index)
        {
            var field = (uint) (PlayerFields.PLAYER_FIELD_ARENA_TEAM_INFO_1_1 + (index * 4));
            return GetStorageField<uint>(field);
        }

        /// <summary>
        /// 0-3
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float RuneRegen(int index)
        {
            var field = (uint) (PlayerFields.PLAYER_RUNE_REGEN_1 + (index * 4));
            return GetStorageField<float>(field);
        }

        /// <summary>
        /// 0-5
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public uint GlyphSlots(int index)
        {
            var field = (uint) (PlayerFields.PLAYER_FIELD_GLYPH_SLOTS_1 + (index * 4));
            return GetStorageField<uint>(field);
        }

        /// <summary>
        /// 0-5
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public uint GlyphId(int index)
        {
            var field = (uint) (PlayerFields.PLAYER_FIELD_GLYPHS_1 + (index * 4));
            return GetStorageField<uint>(field);
        }

        public void ToggleAttack()
        {
            Lua.DoString("AttackTarget()");
        }

        public void ClearTarget()
        {
            Lua.DoString("ClearTarget()");
        }

        public WoWContainer GetBag(int index)
        {
            var field = (uint) ((uint) PlayerFields.PLAYER_FIELD_PACK_SLOT_1 + (2 * index));

            Logging.WriteDebug(GetStorageField<ulong>(field).ToString("X"));

            return null;
        }

        public bool CanCastSpell(string name)
        {
            var tmp = Lua.GetReturnVal<bool>("IsUsableSpell(\"" + name + "\")", 0);
            return tmp;
        }
    }
}