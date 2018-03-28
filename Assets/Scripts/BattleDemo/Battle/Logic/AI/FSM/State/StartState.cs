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
