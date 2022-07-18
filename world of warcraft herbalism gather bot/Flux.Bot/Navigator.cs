using System;
using System.Collections.Generic;

using Flux.Utilities;
using Flux.WoW;
using Flux.WoW.Objects;

using Pather.Graph;

using WowTriangles;

namespace Flux.Bot
{
    public static class Navigator
    {
        private static Point _currentPathHop;
        private static Queue<Point> _path;
        private static PathGenerator _pathGen;

        static Navigator()
        {
            PathPrecision = 2d;
            Destination = Point.Empty;
            FluxWoW.OnFrame += WalkPath;
        }

        public static Point Destination { get; set; }
        public static double PathPrecision { get; set; }

        public static void MoveTo(WoWObject obj)
        {
            MoveTo(obj.Position);
        }

        public static void MoveTo(Point pt)
        {
            // Do nothing if it's within the precision range.
            if (pt.Distance(Destination) <= PathPrecision)
            {
                return;
            }
            Destination = pt;
        }

        //private static bool GenPath(Point to)
        //{
        //    if (!to.IsValid)
        //        return false;

        //    try
        //    {
        //        if (_pathGen == null)
        //            _pathGen = new PathGenerator();

        //        _path = _pathGen.GetPath(WoW.Flux.Me.Position, to);
        //        return _path != null;
        //    }
        //    catch(Exception ex)
        //    {
        //        Logging.WriteException(ex);
        //    }
        //    return false;
        //}

        public static Queue<Point> GeneratePath(Point to)
        {
            if (!to.IsValid)
            {
                return null;
            }

            try
            {
                if (_pathGen == null)
                {
                    _pathGen = new PathGenerator();
                }

                return _pathGen.GetPath(FluxWoW.Me.Position, to);
            }
            catch (Exception ex)
            {
                Logging.WriteException(ex);
            }
            return null;
        }

        public static void WalkPath()
        {
            if (!Destination.IsValid)
                return;
            if (_path == null)
            {
                _path = GeneratePath(Destination);
                if (_path == null)
                {
                    Logging.WriteDebug("Path not found... :(");

                    Destination = Point.Empty;
                }
                else
                {
                    Logging.WriteDebug("PATH WAS FOUND!!!");
                }
            }
            else if (_path != null && _path.Count > 0)
            {
                if (!FluxWoW.Me.MovementInfo.IsMoving ||
                    (_currentPathHop.IsValid && _currentPathHop.Distance(FluxWoW.Me.Position) <= PathPrecision))
                {
                    _currentPathHop = _path.Dequeue();
                    Logging.WriteDebug("Next hop: " + _currentPathHop);
                    FluxWoW.Movement.ClickToMove(_currentPathHop);
                }
            }
            else if (_path != null && _path.Count == 0)
            {
                if (Destination.Distance(FluxWoW.Me.Position) <= PathPrecision)
                {
                    Stop();
                }
                else if (!FluxWoW.Me.MovementInfo.IsMoving)
                {
                    FluxWoW.Movement.ClickToMove(Destination);
                }
            }
        }

        public static void Stop()
        {
            Destination = Point.Empty;
            _currentPathHop = Point.Empty;
            _path = null;

            if (!FluxWoW.Me.MovementInfo.IsMoving)
            {
                return;
            }
            FluxWoW.Movement.MoveStop();
        }

        // ReSharper disable InconsistentNaming
        public static bool IntersectsPathXY(float fromX, float fromY, float toX, float toY)
            // ReSharper restore InconsistentNaming
        {
            return IntersectsPath(WoWMovement.FaceToCalc(FluxWoW.Me.Position, new Point(fromX, fromY, 0)),
                                  WoWMovement.FaceToCalc(FluxWoW.Me.Position, new Point(toX, toY, 0)), 45f);
        }

        public static bool IntersectsPath(float pathHeading, float toHeading)
        {
            return IntersectsPath(pathHeading, toHeading, 45f);
        }

        public static bool IntersectsPath(float pathHeading, float toHeading, float maxDegrees)
        {
            // No destination
            if (!Destination.IsValid)
            {
                return true;
            }

            float maxLeft = pathHeading + maxDegrees;
            float maxRight = pathHeading - maxDegrees;

            if (maxLeft > 360)
            {
                maxLeft -= 360;
            }
            if (maxRight < 0)
            {
                maxRight += 360;
            }

            if (maxLeft > maxRight && toHeading <= maxLeft && toHeading >= maxRight)
            {
                return true;
            }

            if ((maxLeft < maxRight) && (toHeading <= maxLeft && toHeading >= 0) ||
                (toHeading >= maxRight && toHeading <= 360))
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// This class does nothing but handle PPather interop. There's nothing entirely 'special'
    /// about this class, other than properly handling shit. :)
    /// </summary>
    internal class PathGenerator
    {
        private ChunkedTriangleCollection _ctc;
        private string _currContinent;
        private int _lastContinentId = int.MinValue;
        private Queue<Point> _lastPath;
        private MPQTriangleSupplier _mpq;
        private PathGraph _pg;

        public PathGenerator(int continent)
        {
            SetupPathGraph(continent);
        }

        public PathGenerator()
        {
            EnsureCurrentContinent();
        }

        private void SetupPathGraph(int continent)
        {
            _currContinent = FluxWoW.Db[ClientDb.Map].GetRow(continent).GetField<string>(1);
            _mpq = new MPQTriangleSupplier();
            _mpq.SetContinent(_currContinent);
            _ctc = new ChunkedTriangleCollection(512);
            _ctc.SetMaxCached(9);
            _ctc.AddSupplier(_mpq);
            _pg = new PathGraph(_currContinent, _ctc, null);
        }

        private void EnsureCurrentContinent()
        {
            int id = FluxWoW.ContinentId;
            if (_lastContinentId != id)
            {
                _lastContinentId = id;
                SetupPathGraph(id);
            }
        }

        public Queue<Point> GetPath(Point from, Point to)
        {
            try
            {
                if (_pg == null)
                {
                    return null;
                }

                // Grab the path with 5 units between WPs
                Path p = _pg.CreatePath(new Location(from.X, from.Y, from.Z), new Location(to.X, to.Y, to.Z), 5f);

                if (p == null)
                {
                    return null;
                }

                var locs = new List<Location>();
                for (int i = 0; i < p.Count(); i++)
                {
                    locs.Add(p.Get(i));
                }

                _lastPath = new Queue<Point>(TranslateFromLocations(locs));

                return _lastPath;
            }
            catch (Exception e)
            {
                Logging.WriteException(e);
                return null;
            }
        }

        private static List<Point> TranslateFromLocations(IEnumerable<Location> locs)
        {
            var ret = new List<Point>();
            foreach (Location loc in locs)
            {
                ret.Add(new Point(loc.X, loc.Y, loc.Z));
            }
            return ret;
        }
    }
}