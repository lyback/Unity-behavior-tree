using System;
using System.Collections.Generic;

namespace Battle.Logic
{
    public class TroopLogicManager
    {
        private TroopStateMachine m_TroopFSMState;
        private BattleLogicCtrl m_BattleLogic;
        private Dictionary<uint, TroopData> m_AllTroopDic { get { return m_BattleLogic.m_BattleData.mAllTroopDic; } }
        private List<TroopData> m_AtkTroopList { get { return m_BattleLogic.m_BattleData.mAtcTroopList; } }
        private List<TroopData> m_DefTroopList { get { return m_BattleLogic.m_BattleData.mDefTroopList; } }
        private bool isFinish = false;

        public TroopLogicManager(BattleLogicCtrl battleLogic)
        {
            m_BattleLogic = battleLogic;

            m_TroopFSMState = new TroopStateMachine();
            m_TroopFSMState.Init(battleLogic.m_BattleData);
        }

        public bool UpdateLogic(int currentFrame)
        {
            if (isFinish)
            {
                return true;
            }

            bool isTroopEmpty = false;
            isTroopEmpty = CheckTroopCount(m_AtkTroopList) || CheckTroopCount(m_DefTroopList);
            if (!isTroopEmpty)
            {
                DoSoldierLogic();
            }
            if (isTroopEmpty)
            {
                Debugger.Log("War End!!!!");
                SetFinishState();
                isFinish = true;
            }
            return false;
        }

        private void DoSoldierLogic()
        {
            for (int i = 0; i < m_AtkTroopList.Count; i++)
            {
                var _atkTroop = m_AtkTroopList[i];
                m_TroopFSMState.DoLogic(ref _atkTroop);
                m_AtkTroopList[i] = _atkTroop;

            }
            for (int i = 0; i < m_DefTroopList.Count; i++)
            {
                var _defTroop = m_DefTroopList[i];
                m_TroopFSMState.DoLogic(ref _defTroop);
                m_DefTroopList[i] = _defTroop;
            }
        }
        private void SetFinishState()
        {
            foreach (var kv in m_AllTroopDic)
            {
                kv.Value.state = TroopAnimState.Idle;
            }
        }
        private bool CheckTroopCount(List<TroopData> _TroopList)
        {
            for (int i = 0; i < _TroopList.Count; i++)
            {
                if (_TroopList[i].ghostTime>0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
