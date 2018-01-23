using UnityEngine;
using System;
using System.Collections.Generic;

namespace FSM
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

        public virtual void DoLogic(T data)
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
