using System;

namespace BTreeFrame
{
    public class BTreeNodeFactory<T, P>
        where T : BTreeTemplateData
        where P : BTreeTemplateData
    {
        public static BTreeNode<T, P> CreateParallelNode(BTreeNode<T,P> _parent, BTreeParallelFinishCondition _conditionType, string _nodeName)
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
