using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Flux.Utilities;
using Flux.WoW;

namespace Flux.Gather
{
    public class FluxPath
    {
        private readonly List<Point> _locationList;
        private PathRecorder _recorder;
        private WaypointFollow _wp;

        public FluxPath()
        {
            _locationList = new List<Point>();
        }

        public FluxPath(string filePath)
        {
            _locationList = new List<Point>();

            XElement fluxPathDoc = XElement.Load(filePath);
            IEnumerable<Point> nodeList = from l in fluxPathDoc.Descendants("Location")
                                          let x = l.Element("X").Value
                                          let y = l.Element("Y").Value
                                          let z = l.Element("Z").Value
                                          select new Point(float.Parse(x), float.Parse(y), float.Parse(z));

            _locationList.AddRange(nodeList);
        }

        public List<Point> Locations { get { return _locationList; } }

        public CircularQueue<Point> QueuedLocations { get { return new CircularQueue<Point>(Locations); } }

        public PathRecorder Recorder
        {
            get
            {
                if (_recorder == null)
                {
                    _recorder = new PathRecorder(this);
                }
                return _recorder;
            }
        }

        public WaypointFollow WaypointFollower
        {
            get
            {
                if (_wp == null)
                {
                    _wp = new WaypointFollow(QueuedLocations);
                }
                return _wp;
            }
        }


    }
}