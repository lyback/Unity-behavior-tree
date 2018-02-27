using System;
using FSMFrame;

namespace Battle.Logic.AI.FSM
{
    class MoveState : FSMStateBase<FSMInputData, FSMMgrData>
    {
        private int mTar_X;
        private int mTar_Y;
        public MoveState(Enum name) : base(name)
        {
        }
        public override void Init(FSMMgrData dataMgr)
        {
            base.Init(dataMgr);
        }
        public override Enum Enter(ref FSMInputData _input)
        {
            var troop = _input.m_Troop;
            if (troop.targetKey != 0)
            {
                var tarTroop = dataMgr.m_BattleData.mAllTroopDic[troop.targetKey];
                if (tarTroop == null)
                {
                    Debugger.LogError("MoveState 未找到目标");
                }
                troop.state = (int)TroopAnimState.Move;
                return MoveToTarget(ref _input, tarTroop);
            }
            else
            {
                troop.state = (int)TroopAnimState.Move;
                return MoveToCenter(ref _input);
            }
        }
        public override Enum Excute(ref FSMInputData _input)
        {
            var troop = _input.m_Troop;
            if (troop.x > mTar_X)
            {
                troop.x -= 1;
                if (troop.x < mTar_X)
                {
                    troop.x = mTar_X;
                }
            }
            else if (troop.x < mTar_X)
            {
                troop.x += 1;
                if (troop.x > mTar_X)
                {
                    troop.x = mTar_X;
                }
            }
            if (troop.y > mTar_Y)
            {
                troop.y -= 1;
                if (troop.y < mTar_Y)
                {
                    troop.y = mTar_Y;
                }
            }
            else if (troop.y < mTar_Y)
            {
                troop.y += 1;
                if (troop.y > mTar_Y)
                {
                    troop.y = mTar_Y;
                }
            }
            return TroopFSMState.End;
        }
        private Enum MoveToTarget(ref FSMInputData _input, TroopData target)
        {
            var troop = _input.m_Troop;
            mTar_X = target.x;
            mTar_Y = target.y;
            troop.dir_x = mTar_X;
            troop.dir_y = mTar_Y;
            return Excute(ref _input);
        }
        private Enum MoveToCenter(ref FSMInputData _input)
        {
            var troop = _input.m_Troop;
            mTar_X = troop.x;
            mTar_Y = 0;
            troop.dir_x = mTar_X;
            troop.dir_y = mTar_Y;
            return Excute(ref _input);
        }
    }
}
