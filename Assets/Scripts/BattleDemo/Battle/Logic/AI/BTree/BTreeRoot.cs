using BTreeFrame;
namespace Battle.Logic.AI.BTree
{
    public class BTreeRoot
    {
        public BTreeNode m_TreeRoot { get; private set; }

        public void CreateBevTree()
        {
            //m_TreeRoot = BTreeNodeFactory<MyInputData, MyOutputData>.CreateParallelNode(null, "Root_ParallelNode", BTreeParallelFinishCondition.AND);
            //BTreeNodeFactory<MyInputData, MyOutputData>.CreateActionNode<StartActionNode>(m_TreeRoot, "Start");
            //var troopAction = BTreeNodeFactory<MyInputData, MyOutputData>.CreatePrioritySelectorNode(m_TreeRoot, "TroopAction");
            //BTreeNodeFactory<MyInputData, MyOutputData>.CreateActionNode<FindTargetActionNode>(troopAction, "FindTarget")
            //    .SetNodePrecondition(new BTreeNodePreconditionNOT<MyInputData>(new HasTargetCondition()));
            //BTreeNodeFactory<MyInputData, MyOutputData>.CreateActionNode<AttackActionNode>(troopAction, "Attack")
            //    .SetNodePrecondition(new BTreeNodePreconditionAND<MyInputData>(new HasTargetCondition(), new IsInAttackRangeCondition()));
            //BTreeNodeFactory<MyInputData, MyOutputData>.CreateActionNode<MoveToActionNode>(troopAction, "MoveTo")
            //    .SetNodePrecondition(new HasTargetCondition());
           

            //BTreeNodeSerialization.WriteBinary(config, config.m_Name);
            //var _config = BTreeNodeSerialization.ReadBinary(config.m_Name);

            //BTreeNodeSerialization.WriteXML(config, config.m_Name);
            //var _config = BTreeNodeSerialization.ReadXML("Btree");
            var _config = BTreeNodeSerialization.ReadXML("Btree_test");
            _config = BTreeNodeSerialization.ReadBinary("Root_ParallelNode");
            
            BTreeNodeFactory.Init();
            m_TreeRoot = BTreeNodeFactory.CreateBTreeRootFromConfig(_config);
            //var _testconfig = BTreeNodeFactory<BTreeInputData, BTreeOutputData>.CreateConfigFromBTreeRoot(m_TreeRoot);
            //BTreeNodeSerialization.WriteXML(_testconfig, "Btree_test");
        }
        public void Tick(BTreeTemplateData _input, ref BTreeTemplateData _output)
        {
            if (m_TreeRoot.Evaluate(_input))
            {
                m_TreeRoot.Tick(_input, ref _output);
            }
            else
            {
                m_TreeRoot.Transition(_input);
            }
        }
    }
    
}
