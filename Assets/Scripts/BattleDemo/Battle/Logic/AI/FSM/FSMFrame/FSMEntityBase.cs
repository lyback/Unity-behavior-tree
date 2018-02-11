using Neatly;

namespace FSM
{
    public class FSMEntityBase<T> : NeatlyBehaviour where T: FSMDataBase
    {
        public T mData;
        public virtual void Init(T _data)
        {
            mData = _data;
        }
        public virtual void CreateInit()
        {

        }
    }
}
