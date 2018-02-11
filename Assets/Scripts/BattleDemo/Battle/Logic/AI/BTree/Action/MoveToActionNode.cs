using BTreeFrame;

namespace Battle.Logic.AI.BTree
{
    public class MoveToActionNode : BTreeNodeAction<MyInputData, MyOutputData>
    {
        public MoveToActionNode(BTreeNode<MyInputData, MyOutputData> _parentNode) 
            : base(_parentNode)
        {
        }

        //protected override void _DoEnter(BTreeInputData _input)
        //{

        //}

        //protected override BTreeRunningStatus _DoExecute(BTreeInputData _input, out BTreeOutputData _output)
        //{
            
        //}
    }
}
