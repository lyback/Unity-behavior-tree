
using Neatly;
using UnityEngine;
using System.Collections.Generic;
using Battle.Logic;

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
