using BTreeFrame;

namespace Battle.Logic.AI.BTree
{
    public class BTreeInputData : BTreeTemplateData
    {
        public TroopData troop;
        public BattleData battleData;
        public BTreeInputData()
        {
        }
        public void SetData(TroopData _troop, BattleData _battleData)
        {
            troop = _troop;
            battleData = _battleData;
        }
    }
    public class BTreeOutputData : BTreeTemplateData
    {
        public TroopData troop;
        public BTreeOutputData()
        {
        }
        public void SetData(BTreeInputData _input)
        {
            troop = _input.troop;
        }
    }
}
