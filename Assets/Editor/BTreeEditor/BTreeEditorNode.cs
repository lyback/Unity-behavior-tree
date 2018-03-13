using BTreeFrame;
using UnityEngine;

namespace BTree.Editor
{
    public class BTreeEditorNode
    {
        public BTreeNode m_Node;
        //public BTreeEditorNode<T, P>[] m_ChildNodeList;
        public Vector2 m_Pos;
        public bool m_Disable;
        public bool m_IsCollapsed;//损坏
        public BTreeEditorNode(BTreeNode _node)
        {
            m_Node = _node;
        }
        public void AddChildNode(BTreeEditorNode _node)
        {
            m_Node.AddChildNode(_node.m_Node);
        }
        public void DelChildNode(BTreeEditorNode _node)
        {
            m_Node.DelChildNode(_node.m_Node);
        }
    }
}
