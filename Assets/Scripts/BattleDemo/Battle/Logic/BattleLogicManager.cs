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

namespace Battle.Logic
{
    public class BattleLogicManager
    {

        private static float m_logicTime = 1f / BattleLogicDefine.logicSecFrame;

        public BattleData m_BattleData { get; private set; }
        private TroopLogicCtrl m_TroopLogicCtrl;
        public RandHelper m_Rand { get; private set; }

        private bool m_isReport = false;

        private bool m_isFinish = false;
        private float m_logicAddTime = 0f;

        public int m_currentFrame = 0; //当前帧
        public int m_finishFrame = 0; //目标帧

        public BattleLogicManager(BattleData battleData, int _seed)
        {
            m_BattleData = battleData;
            m_TroopLogicCtrl = new TroopLogicCtrl(this);
            m_Rand = new RandHelper(_seed);
            m_BattleData.mSeed = m_Rand.GetSeed();
            SetCurrentFrame(1);
            m_finishFrame = 0;
            m_logicAddTime = 0f;
            m_isFinish = false;
        }

        public LogicState Update(float dt = 0)
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
            if (m_TroopLogicCtrl.UpdateLogic(m_currentFrame))
            {
                m_isFinish = true;
                //Debugger.Log("输出日志...");
            }
            SetCurrentFrame(m_currentFrame + 1);

            return LogicState.Playing;
        }

        private void SetCurrentFrame(int currentFrame)
        {
            m_currentFrame = currentFrame;
            m_BattleData.mCurrentFrame = currentFrame;
        }
        public void SetFinishFrame(int finishFrame)
        {
            m_finishFrame = finishFrame;
        }
        public void SetSpeed(int speed)
        {
            m_logicTime = 1f / BattleLogicDefine.logicSecFrame / speed;
        }
    }
}
