
using System.Collections.Generic;

namespace BTreeFrame
{
    public abstract class BTreeNode
    {   
        //节点名称
        public string m_Name = "UNNAMED";
        //节点索引
        public int m_Index { get; set; }
        //前提条件
        public BTreeNodePrecondition m_NodePrecondition { get; set; }

        //子节点
        protected List<BTreeNode> m_ChildNodes = new List<BTreeNode>();
        public List<BTreeNode> m_ChildNodeList
        {
            get
            {
                List<BTreeNode> _ChildNodes = new List<BTreeNode>(m_ChildNodes);
                return _ChildNodes;
            }
        }
        //子节点数
        public int m_ChildCount{ get; protected set; }
        //父节点
        public BTreeNode m_ParentNode { get; protected set; }
        //上一个激活的节点
        public BTreeNode m_LastActiveNode { get; private set; }
        //当前激活的节点
        public BTreeNode m_ActiveNode { get; private set; }
        //是否是Action节点
        public bool m_IsAcitonNode { get; protected set; }

        protected const int MAX_CHILD_NODE_COUNT = 16;
        protected const int INVALID_CHILD_NODE_INDEX = -1;

        //必须有个无参的构造函数
        public BTreeNode()
        {
            m_Name = GetType().Name;
            m_ChildCount = 0;
            m_ParentNode = null;
            m_NodePrecondition = null;
            m_IsAcitonNode = false;
        }
        public BTreeNode(BTreeNode _parentNode, BTreeNodePrecondition _precondition = null)
        {
            m_ChildCount = 0;
            m_ParentNode = _parentNode;
            m_NodePrecondition = _precondition;
            m_IsAcitonNode = false;
        }

        public bool Evaluate(BTreeTemplateData _input)
        {
            return (m_NodePrecondition == null 
                || m_NodePrecondition.ExternalCondition(_input)) 
                && _DoEvaluate(_input);
        }

        public void Transition(BTreeTemplateData _input)
        {
            _DoTransition(_input);
        }

        public BTreeRunningStatus Tick(BTreeTemplateData _input, ref BTreeTemplateData _output)
        {
            return _DoTick(_input, ref _output);
        }

        public virtual void AddChildNode(BTreeNode _childNode)
        {
            if (m_ChildCount>= MAX_CHILD_NODE_COUNT)
            {
                Debugger.LogError("添加行为树节点失败：超过最大数量16");
                return;
            }
            m_ChildNodes.Add(_childNode);
            m_ChildCount++;
        }

        public virtual void DelChildNode(BTreeNode _childNode)
        {
            for (int i = 0; i < m_ChildCount; i++)
            {
                if (m_ChildNodes[i].Equals(_childNode))
                {
                    m_ChildNodes.RemoveAt(i);
                    m_ChildCount--;
                    break;
                }
            }
        }

        public BTreeNode SetNodePrecondition(BTreeNodePrecondition _NodePrecondition)
        {
            if (m_NodePrecondition != _NodePrecondition)
            {
                m_NodePrecondition = _NodePrecondition;
            }
            return this;
        }
        public BTreeNodePrecondition GetNodePrecondition()
        {
            return m_NodePrecondition;
        }
        public void SetActiveNode(BTreeNode _node)
        {
            m_LastActiveNode = m_ActiveNode;
            m_ActiveNode = _node;
            if (m_ParentNode != null)
                m_ParentNode.SetActiveNode(_node);
        }

        protected virtual bool _DoEvaluate(BTreeTemplateData _input)
        {
            return true;
        }
        protected virtual void _DoTransition(BTreeTemplateData _input)
        {
            return;
        }
        protected virtual BTreeRunningStatus _DoTick(BTreeTemplateData _input, ref BTreeTemplateData _output)
        {
            return BTreeRunningStatus.Finish;
        }

        protected bool _CheckIndex(int _index)
		{
			return _index >= 0 && _index < m_ChildCount;
		}
}
}
