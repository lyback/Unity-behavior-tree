
using System.Collections.Generic;

namespace BTreeFrame
{
    public class BTreeNode<T, P> 
        where T : BTreeTemplateData
        where P : BTreeTemplateData
    {   
        //节点名称
        public string m_Name = "UNNAMED";
        //节点索引
        public int m_Index;
        //子节点
        protected List<BTreeNode<T, P>> m_ChildNodes = new List<BTreeNode<T, P>>();
        public List<BTreeNode<T, P>> m_ChildNodeList
        {
            get
            {
                List<BTreeNode<T, P>> _ChildNodes = new List<BTreeNode<T, P>>(m_ChildNodes);
                return _ChildNodes;
            }
        }
        //子节点数
        public int m_ChildCount{ get; protected set; }
        //父节点
        public BTreeNode<T, P> m_ParentNode { get; protected set; }
        //上一个激活的节点
        public BTreeNode<T, P> m_LastActiveNode { get; private set; }
        //当前激活的节点
        public BTreeNode<T, P> m_ActiveNode { get; private set; }
        //前提条件
        public BTreeNodePrecondition<T> m_NodePrecondition { get; protected set; }
        //是否是Action节点
        public bool m_IsAcitonNode { get; protected set; }

        protected const int MAX_CHILD_NODE_COUNT = 16;
        protected const int INVALID_CHILD_NODE_INDEX = -1;


        public BTreeNode(BTreeNode<T, P> _parentNode, BTreeNodePrecondition<T> _precondition = null)
        {
            m_ChildCount = 0;
            m_ParentNode = _parentNode;
            m_NodePrecondition = _precondition;
            m_IsAcitonNode = false;
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

        public BTreeRunningStatus Tick(T _input, ref P _output)
        {
            return _DoTick(_input, ref _output);
        }

        public virtual void AddChildNode(BTreeNode<T, P> _childNode)
        {
            if (m_ChildCount>= MAX_CHILD_NODE_COUNT)
            {
                Debugger.LogError("添加行为树节点失败：超过最大数量16");
                return;
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
        public BTreeNodePrecondition<T> GetNodePrecondition()
        {
            return m_NodePrecondition;
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
        protected virtual BTreeRunningStatus _DoTick(T _input, ref P _output)
        {
            return BTreeRunningStatus.Finish;
        }

        protected bool _CheckIndex(int _index)
		{
			return _index >= 0 && _index < m_ChildCount;
		}
}
}
