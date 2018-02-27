using BTreeFrame;

namespace Battle.Logic.AI.BTree
{
    public class IdleActionNode : BTreeNodeAction<BTreeInputData,BTreeOutputData>
    {
        public IdleActionNode(BTreeNode<BTreeInputData, BTreeOutputData> _parentNode) 
            : base(_parentNode)
        {
        }

        protected override void _DoEnter(BTreeInputData _input)
        {

        }

        protected override BTreeRunningStatus _DoExecute(BTreeInputData _input, ref BTreeOutputData _output)
        {
            return BTreeRunningStatus.Executing;
        }
    }
}
