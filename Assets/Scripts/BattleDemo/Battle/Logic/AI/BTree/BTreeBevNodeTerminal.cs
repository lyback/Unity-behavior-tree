
//执行节点基类
namespace Battle.Logic.AI.BTree
{
    public class BTreeBevNodeTerminal : BTreeNode
    {
        private BTreeBevNodeStatus m_Status;
        private bool m_NeedExit = false;

        protected virtual void _DoEnter(BTreeInputData _input) { }
        protected virtual BTreeRunningStatus _DoExecute(BTreeInputData _input, out BTreeOutputData _output) { return BTreeRunningStatus.Finish; }
        protected virtual void _DoExit(BTreeInputData _input, BTreeRunningStatus _status) { }

        protected override void _DoTransition(BTreeInputData _input)
        {
            if (m_NeedExit)
            {
                _DoExit(_input, BTreeRunningStatus.Error);
            }
            SetActiveNode(null);
            m_Status = BTreeBevNodeStatus.Ready;
            m_NeedExit = false;
        }

        protected override BTreeRunningStatus _DoTick(BTreeInputData _input, out BTreeOutputData _output)
        {
            BTreeRunningStatus bIsFinish = BTreeRunningStatus.Finish;
            if (m_Status == BTreeBevNodeStatus.Ready)
            {
                _DoEnter(_input);
                m_NeedExit = true;
                m_Status = BTreeBevNodeStatus.Running;
                SetActiveNode(this);
            }
            if (m_Status == BTreeBevNodeStatus.Running)
            {
                bIsFinish = _DoExecute(_input, out _output);
                SetActiveNode(this);
                if (bIsFinish == BTreeRunningStatus.Finish || bIsFinish == BTreeRunningStatus.Error)
                {
                    m_Status = BTreeBevNodeStatus.Finish;
                }
            }
            if (m_Status == BTreeBevNodeStatus.Finish)
            {
                if (m_NeedExit)
                {
                    _DoExit(_input, bIsFinish);
                }
                m_Status = BTreeBevNodeStatus.Ready;
                m_NeedExit = false;
                SetActiveNode(null);
            }
            return bIsFinish;
        }
    }
}
