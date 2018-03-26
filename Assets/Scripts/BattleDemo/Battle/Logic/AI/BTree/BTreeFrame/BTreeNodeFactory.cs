using Battle.Logic.AI.BTree;
using System;
using System.Collections.Generic;
namespace BTreeFrame
{
    public class BTreeNodeFactory
    {

        private static Dictionary<string, Type> _PreconditionTypeDic = null;
        public static Dictionary<string, Type> PreconditionTypeDic
        {
            get
            {
                if (_PreconditionTypeDic == null)
                {
                    Init();
                }
                return _PreconditionTypeDic;
            }
        }
        private static Dictionary<string, Type> _ActionTypeDic = null;
        public static Dictionary<string, Type> ActionTypeDic
        {
            get
            {
                if (_ActionTypeDic == null)
                {
                    Init();
                }
                return _ActionTypeDic;
            }
        }
        public static void AddActionType(Type type)
        {
            if (_ActionTypeDic.ContainsKey(type.Name))
            {
                _ActionTypeDic[type.Name] = type;
            }
            else
            {
                _ActionTypeDic.Add(type.Name, type);
            }
        }
        public static void AddPreconditionType(Type type)
        {
            if (_PreconditionTypeDic.ContainsKey(type.Name))
            {
                _PreconditionTypeDic[type.Name] = type;
            }
            else
            {
                _PreconditionTypeDic.Add(type.Name, type);
            }
        }
        public static void Init()
        {
            _ActionTypeDic = new Dictionary<string, Type>();
            _PreconditionTypeDic = new Dictionary<string, Type>();

            AddActionType(typeof(AttackActionNode));
            AddActionType(typeof(FindTargetActionNode));
            AddActionType(typeof(IdleActionNode));
            AddActionType(typeof(MoveToActionNode));
            AddActionType(typeof(StartActionNode));

            AddPreconditionType(typeof(HasTargetCondition));
            AddPreconditionType(typeof(HasReachedTargetCondition));
            AddPreconditionType(typeof(IsInAttackRangeCondition));
        }

        #region 从配置生成行为树相关方法
        public static BTreeNode CreateBTreeRootFromConfig(TreeConfig _config)
        {
            BTreeNode[] _nodes = CreateBTreeFromConfig(_config);
            return _nodes[0];
        }
        public static BTreeNode[] CreateBTreeFromConfig(TreeConfig _config)
        {
            BTreeNode[] _nodes = new BTreeNode[_config.m_Nodes.Length];
            for (int i = 0; i < _nodes.Length; i++)
            {
                _nodes[i] = null;
            }
            for (int i = 0; i < _config.m_Nodes.Length; i++)
            {
                if (_nodes[i] == null)
                {
                    _nodes[i] = CreateTreeNode(ref _nodes, _config.m_Nodes, i);
                }
                if (_config.m_Nodes[i].m_Preconditions != null)
                {
                    var precondition = CreatePreconditionFromConfig(_config.m_Nodes[i]);
                    //Debugger.Log(precondition);
                    _nodes[i].SetNodePrecondition(precondition);
                }
            }
            return _nodes;
        }
        private static BTreeNode CreateTreeNode(ref BTreeNode[] _nodes, TreeNodeConfig[] _nodeConfigs, int index)
        {
            TreeNodeConfig _nodeConfig = _nodeConfigs[index];
            if (_nodeConfig.m_ParentIndex >= 0 && _nodes[_nodeConfig.m_ParentIndex]==null)
            {
                _nodes[_nodeConfig.m_ParentIndex] = CreateTreeNode(ref _nodes, _nodeConfigs, _nodeConfig.m_ParentIndex);
            }
            BTreeNode parent = null;
            if (_nodeConfig.m_ParentIndex >= 0)
            {
                parent = _nodes[_nodeConfig.m_ParentIndex];
            }
            NodeType type = (NodeType)_nodeConfig.m_NodeType;
            BTreeNode _node = null;
            switch (type)
            {
                case NodeType.SelectorNode:
                    SelectorNodeType subType = (SelectorNodeType)_nodeConfig.m_NodeSubType;
                    _node = CreateSelectorNode(subType, parent, _nodeConfig.m_NodeName, _nodeConfig.m_OtherParams);
                    break;
                case NodeType.ActionNode:
                    _node = CreateActionNode(parent, _nodeConfig.m_NodeName, ActionTypeDic[_nodeConfig.m_ActionNodeName]);
                    break;
                default:
                    break;
            }
            _node.m_Index = index;
            return _node;
        }
        private static BTreeNode CreateSelectorNode(SelectorNodeType _subType, BTreeNode _parent, string _nodeName, params int[] _param)
        {
            BTreeNode _node = null;
            switch (_subType)
            {
                case SelectorNodeType.BTreeNodeParallel:
                    _node = CreateParallelNode(_parent, _nodeName, (BTreeParallelFinishCondition)_param[0]);
                    break;
                case SelectorNodeType.BTreeNodePrioritySelector:
                    _node = CreatePrioritySelectorNode(_parent, _nodeName);
                    break;
                case SelectorNodeType.BTreeNodeNonePrioritySelector:
                    _node = CreateNonePrioritySelectorNode(_parent, _nodeName);
                    break;
                case SelectorNodeType.BTreeNodeSequence:
                    _node = CreateSequenceNode(_parent, _nodeName);
                    break;
                case SelectorNodeType.BTreeNodeLoop:
                    _node = CreateLoopNode(_parent, _nodeName, _param[0]);
                    break;
                default:
                    break;
            }

            return _node;
        }
        private static BTreeNode CreateActionNode(BTreeNode _parent, string _nodeName, Type type)
        {
            BTreeNode node = (BTreeNode)type.GetConstructor(new Type[] { typeof(BTreeNode) }).Invoke(new object[] { _parent });
            CreateNodeCommon(node, _parent, _nodeName);
            return node;
        }
        private static BTreeNodePrecondition CreatePreconditionFromConfig(TreeNodeConfig _nodeConfig)
        {
            PreconditionConfig[] _condConfigs = _nodeConfig.m_Preconditions;
            BTreeNodePrecondition[] _nodePreconditions = new BTreeNodePrecondition[_condConfigs.Length];
            for (int i = 0; i < _nodePreconditions.Length; i++)
            {
                _nodePreconditions[i] = null;
            }
            for (int i = 0; i < _nodePreconditions.Length; i++)
            {
                if (_nodePreconditions[i] == null)
                {
                    _nodePreconditions[i] = CreatePrecondition(ref _nodePreconditions, _condConfigs, i);
                }
            }
            return _nodePreconditions[0];
        }
        private static BTreeNodePrecondition CreatePrecondition(ref BTreeNodePrecondition[] _nodePreconditions, PreconditionConfig[] _condConfigs, int _index)
        {
            int[] _childIndexs = _condConfigs[_index].m_ChildIndexs;
            int _parentIndex = _condConfigs[_index].m_ParentIndex;
            if (_childIndexs != null && _childIndexs.Length != 0)
            {
                for (int i = 0; i < _childIndexs.Length; i++)
                {
                    if (_nodePreconditions[_childIndexs[i]] == null)
                    {
                        _nodePreconditions[_childIndexs[i]] = CreatePrecondition(ref _nodePreconditions, _condConfigs, _childIndexs[i]);
                    }
                }
            }
            BTreeNodePrecondition _precondition = null;
            if (_childIndexs != null && _childIndexs.Length > 0)
            {
                PreconditionType type = (PreconditionType)_condConfigs[_index].m_Type;
                BTreeNodePrecondition[] _childNodePreconditions = new BTreeNodePrecondition[_childIndexs.Length];
                for (int i = 0; i < _childIndexs.Length; i++)
                {
                    _childNodePreconditions[i] = _nodePreconditions[_childIndexs[i]];
                }
                switch (type)
                {
                    case PreconditionType.And:
                        _precondition = new BTreeNodePreconditionAND(_childNodePreconditions);
                        break;
                    case PreconditionType.Or:
                        _precondition = new BTreeNodePreconditionOR(_childNodePreconditions);
                        break;
                    case PreconditionType.Not:
                        _precondition = new BTreeNodePreconditionNOT(_childNodePreconditions[0]);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                string typeName = _condConfigs[_index].m_PreconditionName;
                if (PreconditionTypeDic.ContainsKey(typeName))
                {
                    Type type = PreconditionTypeDic[typeName];
                    _precondition = (BTreeNodePrecondition)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
                }
            }
            
            return _precondition;
        }
        #endregion
        #region 从行为树生成配置方法
        public static TreeConfig CreateConfigFromBTreeRoot(BTreeNode _root)
        {
            TreeConfig _tree = new TreeConfig();
            _tree.m_Name = _root.m_Name;
            int _nodeCount = GetBTreeChildNodeNum(_root) + 1;
            _tree.m_Nodes = new TreeNodeConfig[_nodeCount];
            int index = 0;
            GetTreeNodeConfigFromBTreeRoot(_root, ref _tree.m_Nodes, ref index, -1);
            return _tree;
        }
        private static void GetTreeNodeConfigFromBTreeRoot(BTreeNode _root, ref TreeNodeConfig[] _treeNodeList, ref int _index, int _parentIndex)
        {
            _treeNodeList[_index] = new TreeNodeConfig();
            _treeNodeList[_index].m_NodeName = _root.m_Name;
            _treeNodeList[_index].m_ParentIndex = _parentIndex;
            bool _isAction = _root.m_IsAcitonNode;
            if (_isAction)
            {
                _treeNodeList[_index].m_NodeType = (int)NodeType.ActionNode;
            }
            else
            {
                _treeNodeList[_index].m_NodeType = (int)NodeType.SelectorNode;
            }
            _treeNodeList[_index].m_ActionNodeName = _isAction ? _root.GetType().Name : null;
            _treeNodeList[_index].m_NodeSubType = !_isAction ? (int)(Enum.Parse(typeof(SelectorNodeType), _root.GetType().Name)) : 0;
            _treeNodeList[_index].m_OtherParams = GetOtherParamsFromBTreeNode(_root, (NodeType)_treeNodeList[_index].m_NodeType, (SelectorNodeType)_treeNodeList[_index].m_NodeSubType);
            if (_root.GetNodePrecondition() != null)
            {
                int _preconditionCount = GetBTreeChildPreconditionNum(_root.GetNodePrecondition()) + 1;
                _treeNodeList[_index].m_Preconditions = new PreconditionConfig[_preconditionCount];
                int index = 0;
                GetPreconditionConfigFromBtreeNode(_root.GetNodePrecondition(), ref _treeNodeList[_index].m_Preconditions, ref index, -1);
            }
            int parentIndex = _index;
            for (int i = 0; i < _root.m_ChildCount; i++)
            {
                _index = _index + 1;
                GetTreeNodeConfigFromBTreeRoot(_root.m_ChildNodeList[i], ref _treeNodeList, ref _index, parentIndex);
            }
        }
        private static void GetPreconditionConfigFromBtreeNode(BTreeNodePrecondition _precondition, ref PreconditionConfig[] _preconditionList, ref int _index, int _parentIndex = -1)
        {
            _preconditionList[_index] = new PreconditionConfig();
            _preconditionList[_index].m_ParentIndex = _parentIndex;
            Type type = _precondition.GetType();
            _preconditionList[_index].m_PreconditionName = type.Name.Split('`')[0];
            if (type.Equals(typeof(BTreeNodePreconditionAND)))
            {
                _preconditionList[_index].m_Type = (int)PreconditionType.And;
                BTreeNodePrecondition[] _childPreconditon = ((BTreeNodePreconditionAND)_precondition).GetChildPrecondition();
                _preconditionList[_index].m_ChildIndexs = new int[_childPreconditon.Length];
                int parentIndex = _index;
                for (int i = 0; i < _childPreconditon.Length; i++)
                {
                    _index = _index + 1;
                    _preconditionList[parentIndex].m_ChildIndexs[i] = _index;
                    GetPreconditionConfigFromBtreeNode(_childPreconditon[i], ref _preconditionList, ref _index, parentIndex);
                }
            }
            else if (type.Equals(typeof(BTreeNodePreconditionOR)))
            {
                _preconditionList[_index].m_Type = (int)PreconditionType.Or;
                BTreeNodePrecondition[] _childPreconditon = ((BTreeNodePreconditionOR)_precondition).GetChildPrecondition();
                _preconditionList[_index].m_ChildIndexs = new int[_childPreconditon.Length];
                int parentIndex = _index;
                for (int i = 0; i < _childPreconditon.Length; i++)
                {
                    _index = _index + 1;
                    _preconditionList[parentIndex].m_ChildIndexs[i] = _index;
                    GetPreconditionConfigFromBtreeNode(_childPreconditon[i], ref _preconditionList, ref _index, parentIndex);
                }
            }
            else if (type.Equals(typeof(BTreeNodePreconditionNOT)))
            {
                _preconditionList[_index].m_Type = (int)PreconditionType.Not;
                BTreeNodePrecondition _childPreconditon = ((BTreeNodePreconditionNOT)_precondition).GetChildPrecondition();
                _preconditionList[_index].m_ChildIndexs = new int[1];
                _preconditionList[_index].m_ChildIndexs[0] = _index + 1;
                int parentIndex = _index;
                _index = _index + 1;
                GetPreconditionConfigFromBtreeNode(_childPreconditon, ref _preconditionList, ref _index, parentIndex);
            }
        }
        private static int GetBTreeChildPreconditionNum(BTreeNodePrecondition _precondition)
        {
            if (_precondition == null)
            {
                return 0;
            }
            int _count = 0;
            Type type = _precondition.GetType();
            if (type.Equals(typeof(BTreeNodePreconditionAND)))
            {
                _count += ((BTreeNodePreconditionAND)_precondition).GetChildPreconditionCount();
                BTreeNodePrecondition[] _chlidList = ((BTreeNodePreconditionAND)_precondition).GetChildPrecondition();
                if (_chlidList != null)
                {
                    for (int i = 0; i < _chlidList.Length; i++)
                    {
                        _count += GetBTreeChildPreconditionNum(_chlidList[i]);
                    }
                }
            }
            else if (type.Equals(typeof(BTreeNodePreconditionOR)))
            {
                _count += ((BTreeNodePreconditionOR)_precondition).GetChildPreconditionCount();
                BTreeNodePrecondition[] _chlidList = ((BTreeNodePreconditionOR)_precondition).GetChildPrecondition();
                if (_chlidList != null)
                {
                    for (int i = 0; i < _chlidList.Length; i++)
                    {
                        _count += GetBTreeChildPreconditionNum(_chlidList[i]);
                    }
                }
            }
            else if (type.Equals(typeof(BTreeNodePreconditionNOT)))
            {
                _count += 1;
                _count += GetBTreeChildPreconditionNum(((BTreeNodePreconditionNOT
                    )_precondition).GetChildPrecondition());
            }
            return _count;
        }
        private static int[] GetOtherParamsFromBTreeNode(BTreeNode _node, NodeType _nodeType, SelectorNodeType _subType)
        {
            int[] _otherParams = null;
            switch (_nodeType)
            {
                case NodeType.SelectorNode:
                    switch (_subType)
                    {
                        case SelectorNodeType.BTreeNodeParallel:
                            _otherParams = new int[1];
                            _otherParams[0] = (int)((BTreeNodeParallel)_node).m_FinishCondition;
                            break;
                        case SelectorNodeType.BTreeNodeLoop:
                            _otherParams = new int[1];
                            _otherParams[0] = ((BTreeNodeLoop)_node).GetLoopCount();
                            break;
                        default:
                            break;
                    }
                    break;
                case NodeType.ActionNode:
                    _otherParams = null;
                    break;
                default:
                    break;
            }
            return _otherParams;
        }
        private static int GetBTreeChildNodeNum(BTreeNode _root)
        {
            int _count = _root.m_ChildCount;
            for (int i = 0; i < _root.m_ChildNodeList.Count; i++)
            {
                if (_root.m_ChildNodeList[i].m_ChildCount != 0)
                {
                    _count += GetBTreeChildNodeNum(_root.m_ChildNodeList[i]);
                }
            }
            return _count;
        }
        #endregion

        public static BTreeNode CreateParallelNode(BTreeNode _parent, string _nodeName, BTreeParallelFinishCondition _conditionType)
        {
            BTreeNodeParallel node = new BTreeNodeParallel(_parent);
            node.m_FinishCondition = _conditionType;
            CreateNodeCommon(node, _parent, _nodeName);
            return node;
        }

        public static BTreeNode CreatePrioritySelectorNode(BTreeNode _parent, string _nodeName)
        {
            BTreeNodePrioritySelector node = new BTreeNodePrioritySelector(_parent);
            CreateNodeCommon(node, _parent, _nodeName);
            return node;
        }

        public static BTreeNode CreateNonePrioritySelectorNode(BTreeNode _parent, string _nodeName)
        {
            BTreeNodeNonePrioritySelector node = new BTreeNodeNonePrioritySelector(_parent);
            CreateNodeCommon(node, _parent, _nodeName);
            return node;
        }

        public static BTreeNode CreateSequenceNode(BTreeNode _parent, string _nodeName)
        {
            BTreeNodeSequence node = new BTreeNodeSequence(_parent);
            CreateNodeCommon(node, _parent, _nodeName);
            return node;
        }

        public static BTreeNode CreateLoopNode(BTreeNode _parent, string _nodeName, int _loopCount)
        {
            BTreeNodeLoop node = new BTreeNodeLoop(_parent, null, _loopCount);
            CreateNodeCommon(node, _parent, _nodeName);
            return node;
        }
        
        public static BTreeNode CreateActionNode<F>(BTreeNode _parent, string _nodeName) where F: BTreeNodeAction
        {
            F node = (F)Activator.CreateInstance(typeof(F), _parent);
            CreateNodeCommon(node, _parent, _nodeName);
            return node;
        }
        private static void CreateNodeCommon(BTreeNode _node, BTreeNode _parent, string _nodeName)
        {
            if (_parent != null)
            {
                _parent.AddChildNode(_node);
            }
            _node.m_Name = _nodeName;
        }
    }
    
    
}
