using Neatly.Timer;
using UnityEngine;
using System;
using Battle.Logic;
namespace Battle.View
{
    public class SoldierObject : UnitEntityBase<SoldierData>
    {
        private static string[] MONSTER_STATE_NAME = { "idle", "move", "attack", "die" };
        private static int[] m_HashStateName;
        private int[] HashStateName
        {
            get
            {
                if (m_HashStateName == null)
                {
                    m_HashStateName = new int[Enum.GetNames(typeof(TroopAnimState)).Length];
                    for (int i = 0; i < m_HashStateName.Length; i++)
                    {
                        m_HashStateName[i] = Animator.StringToHash(MONSTER_STATE_NAME[i]);
                    }
                }
                return m_HashStateName;
            }
        }
        private Animator m_Animator;
        private TroopAnimState m_CurrentState;

        public override void Init(SoldierData _data)
        {
            base.Init(_data);
            transform.localPosition = new Vector3(_data.x, 0, _data.y);
        }
        public override void CreateInit()
        {
            base.CreateInit();
            m_Animator = GetComponent<Animator>();
        }

        void OnPlayEnd(string name)
        {
            if (m_CurrentState == TroopAnimState.Attack)
            {
                m_CurrentState = TroopAnimState.Idle;
            }
            Play();
        }

        public void SetState(TroopAnimState state)
        {
            if (m_CurrentState == state) return;
            if (state == TroopAnimState.Die)
            {
                m_CurrentState = state;
                //执行 
                Play();
                //渐隐
                NeatlyTimer.AddClock(this, f => { this.SetActive(false); }, 2, true);
                return;
            }
            if (m_CurrentState == TroopAnimState.Attack) return;
            m_CurrentState = state;
            Play();
        }
        public void SetSpeed(int speed)
        {
            m_Animator.speed = speed;
        }
        public void Play()
        {
            m_Animator.Play(HashStateName[(int)m_CurrentState]);
        }

        public void Dispose()
        {
            NeatlyTimer.Remove(this);
            MainBattleManager.Instance.RecycleSoldierObject(mData.type, this);
        }
    }


}
