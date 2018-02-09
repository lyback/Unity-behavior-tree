using System;
using FSM;

namespace Battle.Logic
{
    class AttackState : FSMStateBase<TroopData, BattleData>
    {
        public AttackState(Enum name) : base(name)
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
                TroopData tarTroop = dataMgr.mAllTroopDic[troop.targetKey];
                if (tarTroop == null)
                {
                    Debugger.LogError("AttackState 未找到目标");
                }
                uint atkDis = TroopHelper.GetTroopAtkDis(troop.type);
                uint myRange = TroopHelper.GetTroopRange(troop.type);
                uint tarRange = TroopHelper.GetTroopRange(tarTroop.type);
                int dis = MathHelper.TroopDistanceV2(troop, tarTroop);
                if (dis > atkDis + myRange + tarRange)
                {
                    return TroopFSMState.Move;
                }
                else
                {
                    return Excute(ref troop);
                }
            }
            return TroopFSMState.End;
        }
        public override Enum Excute(ref TroopData troop)
        {
            TroopData tarTroop = dataMgr.mAllTroopDic[troop.targetKey];
            if (tarTroop == null)
            {
                Debugger.LogError("AttackState 未找到目标");
            }
            if (troop.norAtkCD == 0)
            {
                troop.state = TroopAnimState.Attack;
                troop.dir_x = tarTroop.x;
                troop.dir_y = tarTroop.y;
                troop.inPrepose = true;
                troop.preTime = TroopHelper.GetTroopAtkPrepareTime(troop.type);
                troop.isAtk = true;
                troop.norAtkCD = TroopHelper.GetTroopAtkCDTime(troop.type);
            }
            return TroopFSMState.End;
        }
    }
}
