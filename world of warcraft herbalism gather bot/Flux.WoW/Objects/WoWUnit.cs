using System;
using System.Runtime.InteropServices;

using Flux.Utilities;
using Flux.WoW.Native;
using Flux.WoW.Patchables;

namespace Flux.WoW.Objects
{
    public partial class WoWUnit : WoWObject
    {
        public WoWUnit(IntPtr objectPtr) : base(objectPtr)
        {
        }

        public WoWUnit(ulong guid) : base(guid)
        {
        }

        /// <summary>
        /// The bounding box height of the unit. Note: Setting this value is considered a HACK.
        /// </summary>
        public float Height { get { return Reader.Read<float>(ObjPtr.ToUInt32() + (uint) UnitBaseFields.BoundingBoxHeight); } }

        /// <summary>
        /// The bounding box width of the unit. Note: Setting this value is considered a HACK.
        /// </summary>
        public float Width { get { return Reader.Read<float>(ObjPtr.ToUInt32() + (uint) UnitBaseFields.BoundingBoxWidth); }}

        public bool IsAutoAttacking { get {
            return Reader.Read<byte>(ObjPtr.ToUInt32() + (uint) UnitBaseFields.IsAutoAttacking) != 0 ||
                   Reader.Read<byte>(ObjPtr.ToUInt32() + (uint) UnitBaseFields.IsAutoAttacking2) != 0; } }
        public MovementInfo MovementInfo { get { return new MovementInfo(new IntPtr(ObjPtr.ToInt64() + (uint) UnitBaseFields.MovementInfoBase)); } }
        public bool Moving { get { return MovementInfo.IsMoving; } }
        public WoWUnit CharmedBy { get { return new WoWUnit(GetStorageField<ulong>(UnitFields.UNIT_FIELD_CHARMEDBY)); } }
        public WoWUnit CurrentTarget { get { return new WoWUnit(CurrentTargetGuid); } }
        public ulong Charmed { get { return GetStorageField<ulong>(UnitFields.UNIT_FIELD_CHARM); } }
        public ulong Critter { get { return GetStorageField<ulong>(UnitFields.UNIT_FIELD_CRITTER); } }
        public WoWUnit SummonedBy { get { return new WoWUnit(ObjMgr.GetObjectByGuid(GetStorageField<ulong>(UnitFields.UNIT_FIELD_SUMMONEDBY))); } }
        public bool IsPet { get { return SummonedBy.IsValid; } }
        public ulong ChannelObject { get { return GetStorageField<ulong>(UnitFields.UNIT_FIELD_CHANNEL_OBJECT); } }
        public int ChannelSpell { get { return GetStorageField<int>(UnitFields.UNIT_CHANNEL_SPELL); } }
        public int CastingSpellId { get { return Marshal.ReadInt32(this, (int) UnitBaseFields.SpellTimeStart); } }
        public uint CurrentHealth { get { return GetCurrentPower(WoWPowerType.Health); } }
        public uint CurrentMana { get { return GetCurrentPower(WoWPowerType.Mana); } }
        public uint CurrentRage { get { return GetCurrentPower(WoWPowerType.Rage); } }
        public uint CurrentEnergy { get { return GetCurrentPower(WoWPowerType.Energy); } }
        public uint CurrentFocus { get { return GetCurrentPower(WoWPowerType.Focus); } }
        public uint CurrentHappiness { get { return GetCurrentPower(WoWPowerType.Happiness); } }
        public uint CurrentRunicPower { get { return GetCurrentPower(WoWPowerType.RunicPower); } }
        public uint MaxHealth { get { return GetMaxPower(WoWPowerType.Health); } }
        public uint MaxMana { get { return GetMaxPower(WoWPowerType.Mana); } }
        public uint MaxRage { get { return GetMaxPower(WoWPowerType.Rage); } }
        public uint MaxEnergy { get { return GetMaxPower(WoWPowerType.Energy); } }
        public uint MaxFocus { get { return GetMaxPower(WoWPowerType.Focus); } }
        public uint MaxHappiness { get { return GetMaxPower(WoWPowerType.Happiness); } }
        public uint MaxRunicPower { get { return GetMaxPower(WoWPowerType.RunicPower); } }
        public int Level { get { return GetStorageField<int>(UnitFields.UNIT_FIELD_LEVEL); } }
        public bool Alive { get { return !Dead; } }
        public double HealthPercent { get { return GetPowerPercent(WoWPowerType.Health); } }
        public double ManaPercent { get { return GetPowerPercent(WoWPowerType.Mana); } }
        public long Faction { get { return GetStorageField<long>(UnitFields.UNIT_FIELD_FACTIONTEMPLATE); } }
        public ulong CurrentTargetGuid { get { return GetStorageField<ulong>(UnitFields.UNIT_FIELD_TARGET); } }
        public bool IsTargetingMe { get { return CurrentTargetGuid == ObjMgr.Me.Guid; } }
        public float BoundingRadius { get { return GetStorageField<int>(UnitFields.UNIT_FIELD_BOUNDINGRADIUS); } }
        public float CombatReach { get { return GetStorageField<int>(UnitFields.UNIT_FIELD_COMBATREACH); } }
        public int MountDisplayId { get { return GetStorageField<int>(UnitFields.UNIT_FIELD_MOUNTDISPLAYID); } }
        public WoWUnitRelation Relation { get { return (WoWUnitRelation) GetReaction(ObjMgr.Me); } }
        public WoWCreatureType CreatureType { get { return (WoWCreatureType) GetUnitType(); } }

        public uint CurrentPower { get { return GetCurrentPower(PowerType); } }
        public uint MaxPower { get { return GetCurrentPower(PowerType); } }
        public double PowerPercent { get { return GetPowerPercent(PowerType); } }

        public WoWUnit Pet
        {
            get
            {
                // Hunter pets
                var guid = GetStorageField<ulong>(UnitFields.UNIT_FIELD_CHARM);
                // Everything else. (Locks/mages/etc)
                if (guid == 0)
                {
                    guid = GetStorageField<ulong>(UnitFields.UNIT_FIELD_SUMMON);
                }
                return new WoWUnit(ObjMgr.GetObjectByGuid(guid));
            }
        }

        public int CreatureTypeInt { get { return GetUnitType(); } }
        public bool Casting { get { return CastingSpellId != 0 || ChannelObject != 0 || ChannelSpell != 0; } }
        public int SpellInCastId
        {
            get
            {
                int spellId = CastingSpellId;
                if (spellId == 0)
                {
                    spellId = ChannelSpell;
                }
                return spellId;
            }
        }

        public bool BehindTarget { get { return Behind(CurrentTarget); } }

        public WoWSpell SpellInCast
        {
            get
            {
                return new WoWSpell(SpellInCastId);
            }
        }

        public WoWUnit SpellTarget { get { return new WoWUnit(Marshal.ReadIntPtr(this, (int) UnitBaseFields.SpellTarget).ToUInt64()); } }


        public int DisplayId
        {
            get
            {
                return GetStorageField<int>(UnitFields.UNIT_FIELD_DISPLAYID);
            }
            set
            {
                SetStorageField((uint)UnitFields.UNIT_FIELD_DISPLAYID, value);
                if (_updateDisplayInfo == null)
                {
                    _updateDisplayInfo = Utilities.RegisterDelegate<UpdateDisplayInfoDelegate>(0x006B7720);
                }
                _updateDisplayInfo(this, 1, 1);
            }
        }

        public int BuffCount
        {
            get
            {
                var tmp = Reader.Read<int>(ObjPtr.ToUInt32() + (uint) UnitBaseFields.AuraCount1);
                return tmp == -1 ? Reader.Read<int>(ObjPtr.ToUInt32() + (uint) UnitBaseFields.AuraCount2) : tmp;
            }
        }
    }
}