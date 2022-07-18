using System;
using System.Collections.Generic;
using System.Linq;

using Flux.Utilities;
using Flux.WoW;
using Flux.WoW.Objects;

namespace Flux.Bot
{
    public class TargetManager
    {
        public static readonly List<string> GuiList = new List<string>();
        private static int _maxLevel = 80;
        private static int _minLevel = 1;

        static TargetManager()
        {
            MaxTargets = 7;
            CollectionRange = 100;
            TargetList = new List<WoWUnit>();
            FluxWoW.OnFrame += Pulse;
        }

        /// <summary>
        /// The current list of viable targets.
        /// </summary>
        public static List<WoWUnit> TargetList { get; private set; }

        /// <summary>
        /// The 'limit' on the number of possible targets at any one time. [Default: 7]
        /// </summary>
        public static int MaxTargets { get; set; }

        /// <summary>
        /// The distance from ourselves, where targets will be collected. [Default: 200]
        /// </summary>
        public static int CollectionRange { get; set; }

        public static bool IncludeElites { get; set; }

        public static bool IgnoreLevel { get; set; }

        internal static void Pulse()
        {
            //Logging.WriteDebug("Pulse in target");
            SetMinMaxLevels();

            // TODO: We need to add contextual based target searches.
            // Possibly make this API a little bit easier to use? (ISXWoW string style maybe?)
            ObjectSearchParams p;
            if (FluxWoW.Me.InCombat)
            {
                p = new ObjectSearchParams
                        {
                            Dead = false,
                            //Aggro = true,
                            LineOfSight = true,
                            NoCritters = true,
                            NonFriendly = true,
                            TargetingMe = true,
                            Flying = false,
                            Attackable = true,
                            DistanceMax = 50,
                            //SortByDistance = ObjSearchSort.Ascending
                        };
                //p.ZFilter=7; // Make sure the +/- Z distance is < this
            }
            else
            {
                p = new ObjectSearchParams
                        {
                            Dead = false,
                            NonFriendly = true,
                            NoCritters = true,
                            Flying = false,
                            Attackable = true,
                            DistanceMax = CollectionRange,
                            Tapped = false
                        };
                if (!IgnoreLevel)
                {
                    p.LevelMin = _minLevel;
                    p.LevelMax = _maxLevel;
                }
            }

            var units = new ObjectList<WoWUnit>(p);
            try
            {
                // Find *everything*. We'll limit it after running our priority filter.
                List<WoWUnit> lst = units.Search();

                if (lst.Count == 0)
                {
                    // No list to use.
                    TargetList.Clear();
                    GuiList.Clear();
                    return;
                }

                IEnumerable<Priority> weights = CalculateWeights(lst);

                GuiList.Clear();
                TargetList.Clear();
                int counter = 0;

                foreach (Priority weight in weights)
                {
                    if (counter++ == MaxTargets)
                        break;

                    TargetList.Add(weight.Unit);
                    GuiList.Add(string.Format("{0} - {1}", weight.Unit.Name, weight.Value));
                }
            }
            catch (Exception e)
            {
                Logging.WriteException(e);
            }
        }

        internal static void SetMinMaxLevels()
        {
            int l = FluxWoW.Me.Level;
            _maxLevel = l + 2;

            if (l <= 5)
            {
                _minLevel = 1;
            }
            else if (l < 40)
            {
                _minLevel = (l - 5 - (l / 10)) + 2;
            }
            else if (l < 60)
            {
                _minLevel = (l - 1 - (l / 5)) + 1;
            }
            else
            {
                _minLevel = l - 9;
            }
        }

        /// <summary>
        /// Calculates the target listing weights. This should now do better than what we had before!
        /// It now takes into account many more things... properly!
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        private static IEnumerable<Priority> CalculateWeights(List<WoWUnit> units)
        {
            var ret = new List<Priority>();
            WoWActivePlayer me = FluxWoW.Me;

            foreach (WoWUnit u in units)
            {
                var p = new Priority {Unit = u};
                double w = 200;

                // Further mobs have lower weight..
                w -= u.Distance;

                if (me.InCombat)
                {
                    // Try not to switch targets like crazy...
                    if (u==me.CurrentTarget)
                    {
                        w += 50;
                    }

                    // Favor mobs with mana
                    w += u.ManaPercent;
                    w -= u.HealthPercent;
                }
                else
                {
                    if (u.Relation <= WoWUnitRelation.Neutral && u.Distance < 50)
                    {
                        // Favor hostile mobs over neutral mobs...
                        w += (50 - u.Distance) * Math.Abs(4 - (uint) u.Relation);
                    }

                    if (!IncludeElites)
                    {
                        if (u.Elite)
                        {
                            // Ignore mob completely...
                            if (u.Distance > 20)
                                continue;

                            w -= 1000;
                        }
                    }

                    if (u.InLineOfSight)
                    {
                        w += 100;
                    }
                    else
                    {
                        w -= 100;
                    }
                }


                p.Value = w;
                ret.Add(p);
            }

            return ret.OrderByDescending(p => p.Value);
        }

        public static bool TargetingMeOrPet(WoWUnit u)
        {
            if (!u.IsValid || !FluxWoW.Me.IsValid)
            {
                return false;
            }
            if (u.CurrentTarget == FluxWoW.Me)
            {
                return true;
            }
            if (!FluxWoW.Me.Pet.IsValid)
            {
                return false;
            }
            if (u.CurrentTarget == FluxWoW.Me.Pet)
            {
                return true;
            }

            return false;
        }

        public static bool AttackingMeOrPet(WoWUnit u)
        {
            return TargetingMeOrPet(u) && u.InCombat && (u.IsAutoAttacking || u.Casting);
        }

        #region Nested type: Priority

        private class Priority
        {
            public WoWUnit Unit;
            public double Value;
        }

        #endregion
    }
}