using FSM;
using System;
namespace Battle.Logic
{
    public class TroopStateMachine : FSMStateMachineBase<TroopData,BattleDataManager>
    {
        public TroopStateMachine() : base()
        {

        }

        public override void Init(BattleDataManager dataMgr)
        {
            base.Init(dataMgr);
            InitState();
            SetStartState(TroopFSMState.Start);
            foreach (var kv in mStateDic)
            {
                kv.Value.Init(dataMgr);
            }
        }

        public override void InitState()
        {
            base.InitState();
            AddState(new StartState(TroopFSMState.Start));
            AddState(new FindTargetState(TroopFSMState.FindTarget));
            AddState(new MoveState(TroopFSMState.Move));
            AddState(new MoveState(TroopFSMState.Attack));
        }

        public override void DoLogic(TroopData data)
        {
            base.DoLogic(data);
            Enum _nextState = mStartState.Enter(data);
            while ((TroopFSMState)_nextState != TroopFSMState.End)
            {
                _nextState = GetState(_nextState).Enter(data);
            }

        }
    }
}

