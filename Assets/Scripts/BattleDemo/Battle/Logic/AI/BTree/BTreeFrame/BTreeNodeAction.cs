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

//执行节点基类
namespace BTreeFrame
{
    public abstract class BTreeNodeAction : BTreeNode
    {
        private BTreeNodeStatus m_Status = BTreeNodeStatus.Ready;
        private bool m_NeedExit = false;

        public BTreeNodeAction()
            :base()
        {
            m_IsAcitonNode = true;
        }

        public BTreeNodeAction(BTreeNode _parentNode, BTreeNodePrecondition _precondition = null) 
            : base(_parentNode, _precondition)
        {
            m_IsAcitonNode = true;
        }

        protected virtual void _DoEnter(BTreeTemplateData _input)
        {
            Debugger.Log_Btree("_DoEnter:" + m_Name);
        }
        protected virtual BTreeRunningStatus _DoExecute(BTreeTemplateData _input, ref BTreeTemplateData _output)
        {
            Debugger.Log_Btree("_DoExecute:" + m_Name);
            return BTreeRunningStatus.Finish;
        }
        protected virtual void _DoExit(BTreeTemplateData _input, BTreeRunningStatus _status)
        {
            Debugger.Log_Btree("_DoExit:" + m_Name);
        }

        protected override void _DoTransition(BTreeTemplateData _input)
        {
            base._DoTransition(_input);
            if (m_NeedExit)
            {
                _DoExit(_input, BTreeRunningStatus.Error);
            }
            SetActiveNode(null);
            m_Status = BTreeNodeStatus.Ready;
            m_NeedExit = false;
        }

        protected override BTreeRunningStatus _DoTick(BTreeTemplateData _input, ref BTreeTemplateData _output)
        {
            BTreeRunningStatus runningStatus = base._DoTick(_input, ref _output);
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
