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

        }
        public void Dispose()
        {

        }
    }
}
