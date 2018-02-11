using System;
using FSMFrame;

namespace Battle.Logic.AI.FSM
{
    class MoveState : FSMStateBase<TroopData, BattleData>
    {
        private int mTar_X;
        private int mTar_Y;
        public MoveState(Enum name) : base(name)
        {
        }
        public override void Init(BattleData dataMgr)
        {
            base.Init(dataMgr);
        }
        public override Enum Enter(ref TroopData troop)
        {
            if (troop.targetKey != 0)
            {
                var tarTroop = dataMgr.mAllTroopDic[troop.targetKey];
                if (tarTroop == null)
                {
                    Debugger.LogError("MoveState 未找到目标");
                }
                troop.state = (int)TroopAnimState.Move;
                return MoveToTarget(ref troop, tarTroop);
            }
            else
            {
                troop.state = (int)TroopAnimState.Move;
                return MoveToCenter(ref troop);
            }
        }
        public override Enum Excute(ref TroopData troop)
        {
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
        private Enum MoveToTarget(ref TroopData troop, TroopData target)
        {
            mTar_X = target.x;
            mTar_Y = target.y;
            troop.dir_x = mTar_X;
            troop.dir_y = mTar_Y;
            return Excute(ref troop);
        }
        private Enum MoveToCenter(ref TroopData troop)
        {
            mTar_X = troop.x;
            mTar_Y = 0;
            troop.dir_x = mTar_X;
            troop.dir_y = mTar_Y;
            return Excute(ref troop);
        }
    }
}
