using BTreeFrame;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BTree.Editor
{
    [Serializable]
    public class BTreeEditorConfig
    {
        public BTreeEditorNodeConfig m_RootNode;
        public List<BTreeEditorNodeConfig> m_DetachedNode;
        public BTreeEditorConfig()
        {
            m_RootNode = new BTreeEditorNodeConfig();
            m_DetachedNode = new List<BTreeEditorNodeConfig>();
            m_DetachedNode.Add(new BTreeEditorNodeConfig());
        }
    }
    [Serializable]
    public class BTreeEditorNodeConfig
    {
        public TreeConfig m_Node;
        public Vector2 m_Pos;
        public bool m_Disable;
        public BTreeEditorNodeConfig()
        {
            m_Node = BTreeNodeSerialization.ReadBinary("Btree");
            m_Pos = new Vector2(1, 1);
        }
    }
}
