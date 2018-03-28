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
