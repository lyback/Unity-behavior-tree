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
            Transform trans = Root.Find(name);
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
            Transform trans = Root.Find(name);
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
