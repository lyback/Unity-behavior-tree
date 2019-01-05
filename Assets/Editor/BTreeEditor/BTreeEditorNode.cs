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
        public void MoveUpIndex()
        {
            if (m_Node.m_ParentNode != null)
            {
                var childNodes = m_Node.m_ParentNode.m_ChildNodes;
                for (int i = 0; i < childNodes.Count; i++)
                {
                    if (childNodes[i].Equals(m_Node))
                    {
                        if (i > 0)
                        {
                            var temp = childNodes[i - 1];
                            childNodes[i - 1] = m_Node;
                            childNodes[i] = temp;
                        }
                    }
                }
            }
        }
        public void MoveDownIndex()
        {
            if (m_Node.m_ParentNode != null)
            {
                var childNodes = m_Node.m_ParentNode.m_ChildNodes;
                for (int i = 0; i < childNodes.Count; i++)
                {
                    if (childNodes[i].Equals(m_Node))
                    {
                        if (i < childNodes.Count - 1)
                        {
                            var temp = childNodes[i + 1];
                            childNodes[i + 1] = m_Node;
                            childNodes[i] = temp;
                        }
                    }
                }
            }
        }
    }
}
