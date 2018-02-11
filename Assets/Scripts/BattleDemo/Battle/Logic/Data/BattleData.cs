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
            RandHelper rand = new RandHelper(1);
            mBattleKey = "test";
            mCurrentFrame = 1;
            mFinishFrame = 10000;
            mAtcTroopList = new List<TroopData>();
            mDefTroopList = new List<TroopData>();
            mAllTroopDic = new Dictionary<uint, TroopData>();
            int atkCount = 4;
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
            int defCount = 3;
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
