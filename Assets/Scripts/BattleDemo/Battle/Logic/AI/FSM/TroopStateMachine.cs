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

using FSMFrame;
using System;
namespace Battle.Logic.AI.FSM
{
    public class TroopStateMachine : FSMStateMachineBase<FSMInputData,FSMMgrData>
    {
        public TroopStateMachine() : base()
        {

        }

        public override void Init(FSMMgrData dataMgr)
        {
            base.Init(dataMgr);
            InitState();
            SetStartState(TroopFSMState.Start);
            foreach (var kv in mStateDic)
            {
                kv.Value.Init(dataMgr);
            }
        }

        public override void InitState()
        {
            base.InitState();
            AddState(new StartState(TroopFSMState.Start));
            AddState(new FindTargetState(TroopFSMState.FindTarget));
            AddState(new MoveState(TroopFSMState.Move));
            AddState(new AttackState(TroopFSMState.Attack));
        }

        public override void DoLogic(ref FSMInputData _input)
        {
            base.DoLogic(ref _input);
            Enum _nextState = mStartState.Enter(ref _input);
            while ((TroopFSMState)_nextState != TroopFSMState.End)
            {
                _nextState = GetState(_nextState).Enter(ref _input);
            }

        }
    }
}

