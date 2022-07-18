using System;
using System.Collections.Generic;

using Flux.WoW.Patchables;

namespace Flux.WoW.Objects
{
    public partial class WoWUnit
    {
        public WoWBuff GetBuff(int index)
        {
            if (_unitAura == null)
            {
                //Logging.WriteDebug("Registering the GetUnitAura delegate");
                _unitAura = Utilities.RegisterDelegate<GetUnitAuraDelegate>((uint) GlobalOffsets.CGUnit_C__GetAura);
            }
            //Logging.Write("Buff index: " + index);
            IntPtr address = _unitAura(this, index);
            //Logging.WriteDebug("Offset: " + address.ToString("X"));
            return new WoWBuff(address);
        }

        public bool HasBuff(string buff)
        {
            return HasBuff(new WoWSpell(buff));
        }

        public bool HasBuff(WoWSpell buff)
        {
            if (buff == null || !buff.IsValid)
                return false;

            for (int i = 0; i < BuffCount; i++)
            {
                if (GetBuff(i).Name == buff.Name)
                    return true;
            }
            return false;
        }

        public bool Behind(WoWUnit obj)
        {
            if (obj.IsValid && obj.Distance < 10)
            {
                float f = Math.Abs(obj.Facing - WoWMovement.FaceToCalc(Position, obj.Position));

                if (f < (90 * Math.PI / 180))//convert to radians
                    return true;
            }

            return false;
        }

        #region Private helper methods

        private uint GetCurrentPower(WoWPowerType power)
        {
            switch (power)
            {
                case WoWPowerType.Health:
                    return GetStorageField<uint>(UnitFields.UNIT_FIELD_HEALTH);
                case WoWPowerType.Mana:
                    return GetStorageField<uint>(UnitFields.UNIT_FIELD_POWER1);
                case WoWPowerType.Rage:
                    return GetStorageField<uint>(UnitFields.UNIT_FIELD_POWER2);
                case WoWPowerType.Energy:
                    return GetStorageField<uint>(UnitFields.UNIT_FIELD_POWER3);
                case WoWPowerType.Focus:
                    return GetStorageField<uint>(UnitFields.UNIT_FIELD_POWER4);
                case WoWPowerType.Happiness:
                    return GetStorageField<uint>(UnitFields.UNIT_FIELD_POWER5);
                    //case WoWPowerType.Runes: // TODO: What the hell is this one?
                    //    return GetStorageField<uint>(UnitFields.UNIT_FIELD_POWER6);
                case WoWPowerType.RunicPower:
                    return GetStorageField<uint>(UnitFields.UNIT_FIELD_POWER7);
                default:
                    throw new ArgumentOutOfRangeException("power");
            }
        }

        private uint GetMaxPower(WoWPowerType power)
        {
            switch (power)
            {
                case WoWPowerType.Health:
                    return GetStorageField<uint>(UnitFields.UNIT_FIELD_MAXHEALTH);
                case WoWPowerType.Mana:
                    return GetStorageField<uint>(UnitFields.UNIT_FIELD_MAXPOWER1);
                case WoWPowerType.Rage:
                    return GetStorageField<uint>(UnitFields.UNIT_FIELD_MAXPOWER2);
                case WoWPowerType.Energy:
                    return GetStorageField<uint>(UnitFields.UNIT_FIELD_MAXPOWER3);
                case WoWPowerType.Focus:
                    return GetStorageField<uint>(UnitFields.UNIT_FIELD_MAXPOWER4);
                case WoWPowerType.Happiness:
                    return GetStorageField<uint>(UnitFields.UNIT_FIELD_MAXPOWER5);
                    //case WoWPowerType.Runes: // TODO: What the hell is this one?
                    //    return GetStorageField<uint>(UnitFields.UNIT_FIELD_MAXPOWER6);
                case WoWPowerType.RunicPower:
                    return GetStorageField<uint>(UnitFields.UNIT_FIELD_MAXPOWER7);
                default:
                    throw new ArgumentOutOfRangeException("power");
            }
        }

        private double GetPowerPercent(WoWPowerType p)
        {
            uint a = GetCurrentPower(p);
            uint b = GetMaxPower(p);
            return Math.Round((a * 100) / (double) b);
        }

        private static bool Flag(uint flag, uint val)
        {
            return (flag & val) != 0;
        }

        private bool Flag(UnitFlags flag)
        {
            return Flags[(int) flag];
        }

        private bool Flag(UnitNPCFlags flag)
        {
            return FlagsNpc[(int)flag];
        }

        private T GetStorageField<T>(UnitFields field) where T : struct
        {
            return GetStorageField<T>((uint) field);
        }

        private int GetReaction(WoWUnit other)
        {
            if (_unitReaction == null)
            {
                _unitReaction = Utilities.RegisterDelegate<GetUnitReactionDelegate>((uint) GlobalOffsets.CGUnit_C__UnitReaction);
            }
            return _unitReaction(this, other);
        }

        private int GetUnitType()
        {
            if (_unitType == null)
            {
                _unitType = Utilities.RegisterDelegate<UnitTypeDelegate>((uint) GlobalOffsets.GetUnitType);
            }
            return _unitType(this);
        }

        #endregion
    }
}