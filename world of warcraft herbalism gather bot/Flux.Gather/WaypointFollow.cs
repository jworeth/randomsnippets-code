using System.Collections.Generic;

using Flux.Utilities;
using Flux.WoW;

namespace Flux.Gather
{
    public class WaypointFollow
    {
        private readonly CircularQueue<Point> _waypoints;

/*
        public WaypointFollow(IEnumerable<Point> followList)
        {
            _waypoints = new CircularQueue<Point>(followList);
            FluxWoW.OnFrame += Mover;
        }
*/

        public WaypointFollow(CircularQueue<Point> followList)
        {
            _waypoints = followList;
            FluxWoW.OnFrame += Mover;
        }

        public bool IsMoving { get; set; }

        private void Mover()
        {
            // Sanity checks...
            if (!FluxWoW.IsInGame || !FluxWoW.Me.IsValid || !IsMoving)
            {
                return;
            }

            Point destination = _waypoints.Peek();

            if (destination.Distance(FluxWoW.Me.Position) < 3)
            {
                _waypoints.Dequeue();
                return;
            }
            FluxWoW.Movement.ClickToMove(destination);
        }

        public Point GetClosestToMe()
        {
            // Store my location.
            Point myLoc = FluxWoW.Me.Position;

            // Since we want the closest, we start with the highest possible value
            // to make sure we have some valid data!
            double closest = double.MaxValue;

            var ret = new Point();

            // Cycle through, testing distance, and set our return Point to the closest
            // one we find.
            foreach (Point p in _waypoints)
            {
                if (p.Distance(myLoc) < closest)
                {
                    closest = p.Distance(myLoc);
                    ret = p;
                }
            }
            return ret;
        }

        public void SetDestinationToNearest()
        {
            _waypoints.CycleTo(GetClosestToMe());
        }
    }
}