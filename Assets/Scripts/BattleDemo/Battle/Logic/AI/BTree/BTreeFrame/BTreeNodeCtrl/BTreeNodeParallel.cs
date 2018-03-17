using System.Collections.Generic;

namespace BTreeFrame
{
    /// <summary>
    /// class:      并行节点
    /// Evaluate:   依次调用所有的子节点的Evaluate方法，若所有的子节点都返回True，则自身也返回True，否则，返回False
    /// Tick:       调用所有子节点的Tick方法，若并行节点是“或者”的关系，则只要有一个子节点返回运行结束，那自身就返回运行结束。
    ///             若并行节点是“并且”的关系，则只有所有的子节点返回结束，自身才返回运行结束
    /// </summary>
    public class BTreeNodeParallel : BTreeNodeCtrl
    {
        public BTreeParallelFinishCondition m_FinishCondition;
        private List<BTreeRunningStatus> m_ChildNodeSatuses = new List<BTreeRunningStatus>();
        public BTreeNodeParallel()
            : base()
        {
        }
        public BTreeNodeParallel(BTreeNode _parentNode, BTreeNodePrecondition _precondition = null)
            : base(_parentNode, _precondition)
        {
        }

        protected override bool _DoEvaluate(BTreeTemplateData _input)
        {
            for (int i = 0; i < m_ChildCount; i++)
            {
                BTreeNode bn = m_ChildNodes[i];
                if (m_ChildNodeSatuses[i] == BTreeRunningStatus.Executing)
                {
                    if (!bn.Evaluate(_input))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected override void _DoTransition(BTreeTemplateData _input)
        {
            for (int i = 0; i < m_ChildCount; i++)
            {
                m_ChildNodeSatuses[i] = BTreeRunningStatus.Executing;
                BTreeNode bn = m_ChildNodes[i];
                bn.Transition(_input);
            }
        }

        protected override BTreeRunningStatus _DoTick(BTreeTemplateData _input, ref BTreeTemplateData _output)
        {
            int finishedChildCount = 0;
            for (int i = 0; i < m_ChildCount; i++)
            {
                BTreeNode bn = m_ChildNodes[i];
                if (m_FinishCondition == BTreeParallelFinishCondition.OR)
                {
                    if (m_ChildNodeSatuses[i] == BTreeRunningStatus.Executing)
                    {
                        m_ChildNodeSatuses[i] = bn.Tick(_input, ref _output);
                    }
                    if (m_ChildNodeSatuses[i] != BTreeRunningStatus.Executing)
                    {
                        _RestChildNodeStatus();
                        return BTreeRunningStatus.Finish;
                    }
                }
                else if (m_FinishCondition == BTreeParallelFinishCondition.AND)
                {
                    if (m_ChildNodeSatuses[i] == BTreeRunningStatus.Executing)
                    {
                        m_ChildNodeSatuses[i] = bn.Tick(_input, ref _output);
                    }
                    if (m_ChildNodeSatuses[i] != BTreeRunningStatus.Executing)
                    {
                        finishedChildCount++;
                    }
                }
                else
                {
                    Debugger.LogError("BTreeParallelFinishCondition Error");
                }
            }
            if (finishedChildCount == m_ChildCount)
            {
                _RestChildNodeStatus();
                return BTreeRunningStatus.Finish;
            }
            return BTreeRunningStatus.Executing;
        }

        public override void AddChildNode(BTreeNode _childNode)
        {
            base.AddChildNode(_childNode);
            m_ChildNodeSatuses.Add(BTreeRunningStatus.Executing);
        }

        protected void _RestChildNodeStatus()
        {
            for (int i = 0; i < m_ChildCount; i++)
            {
                m_ChildNodeSatuses[i] = BTreeRunningStatus.Executing;
            }
        }
    }
}
