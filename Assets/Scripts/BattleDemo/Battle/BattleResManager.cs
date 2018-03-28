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

using Neatly;
using UnityEngine;
using Battle.Logic;
using Battle.View;

public sealed class BattleResManager : NeatlyBehaviour
{
    #region Hierachy
    [SerializeField]
    private GameObject m_SoldierGroupPrefab;
    [SerializeField]
    private GameObject[] m_SoldierPrefab;

    //远程攻击预制
    [SerializeField]
    private GameObject[] m_WeaponEffectPrefabs;
    //受击预制
    [SerializeField]
    private GameObject[] m_HitEffectPrefabs;

    #endregion

    #region Pools

    [SerializeField]
    [Range(0, 16)]
    private int m_SoldierGroupCC;
    [SerializeField]
    [Range(0, 20)]
    private int m_SoldierCC;

    public BattlePool<SoldierGroup, TroopData> m_SoldierGroupPool { get; private set; }
    public BattlePool<SoldierObject,SoldierData>[] m_SoldierObjectPools { get; private set; }
    #endregion

    public void Init()
    {
        //士兵Group
        m_SoldierGroupPool = new BattlePool<SoldierGroup, TroopData>();
        m_SoldierGroupPool.Init(m_SoldierGroupPrefab);
        //士兵
        m_SoldierObjectPools = new BattlePool<SoldierObject, SoldierData>[m_SoldierPrefab.Length];
        for (int i = 0; i < m_SoldierObjectPools.Length; i++)
        {
            m_SoldierObjectPools[i] = new BattlePool<SoldierObject, SoldierData>();
            m_SoldierObjectPools[i].Init(m_SoldierPrefab[i]);
        }
        InitCache();
    }

    private void InitCache()
    {
        m_SoldierGroupPool.Prepare(m_SoldierGroupCC);
        for (int i = 0; i < m_SoldierObjectPools.Length; i++)
        {
            m_SoldierObjectPools[i].Prepare(m_SoldierCC);
        }
    }
    public void Dispose()
    {

    }

}
