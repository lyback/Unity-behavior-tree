using Neatly;
using FSMFrame;
namespace Battle.View
{
    public class AIEntityBase<T> : NeatlyBehaviour
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
