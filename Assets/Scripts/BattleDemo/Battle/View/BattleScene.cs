using Battle.Logic;
using System.Collections.Generic;

public class BattleScene
{
    private BattleLogicManager m_BattleLogicMgr;

    private Dictionary<uint, TroopData> m_AllTroopDic { get { return m_BattleLogicMgr.m_BattleData.mAllTroopDic; } }
    private List<TroopData> m_AtkTroopList { get { return m_BattleLogicMgr.m_BattleData.mAtcTroopList; } }
    private List<TroopData> m_DefTroopList { get { return m_BattleLogicMgr.m_BattleData.mDefTroopList; } }

    private Dictionary<uint, SoldierGroup> m_AtkSoldierGroup = new Dictionary<uint, SoldierGroup>();
    private Dictionary<uint, SoldierGroup> m_DefSoldierGroup = new Dictionary<uint, SoldierGroup>();

    public BattleScene(BattleLogicManager battleLogicMgr)
    {
        m_BattleLogicMgr = battleLogicMgr;
    }

    public void Update()
    {
        RefreshTroop();
    }

    private void RefreshTroop()
    {
        for (int i = 0; i < m_AtkTroopList.Count; i++)
        {
            TroopData _atkTroop = m_AtkTroopList[i];
            float x = _atkTroop.x;
            float y = _atkTroop.y;
            CorrectPos(ref x, ref y);
            float dir_x = _atkTroop.dir_x;
            float dir_y = _atkTroop.dir_y;
            CorrectPos(ref dir_x, ref dir_y);
            if (m_AtkSoldierGroup.ContainsKey(_atkTroop.key))
            {
                m_AtkSoldierGroup[_atkTroop.key].Doing(x, y, dir_x, dir_y, _atkTroop);
            }
            else
            {
                SoldierGroup soldierGroup = MainBattleManager.Instance.CreateSoldierGroup(x, y, dir_x, dir_y, _atkTroop);
                m_AtkSoldierGroup[_atkTroop.key] = soldierGroup;
            }
            _atkTroop.addTroop = false;
        }
        for (int i = 0; i < m_DefTroopList.Count; i++)
        {
            TroopData _defTroop = m_DefTroopList[i];
            float x = _defTroop.x;
            float y = _defTroop.y;
            CorrectPos(ref x, ref y);
            float dir_x = _defTroop.dir_x;
            float dir_y = _defTroop.dir_y;
            CorrectPos(ref dir_x, ref dir_y);
            if (m_DefSoldierGroup.ContainsKey(_defTroop.key))
            {
                m_DefSoldierGroup[_defTroop.key].Doing(x, y, dir_x, dir_y, _defTroop);
            }
            else
            {
                SoldierGroup soldierGroup = MainBattleManager.Instance.CreateSoldierGroup(x, y, dir_x, dir_y, _defTroop);
                m_DefSoldierGroup[_defTroop.key] = soldierGroup;
            }
            _defTroop.addTroop = false;
        }
    }

    private void CorrectPos(ref float x, ref float y)
    {
        x = x / 10;
        y = y / 10;
        return;
    }

    public void SetSpeed(int speed)
    {
        foreach (var kv in m_AtkSoldierGroup)
        {
            kv.Value.SetSpeed(speed);
        }
        foreach (var kv in m_DefSoldierGroup)
        {
            kv.Value.SetSpeed(speed);
        }
    }
}

