namespace Flux.Gather
{
    internal class GatherSettings
    {
        static GatherSettings()
        {
            NodeSearchRadius = 100;
            GatherType = NodeList.NodeType.Herb;
        }

        public static NodeList.NodeType GatherType { get; set; }

        public static double NodeSearchRadius { get; set; }
    }
}