using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
namespace Battle.Logic
{
    public class BattleDataManager : FSMDataMgrBase
    {
        public Dictionary<uint, TroopData> mAllTroopDic;
        public List<TroopData> mAtcTroopList;
        public List<TroopData> mDefTroopList;

        public void Init()
        {

        }
        public void Dispose()
        {

        }
    }
}
