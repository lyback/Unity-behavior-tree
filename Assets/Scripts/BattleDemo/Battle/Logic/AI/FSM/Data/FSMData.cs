
namespace Battle.Logic.AI.FSM
{
    public class FSMInputData : FSMFrame.FSMDataBase
    {
        public TroopData m_Troop;
        public void SetTroop(TroopData _troop)
        {
            m_Troop = _troop;
        }
    }
    public class FSMMgrData : FSMFrame.FSMDataMgrBase
    {
        public BattleData m_BattleData;
        public void SetBattleData(BattleData _battleData)
        {
            m_BattleData = _battleData;
        }
    }
}
