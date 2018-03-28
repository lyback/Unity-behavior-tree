//////////////////////////////////////////////////////////////////////////////////////
// The MIT License(MIT)
// Copyright(c) 2018 lycoder

// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

//////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FSMFrame;
namespace Battle.Logic.AI.FSM
{
    class FindTargetState : FSMStateBase<FSMInputData, FSMMgrData>
    {
        public FindTargetState(Enum name) : base(name)
        {
        }
        public override void Init(FSMMgrData dataMgr)
        {
            base.Init(dataMgr);
        }
        public override Enum Enter(ref FSMInputData _input)
        {
            var troop = _input.m_Troop;
            //已有目标
            if (troop.targetKey != 0)
            {
                var target = dataMgr.m_BattleData.mAllTroopDic[troop.targetKey];
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
            return Excute(ref _input);
        }
        public override Enum Excute(ref FSMInputData _input)
        {
            var troop = _input.m_Troop;
            List<TroopData> enemys;
            if (troop.isAtkTroop)
            {
                enemys = dataMgr.m_BattleData.mDefTroopList;
            }
            else
            {
                enemys = dataMgr.m_BattleData.mAtcTroopList;
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
