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

using UnityEngine;
using System;
using System.Collections.Generic;

namespace FSMFrame
{
    public class FSMStateMachineBase<T, P>
        where T : FSMDataBase
        where P : FSMDataMgrBase
    {
        protected Dictionary<Enum, FSMStateBase<T, P>> mStateDic = new Dictionary<Enum, FSMStateBase<T, P>>();
        protected FSMStateBase<T, P> mStartState = null;

        public FSMStateMachineBase()
        {

        }

        public virtual void InitState()
        {

        }

        public virtual void Init(P dataMgr)
        {

        }

        public virtual void DoLogic(ref T data)
        {

        }

        public void AddState(FSMStateBase<T,P> state)
        {
            mStateDic.Add(state.name, state);
        }

        public FSMStateBase<T, P> GetState(Enum name)
        {
            if (mStateDic.ContainsKey(name))
            {
                return mStateDic[name];
            }
            return null;
        }

        public void SetStartState(Enum name)
        {
            if (mStateDic.ContainsKey(name))
            {
                mStartState = mStateDic[name];
            }
            else
            {
                Debugger.LogError("No contain this StartState, Plaese add then state first");
            }
        }

    }
}
