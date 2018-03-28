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
using System.Collections.Generic;
using UnityEngine;

namespace Battle.View
{
    public sealed class BattlePool<T, P> where T : UnitEntityBase<P>
    {
        private GameObject m_SoldierPrefab;
        private bool m_IsUI;
        List<T> pool = new List<T>();
        public static BattlePool<T, P> Instance { get; private set; }

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
}
