
namespace Battle.Logic
{
    public class BattleLogicManager
    {

        private static float m_logicTime = 1 / BattleLogicDefine.logicSecFrame;

        public BattleDataManager m_BattleData { get; private set; }
        public RandHelper m_Rand { get; private set; }

        private bool m_isReport = false;

        private bool m_isFinish = false;
        private float m_logicAddTime = 0f;

        public int m_currentFrame = 0; //当前帧
        public int m_finishFrame = 0; //目标帧

        public void Init(BattleDataManager _battleData, int _seed)
        {
            m_BattleData = _battleData;
            m_Rand = new RandHelper(_seed);
            m_BattleData.mSeed = m_Rand.GetSeed();
            SetCurrentFrame(1);
            m_finishFrame = 0;
            m_logicAddTime = 0f;
            m_isFinish = false;
        }

        public LogicState Update(float dt)
        {
            if (m_isFinish)
            {
                return LogicState.End;
            }
            if (!BattleLogicDefine.isServer && !m_isReport)
            {
                if (m_currentFrame > m_finishFrame)
                {
                    return LogicState.Nothing;
                }
            }
            if (dt > 0)
            {
                m_logicAddTime += dt;
                if (m_logicAddTime < m_logicTime)
                {
                    return LogicState.Nothing;
                }
                else
                {
                    m_logicAddTime -= m_logicTime;
                }
            }

            string frameKey = m_currentFrame.ToString();
            if (true)
            {

            }

            return LogicState.Playing;
        }

        private void SetCurrentFrame(int currentFrame)
        {
            m_currentFrame = currentFrame;
            m_BattleData.mCurrentFrame = currentFrame;
        }
    }
}
