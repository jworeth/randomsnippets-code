using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flux.Gather
{
    static class GatherBrain
    {
        public static WoW.Objects.WoWGameObject CurrentGatherNode { get; set; }


        public static FluxPath CurrentPath { get; set; }

        public static bool IsRunning { get; set; }

        public static void Run()
        {
            IsRunning = true;

            CurrentPath.WaypointFollower.SetDestinationToNearest();
            CurrentPath.WaypointFollower.IsMoving = true;
        }

        public static void Stop()
        {
            IsRunning = false;

            CurrentPath.WaypointFollower.IsMoving = false;
        }
    }
}
