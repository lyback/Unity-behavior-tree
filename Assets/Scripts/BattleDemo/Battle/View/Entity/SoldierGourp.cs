
using Battle.Logic;
using System.Collections.Generic;

public class SoldierGourp : TroopGroupEntity
{
    private int m_BaseCount;
    private SoldierObject[,] soldierArray;
    private List<int> soldierIndexList;

    private const uint m_LineInterval = 10;
    private const uint m_RowInterval = 10;

    private uint m_Scale = 1;

    /// <summary>
    /// 初始化士兵
    /// </summary>
    private void InitAllSoldiers(int count, int nState, float deg)
    {
        m_BaseCount = count;
        if (soldierArray == null)
        {
            soldierArray = new SoldierObject[mData.line, mData.row];
        }
        if (soldierIndexList == null)
        {
            soldierIndexList = new List<int>();
        }

        for (int i = 0; i < soldierArray.GetLength(0); i++)
        {
            for (int j = 0; j < soldierArray.GetLength(1); j++)
            {
                if (soldierArray[i, j] != null) continue;
                var soldier = MainBattleManager.Instance.CreateSoldierObject(transform);
                SoldierData soldierData = new SoldierData(mData, i, j);
                soldier.Init(soldierData);
                soldierArray[i, j] = soldier;
                soldierIndexList.Add(GetIndexValue(i, j));
            }
        }
    }
    private int GetIndexValue(int i, int j)
    {
        int r = i << 4;
        r += j;
        return r;
    }
}
