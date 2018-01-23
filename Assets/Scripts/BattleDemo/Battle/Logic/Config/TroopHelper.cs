namespace Battle.Logic
{
    public static class TroopHelper
    {
        public static uint GetTroopSpyDis(SoldierType type)
        {
            switch (type)
            {
                case SoldierType.NONE:
                    break;
                case SoldierType.Infantry:
                    return 10;
                case SoldierType.Cavalry:
                    return 20;
                case SoldierType.Bowman:
                    return 5;
                case SoldierType.Enchanter:
                    return 5;
                default:
                    break;
            }
            return 10;
        }
        public static uint GetTroopAtkDis(SoldierType type)
        {
            switch (type)
            {
                case SoldierType.NONE:
                    break;
                case SoldierType.Infantry:
                    return 1;
                case SoldierType.Cavalry:
                    return 2;
                case SoldierType.Bowman:
                    return 5;
                case SoldierType.Enchanter:
                    return 10;
                default:
                    break;
            }
            return 1;
        }
        public static uint GetTroopRange(SoldierType type)
        {
            switch (type)
            {
                case SoldierType.NONE:
                    break;
                case SoldierType.Infantry:
                    return 2;
                case SoldierType.Cavalry:
                    return 2;
                case SoldierType.Bowman:
                    return 2;
                case SoldierType.Enchanter:
                    return 2;
                default:
                    break;
            }
            return 2;
        }
        public static uint GetTroopAtkPrepareTime(SoldierType type)
        {
            switch (type)
            {
                case SoldierType.NONE:
                    break;
                case SoldierType.Infantry:
                    return 2;
                case SoldierType.Cavalry:
                    return 2;
                case SoldierType.Bowman:
                    return 2;
                case SoldierType.Enchanter:
                    return 2;
                default:
                    break;
            }
            return 2;
        }
        public static uint GetTroopAtkCDTime(SoldierType type)
        {
            switch (type)
            {
                case SoldierType.NONE:
                    break;
                case SoldierType.Infantry:
                    return 4;
                case SoldierType.Cavalry:
                    return 2;
                case SoldierType.Bowman:
                    return 6;
                case SoldierType.Enchanter:
                    return 8;
                default:
                    break;
            }
            return 2;
        }
        public static void GetTroopLRSpace(SoldierType type, out uint lineSpace, out uint rowSpace)
        {
            switch (type)
            {
                case SoldierType.NONE:
                    break;
                case SoldierType.Infantry:
                    lineSpace = 10;
                    rowSpace = 10;
                    break;
                case SoldierType.Cavalry:
                    lineSpace = 10;
                    rowSpace = 10;
                    break;
                case SoldierType.Bowman:
                    lineSpace = 10;
                    rowSpace = 10;
                    break;
                case SoldierType.Enchanter:
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

