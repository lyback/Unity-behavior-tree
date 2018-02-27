using System;
using FSMFrame;
namespace Battle.Logic.AI.FSM
{
    class StartState : FSMStateBase<FSMInputData, FSMMgrData>
    {
        public StartState(Enum name) : base(name)
        {
        }
        public override void Init(FSMMgrData dataMgr)
        {
            base.Init(dataMgr);
        }
        public override Enum Enter(ref FSMInputData _input)
        {
            var troop = _input.m_Troop;
            //死亡
            if (troop.count==0)
            {
                troop.state = (int)TroopAnimState.Die;
                return TroopFSMState.End;
            }
            //前置动作
            if (troop.inPrepose)
            {
                if (troop.preTime>0)
                {
                    troop.preTime -= 1;
                }
                else
                {
                    //发射武器
                    troop.inPrepose = false;
                }
                troop.state = (int)TroopAnimState.Idle;
                return TroopFSMState.End;
            }
            //普攻CD
            if (troop.norAtkCD > 0)
            {
                troop.norAtkCD -= 1;
                troop.state = (int)TroopAnimState.Idle;
                return TroopFSMState.End;
            }
            //寻找目标
            return TroopFSMState.FindTarget;
        }

    }
}
