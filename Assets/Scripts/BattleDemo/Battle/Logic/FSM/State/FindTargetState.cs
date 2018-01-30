using System;
using System.Collections.Generic;
using FSM;
namespace Battle.Logic
{
    class FindTargetState : FSMStateBase<TroopData, BattleData>
    {
        public FindTargetState(Enum name) : base(name)
        {
        }
        public override void Init(BattleData dataMgr)
        {
            base.Init(dataMgr);
        }
        public override Enum Enter(ref TroopData troop)
        {
            //已有目标
            if (troop.targetKey != 0)
            {
                var target = dataMgr.mAllTroopDic[troop.targetKey];
                if (target == null)
                {
                    Debugger.LogError("FindTargetState 未找到目标");
                }
                if (target.count != 0)
                {
                    return TroopFSMState.Attack;
                }
            }
            //寻找新目标
            troop.targetKey = 0;
            return Excute(ref troop);
        }
        public override Enum Excute(ref TroopData troop)
        {
            List<TroopData> enemys;
            if (troop.isAtkTroop)
            {
                enemys = dataMgr.mDefTroopList;
            }
            else
            {
                enemys = dataMgr.mAtcTroopList;
            }
            //找最近的目标
            int minDis = -1;
            uint targetKey = 0;
            for (int i = 0; i < enemys.Count; i++)
            {
                int dis = MathHelper.TroopDistanceV2(troop, enemys[i]);
                if (minDis < 0)
                {
                    minDis = dis;
                    targetKey = enemys[i].key;
                }
                else if (minDis > dis)
                {
                    minDis = dis;
                    targetKey = enemys[i].key;
                }
            }
            if (minDis < 0)
            {
                return TroopFSMState.End;
            }
            else if (troop.inWar || minDis <= TroopHelper.GetTroopSpyDis(troop.type))
            {
                troop.targetKey = targetKey;
                troop.inWar = true;
                return TroopFSMState.Attack;
            }
            return TroopFSMState.Move;
        }
    }
}
