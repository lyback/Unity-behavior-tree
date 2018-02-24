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
                case SelectorNodeType.Parallel:
                    _node = CreateParallelNode(_parent, _nodeName, (BTreeParallelFinishCondition)_param[0]);
                    break;
                case SelectorNodeType.PrioritySelector:
                    _node = CreatePrioritySelectorNode(_parent, _nodeName);
                    break;
                case SelectorNodeType.NonePrioritySelector:
                    _node = CreateNonePrioritySelectorNode(_parent, _nodeName);
                    break;
                case SelectorNodeType.Sequence:
                    _node = CreateSequenceNode(_parent, _nodeName);
                    break;
                case SelectorNodeType.Loop:
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
    [System.Serializable]
    public class TreeConfig
    {
        public string m_Name;
        public TreeNodeConfig[] m_Nodes;
    }
    [System.Serializable]
    public class TreeNodeConfig
    {
        public int m_ParentIndex = -1;
        public int m_NodeType;
        public int m_NodeSubType;
        public int[] m_OtherParams;
        public string m_ActionNodeName;
        public string m_NodeName;
        public PreconditionConfig[] m_Preconditions;
    }
    [System.Serializable]
    public class PreconditionConfig
    {
        public int m_ParentIndex = -1;
        public int m_Type;
        public string m_PreconditionName;
        public int[] m_ChildIndexs;
    }

    public enum NodeType
    {
        SelectorNode = 1,
        ActionNode = 2,
    }
    public enum SelectorNodeType
    {
        Parallel = 1,
        PrioritySelector = 2,
        NonePrioritySelector = 3,
        Sequence = 4,
        Loop = 5
    }
    public enum PreconditionType
    {
        And = 1,
        Or = 2,
        Not = 3,
    }
}
