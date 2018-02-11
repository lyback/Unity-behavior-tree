
using System.Collections.Generic;

namespace BTreeFrame
{
    public class BTreeNode<T, P> 
        where T : BTreeTemplateData
        where P : BTreeTemplateData
    {   
        //节点名称
        public string m_Name = "UNNAMED";
        //子节点
        protected List<BTreeNode<T, P>> m_ChildNodes = new List<BTreeNode<T, P>>();
        //子节点数
        protected int m_ChildCount = 0;
        //父节点
        protected BTreeNode<T, P> m_ParentNode { get; set; }
        //上一个激活的节点
        public BTreeNode<T, P> m_LastActiveNode { get; private set; }
        //当前激活的节点
        public BTreeNode<T, P> m_ActiveNode { get; private set; }
        //前提条件
        protected BTreeNodePrecondition<T> m_NodePrecondition;

        protected const int MAX_CHILD_NODE_COUNT = 16;
        protected const int INVALID_CHILD_NODE_INDEX = -1;

        public BTreeNode(BTreeNode<T, P> _parentNode, BTreeNodePrecondition<T> _precondition)
        {
            m_ParentNode = _parentNode;
            m_NodePrecondition = _precondition;
        }

        public bool Evaluate(T _input)
        {
            return (m_NodePrecondition == null 
                || m_NodePrecondition.ExternalCondition(_input)) 
                && _DoEvaluate(_input);
        }

        public void Transition(T _input)
        {
            _DoTransition(_input);
        }

        public BTreeRunningStatus Tick(T _input, out P _output)
        {
            return _DoTick(_input, out _output);
        }

        public void AddChildNode(BTreeNode<T, P> _childNode)
        {
            if (m_ChildCount>= MAX_CHILD_NODE_COUNT)
            {
                Debugger.LogError("添加行为树节点失败：超过最大数量16");
            }
            m_ChildNodes.Add(_childNode);
            m_ChildCount++;
        }

        public BTreeNode<T, P> SetNodePrecondition(BTreeNodePrecondition<T> _NodePrecondition)
        {
            if (m_NodePrecondition != _NodePrecondition)
            {
                m_NodePrecondition = _NodePrecondition;
            }
            return this;
        }

        public void SetActiveNode(BTreeNode<T, P> _node)
        {
            m_LastActiveNode = m_ActiveNode;
            m_ActiveNode = _node;
            if (m_ParentNode != null)
                m_ParentNode.SetActiveNode(_node);
        }

        protected virtual bool _DoEvaluate(T _input)
        {
            return true;
        }
        protected virtual void _DoTransition(T _input)
        {
            return;
        }
        protected virtual BTreeRunningStatus _DoTick(T _input, out P _output)
        {
            _output = null;
            return BTreeRunningStatus.Finish;
        }

        protected bool _CheckIndex(int _index)
		{
			return _index >= 0 && _index < m_ChildCount;
		}
}
}
