using Battle.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleViewManager : MonoBehaviour {

    private BattleDataManager m_BattleData;
    private BattleLogicManager m_BattleLogic;

    public void Init(BattleDataManager _battleData)
    {
        m_BattleData = _battleData;
        m_BattleLogic = new BattleLogicManager();
        m_BattleLogic.Init(m_BattleData, m_BattleData.mSeed);
    }

    public void UpdateView()
    {

    }
}
