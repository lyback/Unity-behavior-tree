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

using Battle.Logic;
using Neatly.Timer;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.View
{
    public class SoldierGroup : TroopGroupEntity
    {
        private SoldierType m_Type;
        private uint m_BaseCount;
        private TroopAnimState m_CurrentState;
        private bool m_IsAttacker;
        private uint m_Line;
        private uint m_Row;
        private float m_Scale;
        private float m_LineInterval;
        private float m_RowInterval;

        private SoldierObject[,] soldierArray;
        private List<int> soldierIndexList;
        private List<SoldierObject> m_CacheDieSoldier = new List<SoldierObject>();

        public void Init(float x, float y, float dir_x, float dir_y, TroopData data)
        {
            base.Init(data);
            m_Type = data.type;
            m_IsAttacker = data.isAtkTroop;
            m_Line = data.line;
            m_Row = data.row;
            m_Scale = 1f;
            m_LineInterval = 1f;
            m_RowInterval = 1f;
            //初始化坐标
            InitPosition(x, y);
            m_BaseCount = data.count;
            if (data.count > 0)
            {
                //float deg = CalcRad(dir_x, dir_y);
                InitAllSoldiers(data.count, (TroopAnimState)data.state);
            }
        }
        /// <summary>
        /// 初始化士兵
        /// </summary>
        private void InitAllSoldiers(uint count, TroopAnimState nState, float deg = 0f)
        {
            m_BaseCount = count;
            if (soldierArray == null)
            {
                soldierArray = new SoldierObject[mData.line, mData.row];
            }
            if (soldierIndexList == null)
            {
                soldierIndexList = new List<int>();
            }

            for (int i = 0; i < soldierArray.GetLength(0); i++)
            {
                for (int j = 0; j < soldierArray.GetLength(1); j++)
                {
                    if (soldierArray[i, j] != null) continue;
                    SoldierData soldierData = new SoldierData(mData, i, j);
                    var soldier = MainBattleManager.Instance.CreateSoldierObject(soldierData.type, transform);
                    soldier.Init(soldierData);
                    soldierArray[i, j] = soldier;
                    soldierIndexList.Add(GetIndexValue(i, j));
                }
            }
        }

        public void Doing(float x, float y, float dir_x, float dir_y, TroopData data)
        {
            m_CurrentState = (TroopAnimState)data.state;
            //士兵已死
            if (m_BaseCount == 0 && data.count == 0) return;
            //复活(从出生点)
            if (m_BaseCount == 0)
            {
                InitPosition(x, y);
            }
            //初始化显示
            if (!m_IsInit)
            {
                Show(true);
            }
            float deg = CalcRad(dir_x, dir_y);
            //有其他玩家加入，士兵刷新
            if (data.addTroop && data.count > 0)
            {
                InitAllSoldiers(data.count, (TroopAnimState)data.state, deg);
                //不影响移动
                if (m_CurrentState == TroopAnimState.Move)
                {
                    SetTweenPosition(x, y);
                }
                return;
            }
            //判断死亡数
            uint allObject = data.row * data.line;
            int remainIndexCount = soldierIndexList == null ? 0 : soldierIndexList.Count;
            int loseCount = (int)((m_BaseCount - data.count) * allObject / m_BaseCount + remainIndexCount - allObject);
            for (int i = 0; i < loseCount; i++)
            {
                DieOne(deg);
            }

            for (int i = 0; i < soldierArray.GetLength(0); i++)
            {
                for (int j = 0; j < soldierArray.GetLength(1); j++)
                {
                    if (soldierArray[i, j] != null)
                    {
                        soldierArray[i, j].SetState(m_CurrentState);
                    }
                }
            }
            if (m_CurrentState == TroopAnimState.Move)
            {
                //如果状态是移动则播放
                SetTweenPosition(x, y);
            }
            SetSoldierLocalRotation(deg);
            if (data.count == 0)
            {
                m_BaseCount = 0;
                //ShowUI(false);
            }
        }

        private void DieOne(float deg)
        {
            int randIndex = Random.Range(0, soldierIndexList.Count);
            var soldierObject = GetSoldierByIndex(soldierIndexList[randIndex]);
            soldierObject.SetState(TroopAnimState.Die);
            m_CacheDieSoldier.Add(soldierObject);
            //固定时间后回收
            soldierObject.SetParent(MainBattleManager.Instance.m_BattleViewRoot);
            //SoarDTween.Alpha(soldierObject.Renderer, 0, 1);
            NeatlyTimer.AddClock(soldierObject, f =>
            {
                soldierObject.Dispose();
                m_CacheDieSoldier.Clear();
            }, 1.2f, true);
            soldierIndexList.RemoveAt(randIndex);
        }

        private void SetSoldierLocalRotation(float y)
        {
            for (int i = 0; i < soldierArray.GetLength(0); i++)
            {
                for (int j = 0; j < soldierArray.GetLength(1); j++)
                {
                    if (soldierArray[i, j] != null)
                    {
                        soldierArray[i, j].SetLocalRotation(y);
                    }
                }
            }
        }

        private SoldierObject GetSoldierByIndex(int index)
        {
            int i = index >> 4;
            int j = index & 0xf;
            Debugger.Log(i + "  " + j);
            var soldier = soldierArray[i, j];
            soldierArray[i, j] = null;
            return soldier;
        }

        private int GetIndexValue(int i, int j)
        {
            int r = i << 4;
            r += j;
            return r;
        }

        private float CalcRad(float x, float y)
        {
            var disX = x - m_CurPosX;
            var disY = y - m_CurPosY;
            float deg = Mathf.Atan2(disY, disX) * Mathf.Rad2Deg;
            deg = -deg + 270;
            if (deg > 180)
            {
                deg -= 360;
            }
            if (deg < -180)
            {
                deg += 360;
            }
            return deg;
        }
        public void Dispose()
        {
            for (int i = 0; i < m_CacheDieSoldier.Count; i++)
            {
                m_CacheDieSoldier[i].Dispose();
            }
            m_CacheDieSoldier.Clear();
            if (soldierArray != null)
            {
                for (int i = 0; i < soldierArray.GetLength(0); i++)
                {
                    for (int j = 0; j < soldierArray.GetLength(1); j++)
                    {
                        if (soldierArray[i, j] != null)
                        {
                            soldierArray[i, j].Dispose();
                        }
                    }
                }
                soldierArray = null;
            }
            soldierIndexList = null;

            MainBattleManager.Instance.RecycleSoldierGroup(this);
        }
        public void SetSpeed(int speed)
        {
            for (int i = 0; i < soldierArray.GetLength(0); i++)
            {
                for (int j = 0; j < soldierArray.GetLength(1); j++)
                {
                    if (soldierArray[i, j] != null)
                    {
                        soldierArray[i, j].SetSpeed(speed);
                    }
                }
            }
        }
    }

}
