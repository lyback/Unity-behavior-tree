using System;

namespace Battle.Logic
{
    public static class MathHelper
    {
        public static int TroopDistanceV2(TroopData a1, TroopData a2)
        {
            return DistanceV2(a1.x, a1.y, a2.x, a2.y);
        }
        public static int DistanceV2(int x1, int y1, int x2, int y2)
        {
            return Math.Abs((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
    }
}

