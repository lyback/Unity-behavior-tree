using BTreeFrame;
namespace Battle.Logic.AI.BTree
{
    public class BTreeRoot
    {
        public BTreeNode<MyInputData, MyOutputData> m_TreeRoot { get; private set; }

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

            BTreeNodeFactory<MyInputData, MyOutputData>.AddActionType(typeof(AttackActionNode));
            BTreeNodeFactory<MyInputData, MyOutputData>.AddActionType(typeof(FindTargetActionNode));
            BTreeNodeFactory<MyInputData, MyOutputData>.AddActionType(typeof(IdleActionNode));
            BTreeNodeFactory<MyInputData, MyOutputData>.AddActionType(typeof(MoveToActionNode));
            BTreeNodeFactory<MyInputData, MyOutputData>.AddActionType(typeof(StartActionNode));

            BTreeNodeFactory<MyInputData, MyOutputData>.AddPreconditionType(typeof(HasTargetCondition));
            BTreeNodeFactory<MyInputData, MyOutputData>.AddPreconditionType(typeof(HasReachedTargetCondition));
            BTreeNodeFactory<MyInputData, MyOutputData>.AddPreconditionType(typeof(IsInAttackRangeCondition));

            //TreeConfig config = new TreeConfig();
            //config.m_Name = "Btree";
            //config.m_Nodes = new TreeNodeConfig[6];
            //config.m_Nodes[0] = new TreeNodeConfig();
            //config.m_Nodes[0].m_NodeType = (int)NodeType.SelectorNode;
            //config.m_Nodes[0].m_NodeSubType = (int)SelectorNodeType.Parallel;
            //config.m_Nodes[0].m_NodeName = "Root_ParallelNode";
            //config.m_Nodes[0].m_OtherParams = new int[] { (int)BTreeParallelFinishCondition.AND };

            //config.m_Nodes[1] = new TreeNodeConfig();
            //config.m_Nodes[1].m_NodeType = (int)NodeType.ActionNode;
            //config.m_Nodes[1].m_ActionNodeName = typeof(StartActionNode).Name;
            //config.m_Nodes[1].m_NodeName = "Start";
            //config.m_Nodes[1].m_ParentIndex = 0;

            //config.m_Nodes[2] = new TreeNodeConfig();
            //config.m_Nodes[2].m_NodeType = (int)NodeType.SelectorNode;
            //config.m_Nodes[2].m_NodeSubType = (int)SelectorNodeType.PrioritySelector;
            //config.m_Nodes[2].m_NodeName = "TroopAction";
            //config.m_Nodes[2].m_ParentIndex = 0;



            //config.m_Nodes[3] = new TreeNodeConfig();
            //config.m_Nodes[3].m_NodeType = (int)NodeType.ActionNode;
            //config.m_Nodes[3].m_ActionNodeName = typeof(FindTargetActionNode).Name;
            //config.m_Nodes[3].m_NodeName = "FindTarget";
            //config.m_Nodes[3].m_ParentIndex = 2;
            //config.m_Nodes[3].m_Preconditions = new PreconditionConfig[2];
            //config.m_Nodes[3].m_Preconditions[0] = new PreconditionConfig();
            //config.m_Nodes[3].m_Preconditions[0].m_ParentIndex = -1;
            //config.m_Nodes[3].m_Preconditions[0].m_Type = (int)PreconditionType.Not;
            //config.m_Nodes[3].m_Preconditions[0].m_ChildIndexs = new int[] { 1 };
            //config.m_Nodes[3].m_Preconditions[1] = new PreconditionConfig();
            //config.m_Nodes[3].m_Preconditions[1].m_ParentIndex = 0;
            //config.m_Nodes[3].m_Preconditions[1].m_Type = -1;
            //config.m_Nodes[3].m_Preconditions[1].m_PreconditionName = "HasTargetCondition";
            //config.m_Nodes[3].m_Preconditions[1].m_ChildIndexs = null;

            //config.m_Nodes[4] = new TreeNodeConfig();
            //config.m_Nodes[4].m_NodeType = (int)NodeType.ActionNode;
            //config.m_Nodes[4].m_ActionNodeName = typeof(AttackActionNode).Name;
            //config.m_Nodes[4].m_NodeName = "Attack";
            //config.m_Nodes[4].m_ParentIndex = 2;
            //config.m_Nodes[4].m_Preconditions = new PreconditionConfig[3];
            //config.m_Nodes[4].m_Preconditions[0] = new PreconditionConfig();
            //config.m_Nodes[4].m_Preconditions[0].m_ParentIndex = -1;
            //config.m_Nodes[4].m_Preconditions[0].m_Type = (int)PreconditionType.And;
            //config.m_Nodes[4].m_Preconditions[0].m_ChildIndexs = new int[] { 1, 2 };
            //config.m_Nodes[4].m_Preconditions[1] = new PreconditionConfig();
            //config.m_Nodes[4].m_Preconditions[1].m_ParentIndex = 0;
            //config.m_Nodes[4].m_Preconditions[1].m_Type = -1;
            //config.m_Nodes[4].m_Preconditions[1].m_PreconditionName = "HasTargetCondition";
            //config.m_Nodes[4].m_Preconditions[1].m_ChildIndexs = null;
            //config.m_Nodes[4].m_Preconditions[2] = new PreconditionConfig();
            //config.m_Nodes[4].m_Preconditions[2].m_ParentIndex = 0;
            //config.m_Nodes[4].m_Preconditions[2].m_Type = -1;
            //config.m_Nodes[4].m_Preconditions[2].m_PreconditionName = "IsInAttackRangeCondition";
            //config.m_Nodes[4].m_Preconditions[2].m_ChildIndexs = null;

            //config.m_Nodes[5] = new TreeNodeConfig();
            //config.m_Nodes[5].m_NodeType = (int)NodeType.ActionNode;
            //config.m_Nodes[5].m_ActionNodeName = typeof(MoveToActionNode).Name;
            //config.m_Nodes[5].m_NodeName = "MoveTo";
            //config.m_Nodes[5].m_ParentIndex = 2;
            //config.m_Nodes[5].m_Preconditions = new PreconditionConfig[1];
            //config.m_Nodes[5].m_Preconditions[0] = new PreconditionConfig();
            //config.m_Nodes[5].m_Preconditions[0].m_ParentIndex = -1;
            //config.m_Nodes[5].m_Preconditions[0].m_Type = -1;
            //config.m_Nodes[5].m_Preconditions[0].m_PreconditionName = "HasTargetCondition";
            //config.m_Nodes[5].m_Preconditions[0].m_ChildIndexs = null;

            //BTreeNodeSerialization.WriteBinary(config, config.m_Name);
            //var _config = BTreeNodeSerialization.ReadBinary(config.m_Name);

            //BTreeNodeSerialization.WriteXML(config, config.m_Name);
            var _config = BTreeNodeSerialization.ReadXML("Btree");

            m_TreeRoot = BTreeNodeFactory<MyInputData, MyOutputData>.CreateBTreeFromConfig(_config);
        }
        public void Tick(MyInputData _input, ref MyOutputData _output)
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
