using BTreeFrame;

namespace Battle.Logic.AI.BTree
{
    public class IdleActionNode : BTreeNodeAction<MyInputData,MyOutputData>
    {
        public IdleActionNode(BTreeNode<MyInputData, MyOutputData> _parentNode) 
            : base(_parentNode)
        {
        }

        protected override void _DoEnter(MyInputData _input)
        {

        }

        protected override BTreeRunningStatus _DoExecute(MyInputData _input, out MyOutputData _output)
        {
            _output = null;
            return BTreeRunningStatus.Executing;
        }
    }
}
