using BTreeFrame;

namespace Battle.Logic.AI.BTree
{
    public class IdleActionNode : BTreeNodeAction
    {
        public IdleActionNode()
            : base()
        {
        }
        public IdleActionNode(BTreeNode _parentNode) 
            : base(_parentNode)
        {
        }

        protected override void _DoEnter(BTreeTemplateData _input)
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

            return BTreeRunningStatus.Executing;
        }
    }
}
