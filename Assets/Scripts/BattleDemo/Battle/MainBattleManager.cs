using Battle.Logic;
using Battle.View;
using System.Collections.Generic;
using UnityEngine;
class MainBattleManager : Singleton<MainBattleManager>
{
    private BattleResManager m_BattleResManager;
    private Dictionary<string, BattleView> m_BattleViewDic;

    public Transform m_BattleViewRoot;

    private float _LogicTime;
    public float m_LogicTime
    {
        get
        {
            return _LogicTime / _Speed;
        }
    }
    private int _Speed;
    public int m_Speed
    {
        get
        {
            return _Speed;
        }
    }

    public override void Init()
    {
        Debugger.Log("战斗管理器初始化");

        if (m_BattleResManager == null)
        {
            Object battleResobj = Resources.Load("battle/BattleRes");
            GameObject battleRes = GameObject.Instantiate(battleResobj) as GameObject;
            battleRes.name = "~Battle";
            m_BattleResManager = battleRes.GetComponent<BattleResManager>();
            m_BattleViewRoot = battleRes.transform;
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

    public void Start()
    {
        BattleData data = new BattleData();
        data.Init();
        CreateBattle(data);
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
        m_BattleViewDic[data.mBattleKey].SyncFrame(data);
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
    public SoldierGroup CreateSoldierGroup(float x, float y, float dir_x, float dir_y, TroopData data)
    {
        SoldierGroup group = m_BattleResManager.m_SoldierGroupPool.Create(m_BattleViewRoot);
        group.Init(x, y, dir_x, dir_y, data);
        return group;
    }
    public void RecycleSoldierGroup(SoldierGroup obj)
    {
        m_BattleResManager.m_SoldierGroupPool.Recycle(obj);
    }

    public SoldierObject CreateSoldierObject(SoldierType type, Transform parent)
    {
        return m_BattleResManager.m_SoldierObjectPools[(int)type].Create(parent);
    }
    public void RecycleSoldierObject(SoldierType type, SoldierObject obj)
    {
        m_BattleResManager.m_SoldierObjectPools[(int)type].Recycle(obj);
    }
    #endregion

    public void InitLogicTime(float logicTime)
    {
        _LogicTime = logicTime;
    }
    public void SetSpeed(int speed)
    {
        _Speed = speed;
        foreach (var kv in m_BattleViewDic)
        {
            kv.Value.SetSpeed(speed);
        }
    }
}
