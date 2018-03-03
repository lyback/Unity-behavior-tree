
//执行节点基类
namespace BTreeFrame
{
    public class BTreeNodeAction<T, P> : BTreeNode<T, P>
        where T : BTreeTemplateData
        where P : BTreeTemplateData
    {
        private BTreeNodeStatus m_Status = BTreeNodeStatus.Ready;
        private bool m_NeedExit = false;

        public BTreeNodeAction(BTreeNode<T, P> _parentNode, BTreeNodePrecondition<T> _precondition = null) 
            : base(_parentNode, _precondition)
        {
            m_IsAcitonNode = true;
        }

        protected virtual void _DoEnter(T _input) { }
        protected virtual BTreeRunningStatus _DoExecute(T _input, ref P _output) { return BTreeRunningStatus.Finish; }
        protected virtual void _DoExit(T _input, BTreeRunningStatus _status) { }


        protected override void _DoTransition(T _input)
        {
            if (m_NeedExit)
            {
                _DoExit(_input, BTreeRunningStatus.Error);
            }
            SetActiveNode(null);
            m_Status = BTreeNodeStatus.Ready;
            m_NeedExit = false;
        }

        protected override BTreeRunningStatus _DoTick(T _input, ref P _output)
        {
            BTreeRunningStatus runningStatus = BTreeRunningStatus.Finish;
            if (m_Status == BTreeNodeStatus.Ready)
            {
                _DoEnter(_input);
                m_NeedExit = true;
                m_Status = BTreeNodeStatus.Running;
                SetActiveNode(this);
            }
            if (m_Status == BTreeNodeStatus.Running)
            {
                runningStatus = _DoExecute(_input, ref _output);
                SetActiveNode(this);
                if (runningStatus == BTreeRunningStatus.Finish || runningStatus == BTreeRunningStatus.Error)
                {
                    m_Status = BTreeNodeStatus.Finish;
                }
            }
            if (m_Status == BTreeNodeStatus.Finish)
            {
                if (m_NeedExit)
                {
                    _DoExit(_input, runningStatus);
                }
                m_Status = BTreeNodeStatus.Ready;
                m_NeedExit = false;
                SetActiveNode(null);
            }
            return runningStatus;
        }
    }
}
