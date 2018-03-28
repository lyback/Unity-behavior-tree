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
