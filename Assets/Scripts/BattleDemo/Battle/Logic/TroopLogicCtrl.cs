#define BTREE
using System.Collections.Generic;
#if FSM
using Battle.Logic.AI.FSM;
#endif
#if BTREE
using Battle.Logic.AI.BTree;
#endif
namespace Battle.Logic
{
    public class TroopLogicCtrl
    {
#if BTREE
        private Dictionary<uint, BTreeRoot> m_TroopBTreeDic;
#endif
#if FSM
        private TroopStateMachine m_TroopFSMState;
#endif
        private BattleLogicManager m_BattleLogicMgr;
        private Dictionary<uint, TroopData> m_AllTroopDic { get { return m_BattleLogicMgr.m_BattleData.mAllTroopDic; } }
        private List<TroopData> m_AtkTroopList { get { return m_BattleLogicMgr.m_BattleData.mAtcTroopList; } }
        private List<TroopData> m_DefTroopList { get { return m_BattleLogicMgr.m_BattleData.mDefTroopList; } }
        private bool isFinish = false;

        public TroopLogicCtrl(BattleLogicManager battleLogicMgr)
        {
            m_BattleLogicMgr = battleLogicMgr;
#if FSM
            m_TroopFSMState = new TroopStateMachine();
            FSMMgrData _fsmMgrData = new FSMMgrData();
            _fsmMgrData.SetBattleData(battleLogicMgr.m_BattleData);
            m_TroopFSMState.Init(_fsmMgrData);
#endif
#if BTREE
            m_TroopBTreeDic = new Dictionary<uint, BTreeRoot>();
#endif
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

#if FSM
        FSMInputData m_Input = new FSMInputData();
        private void DoSoldierLogic()
        {
            for (int i = 0; i < m_AtkTroopList.Count; i++)
            {
                var _atkTroop = m_AtkTroopList[i];
                m_Input.SetTroop(_atkTroop);
                m_TroopFSMState.DoLogic(ref m_Input);
                m_AtkTroopList[i] = _atkTroop;

            }
            for (int i = 0; i < m_DefTroopList.Count; i++)
            {
                var _defTroop = m_DefTroopList[i];
                m_Input.SetTroop(_defTroop);
                m_TroopFSMState.DoLogic(ref m_Input);
                m_DefTroopList[i] = _defTroop;
            }
        }
#endif
#if BTREE
        BTreeOutputData m_Output = new BTreeOutputData();
        BTreeInputData m_Input = new BTreeInputData();
        private void DoSoldierLogic()
        {
            for (int i = 0; i < m_AtkTroopList.Count; i++)
            {
                var _atkTroop = m_AtkTroopList[i];
                m_Input.SetData(_atkTroop, m_BattleLogicMgr.m_BattleData);
                m_Output.SetData(m_Input);
                if (!m_TroopBTreeDic.ContainsKey(_atkTroop.key))
                {
                    m_TroopBTreeDic.Add(_atkTroop.key, new BTreeRoot());
                    m_TroopBTreeDic[_atkTroop.key].CreateBevTree();
                }
                m_TroopBTreeDic[_atkTroop.key].Tick(m_Input, ref m_Output);
                m_AtkTroopList[i] = m_Output.troop;

            }
            for (int i = 0; i < m_DefTroopList.Count; i++)
            {
                var _defTroop = m_DefTroopList[i];
                m_Input.SetData(_defTroop, m_BattleLogicMgr.m_BattleData);
                m_Output.SetData(m_Input);
                if (!m_TroopBTreeDic.ContainsKey(_defTroop.key))
                {
                    m_TroopBTreeDic.Add(_defTroop.key, new BTreeRoot());
                    m_TroopBTreeDic[_defTroop.key].CreateBevTree();
                }
                m_TroopBTreeDic[_defTroop.key].Tick(m_Input, ref m_Output);
                m_DefTroopList[i] = m_Output.troop;
            }
        }
#endif
        private void SetFinishState()
        {
            foreach (var kv in m_AllTroopDic)
            {
                kv.Value.state = (int)TroopAnimState.Idle;
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
