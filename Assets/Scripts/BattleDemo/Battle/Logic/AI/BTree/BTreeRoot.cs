using BTreeFrame;

namespace Battle.Logic.AI.BTree
{
    public class BTreeRoot
    {
        public BTreeNode<MyInputData, MyOutputData> m_TreeRoot { get; private set; }

        public void CreateBevTree()
        {
            m_TreeRoot = BTreeNodeFactory<MyInputData, MyOutputData>.CreateParallelNode(null, BTreeParallelFinishCondition.AND, "Root_ParallelNode");
                BTreeNodeFactory<MyInputData, MyOutputData>.CreateActionNode<StartActionNode>(m_TreeRoot, "Start");
                var troopAction = BTreeNodeFactory<MyInputData, MyOutputData>.CreatePrioritySelectorNode(m_TreeRoot, "TroopAction");
                    BTreeNodeFactory<MyInputData, MyOutputData>.CreateActionNode<FindTargetActionNode>(troopAction, "FindTarget")
                        .SetNodePrecondition(new BTreeNodePreconditionNOT<MyInputData>(new HasTargetCondition()));
                    BTreeNodeFactory<MyInputData, MyOutputData>.CreateActionNode<AttackActionNode>(troopAction, "Attack")
                        .SetNodePrecondition(new BTreeNodePreconditionAND<MyInputData>(new HasTargetCondition(),new IsInAttackRangeCondition()));
                    BTreeNodeFactory<MyInputData, MyOutputData>.CreateActionNode<MoveToActionNode>(troopAction, "MoveTo")
                        .SetNodePrecondition(new HasTargetCondition());
        }
        public void Tick(MyInputData _input, ref MyOutputData _output)
        {
            if (m_TreeRoot.Evaluate(_input))
            {
                m_TreeRoot.Tick(_input, ref _output);
            }
        }
    }
}
