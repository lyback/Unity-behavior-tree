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
    /// <summary>
    /// class:      不带优先级的选择节点
    /// Evaluate:   先调用上一个运行的子节点（若存在）的Evaluate方法，如果可以运行，则继续运保存该节点的索引，返回True，如果不能运行，则重新选择（同带优先级的选择节点的选择方式）
    /// Tick:       调用可以运行的子节点的Tick方法，用它所返回的运行状态作为自身的运行状态返回
    /// </summary>
    public class BTreeNodeNonePrioritySelector : BTreeNodePrioritySelector
    {
        public BTreeNodeNonePrioritySelector()
            : base()
        {
        }
        public BTreeNodeNonePrioritySelector(BTreeNode _parentNode, BTreeNodePrecondition _precondition = null)
            : base(_parentNode, _precondition)
        {
        }
        protected override bool _DoEvaluate(BTreeTemplateData _input)
        {
            base._DoEvaluate(_input);
            if (_CheckIndex(m_CurrentSelectIndex))
            {
                BTreeNode bn = m_ChildNodes[m_CurrentSelectIndex];
                if (bn.Evaluate(_input))
                {
                    return true;
                }
            }
            return base._DoEvaluate(_input);
        }
    }
}
