using System;

#if ISSERVER

#else
using UnityEngine;
using Neatly.Timer;
using Neatly;
#endif


namespace Battle.Logic
{
    public class TimerHelper
    {
#if ISSERVER
        public static object AddClock()
        {
            return null;
        }
#else
        public static GameObject AddClock(string name, Action<float> action, float intervalClock = 1, bool once = false)
        {
            Transform Root = MainBattleManager.Instance.m_BattleViewRoot;
            Transform trans = Root.FindChild(name);
            NeatlyBehaviour clock;
            if (trans == null)
            {
                clock = new GameObject(name).AddComponent<NeatlyBehaviour>();
                clock.transform.SetParent(Root);
            }
            else
            {
                clock = trans.GetComponent<NeatlyBehaviour>();
                if (clock == null)
                {
                    clock = trans.gameObject.AddComponent<NeatlyBehaviour>();
                }
            }
            NeatlyTimer.AddClock(clock, action);
            return clock.gameObject;
        }
#endif

#if ISSERVER
        public static object AddFrame()
        {
            return null;
        }
#else
        public static GameObject AddFrame(string name, Action<float> action, float intervalClock = 1, bool once = false)
        {
            Transform Root = MainBattleManager.Instance.m_BattleViewRoot;
            Transform trans = Root.FindChild(name);
            NeatlyBehaviour frame;
            if (trans == null)
            {
                frame = new GameObject(name).AddComponent<NeatlyBehaviour>();
                frame.transform.SetParent(Root);
            }
            else
            {
                frame = trans.GetComponent<NeatlyBehaviour>();
                if (frame == null)
                {
                    frame = trans.gameObject.AddComponent<NeatlyBehaviour>();
                }
            }
            NeatlyTimer.AddFrame(frame, action);
            return frame.gameObject;
        }
#endif
#if ISSERVER
        public static void Remove()
        {
            return;
        }
#else
        public static void Remove(object timeObj)
        {
            NeatlyTimer.Remove(timeObj as GameObject);
        }
#endif
    }
}
