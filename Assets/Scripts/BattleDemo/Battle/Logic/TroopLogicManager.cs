using System;

namespace Battle.Logic
{
    public class TroopLogicManager
    {
        private TroopStateMachine m_TroopFSMState;
        private BattleLogicManager m_BattleLogic;

        public TroopLogicManager(BattleLogicManager battleLogic)
        {
            m_BattleLogic = battleLogic;
            m_TroopFSMState = new TroopStateMachine();
            m_TroopFSMState.Init(battleLogic.m_BattleData);
        }


    }
}
