
using Neatly;
using UnityEngine;
using System.Collections.Generic;
using Battle.Logic;

public sealed class BattleResManager : NeatlyBehaviour
{
    #region Hierachy
    [SerializeField]
    private GameObject m_TroopGroupPrefab;
    [SerializeField]
    private GameObject m_SoldierPrefab;

    //远程攻击预制
    [SerializeField]
    private GameObject[] m_WeaponEffectPrefabs;
    //受击预制
    [SerializeField]
    private GameObject[] m_HitEffectPrefabs;

    [SerializeField]
    private int m_PlaySpeed = 1;
    #endregion

    #region Pools
    public BattlePool<TroopGroupEntity, TroopData> m_TroopGroupPool;
    public BattlePool<SoldierObject, SoldierData> m_SoldierPool;
    public T GetPool<T>() where T:class
    {
        if (typeof(T) == typeof(BattlePool<TroopGroupEntity, TroopData>))
        {
            return m_TroopGroupPool as T;
        }
        else if(typeof(T) == typeof(BattlePool<TroopGroupEntity, TroopData>))
        {
            return m_SoldierPool as T;
        }
        return null;
    }
    #endregion

    public void Init()
    {

    }

    public void Dispose()
    {

    }


}
