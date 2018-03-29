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

using BTreeFrame;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BTree.Editor
{
    [Serializable]
    public class BTreeEditorConfig
    {
        public BTreeEditorTreeConfig m_RootNode;
        public List<BTreeEditorTreeConfig> m_DetachedNode;
        public BTreeEditorConfig()
        {
            m_RootNode = new BTreeEditorTreeConfig(BTreeNodeSerialization.ReadXML("Btree"));
            m_RootNode.m_IsEnterNode = true;
            m_RootNode.m_EditorNodes = new BTreeEditorNodeConfig[m_RootNode.m_Nodes.Length]; 
            for (int i = 0; i < m_RootNode.m_EditorNodes.Length; i++)
            {
                m_RootNode.m_EditorNodes[i] = new BTreeEditorNodeConfig(m_RootNode.m_Nodes[i]);
                m_RootNode.m_EditorNodes[i].m_PosX = (i + 1) * 60;
                m_RootNode.m_EditorNodes[i].m_PosY = (i + 1) * 60;
                m_RootNode.m_EditorNodes[i].m_Disable = false;
            }
        }
    }
    [Serializable]
    public class BTreeEditorTreeConfig : TreeConfig
    {
        public BTreeEditorTreeConfig()
        {

        }
        public BTreeEditorTreeConfig(TreeConfig _config)
        {
            m_Nodes = _config.m_Nodes;
            m_Name = _config.m_Name;
            m_EditorNodes = new BTreeEditorNodeConfig[m_Nodes.Length];
            for (int i = 0; i < m_EditorNodes.Length; i++)
            {
                m_EditorNodes[i] = new BTreeEditorNodeConfig(m_Nodes[i]);
            }
            m_IsEnterNode = false;
        }
        public BTreeEditorNodeConfig[] m_EditorNodes;
        public bool m_IsEnterNode;

    }
    [Serializable]
    public class BTreeEditorNodeConfig : TreeNodeConfig
    {
        public BTreeEditorNodeConfig()
        {

        }
        public BTreeEditorNodeConfig(TreeNodeConfig _config)
        {
            m_ActionNodeName = _config.m_ActionNodeName;
            m_NodeName = _config.m_NodeName;
            m_NodeSubType = _config.m_NodeSubType;
            m_NodeType = _config.m_NodeType;
            m_OtherParams = _config.m_OtherParams;
            m_ParentIndex = _config.m_ParentIndex;
            m_Preconditions = _config.m_Preconditions;
        }
        public float m_PosX;
        public float m_PosY;
        public bool m_Disable;
    }
}
