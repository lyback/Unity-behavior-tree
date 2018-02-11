
//执行节点基类
namespace BTreeFrame
{
    public class BTreeNodeAction : BTreeNode
    {
        private BTreeNodeStatus m_Status;
        private bool m_NeedExit = false;

        public BTreeNodeAction(BTreeNode _parentNode, BTreeNodePrecondition _precondition = null) 
            : base(_parentNode, _precondition)
        {
        }

        protected virtual void _DoEnter(BTreeInputData _input) { }
        protected virtual BTreeRunningStatus _DoExecute(BTreeInputData _input, out BTreeOutputData _output) { _output = null; return BTreeRunningStatus.Finish; }
        protected virtual void _DoExit(BTreeInputData _input, BTreeRunningStatus _status) { }


        protected override void _DoTransition(BTreeInputData _input)
        {
            if (m_NeedExit)
            {
                _DoExit(_input, BTreeRunningStatus.Error);
            }
            SetActiveNode(null);
            m_Status = BTreeNodeStatus.Ready;
            m_NeedExit = false;
        }

        protected override BTreeRunningStatus _DoTick(BTreeInputData _input, out BTreeOutputData _output)
        {
            _output = null;
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
                runningStatus = _DoExecute(_input, out _output);
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
