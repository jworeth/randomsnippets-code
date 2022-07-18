using System.Runtime.InteropServices;

using Flux.Utilities;
using Flux.WoW.Patchables;

namespace Flux.WoW
{
    public class WoWSpell
    {
        private static CastSpellByIdDelegate _castSpellById;
        private static GetSpellIdByNameDelegate _getSpellIdByName;
        private readonly int _id;
        private readonly WoWDB.DbTable.Row _row;

        public WoWSpell(string name) : this(GetSpellIdByName(name))
        {
        }

        public WoWSpell(int id)
        {
            _id = id;
            _row = FluxWoW.Db[ClientDb.Spell].GetLocalizedRow(id);
        }

        public WoWSpell(uint id) : this((int) id)
        {
        }

        public bool IsValid { get { return _id != 0 || _row == null; } }

        public uint BaseLevel { get { return _row.GetField<uint>(38); } }
        public uint Level { get { return _row.GetField<uint>(39); } }

        private uint RangeIndex { get { return _row.GetField<uint>(46); } }

        public uint ManaCostPercent { get { return _row.GetField<uint>(146); } }

        public int ID { get { return _id; } }
        public uint Category { get { return _row.GetField<uint>(1); } }
        public WoWDispelType DispelType { get { return (WoWDispelType) _row.GetField<uint>(2); } }
        public WoWSpellMechanic Mechanic { get { return (WoWSpellMechanic) _row.GetField<int>(3); } }
        public int MaxTargets { get { return _row.GetField<int>(13); } }
        public WoWCreatureType TargetType { get { return (WoWCreatureType) _row.GetField<int>(15); } }

        public WoWPowerType PowerType { get { return (WoWPowerType) _row.GetField<int>(41); } }

        public int ManaCost { get { return _row.GetField<int>(39); /*Lua.GetReturnVal<int>("GetSpellInfo(" + ID + ")", 3);*/ } }
        public int MaxStackCount { get { return _row.GetField<int>(46); } }
        public string Name { get { return _row != null ? _row.GetField<string>(142) : Lua.GetReturnVal<string>("GetSpellInfo(" + ID + ")", 0); } }
        public string Rank { get { return _row != null ? _row.GetField<string>(143) : Lua.GetReturnVal<string>("GetSpellInfo(" + ID + ")", 1); } }
        public int CastTime { get { return Lua.GetReturnVal<int>("GetSpellInfo(" + ID + ")", 6); } }
        public int Cooldown { get { return Lua.GetReturnVal<int>("GetSpellCooldown(\"" + Name + "\")", 1); } }

        public float MinFriendlyRange { get { return FluxWoW.Db[ClientDb.SpellRange].GetRow((int)RangeIndex).GetField<float>(2); } }
        public float MinHostileRange { get { return FluxWoW.Db[ClientDb.SpellRange].GetRow((int)RangeIndex).GetField<float>(1); } }
        public float MaxFriendlyRange { get { return FluxWoW.Db[ClientDb.SpellRange].GetRow((int)RangeIndex).GetField<float>(4); } }
        public float MaxHostileRange { get { return FluxWoW.Db[ClientDb.SpellRange].GetRow((int)RangeIndex).GetField<float>(3); } }
        public string RangeDescription { get { return FluxWoW.Db[ClientDb.SpellRange].GetRow((int)RangeIndex).GetField<string>(6); } }
        public float MaxRange { get { return System.Math.Max(MaxFriendlyRange, MaxHostileRange); } }
        public float MinRange { get { return System.Math.Min(MinFriendlyRange, MinHostileRange); } }
        public bool HasRange { get { return MinRange != 0 && MaxRange != 0; } }

        public bool CanCast { get { return Lua.GetReturnVal<bool>("IsUsableSpell(\"" + Name + "\")", 0); } }

        public string Tooltip { get { return _row != null ? _row.GetField<string>(144) : null; } }

        public override string ToString()
        {
            return
                string.Format(
                    "{0} ({1}), Range: {2}-{3}, CastTime: {4}, Cost: {5} ({6}%), Mechanic: {7}, Dispel: {8}, TargetType: {9}, Power: {10}",
                    Name, Rank, MinRange, MaxRange, CastTime, ManaCost, ManaCostPercent, Mechanic, DispelType, TargetType, (int)PowerType);
        }

        public void DumpSpellDbc<T>(T lookFor)
        {
            if (_row != null)
            {
                for (uint i = 0; i < 250; i++)
                {
                    try
                    {
                        T field = _row.GetField<T>(i);
                        if (field.Equals(lookFor))
                        {
                            Logging.WriteDebug(i + "->" + field);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public int Cast()
        {
            if (_castSpellById == null)
            {
                _castSpellById = Utilities.RegisterDelegate<CastSpellByIdDelegate>((uint) GlobalOffsets.Spell_C_CastSpell);
            }
            return _castSpellById(ID, 0, 0, 0);
        }

        private static int GetSpellIdByName(string name)
        {
            if (_getSpellIdByName == null)
            {
                _getSpellIdByName =
                    Utilities.RegisterDelegate<GetSpellIdByNameDelegate>((uint) GlobalOffsets.GetSpellIdByName);
            }
            int id = 0;
            return _getSpellIdByName(name, ref id);
        }

        #region Nested type: CastSpellByIdDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CastSpellByIdDelegate(int id, int unk, int unk2, int unk3);

        #endregion

        #region Nested type: GetSpellCooldownDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetSpellCooldownDelegate(int spellId, int passZero, int unk1, int unk2, int unk3);

        #endregion

        #region Nested type: GetSpellIdByNameDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetSpellIdByNameDelegate(string name, ref int id);

        #endregion
    }

    public enum WoWSpellMechanic : uint
    {
        None = 0,
        Charmed = 1,
        Disoriented = 2,
        Disarmed = 3,
        Distracted = 4,
        Fleeing = 5,
        Gripped = 6,
        Rooted = 7,
        Slowed = 8,
        Silenced = 9,
        Asleep = 10,
        Snared = 11,
        Stunned = 12,
        Frozen = 13,
        Incapacitated = 14,
        Bleeding = 15,
        Healing = 16,
        Polymorphed = 17,
        Banished = 18,
        Shielded = 19,
        Shackled = 20,
        Mounted = 21,
        Infected = 22,
        Turned = 23,
        Horrified = 24,
        Invulnerable = 25,
        Interrupted = 26,
        Dazed = 27,
        Discovery = 28,
        Invulnerable2 = 29,
        Sapped = 30,
        Enraged = 31,
        End
    }
}