using Battle.Logic;
public class BattleView
{
    private BattleData m_BattleData;
    private BattleLogicManager m_BattleLogicMgr;

    private BattleScene m_BattleScene;

    private int m_Speed;
    private object m_TimeObj;

    public void Dispose()
    {
        Exit();
    }

    public void Init(BattleData _battleData)
    {
        m_BattleData = _battleData;
        m_BattleLogicMgr = new BattleLogicManager(m_BattleData, m_BattleData.mSeed);

        m_BattleScene = new BattleScene(m_BattleLogicMgr);

        m_TimeObj = TimerHelper.AddFrame(m_BattleData.mBattleKey, OnUpdate);
    }

    public void SyncFrame(BattleData data)
    {
        Debugger.Log("完成帧：" + data.mFinishFrame);
        m_BattleLogicMgr.SetFinishFrame(data.mFinishFrame);
        AddCommand(data.mOperators);
        int currentFrame = data.mCurrentFrame;
        while (true)
        {
            if (m_BattleLogicMgr.m_currentFrame >= currentFrame - 1)
            {
                return;
            }
            LogicState state = m_BattleLogicMgr.Update();
            if (state == LogicState.End)
            {
                Exit();
                return;
            }
        }
    }
    public void SetSpeed(int speed)
    {
        m_Speed = speed;
        m_BattleLogicMgr.SetSpeed(speed);
        m_BattleScene.SetSpeed(speed);
    }
    private void OnUpdate(float dt)
    {
        LogicState result = m_BattleLogicMgr.Update(dt);
        //Debugger.Log("curState:"+result);
        if (result == LogicState.Playing)
        {
            m_BattleScene.Update();
        }
        else if (result == LogicState.End)
        {
            Exit();
        }
    }
    
    //添加指令
    private void AddCommand(string operators)
    {

    }

    //退出战场
    private void Exit()
    {
        TimerHelper.Remove(m_TimeObj);
        if (BattleLogicDefine.isServer)
        {

        }
    }
}
