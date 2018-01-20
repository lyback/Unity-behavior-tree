using FSM;
using System.Collections.Generic;
using UnityEngine;

public sealed class BattlePool<T,P> where T : UnitEntityBase<P> where P:FSMDataBase
{
    private GameObject m_SoldierPrefab;
    private bool m_IsUI;
    List<T> pool = new List<T>();
    public static BattlePool<T,P> Instance { get; private set; }

    public void Init(GameObject soldierPrefab, bool isUI = false)
    {
        Instance = this;
        m_SoldierPrefab = soldierPrefab;
        m_IsUI = isUI;
    }

    public T Create(Transform parent = null)
    {
        T t;
        if (Count > 0)
        {
            t = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
        }
        else
        {
            GameObject go = Object.Instantiate(m_SoldierPrefab);
            t = go.GetComponent<T>() ?? go.AddComponent<T>();
        }
        t.CreateInit();
        if (parent)
        {
            t.SetParent(parent);
        }
        if (m_IsUI)
        {
            t.SetLocalScale(Vector3.one);
        }
        else
        {
            t.transform.localPosition = Vector3.zero;
            t.SetActive(true);
        }
        return t;
    }

    public void Recycle(T t)
    {
        if (m_IsUI)
        {
            t.SetLocalScale(Vector3.zero);
        }
        else
        {
            t.SetActive(false);
        }
        pool.Add(t);
    }

    public int Count { get { return pool.Count; } }

    public void Prepare(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = Object.Instantiate(m_SoldierPrefab);
            var t = go.GetComponent<T>() ?? go.AddComponent<T>();
            t.transform.SetParent(MainBattleManager.Instance.m_BattleViewRoot);
            if (m_IsUI)
            {
                t.SetActive(true);
            }
            Recycle(t);
        }
    }
}