//////////////////////////////////////////////////////////////////////////////////////
// The MIT License(MIT)
// Copyright(c) 2018 lycoder

// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

//////////////////////////////////////////////////////////////////////////////////////

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
            base._DoEvaluate(_input);
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
            base._DoTransition(_input);
            if (_CheckIndex(m_LastSelectIndex))
            {
                BTreeNode bn = m_ChildNodes[m_LastSelectIndex];
                bn.Transition(_input);
            }
            m_LastSelectIndex = INVALID_CHILD_NODE_INDEX;
        }
        protected override BTreeRunningStatus _DoTick(BTreeTemplateData _input, ref BTreeTemplateData _output)
        {
            base._DoTick(_input, ref _output);
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
