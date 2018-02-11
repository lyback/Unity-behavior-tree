
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
