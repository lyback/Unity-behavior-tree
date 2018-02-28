using BTreeFrame;
using UnityEngine;

namespace BTree.Editor
{
    public class BTreeEditorNode<T, P>
        where T : BTreeTemplateData
        where P : BTreeTemplateData
    {
        public BTreeNode<T, P> m_BTreeNode;
        public Vector2 m_Pos;
        public bool m_Disable;
        public bool m_IsCollapsed;//损坏
    }
}
