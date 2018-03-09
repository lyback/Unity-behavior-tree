
//控制节点
using System.Collections.Generic;

namespace BTreeFrame
{
    /// <summary>
    /// class:      带优先级的选择节点
    /// Evaluate:   从第一个子节点开始依次遍历所有的子节点，调用其Evaluate方法，当发现存在可以运行的子节点时，记录子节点索引，停止遍历，返回True。
    /// Tick:       调用可以运行的子节点的Tick方法，用它所返回的运行状态作为自身的运行状态返回
    /// </summary>
    public class BTreeNodePrioritySelector<T, P> : BTreeNode<T, P>
        where T : BTreeTemplateData
        where P : BTreeTemplateData
    {
        protected int m_CurrentSelectIndex = INVALID_CHILD_NODE_INDEX;
        protected int m_LastSelectIndex = INVALID_CHILD_NODE_INDEX;
        public BTreeNodePrioritySelector()
            : base()
        {
        }
        public BTreeNodePrioritySelector(BTreeNode<T, P> _parentNode, BTreeNodePrecondition<T> _precondition = null)
            : base(_parentNode, _precondition)
        {

        }

        protected override bool _DoEvaluate(T _input)
        {
            for (int i = 0; i < m_ChildCount; i++)
            {
                BTreeNode<T, P> bn = m_ChildNodes[i];
                if (bn.Evaluate(_input))
                {
                    m_CurrentSelectIndex = i;
                    return true;
                }
            }
            return false;
        }
        protected override void _DoTransition(T _input)
        {
            if (_CheckIndex(m_LastSelectIndex))
            {
                BTreeNode<T, P> bn = m_ChildNodes[m_LastSelectIndex];
                bn.Transition(_input);
            }
            m_LastSelectIndex = INVALID_CHILD_NODE_INDEX;
        }
        protected override BTreeRunningStatus _DoTick(T _input, ref P _output)
        {
            BTreeRunningStatus RunningStatus = BTreeRunningStatus.Finish;
            if (_CheckIndex(m_CurrentSelectIndex))
            {
                if (m_LastSelectIndex != m_CurrentSelectIndex)
                {
                    if (_CheckIndex(m_LastSelectIndex))
                    {
                        BTreeNode<T, P> bn = m_ChildNodes[m_LastSelectIndex];
                        bn.Transition(_input);
                    }
                    m_LastSelectIndex = m_CurrentSelectIndex;
                }
            }
            if (_CheckIndex(m_LastSelectIndex))
            {
                BTreeNode<T, P> bn = m_ChildNodes[m_LastSelectIndex];
                RunningStatus = bn.Tick(_input, ref _output);
                if (RunningStatus == BTreeRunningStatus.Finish)
                {
                    m_LastSelectIndex = INVALID_CHILD_NODE_INDEX;
                }
            }
            return RunningStatus;
        }
    }

    /// <summary>
    /// class:      不带优先级的选择节点
    /// Evaluate:   先调用上一个运行的子节点（若存在）的Evaluate方法，如果可以运行，则继续运保存该节点的索引，返回True，如果不能运行，则重新选择（同带优先级的选择节点的选择方式）
    /// Tick:       调用可以运行的子节点的Tick方法，用它所返回的运行状态作为自身的运行状态返回
    /// </summary>
    public class BTreeNodeNonePrioritySelector<T,P> : BTreeNodePrioritySelector<T,P>
        where T : BTreeTemplateData
        where P : BTreeTemplateData
    {
        public BTreeNodeNonePrioritySelector()
            : base()
        {
        }
        public BTreeNodeNonePrioritySelector(BTreeNode<T, P> _parentNode, BTreeNodePrecondition<T> _precondition = null)
            : base(_parentNode, _precondition)
        {
        }
        protected override bool _DoEvaluate(T _input)
        {
            if (_CheckIndex(m_CurrentSelectIndex))
            {
                BTreeNode<T, P> bn = m_ChildNodes[m_CurrentSelectIndex];
                if (bn.Evaluate(_input))
                {
                    return true;
                }
            }
            return base._DoEvaluate(_input);
        }
    }

    /// <summary>
    /// class:      序列节点
    /// Evaluate:   若是从头开始的，则调用第一个子节点的Evaluate方法，将其返回值作为自身的返回值返回。否则，调用当前运行节点的Evaluate方法，将其返回值作为自身的返回值返回。
    /// Tick:       调用可以运行的子节点的Tick方法，若返回运行结束，则将下一个子节点作为当前运行节点，若当前已是最后一个子节点，表示该序列已经运行结束，则自身返回运行结束。
    ///             若子节点返回运行中，则用它所返回的运行状态作为自身的运行状态返回
    /// </summary>
    public class BTreeNodeSequence<T,P> : BTreeNode<T, P>
        where T : BTreeTemplateData
        where P : BTreeTemplateData
    {
        private int m_CurrentNodeIndex = INVALID_CHILD_NODE_INDEX;
        public BTreeNodeSequence()
            : base()
        {
        }
        public BTreeNodeSequence(BTreeNode<T, P> _parentNode, BTreeNodePrecondition<T> _precondition = null)
            : base(_parentNode, _precondition)
        {
        }

        protected override bool _DoEvaluate(T _input)
        {
            int testNode;
            if (m_CurrentNodeIndex == INVALID_CHILD_NODE_INDEX)
            {
                testNode = 0;
            }
            else
            {
                testNode = m_CurrentNodeIndex;
            }
            if (_CheckIndex(testNode))
            {
                BTreeNode<T, P> bn = m_ChildNodes[testNode];
                if (bn.Evaluate(_input))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void _DoTransition(T _input)
        {
            if (_CheckIndex(m_CurrentNodeIndex))
            {
                BTreeNode<T, P> bn = m_ChildNodes[m_CurrentNodeIndex];
                bn.Transition(_input);
            }
            m_CurrentNodeIndex = INVALID_CHILD_NODE_INDEX;
        }

        protected override BTreeRunningStatus _DoTick(T _input, ref P _output)
        {
            BTreeRunningStatus runningStatus = BTreeRunningStatus.Finish;
            //First Time
            if (m_CurrentNodeIndex == INVALID_CHILD_NODE_INDEX)
            {
                m_CurrentNodeIndex = 0;
            }
            BTreeNode<T, P> bn = m_ChildNodes[m_CurrentNodeIndex];
            runningStatus = bn.Tick(_input, ref _output);
            if (runningStatus == BTreeRunningStatus.Finish)
            {
                m_CurrentNodeIndex++;
                if (m_CurrentNodeIndex == m_ChildCount)
                {
                    m_CurrentNodeIndex = INVALID_CHILD_NODE_INDEX;
                }
                else
                {
                    runningStatus = BTreeRunningStatus.Executing;
                }
            }
            if (runningStatus == BTreeRunningStatus.Error)
            {
                m_CurrentNodeIndex = INVALID_CHILD_NODE_INDEX;
            }
            return runningStatus;
        }
    }

    /// <summary>
    /// class:      并行节点
    /// Evaluate:   依次调用所有的子节点的Evaluate方法，若所有的子节点都返回True，则自身也返回True，否则，返回False
    /// Tick:       调用所有子节点的Tick方法，若并行节点是“或者”的关系，则只要有一个子节点返回运行结束，那自身就返回运行结束。
    ///             若并行节点是“并且”的关系，则只有所有的子节点返回结束，自身才返回运行结束
    /// </summary>
    public class BTreeNodeParallel<T,P> : BTreeNode<T, P>
        where T : BTreeTemplateData
        where P : BTreeTemplateData
    {
        private BTreeParallelFinishCondition m_FinishCondition;
        private List<BTreeRunningStatus> m_ChildNodeSatuses = new List<BTreeRunningStatus>();
        public BTreeNodeParallel()
            : base()
        {
        }
        public BTreeNodeParallel(BTreeNode<T, P> _parentNode, BTreeNodePrecondition<T> _precondition = null)
            : base(_parentNode, _precondition)
        {
        }

        protected override bool _DoEvaluate(T _input)
        {
            for (int i = 0; i < m_ChildCount; i++)
            {
                BTreeNode<T, P> bn = m_ChildNodes[i];
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

        protected override void _DoTransition(T _input)
        {
            for (int i = 0; i < m_ChildCount; i++)
            {
                m_ChildNodeSatuses[i] = BTreeRunningStatus.Executing;
                BTreeNode<T, P> bn = m_ChildNodes[i];
                bn.Transition(_input);
            }
        }

        protected override BTreeRunningStatus _DoTick(T _input, ref P _output)
        {
            int finishedChildCount = 0;
            for (int i = 0; i < m_ChildCount; i++)
            {
                BTreeNode<T, P> bn = m_ChildNodes[i];
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

        public override void AddChildNode(BTreeNode<T, P> _childNode)
        {
            base.AddChildNode(_childNode);
            m_ChildNodeSatuses.Add(BTreeRunningStatus.Executing);
        }
        public void SetFinishCondition(BTreeParallelFinishCondition _condition)
        {
            m_FinishCondition = _condition;
        }
        public BTreeParallelFinishCondition GetFinishCondition()
        {
            return m_FinishCondition;
        }
        protected void _RestChildNodeStatus()
        {
            for (int i = 0; i < m_ChildCount; i++)
            {
                m_ChildNodeSatuses[i] = BTreeRunningStatus.Executing;
            }
        }
    }

    /// <summary>
    /// class:      循环节点
    /// Evaluate:   预设的循环次数到了就返回False，否则，只调用第一个子节点的Evaluate方法，用它所返回的值作为自身的值返回
    /// Tick:       只调用第一个节点的Tick方法，若返回运行结束，则看是否需要重复运行，若循环次数没到，则自身返回运行中，若循环次数已到，则返回运行结束
    /// </summary>
    public class BTreeNodeLoop<T,P> : BTreeNode<T, P>
        where T : BTreeTemplateData
        where P : BTreeTemplateData
    {
        public const int INFINITELOOP = -1;
        private int m_LoopCount;
        private int m_CurrentCount;
        public BTreeNodeLoop()
            : base()
        {
        }
        public BTreeNodeLoop(BTreeNode<T, P> _parentNode, BTreeNodePrecondition<T> _precondition = null, int _loopCount = INFINITELOOP)
            : base(_parentNode, _precondition)
        {
            m_LoopCount = _loopCount;
            m_CurrentCount = 0;
        }

        protected override bool _DoEvaluate(T _input)
        {
            bool checkLoopCount = m_LoopCount == INFINITELOOP || m_CurrentCount < m_LoopCount;
            if (!checkLoopCount)
            {
                return false;
            }
            if (_CheckIndex(0))
            {
                BTreeNode<T, P> bn = m_ChildNodes[0];
                if (bn.Evaluate(_input))
                {
                    return true;
                }
            }
            return false;
        }
        protected override void _DoTransition(T _input)
        {
            if (_CheckIndex(0))
            {
                BTreeNode<T, P> bn = m_ChildNodes[0];
                bn.Transition(_input);
            }
            m_CurrentCount = 0;
        }
        protected override BTreeRunningStatus _DoTick(T _input, ref P _output)
        {
            BTreeRunningStatus runningStatus = BTreeRunningStatus.Finish;
            if (_CheckIndex(0))
            {
                BTreeNode<T, P> bn = m_ChildNodes[0];
                runningStatus = bn.Tick(_input, ref _output);

                if (runningStatus == BTreeRunningStatus.Finish)
                {
                    if (m_LoopCount != INFINITELOOP)
                    {
                        m_CurrentCount++;
                        if (m_CurrentCount == m_LoopCount)
                        {
                            runningStatus = BTreeRunningStatus.Executing;
                        }
                    }
                    else
                    {
                        runningStatus = BTreeRunningStatus.Executing;
                    }
                }
            }
            if (runningStatus == BTreeRunningStatus.Finish)
            {
                m_CurrentCount = 0;
            }
            return runningStatus;
        }
        public int GetLoopCount()
        {
            return m_LoopCount;
        }
    }
}
