using BTreeFrame;

namespace Battle.Logic.AI.BTree
{
    class AttackActionNode : BTreeNodeAction<MyInputData, MyOutputData>
    {
        public AttackActionNode(BTreeNode<MyInputData, MyOutputData> _parentNode) 
            : base(_parentNode)
        {
        }

        protected override BTreeRunningStatus _DoExecute(MyInputData _input, ref MyOutputData _output)
        {
            var outTroop = _output.troop;

            outTroop.state = (int)TroopAnimState.Attack;
            outTroop.inPrepose = true;
            outTroop.preTime = TroopHelper.GetTroopAtkPrepareTime(outTroop.type);
            outTroop.isAtk = true;
            outTroop.norAtkCD = TroopHelper.GetTroopAtkCDTime(outTroop.type);
            return BTreeRunningStatus.Finish;
        }
    }
}
