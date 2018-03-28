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

using System.Collections.Generic;
using FSMFrame;
namespace Battle.Logic
{
    public class BattleData
    {
        public string mBattleKey;
        public Dictionary<uint, TroopData> mAllTroopDic;
        public List<TroopData> mAtcTroopList;
        public List<TroopData> mDefTroopList;

        public int mCurrentFrame; //当前帧
        public int mFinishFrame; //完成帧
        public int mSeed = 0;

        public string mOperators; //指令（预留）

        public void Init()
        {
            RandHelper rand = new RandHelper(1);
            mBattleKey = "test";
            mCurrentFrame = 1;
            mFinishFrame = 10000;
            mAtcTroopList = new List<TroopData>();
            mDefTroopList = new List<TroopData>();
            mAllTroopDic = new Dictionary<uint, TroopData>();
            int atkCount = 30;
            for (int i = 0; i < atkCount; i++)
            {
                TroopData troop = new TroopData();
                troop.count = 100;
                troop.isAtkTroop = true;
                troop.key = (uint)i+1;
                troop.type = (SoldierType)rand.Random(4);
                troop.x = i * 100;
                troop.y = 100 ;
                troop.line = 3;
                troop.row = 2;
                mAtcTroopList.Add(troop);
                mAllTroopDic.Add(troop.key, troop);
            }
            int defCount = 30;
            for (int i = 0; i < defCount; i++)
            {
                TroopData troop = new TroopData();
                troop.count = 100;
                troop.isAtkTroop = false;
                troop.key = (uint)i+100;
                troop.type = (SoldierType)rand.Random(4);
                troop.x = i * 100;
                troop.y = -100;
                troop.line = 3;
                troop.row = 2;
                mDefTroopList.Add(troop);
                mAllTroopDic.Add(troop.key, troop);
            }

        }
        public void Dispose()
        {

        }
    }
}
