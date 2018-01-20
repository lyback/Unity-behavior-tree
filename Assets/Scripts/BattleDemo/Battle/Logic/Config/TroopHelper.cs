namespace Battle.Logic
{
    public static class TroopHelper
    {
        public static uint GetTroopSpyDis(TroopType type)
        {
            switch (type)
            {
                case TroopType.NONE:
                    break;
                case TroopType.Infantry:
                    return 10;
                case TroopType.Cavalry:
                    return 20;
                case TroopType.Bowman:
                    return 5;
                case TroopType.Enchanter:
                    return 5;
                default:
                    break;
            }
            return 10;
        }
        public static uint GetTroopAtkDis(TroopType type)
        {
            switch (type)
            {
                case TroopType.NONE:
                    break;
                case TroopType.Infantry:
                    return 1;
                case TroopType.Cavalry:
                    return 2;
                case TroopType.Bowman:
                    return 5;
                case TroopType.Enchanter:
                    return 10;
                default:
                    break;
            }
            return 1;
        }
        public static uint GetTroopRange(TroopType type)
        {
            switch (type)
            {
                case TroopType.NONE:
                    break;
                case TroopType.Infantry:
                    return 2;
                case TroopType.Cavalry:
                    return 2;
                case TroopType.Bowman:
                    return 2;
                case TroopType.Enchanter:
                    return 2;
                default:
                    break;
            }
            return 2;
        }
        public static uint GetTroopAtkPrepareTime(TroopType type)
        {
            switch (type)
            {
                case TroopType.NONE:
                    break;
                case TroopType.Infantry:
                    return 2;
                case TroopType.Cavalry:
                    return 2;
                case TroopType.Bowman:
                    return 2;
                case TroopType.Enchanter:
                    return 2;
                default:
                    break;
            }
            return 2;
        }
        public static uint GetTroopAtkCDTime(TroopType type)
        {
            switch (type)
            {
                case TroopType.NONE:
                    break;
                case TroopType.Infantry:
                    return 4;
                case TroopType.Cavalry:
                    return 2;
                case TroopType.Bowman:
                    return 6;
                case TroopType.Enchanter:
                    return 8;
                default:
                    break;
            }
            return 2;
        }
        public static void GetTroopLRSpace(TroopType type, out uint lineSpace, out uint rowSpace)
        {
            switch (type)
            {
                case TroopType.NONE:
                    break;
                case TroopType.Infantry:
                    lineSpace = 10;
                    rowSpace = 10;
                    break;
                case TroopType.Cavalry:
                    lineSpace = 10;
                    rowSpace = 10;
                    break;
                case TroopType.Bowman:
                    lineSpace = 10;
                    rowSpace = 10;
                    break;
                case TroopType.Enchanter:
                    lineSpace = 10;
                    rowSpace = 10;
                    break;
                default:
                    break;
            }
            lineSpace = 10;
            rowSpace = 10;
            return;
        }
    }
}

