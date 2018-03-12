using BTreeFrame;

namespace Battle.Logic.AI.BTree
{
    public class StartActionNode : BTreeNodeAction
    {
        public StartActionNode()
            : base()
        {

        }
        public StartActionNode(BTreeNode _parentNode)
            : base(_parentNode)
        {

        }

        protected override BTreeRunningStatus _DoExecute(BTreeTemplateData input, ref BTreeTemplateData output)
        {
            BTreeInputData _input = input as BTreeInputData;
            BTreeOutputData _output = output as BTreeOutputData;
            if (_input == null || _output == null)
            {
                Debugger.LogError("数据类型错误");
            }

            var troop = _input.troop;
            var outTroop = _output.troop;

            if (troop.count == 0)
            {
                outTroop.state = (int)TroopAnimState.Die;
                return BTreeRunningStatus.Executing;
            }
            if (troop.inPrepose)
            {
                if (troop.preTime > 0)
                {
                    outTroop.preTime = troop.preTime - 1;
                }
                else
                {
                    outTroop.inPrepose = false;
                }
                outTroop.state = (int)TroopAnimState.Idle;
                return BTreeRunningStatus.Executing;
            }
            if (troop.norAtkCD > 0)
            {
                outTroop.norAtkCD = troop.norAtkCD - 1;
                outTroop.state = (int)TroopAnimState.Idle;
                return BTreeRunningStatus.Executing;
            }
            outTroop.state = (int)TroopAnimState.Idle;
            return BTreeRunningStatus.Finish;
        }
    }
}
