using BTreeFrame;

namespace Battle.Logic.AI.BTree
{
    public class MyInputData : BTreeTemplateData
    {
        public TroopData troop;
        public BattleData battleData;
        public MyInputData()
        {
        }
        public void SetData(TroopData _troop, BattleData _battleData)
        {
            troop = _troop;
            battleData = _battleData;
        }
    }
    public class MyOutputData : BTreeTemplateData
    {
        public TroopData troop;
        public MyOutputData()
        {
        }
        public void SetData(MyInputData _input)
        {
            troop = _input.troop;
        }
    }
}
