using FSM;
using UnityEngine;

public class UnitEntityBase<T>: FSMEntityBase<T> where T:FSMDataBase
{
    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void SetLocalPosition(float x, float y)
    {
        transform.localPosition = new Vector3(x, 0, y);
    }

    public void SetLocalRotation(float y)
    {
        transform.localEulerAngles = new Vector3(0, y, 0);
    }
}
