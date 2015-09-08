using System.Collections.Generic;

namespace ImaginationServer.Common
{
    public class ZonePositions
    {
        public static readonly float[] VentureExplorer = {-627.1862f, 613.3262f, -47.22317f};

        public static Dictionary<ZoneId, float[]> Positions { get; } = new Dictionary<ZoneId, float[]>
        { {ZoneId.VentureExplorer, VentureExplorer} };
    }
}