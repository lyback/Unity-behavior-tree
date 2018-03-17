
namespace BTreeFrame
{
    /// <summary>
    /// class:      不带优先级的选择节点
    /// Evaluate:   先调用上一个运行的子节点（若存在）的Evaluate方法，如果可以运行，则继续运保存该节点的索引，返回True，如果不能运行，则重新选择（同带优先级的选择节点的选择方式）
    /// Tick:       调用可以运行的子节点的Tick方法，用它所返回的运行状态作为自身的运行状态返回
    /// </summary>
    public class BTreeNodeNonePrioritySelector : BTreeNodePrioritySelector
    {
        public BTreeNodeNonePrioritySelector()
            : base()
        {
        }
        public BTreeNodeNonePrioritySelector(BTreeNode _parentNode, BTreeNodePrecondition _precondition = null)
            : base(_parentNode, _precondition)
        {
        }
        protected override bool _DoEvaluate(BTreeTemplateData _input)
        {
            if (_CheckIndex(m_CurrentSelectIndex))
            {
                BTreeNode bn = m_ChildNodes[m_CurrentSelectIndex];
                if (bn.Evaluate(_input))
                {
                    return true;
                }
            }
            return base._DoEvaluate(_input);
        }
    }
}
