using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Flux.Utilities;
using Flux.WoW.Patchables;

namespace Flux.WoW
{
    public enum CacheDb
    {
        Creature = 0,
        GameObject = 1,
        ItemName = 2,
        Item = 3,
        Npc = 4,
        Name = 5,
        Guild = 6,
        Quest = 7,
        PageText = 8,
        PetName = 9,
        Petition = 10,
        ItemText = 11,
        WoW = 12,
        ArenaTeam = 13,
        Dance = 14
    }

    public class WoWCache
    {
        #region QuestFlags enum

        [Flags]
        public enum QuestFlags : uint
        {
            None = 0x0000,
            Deliver = 0x0001,
            Escort = 0x0002,
            Explore = 0x0004,
            Sharable = 0x0008,
            Exploration = 0x0010,
            Timed = 0x0020,
            Raid = 0x0040,
            TBCOnly = 0x0080,
            DeliverMore = 0x0100,
            HiddenRewards = 0x0200,
            Unknown4 = 0x0400,
            TBCRaces = 0x0800,
            Daily = 0x1000
        }

        #endregion

        private static GetInfoBlockByIdDelegate _getInfoBlockByIDDelegate;
        private readonly List<Cache> _cacheAddresses = new List<Cache>();

        public WoWCache()
        {
            // Base ptr to the DBCache shit
            var db = (IntPtr) GlobalOffsets.Base_DbCache;

            for (int i = 0; i < 15; i++)
            {
                uint infoBlockByIDAddress;

                switch ((CacheDb) i)
                {
                    case CacheDb.Creature:
                        infoBlockByIDAddress = (uint) GlobalOffsets.DbCreatureCache_GetInfoBlockById;
                        break;
                    case CacheDb.GameObject:
                        infoBlockByIDAddress = (uint)GlobalOffsets.DbGameObjectCache_GetInfoBlockById;
                        break;
                    case CacheDb.ItemName:
                        infoBlockByIDAddress = (uint)GlobalOffsets.DbItemNameCache_GetInfoBlockById;
                        break;
                    case CacheDb.Item:
                        infoBlockByIDAddress = (uint)GlobalOffsets.DBItemCache_GetInfoBlockByID;
                        break;
                    case CacheDb.Npc:
                        infoBlockByIDAddress = (uint)GlobalOffsets.DbNpcCache_GetInfoBlockById;
                        break;
                    case CacheDb.Name:
                        infoBlockByIDAddress = (uint)GlobalOffsets.DbNameCache_GetInfoBlockById;
                        break;
                    case CacheDb.Guild:
                        infoBlockByIDAddress = (uint)GlobalOffsets.DbGuildCache_GetInfoBlockById;
                        break;
                    case CacheDb.Quest:
                        infoBlockByIDAddress = (uint)GlobalOffsets.DbQuestCache_GetInfoBlockById;
                        break;
                    case CacheDb.PageText:
                        infoBlockByIDAddress = (uint)GlobalOffsets.DbPageTextCache_GetInfoBlockById;
                        break;
                    case CacheDb.PetName:
                        infoBlockByIDAddress = (uint)GlobalOffsets.DbPetNameCache_GetInfoBlockById;
                        break;
                    case CacheDb.Petition:
                        infoBlockByIDAddress = (uint)GlobalOffsets.DbPetitionCache_GetInfoBlockById;
                        break;
                    case CacheDb.ItemText:
                        infoBlockByIDAddress = (uint)GlobalOffsets.DbItemTextCache_GetInfoBlockById;
                        break;
                    case CacheDb.WoW:
                        infoBlockByIDAddress = (uint)GlobalOffsets.DbWoWCache_GetInfoBlockById;
                        break;
                    case CacheDb.ArenaTeam:
                        infoBlockByIDAddress = (uint)GlobalOffsets.DbArenaTeamCache_GetInfoBlockById;
                        break;
                    case CacheDb.Dance:
                        infoBlockByIDAddress = (uint)GlobalOffsets.DbDanceCache_GetInfoBlockById;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _cacheAddresses.Add(new Cache(db, infoBlockByIDAddress));
                //Logging.WriteDebug("Cache {0}: 0x{1}", (CacheDb) i, db.ToString("X8"));
                db = (IntPtr) (db.ToUInt32() + 0x88);
            }
        }

        public Cache this[CacheDb index] { get { return _cacheAddresses[(int) index]; } }

        #region Nested type: Cache

        public class Cache
        {
            internal readonly IntPtr _address;
            internal readonly uint _infoBlockByIDAddress;

            internal Cache(IntPtr address, uint infoBlockAddr)
            {
                _address = address;
                _infoBlockByIDAddress = infoBlockAddr;
            }

            public InfoBlock GetInfoBlockById(uint index)
            {
                if (_getInfoBlockByIDDelegate == null)
                {
                    _getInfoBlockByIDDelegate =
                        Utilities.RegisterDelegate<GetInfoBlockByIdDelegate>((uint) _infoBlockByIDAddress);
                }
                return new InfoBlock(_getInfoBlockByIDDelegate(_address, (int) index, 0, 0, 0, 0));
            }
        }

        #endregion

        #region Nested type: GetInfoBlockByIdDelegate

        /// <summary>
        /// Typedef for the virtual DbCache_GetInfoBlockById func
        /// </summary>
        /// <param name="instance">'this' needs to be the specific cache type pointer</param>
        /// <param name="index">The index to search</param>
        /// <param name="a3">Pass 0</param>
        /// <param name="a4">Pass 0</param>
        /// <param name="a5">Pass 0</param>
        /// <param name="a6">Pass 0</param>
        /// <returns>A pointer to an info block (struct) depending on the type of cache, and function pointer/cache pointer.</returns>
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetInfoBlockByIdDelegate(IntPtr instance, int index, int a3, int a4, int a5, int a6);

        #endregion

        #region Nested type: InfoBlock

        public class InfoBlock
        {
            private readonly IntPtr _add;

            public InfoBlock(IntPtr a)
            {
                _add = a;
            }

            public IntPtr Address { get { return _add; } }

            public ItemInfo Item
            {
                get
                {
                    if (_add == IntPtr.Zero)
                    {
                        return new ItemInfo();
                    }
                    return (ItemInfo) Marshal.PtrToStructure(_add, typeof (ItemInfo));
                }
            }
        }

        #endregion

        #region Nested type: ItemInfo

        [StructLayout(LayoutKind.Sequential)]
        public struct ItemInfo
        {
            /* 0x000*/
            internal int Unknown0x00; // Entry length?
            /* 0x004*/
            internal int ClassID;
            /* 0x008*/
            internal int SubTypeID;
            /* 0x00C*/
            internal int Unknown08; // padding?
            /* 0x010*/
            internal int DisplayInfoID;
            /* 0x014*/
            internal int Rarity;
            /* 0x018*/
            internal int Flags; // http://www.sourcepeek.com/wiki/Item_Types
            /* 0x01C*/
            internal int BuyPrice;
            /* 0x020*/
            internal int SellPrice;
            /* 0x024*/
            internal int EquipSlot;
            /* 0x028*/
            internal int AllowedClasses; // -1
            /* 0x02C*/
            internal int AllowedRaces; // -1
            /* 0x030*/
            internal int ItemLevel; // The actual base level of an item 
            /* 0x034*/
            internal int MinLevel; // Player's required level to use 
            /* 0x038*/
            internal int RequireSkill;
            /* 0x03C*/
            internal int RequireSkillLevel; //
            /* 0x040*/
            internal int RequireSpell;
            /* 0x044*/
            internal int RequireHonorRank;
            /* 0x048*/
            internal int RequireCityRank;
            /* 0x04C*/
            internal int RequireReputationFaction;
            /* 0x050*/
            internal int RequireReputationRank;
            /* 0x054*/
            internal int UniqueCount;
            /* 0x058*/
            internal int MaxStack;
            /* 0x05C*/
            internal int BagSlots;
            /* 0x060*/
            internal int Unk2;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)] /* 0x064*/ internal int[] Stat;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)] /* 0x08C*/ internal int[] StatModifier;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)] /* 0x0B4*/ internal float[] MinDamage;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)] /* 0x0C8*/ internal float[] MaxDamage;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)] /* 0x0DC*/ internal int[] DamageType;
            /* 0x0F0*/
            internal int Unk3;
            /* 0x0F4*/
            internal int Unk4;
            /* 0x0F8*/
            internal int Armor;
            /* 0x0FC*/
            internal int HolyRes;
            /* 0x100*/
            internal int FireRes;
            /* 0x104*/
            internal int NatureRes;
            /* 0x108*/
            internal int FrostRes;
            /* 0x10C*/
            internal int ShadowRes;
            /* 0x110*/
            internal int ArcaneRes;
            /* 0x114*/
            internal int Delay;
            /* 0x118*/
            internal int AmmoType;
            /* 0x11C*/
            internal float RangedModRange;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)] /* 0x120*/ internal int[] SpellID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)] /* 0x134*/ internal int[] SpellTrigger;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)] /* 0x13C*/ internal int[] SpellCharges;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)] /* 0x150*/ internal int[] SpellCooldown;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)] /* 0x164*/ internal int[] SpellCategory;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)] /* 0x178*/ internal int[] SpellCategoryCooldown;
            /* 0x18C*/
            internal int BondingType; //http://www.sourcepeek.com/wiki/Bond_Types
            /* 0x190*/
            internal int Text; // "Used by rogues to brew poison."
            /* 0x194*/
            internal int PageText;
            /* 0x198*/
            internal int LanguageID;
            /* 0x19C*/
            internal int PageMaterial;
            /* 0x1A0*/
            internal int StartQuest;
            /* 0x1A4*/
            internal int LockID;
            /* 0x1A8*/
            internal int Material; //lock type?
            /* 0x1AC*/
            internal int Unknown1BC;
            /* 0x1B0*/
            internal int Sheath;
            /* 0x1B4*/
            internal int Extra;
            /* 0x1B8*/
            internal int Block;
            /* 0x1BC*/
            internal int ItemSet;
            /* 0x1C0*/
            internal int MaxDurability;
            /* 0x1C4*/
            internal int Area;
            /* 0x1C8*/
            internal int BagFamily;
            /* 0x1CC*/
            internal int Unknown1DC; // Reference to which tools this item acts as 
            /* 0x1D0*/
            internal int ScriptName; // Color of socket 1 
            /* 0x1D4*/
            internal int DisenchantID;
            /* 0x1D8*/
            internal int Unknown0x1D8;
            /* 0x1DC*/
            internal int Unknown0x1DC;
            /* 0x1E0*/
            internal int Unknown0x1E0;
            /* 0x1E4*/

            [MarshalAs(UnmanagedType.LPStr)]
            public string Name;

            /* 0x1E8*/
            internal int Unknown0x1E8;
            /* 0x1EC*/
            internal int Unknown0x1EC;
            /* 0x1F0*/
            internal int Unknown0x1F0;
            /* 0x1F4*/
            internal int Unknown0x1F4;
            /* 0x1F8*/
            internal int Unknown0x1F8;
            /* 0x1FC*/
            internal int Unknown0x1FC;
            /* 0x200*/
            internal int Unknown0x200;
            /* 0x204*/
            internal int Unknown0x204;
            /* 0x208*/
            internal int Unknown0x208;
            /* 0x20C*/
            internal int Unknown0x20C;
            /* 0x210*/
            internal int Unknown0x210;
            /* 0x214*/
            internal int Unknown0x214;
            /* 0x218*/
            internal int Unknown0x218;
            /* 0x21C*/
            internal int Unknown0x21C;
            /* 0x220 */
        }

        #endregion
    }
}