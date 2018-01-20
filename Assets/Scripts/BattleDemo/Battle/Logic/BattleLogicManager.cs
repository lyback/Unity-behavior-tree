
namespace Battle.Logic
{
    public class BattleLogicManager
    {

        private static float m_logicTime = 1 / BattleLogicDefine.logicSecFrame;

        private BattleDataManager m_BattleData;

        public bool m_isFinish = false;
        public float m_logicAddTime = 0f;


        public void Init(BattleDataManager battleData)
        {
            m_BattleData = battleData;

        }

        public LogicState Update(float dt)
        {
            if (m_isFinish)
            {
                return LogicState.End;
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

            return LogicState.Playing;
        }
    }
}
