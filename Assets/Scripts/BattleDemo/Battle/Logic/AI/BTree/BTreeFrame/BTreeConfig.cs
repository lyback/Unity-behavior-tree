using System;

namespace BTreeFrame
{
    [Serializable]
    public class TreeConfig
    {
        public string m_Name;
        public TreeNodeConfig[] m_Nodes;
    }
    [Serializable]
    public class TreeNodeConfig
    {
        public int m_ParentIndex = -1;
        public int m_NodeType;
        public int m_NodeSubType;
        public int[] m_OtherParams;
        public string m_ActionNodeName;
        public string m_NodeName;
        public PreconditionConfig[] m_Preconditions;
    }
    [Serializable]
    public class PreconditionConfig
    {
        public int m_ParentIndex = -1;
        public int m_Type;
        public string m_PreconditionName;
        public int[] m_ChildIndexs;
    }

    public enum NodeType
    {
        SelectorNode = 1,
        ActionNode = 2,
    }
    public enum SelectorNodeType
    {
        BTreeNodeParallel = 1,
        BTreeNodePrioritySelector = 2,
        BTreeNodeNonePrioritySelector = 3,
        BTreeNodeSequence = 4,
        BTreeNodeLoop = 5
    }
    public enum PreconditionType
    {
        And = 1,
        Or = 2,
        Not = 3,
    }
}
