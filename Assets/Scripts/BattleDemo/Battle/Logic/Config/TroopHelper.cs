//////////////////////////////////////////////////////////////////////////////////////
// The MIT License(MIT)
// Copyright(c) 2018 lycoder

// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

//////////////////////////////////////////////////////////////////////////////////////

namespace Battle.Logic
{
    public static class TroopHelper
    {
        public static uint scale = 10;
        public static uint GetTroopSpyDis(SoldierType type)
        {
            uint dis = 20;
            switch (type)
            {
                case SoldierType.NONE:
                    break;
                case SoldierType.Infantry:
                    dis = 20;
                    break;
                case SoldierType.Cavalry:
                    dis = 20;
                    break;
                case SoldierType.Bowman:
                    dis = 20;
                    break;
                case SoldierType.Enchanter:
                    dis = 20;
                    break;
                default:
                    break;
            }
            dis = dis * scale;
            dis = dis * dis;
            return dis;
        }
        public static uint GetTroopAtkDis(SoldierType type)
        {
            uint dis = 1;
            switch (type)
            {
                case SoldierType.NONE:
                    break;
                case SoldierType.Infantry:
                    dis = 1;
                    break;
                case SoldierType.Cavalry:
                    dis = 2;
                    break;
                case SoldierType.Bowman:
                    dis = 5;
                    break;
                case SoldierType.Enchanter:
                    dis = 10;
                    break;
                default:
                    break;
            }
            dis = dis * scale;
            dis = dis * dis;
            return dis;
        }
        public static uint GetTroopRange(SoldierType type)
        {
            uint dis = 2;
            switch (type)
            {
                case SoldierType.NONE:
                    break;
                case SoldierType.Infantry:
                    dis = 2;
                    break;
                case SoldierType.Cavalry:
                    dis = 2;
                    break;
                case SoldierType.Bowman:
                    dis = 2;
                    break;
                case SoldierType.Enchanter:
                    dis = 2;
                    break;
                default:
                    break;
            }
            dis = dis * scale;
            dis = dis * dis;
            return dis;
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
                    lineSpace = 1;
                    rowSpace = 1;
                    break;
                case SoldierType.Cavalry:
                    lineSpace = 1;
                    rowSpace = 1;
                    break;
                case SoldierType.Bowman:
                    lineSpace = 1;
                    rowSpace = 1;
                    break;
                case SoldierType.Enchanter:
                    lineSpace = 1;
                    rowSpace = 1;
                    break;
                default:
                    break;
            }
            lineSpace = 1;
            rowSpace = 1;
            return;
        }
    }
}

