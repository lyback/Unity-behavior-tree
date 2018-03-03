using System;
using System.Collections.Generic;
namespace BTreeFrame
{
    public class BTreeNodeFactory<T, P>
        where T : BTreeTemplateData
        where P : BTreeTemplateData
    {

        private static Dictionary<string, Type> PreconditionTypeDic = new Dictionary<string, Type>();
        private static Dictionary<string, Type> ActionTypeDic = new Dictionary<string, Type>();

        public static void AddActionType(Type type)
        {
            if (ActionTypeDic.ContainsKey(type.Name))
            {
                ActionTypeDic[type.Name] = type;
            }
            else
            {
                ActionTypeDic.Add(type.Name, type);
            }
        }
        public static void AddPreconditionType(Type type)
        {
            if (PreconditionTypeDic.ContainsKey(type.Name))
            {
                PreconditionTypeDic[type.Name] = type;
            }
            else
            {
                PreconditionTypeDic.Add(type.Name, type);
            }
        }

        #region 从配置生成行为树相关方法
        public static BTreeNode<T, P> CreateBTreeFromConfig(TreeConfig _config)
        {
            BTreeNode<T, P>[] _nodes = new BTreeNode<T, P>[_config.m_Nodes.Length];
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
            return _nodes[0];    
        }
        private static BTreeNode<T, P> CreateTreeNode(ref BTreeNode<T, P>[] _nodes, TreeNodeConfig[] _nodeConfigs, int index)
        {
            TreeNodeConfig _nodeConfig = _nodeConfigs[index];
            if (_nodeConfig.m_ParentIndex >= 0 && _nodes[_nodeConfig.m_ParentIndex]==null)
            {
                _nodes[_nodeConfig.m_ParentIndex] = CreateTreeNode(ref _nodes, _nodeConfigs, _nodeConfig.m_ParentIndex);
            }
            BTreeNode<T, P> parent = null;
            if (_nodeConfig.m_ParentIndex >= 0)
            {
                parent = _nodes[_nodeConfig.m_ParentIndex];
            }
            NodeType type = (NodeType)_nodeConfig.m_NodeType;
            BTreeNode<T, P> _node = null;
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
            return _node;
        }
        private static BTreeNode<T, P> CreateSelectorNode(SelectorNodeType _subType, BTreeNode<T, P> _parent, string _nodeName, params int[] _param)
        {
            BTreeNode<T, P> _node = null;
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
        private static BTreeNode<T, P> CreateActionNode(BTreeNode<T, P> _parent, string _nodeName, Type type)
        {
            BTreeNode<T, P> node = (BTreeNode<T, P>)type.GetConstructor(new Type[] { typeof(BTreeNode<T, P>) }).Invoke(new object[] { _parent });
            CreateNodeCommon(node, _parent, _nodeName);
            return node;
        }
        private static BTreeNodePrecondition<T> CreatePreconditionFromConfig(TreeNodeConfig _nodeConfig)
        {
            PreconditionConfig[] _condConfigs = _nodeConfig.m_Preconditions;
            BTreeNodePrecondition<T>[] _nodePreconditions = new BTreeNodePrecondition<T>[_condConfigs.Length];
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
        private static BTreeNodePrecondition<T> CreatePrecondition(ref BTreeNodePrecondition<T>[] _nodePreconditions, PreconditionConfig[] _condConfigs, int _index)
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
            BTreeNodePrecondition<T> _precondition = null;
            if (_childIndexs != null && _childIndexs.Length > 0)
            {
                PreconditionType type = (PreconditionType)_condConfigs[_index].m_Type;
                BTreeNodePrecondition<T>[] _childNodePreconditions = new BTreeNodePrecondition<T>[_childIndexs.Length];
                for (int i = 0; i < _childIndexs.Length; i++)
                {
                    _childNodePreconditions[i] = _nodePreconditions[_childIndexs[i]];
                }
                switch (type)
                {
                    case PreconditionType.And:
                        _precondition = new BTreeNodePreconditionAND<T>(_childNodePreconditions);
                        break;
                    case PreconditionType.Or:
                        _precondition = new BTreeNodePreconditionOR<T>(_childNodePreconditions);
                        break;
                    case PreconditionType.Not:
                        _precondition = new BTreeNodePreconditionNOT<T>(_childNodePreconditions[0]);
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
                    _precondition = (BTreeNodePrecondition<T>)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
                }
            }
            
            return _precondition;
        }
        #endregion
        #region 从行为树生成配置方法
        public static TreeConfig CreateConfigFromBTreeRoot(BTreeNode<T, P> _root)
        {
            TreeConfig _tree = new TreeConfig();
            _tree.m_Name = _root.m_Name;
            int _nodeCount = GetBTreeChildNodeNum(_root) + 1;
            _tree.m_Nodes = new TreeNodeConfig[_nodeCount];
            GetTreeNodeConfigFromBTreeRoot(_root, ref _tree.m_Nodes, 0, -1);
            return _tree;
        }
        private static void GetTreeNodeConfigFromBTreeRoot(BTreeNode<T, P> _root, ref TreeNodeConfig[] _treeNodeList, int _index, int _parentIndex)
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
            _treeNodeList[_index].m_NodeSubType = !_isAction ? (int)(Enum.Parse(typeof(SelectorNodeType), _root.GetType().Name.Split('`')[0])) : 0;
            _treeNodeList[_index].m_OtherParams = GetOtherParamsFromBTreeNode(_root, (NodeType)_treeNodeList[_index].m_NodeType, (SelectorNodeType)_treeNodeList[_index].m_NodeSubType);
            if (_root.GetNodePrecondition() != null)
            {
                int _preconditionCount = GetBTreeChildPreconditionNum(_root.GetNodePrecondition()) + 1;
                _treeNodeList[_index].m_Preconditions = new PreconditionConfig[_preconditionCount];
                GetPreconditionConfigFromBtreeNode(_root.GetNodePrecondition(), ref _treeNodeList[_index].m_Preconditions, 0, -1);
            }
            for (int i = 0; i < _root.m_ChildCount; i++)
            {
                int _childIndex = _index + i + 1;
                GetTreeNodeConfigFromBTreeRoot(_root.m_ChildNodeList[i], ref _treeNodeList, _childIndex, _index);
            }
        }
        private static void GetPreconditionConfigFromBtreeNode(BTreeNodePrecondition<T> _precondition, ref PreconditionConfig[] _preconditionList, int _index, int _parentIndex = -1)
        {
            _preconditionList[_index] = new PreconditionConfig();
            _preconditionList[_index].m_ParentIndex = _parentIndex;
            Type type = _precondition.GetType();
            _preconditionList[_index].m_PreconditionName = type.Name.Split('`')[0];
            if (type.Equals(typeof(BTreeNodePreconditionAND<T>)))
            {
                _preconditionList[_index].m_Type = (int)PreconditionType.And;
                BTreeNodePrecondition<T>[] _childPreconditon = ((BTreeNodePreconditionAND<T>)_precondition).GetChildPrecondition();
                _preconditionList[_index].m_ChildIndexs = new int[_childPreconditon.Length];
                for (int i = 0; i < _childPreconditon.Length; i++)
                {
                    int _childIndex = _index + i + 1;
                    _preconditionList[_index].m_ChildIndexs[i] = _childIndex;
                    GetPreconditionConfigFromBtreeNode(_childPreconditon[i], ref _preconditionList, _childIndex, _index);
                }
            }
            else if (type.Equals(typeof(BTreeNodePreconditionOR<T>)))
            {
                _preconditionList[_index].m_Type = (int)PreconditionType.Or;
                BTreeNodePrecondition<T>[] _childPreconditon = ((BTreeNodePreconditionOR<T>)_precondition).GetChildPrecondition();
                _preconditionList[_index].m_ChildIndexs = new int[_childPreconditon.Length];
                for (int i = 0; i < _childPreconditon.Length; i++)
                {
                    int _childIndex = _index + i + 1;
                    _preconditionList[_index].m_ChildIndexs[i] = _childIndex;
                    GetPreconditionConfigFromBtreeNode(_childPreconditon[i], ref _preconditionList, _childIndex, _index);
                }
            }
            else if (type.Equals(typeof(BTreeNodePreconditionNOT<T>)))
            {
                _preconditionList[_index].m_Type = (int)PreconditionType.Not;
                BTreeNodePrecondition<T> _childPreconditon = ((BTreeNodePreconditionNOT<T>)_precondition).GetChildPrecondition();
                _preconditionList[_index].m_ChildIndexs = new int[1];
                int _childIndex = _index + 1;
                _preconditionList[_index].m_ChildIndexs[0] = _childIndex;
                GetPreconditionConfigFromBtreeNode(_childPreconditon, ref _preconditionList, _childIndex, _index);
            }
        }
        private static int GetBTreeChildPreconditionNum(BTreeNodePrecondition<T> _precondition)
        {
            if (_precondition == null)
            {
                return 0;
            }
            int _count = 0;
            Type type = _precondition.GetType();
            if (type.Equals(typeof(BTreeNodePreconditionAND<T>)))
            {
                _count += ((BTreeNodePreconditionAND<T>)_precondition).GetChildPreconditionCount();
                BTreeNodePrecondition<T>[] _chlidList = ((BTreeNodePreconditionAND<T>)_precondition).GetChildPrecondition();
                if (_chlidList != null)
                {
                    for (int i = 0; i < _chlidList.Length; i++)
                    {
                        _count += GetBTreeChildPreconditionNum(_chlidList[i]);
                    }
                }
            }
            else if (type.Equals(typeof(BTreeNodePreconditionOR<T>)))
            {
                _count += ((BTreeNodePreconditionOR<T>)_precondition).GetChildPreconditionCount();
                BTreeNodePrecondition<T>[] _chlidList = ((BTreeNodePreconditionOR<T>)_precondition).GetChildPrecondition();
                if (_chlidList != null)
                {
                    for (int i = 0; i < _chlidList.Length; i++)
                    {
                        _count += GetBTreeChildPreconditionNum(_chlidList[i]);
                    }
                }
            }
            else if (type.Equals(typeof(BTreeNodePreconditionNOT<T>)))
            {
                _count += 1;
                _count += GetBTreeChildPreconditionNum(((BTreeNodePreconditionNOT<T>)_precondition).GetChildPrecondition());
            }
            return _count;
        }
        private static int[] GetOtherParamsFromBTreeNode(BTreeNode<T, P> _node, NodeType _nodeType, SelectorNodeType _subType)
        {
            int[] _otherParams = null;
            switch (_nodeType)
            {
                case NodeType.SelectorNode:
                    switch (_subType)
                    {
                        case SelectorNodeType.BTreeNodeParallel:
                            _otherParams = new int[1];
                            _otherParams[0] = (int)((BTreeNodeParallel<T, P>)_node).GetFinishCondition();
                            break;
                        case SelectorNodeType.BTreeNodeLoop:
                            _otherParams = new int[1];
                            _otherParams[0] = ((BTreeNodeLoop<T, P>)_node).GetLoopCount();
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
        private static int GetBTreeChildNodeNum(BTreeNode<T, P> _root)
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

        public static BTreeNode<T, P> CreateParallelNode(BTreeNode<T,P> _parent, string _nodeName, BTreeParallelFinishCondition _conditionType)
        {
            BTreeNodeParallel<T, P> node = new BTreeNodeParallel<T, P>(_parent);
            node.SetFinishCondition(_conditionType);
            CreateNodeCommon(node, _parent, _nodeName);
            return node;
        }

        public static BTreeNode<T, P> CreatePrioritySelectorNode(BTreeNode<T, P> _parent, string _nodeName)
        {
            BTreeNodePrioritySelector<T, P> node = new BTreeNodePrioritySelector<T, P>(_parent);
            CreateNodeCommon(node, _parent, _nodeName);
            return node;
        }

        public static BTreeNode<T, P> CreateNonePrioritySelectorNode(BTreeNode<T, P> _parent, string _nodeName)
        {
            BTreeNodeNonePrioritySelector<T, P> node = new BTreeNodeNonePrioritySelector<T, P>(_parent);
            CreateNodeCommon(node, _parent, _nodeName);
            return node;
        }

        public static BTreeNode<T, P> CreateSequenceNode(BTreeNode<T, P> _parent, string _nodeName)
        {
            BTreeNodeSequence<T, P> node = new BTreeNodeSequence<T, P>(_parent);
            CreateNodeCommon(node, _parent, _nodeName);
            return node;
        }

        public static BTreeNode<T, P> CreateLoopNode(BTreeNode<T, P> _parent, string _nodeName, int _loopCount)
        {
            BTreeNodeLoop<T, P> node = new BTreeNodeLoop<T, P>(_parent, null, _loopCount);
            CreateNodeCommon(node, _parent, _nodeName);
            return node;
        }
        
        public static BTreeNode<T, P> CreateActionNode<F>(BTreeNode<T, P> _parent, string _nodeName) where F: BTreeNodeAction<T, P>
        {
            F node = (F)Activator.CreateInstance(typeof(F), _parent);
            CreateNodeCommon(node, _parent, _nodeName);
            return node;
        }
        private static void CreateNodeCommon(BTreeNode<T, P> _node, BTreeNode<T, P> _parent, string _nodeName)
        {
            if (_parent != null)
            {
                _parent.AddChildNode(_node);
            }
            _node.m_Name = _nodeName;
        }
    }
    
    
}
