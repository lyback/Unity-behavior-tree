using System;

namespace FSM
{
    public class FSMStateBase<T,P>
        where T : FSMDataBase
        where P : FSMDataMgrBase
    {
        public Enum name = null;
        public P dataMgr;

        public FSMStateBase(Enum name)
        {
            this.name = name;
        }

        public virtual void Init(P dataMgr)
        {
            this.dataMgr = dataMgr;
        }

        public virtual Enum Enter(ref T data)
        {
            return null;
        }

        public virtual Enum Excute(ref T data)
        {
            return null;
        }

    }
}
