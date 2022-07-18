using System;
using System.Collections.Generic;
using System.Xml.Linq;

using Flux.Utilities;
using Flux.WoW;

namespace Flux.Gather
{
    public class PathRecorder
    {
        private PathRecorder()
        {
            Waypoints = new List<Point>();
            WaypointDistance = 5;
            FluxWoW.OnFrame += Record;
        }

        private FluxPath _parentPath;

        public PathRecorder(FluxPath path) : this()
        {
            _parentPath = path;
        }

        public List<Point> Waypoints { get; set; }
        public bool IsRecording { get; set; }
        public double WaypointDistance { get; set; }

        private void Record()
        {
            if (!IsRecording)
            {
                return;
            }

            // Do nothing
            if (!FluxWoW.IsInGame || !FluxWoW.Me.IsValid)
            {
                return;
            }

            // Make it only 1 read, instead of 4. (Better performance)
            Point myPos = FluxWoW.Me.Position;

            if (Waypoints.Count == 0)
            {
                // Just start off with our current position.
                Logging.WriteDebug("Adding waypoint: " + myPos);
                Waypoints.Add(myPos);
                return;
            }

            if (myPos.Distance(Waypoints[Waypoints.Count - 1]) >= WaypointDistance)
            {
                Logging.WriteDebug("Adding waypoint: " + myPos);
                Waypoints.Add(myPos);
            }
        }

        public void ClearPath()
        {
            Waypoints.Clear();
        }

        public void Save(string filePath)
        {
            var xe = new XElement("Flux.Gather.Waypoints");
            foreach (Point p in Waypoints)
            {
                xe.Add(new XElement("Location", new XElement("X", p.X), new XElement("Y", p.Y), new XElement("Z", p.Z)));
            }

            xe.Save(filePath);
        }
    }
}