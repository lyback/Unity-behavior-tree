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
using FSMFrame;

namespace Battle.Logic.AI.FSM
{
    class AttackState : FSMStateBase<FSMInputData, FSMMgrData>
    {
        public AttackState(Enum name) : base(name)
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
                TroopData tarTroop = dataMgr.m_BattleData.mAllTroopDic[troop.targetKey];
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
                    return Excute(ref _input);
                }
            }
            return TroopFSMState.End;
        }
        public override Enum Excute(ref FSMInputData _input)
        {
            var troop = _input.m_Troop;
            TroopData tarTroop = dataMgr.m_BattleData.mAllTroopDic[troop.targetKey];
            if (tarTroop == null)
            {
                Debugger.LogError("AttackState 未找到目标");
            }
            if (troop.norAtkCD == 0)
            {
                troop.state = (int)TroopAnimState.Attack;
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
