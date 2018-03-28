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

namespace BTreeFrame
{
    public class BTreeNodePreconditionAND : BTreeNodePrecondition
    {
        private BTreeNodePrecondition[] m_Preconditions;
        public BTreeNodePreconditionAND() { }
        public BTreeNodePreconditionAND(params BTreeNodePrecondition[] param)
        {
            if (param == null)
            {
                Debugger.Log("BTreeNodePreconditionAND is null");
                return;
            }
            if (param.Length == 0)
            {
                Debugger.Log("BTreeNodePreconditionAND's length is 0");
                return;
            }
            m_Preconditions = param;
        }
        public override bool ExternalCondition(BTreeTemplateData _input)
        {
            for (int i = 0; i < m_Preconditions.Length; i++)
            {
                if (!m_Preconditions[i].ExternalCondition(_input))
                {
                    return false;
                }
            }
            return true;
        }
        public void SetChildPrecondition(params BTreeNodePrecondition[] param)
        {
            m_Preconditions = param;
        }
        public BTreeNodePrecondition[] GetChildPrecondition()
        {
            return m_Preconditions;
        }
        public int GetChildPreconditionCount()
        {
            if (m_Preconditions != null)
            {
                return m_Preconditions.Length;
            }
            return 0;
        }
    }
}
