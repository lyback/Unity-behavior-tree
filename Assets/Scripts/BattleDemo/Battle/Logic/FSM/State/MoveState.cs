using System;
using FSM;

namespace Battle.Logic
{
    class MoveState : FSMStateBase<TroopData, BattleDataManager>
    {
        private int mTar_X;
        private int mTar_Y;
        public MoveState(Enum name) : base(name)
        {
        }
        public override void Init(BattleDataManager dataMgr)
        {
            base.Init(dataMgr);
        }
        public override Enum Enter(TroopData troop)
        {
            if (troop.targetKey != 0)
            {
                var tarTroop = dataMgr.mAllTroopDic[troop.targetKey];
                if (tarTroop == null)
                {
                    Debugger.LogError("MoveState 未找到目标");
                }
                troop.state = TroopAnimState.Move;
                return MoveToTarget(troop, tarTroop);
            }
            else
            {
                troop.state = TroopAnimState.Move;
                return MoveToCenter(troop);
            }
        }
        public override Enum Excute(TroopData troop)
        {
            if (troop.x > mTar_X)
            {
                troop.x -= 1;
                if (troop.x < mTar_X)
                {
                    troop.x = mTar_X;
                }
            }
            else if (troop.x > mTar_X)
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
            else if (troop.y > mTar_Y)
            {
                troop.y += 1;
                if (troop.y > mTar_Y)
                {
                    troop.y = mTar_Y;
                }
            }
            return TroopFSMState.End;
        }
        private Enum MoveToTarget(TroopData troop, TroopData target)
        {
            mTar_X = target.x;
            mTar_Y = target.y;
            return Excute(troop);
        }
        private Enum MoveToCenter(TroopData troop)
        {
            mTar_X = troop.x;
            mTar_Y = 0;
            return Excute(troop);
        }
    }
}
