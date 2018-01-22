
using Battle.Logic;
using System.Collections.Generic;
using UnityEngine;

class MainBattleManager: Singleton<MainBattleManager>
{
    private BattleResManager m_BattleResManager;
    private Dictionary<string, BattleView>  m_BattleViewDic;

    public Transform m_BattleViewRoot;

    public float m_LogicTime { get; private set; }

    public override void Init()
    {
        Debugger.Log("战斗管理器初始化");
        if (m_BattleResManager == null)
        {
            GameObject battleRes = new GameObject();
            battleRes.name = "~Battle";
            m_BattleResManager = battleRes.AddComponent<BattleResManager>();
        }
        m_BattleResManager.Init();

        if (m_BattleViewDic == null)
        {
            m_BattleViewDic = new Dictionary<string, BattleView>();
        }
    }
    public override void Dispose()
    {
        m_BattleResManager.Dispose();
        foreach (var kv in m_BattleViewDic)
        {
            kv.Value.Dispose();
        }
    }

    public void CreateBattle(BattleData data)
    {
        Debugger.Log("创建战场：" + data.mBattleKey);
        if (m_BattleViewDic.ContainsKey(data.mBattleKey))
        {
            Debugger.LogError("Has exist battle with key:" + data.mBattleKey);
            return;
        }
        m_BattleViewDic.Add(data.mBattleKey, new BattleView());
        m_BattleViewDic[data.mBattleKey].Init(data);
    }
    public void ExitBattle(string battleKey)
    {
        if (m_BattleViewDic.ContainsKey(battleKey))
        {
            Debugger.Log("退出战场：" + battleKey);
            m_BattleViewDic[battleKey].Dispose();
            m_BattleViewDic.Remove(battleKey);
        }
    }

    public void SyncFrame(Dictionary<string, BattleData> data)
    {
        foreach (var kv in data)
        {
            if (m_BattleViewDic.ContainsKey(kv.Key))
            {
                m_BattleViewDic[kv.Key].SyncFrame(kv.Value);
            }
        }
    }

    #region Pools Interface
    public T GetBattlePool<T>() where T : class
    {
        return m_BattleResManager.GetPool<T>();
    }
    public SoldierObject CreateSoldierObject(Transform transform)
    {
        return m_BattleResManager.m_SoldierPool.Create(transform);
    }
    public void RecycleSoldierObject(SoldierObject obj)
    {
        m_BattleResManager.m_SoldierPool.Recycle(obj);
    }
    #endregion

    public void InitLogicTime(float logicTime)
    {
        m_LogicTime = logicTime;
    }
}
