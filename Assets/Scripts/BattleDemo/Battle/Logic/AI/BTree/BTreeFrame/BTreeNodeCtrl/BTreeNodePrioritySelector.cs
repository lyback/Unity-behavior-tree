
namespace BTreeFrame
{
    /// <summary>
    /// class:      带优先级的选择节点
    /// Evaluate:   从第一个子节点开始依次遍历所有的子节点，调用其Evaluate方法，当发现存在可以运行的子节点时，记录子节点索引，停止遍历，返回True。
    /// Tick:       调用可以运行的子节点的Tick方法，用它所返回的运行状态作为自身的运行状态返回
    /// </summary>
    public class BTreeNodePrioritySelector : BTreeNodeCtrl
    {
        protected int m_CurrentSelectIndex = INVALID_CHILD_NODE_INDEX;
        protected int m_LastSelectIndex = INVALID_CHILD_NODE_INDEX;
        public BTreeNodePrioritySelector()
            : base()
        {
        }
        public BTreeNodePrioritySelector(BTreeNode _parentNode, BTreeNodePrecondition _precondition = null)
            : base(_parentNode, _precondition)
        {

        }

        protected override bool _DoEvaluate(BTreeTemplateData _input)
        {
            for (int i = 0; i < m_ChildCount; i++)
            {
                BTreeNode bn = m_ChildNodes[i];
                if (bn.Evaluate(_input))
                {
                    m_CurrentSelectIndex = i;
                    return true;
                }
            }
            return false;
        }
        protected override void _DoTransition(BTreeTemplateData _input)
        {
            if (_CheckIndex(m_LastSelectIndex))
            {
                BTreeNode bn = m_ChildNodes[m_LastSelectIndex];
                bn.Transition(_input);
            }
            m_LastSelectIndex = INVALID_CHILD_NODE_INDEX;
        }
        protected override BTreeRunningStatus _DoTick(BTreeTemplateData _input, ref BTreeTemplateData _output)
        {
            BTreeRunningStatus RunningStatus = BTreeRunningStatus.Finish;
            if (_CheckIndex(m_CurrentSelectIndex))
            {
                if (m_LastSelectIndex != m_CurrentSelectIndex)
                {
                    if (_CheckIndex(m_LastSelectIndex))
                    {
                        BTreeNode bn = m_ChildNodes[m_LastSelectIndex];
                        bn.Transition(_input);
                    }
                    m_LastSelectIndex = m_CurrentSelectIndex;
                }
            }
            if (_CheckIndex(m_LastSelectIndex))
            {
                BTreeNode bn = m_ChildNodes[m_LastSelectIndex];
                RunningStatus = bn.Tick(_input, ref _output);
                if (RunningStatus == BTreeRunningStatus.Finish)
                {
                    m_LastSelectIndex = INVALID_CHILD_NODE_INDEX;
                }
            }
            return RunningStatus;
        }
    }
}
