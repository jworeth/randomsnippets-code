using System;
using System.Collections.Generic;
using System.Linq;

using Flux.Utilities;
using Flux.WoW.Objects;

namespace Flux.WoW
{
    public class ObjectList<T> where T : WoWObject
    {
        public ObjectList()
        {
        }

        public ObjectList(ObjectSearchParams search) : this()
        {
            SearchParams = search;
        }

        public ObjectSearchParams SearchParams { get; set; }

        public List<T> Search()
        {
            return Search(SearchParams);
        }

        public List<T> Search(int max)
        {
            return Search(SearchParams, max);
        }

        public List<T> Search(ObjectSearchParams search)
        {
            return Search(search, int.MaxValue);
        }

        public List<T> Search(ObjectSearchParams sp, int max)
        {
            WoWObject[] objLst = ObjMgr.ObjectList.ToArray();
            if (Compare.Equal(sp.NonFriendly, true))
            {
                sp.HighRelation = WoWUnitRelation.Neutral;
                sp.LowRelation = WoWUnitRelation.Hated;
            }
            var ret = new List<T>();
            var filtered = new List<WoWObject>();
            foreach (WoWObject o in objLst)
            {
                try
                {
                    if (!(o is T))
                    {
                        continue;
                    }
                    if (!Compare.Equal(sp.IncludeMe, o == FluxWoW.Me))
                    {
                        continue;
                    }
                    if (!Compare.Equal(sp.Name, o.Name))
                    {
                        continue;
                    }
                    if (!Compare.Equal(sp.ObjectType, o.Type))
                    {
                        continue;
                    }
                    if (!Compare.Equal(sp.Guid, o.Guid))
                    {
                        continue;
                    }

                    double dist = o.Distance;
                    if (!Compare.LessOrEqual(sp.DistanceMin, dist))
                    {
                        continue;
                    }
                    if (!Compare.GreaterOrEqual(sp.DistanceMax, dist))
                    {
                        continue;
                    }

                    if (!Compare.Equal(sp.LineOfSight, o.InLineOfSight))
                    {
                        continue;
                    }

                    if (o is WoWUnit)
                    {
                        WoWUnit u = o.ToUnit();
                        if (!Compare.Equal(sp.TargetingMe, u.CurrentTarget == FluxWoW.Me))
                            continue;

                        if (!Compare.Equal(sp.Attackable, u.Attackable))
                        {
                            continue;
                        }
                        if (!Compare.Equal(sp.Dead, u.Dead))
                        {
                            continue;
                        }

                        int lvl = u.Level;
                        if (!Compare.Equal(sp.LevelExact, lvl))
                        {
                            continue;
                        }
                        if (!Compare.LessOrEqual(sp.LevelMin, lvl))
                        {
                            continue;
                        }
                        if (!Compare.GreaterOrEqual(sp.LevelMax, lvl))
                        {
                            continue;
                        }

                        if (!Compare.Equal(sp.Flying, u.OnTaxi))
                        {
                            continue;
                        }
                        if (!Compare.Equal(sp.Aggro, u.Aggro))
                        {
                            continue;
                        }
                        if (!Compare.Equal(sp.NoCritters, u.CreatureType != WoWCreatureType.Critter))
                        {
                            continue;
                        }

                        if (!Compare.LessOrEqual(sp.LowRelation, u.Relation))
                        {
                            continue;
                        }
                        if (!Compare.GreaterOrEqual(sp.HighRelation, u.Relation))
                        {
                            continue;
                        }

                        if (!Compare.Equal(sp.Lootable, u.CanLoot))
                        {
                            continue;
                        }

                        if (!Compare.Equal(sp.Totem, u.IsTotem))
                        {
                            continue;
                        }

                        if (!Compare.Equal(sp.Tapped, u.Tapped))
                        {
                            continue;
                        }

                        if (!Compare.Equal(sp.TappedByMe, u.TappedByMe))
                        {
                            continue;
                        }
                    }
                    filtered.Add(o);
                }
                catch (Exception e)
                {
                    Logging.WriteException(e);
                }
            }

            if (sp.SortByDistance.HasValue)
            {
                if (sp.SortByDistance.Value == ObjSearchSort.Ascending)
                {
                    filtered.OrderBy(o => o.Distance);
                }
                else
                {
                    filtered.OrderByDescending(o => o.Distance);
                }
            }

            foreach (WoWObject o in filtered)
            {
                ret.Add(o as T);
            }

            return ret;
        }

        #region Nested type: Compare

        internal static class Compare
        {
            public static bool Equal<TK>(TK? searchParam, TK val) where TK : struct
            {
                // If 'searchParam' has no value, return true (assume user doesn't want this param used)
                // Otherwise, make sure the actual values are equal.
                if (!searchParam.HasValue)
                {
                    return true;
                }
                return searchParam.Value.Equals(val);
            }

            public static bool Equal(string searchParam, string val)
            {
                // Same as the other Equal method. Except we need to adjust for strings.
                if (searchParam == null)
                {
                    return true;
                }
                return val == searchParam;
            }

            public static bool LessOrEqual(int? param, int val)
            {
                if (!param.HasValue)
                {
                    return true;
                }
                return param.Value <= val;
            }

            public static bool LessOrEqual(uint? param, uint val)
            {
                if (!param.HasValue)
                {
                    return true;
                }
                return param.Value <= val;
            }

            public static bool LessOrEqual(double? param, double val)
            {
                if (!param.HasValue)
                {
                    return true;
                }
                return param.Value <= val;
            }

            public static bool LessOrEqual(WoWUnitRelation? param, WoWUnitRelation val)
            {
                if (!param.HasValue)
                {
                    return true;
                }
                return param.Value <= val;
            }

            public static bool GreaterOrEqual(int? param, int val)
            {
                if (!param.HasValue)
                {
                    return true;
                }
                return param.Value >= val;
            }

            public static bool GreaterOrEqual(uint? param, uint val)
            {
                if (!param.HasValue)
                {
                    return true;
                }
                return param.Value >= val;
            }

            public static bool GreaterOrEqual(WoWUnitRelation? param, WoWUnitRelation val)
            {
                if (!param.HasValue)
                {
                    return true;
                }
                return param.Value >= val;
            }

            public static bool GreaterOrEqual(double? param, double val)
            {
                if (!param.HasValue)
                {
                    return true;
                }
                return param.Value >= val;
            }
        }

        #endregion
    }

    public class ObjectSearchParams
    {
        //public string Name;
        public bool? Aggro;
        public bool? Attackable;
        public bool? Dead;
        public double? DistanceMax;
        public double? DistanceMin;
        public bool? Flying;
        public ulong? Guid;
        public WoWUnitRelation? HighRelation;
        public bool? IncludeMe;
        public int? LevelExact;
        public int? LevelMax;
        public int? LevelMin;
        public bool? LineOfSight;
        public bool? Lootable;
        public WoWUnitRelation? LowRelation;
        public string Name;
        public bool? NoCritters;
        public bool? NonFriendly;
        public WoWObjectType? ObjectType;

        public ObjSearchSort? SortByDistance;
        public ObjSearchSort? SortByLevel;

        public bool? Tapped;
        public bool? TappedByMe;
        public bool? Totem;
        public bool? TargetingMe;
    }

    public enum ObjSearchSort
    {
        Ascending,
        Descending
    }
}