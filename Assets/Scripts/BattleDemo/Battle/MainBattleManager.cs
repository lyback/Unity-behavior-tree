
using Battle.Logic;
using UnityEngine;

class MainBattleManager: Singleton<MainBattleManager>
{
    private BattleResManager m_BattleResManager;
    private BattleDataManager m_BattleDataManager;

    public Transform m_BattleViewRoot;

    public float LogicTime { get; private set; }

    public override void Init()
    {
        if (m_BattleResManager == null)
        {
            GameObject battleRes = new GameObject();
            battleRes.name = "~Battle";
            m_BattleResManager = battleRes.AddComponent<BattleResManager>();
        }
        m_BattleResManager.Init();
        if (m_BattleDataManager == null)
        {
            m_BattleDataManager = new BattleDataManager();
        }
        m_BattleDataManager.Init();
    }
    public override void Dispose()
    {
        m_BattleResManager.Dispose();
        m_BattleDataManager.Dispose();
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
        LogicTime = logicTime;
    }
}
