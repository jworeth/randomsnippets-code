using System;
using System.Collections.Generic;

using Flux.WoW;
using Flux.WoW.Objects;

namespace Flux.Gather
{
    internal class NodeList
    {
        public static WoWGameObject Closest
        {
            get
            {
                var tmp = Search(new ObjectSearchParams {SortByDistance = ObjSearchSort.Ascending});
                return tmp.Count > 0 ? tmp[0] : new WoWGameObject(IntPtr.Zero);
            }
        }

        public static bool NodesNear
        {
            get
            {
                return GetNodes().Count != 0;
            }
        }

        public static List<WoWGameObject> GetNodes()
        {
            return Search(new ObjectSearchParams {DistanceMax = GatherSettings.NodeSearchRadius});
        }

        private static List<WoWGameObject> Search(ObjectSearchParams osp)
        {
            var gos = new ObjectList<WoWGameObject>();
            var filtered = gos.Search(osp);

            var ret = new List<WoWGameObject>();

            foreach (WoWGameObject o in filtered)
            {
                switch (GatherSettings.GatherType)
                {
                    case NodeType.Herb:
                        if (o.IsHerb)
                            ret.Add(o);
                        break;
                    case NodeType.Mine:
                        if (o.IsMine)
                            ret.Add(o);
                        break;
                    case NodeType.Both:
                        if (o.IsHerb || o.IsMine)
                            ret.Add(o);
                        break;
                }
            }

            return ret;
        }

        public enum NodeType
        {
            Herb,
            Mine,
            Both
        }
    }
}
