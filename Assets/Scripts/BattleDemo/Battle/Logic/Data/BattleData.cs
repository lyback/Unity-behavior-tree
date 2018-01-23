using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
namespace Battle.Logic
{
    public class BattleData : FSMDataMgrBase
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
            mBattleKey = "test";
            mCurrentFrame = 1;
            mFinishFrame = 10000;
            mAtcTroopList = new List<TroopData>();
            mDefTroopList = new List<TroopData>();
            mAllTroopDic = new Dictionary<uint, TroopData>();
            for (int i = 0; i < 4; i++)
            {
                TroopData troop = new TroopData();
                troop.count = 100;
                troop.isAtkTroop = true;
                troop.key = (uint)i;
                troop.type = (SoldierType)i;
                troop.x = i * 100;
                troop.y = 100;
                mAtcTroopList.Add(troop);
                mAllTroopDic.Add(troop.key, troop);
            }
            for (int i = 0; i < 4; i++)
            {
                TroopData troop = new TroopData();
                troop.count = 100;
                troop.isAtkTroop = false;
                troop.key = (uint)i+4;
                troop.type = (SoldierType)i;
                troop.x = i * 100;
                troop.y = -100;
                mDefTroopList.Add(troop);
                mAllTroopDic.Add(troop.key, troop);
            }

        }
        public void Dispose()
        {

        }
    }
}
