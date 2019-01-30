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
using System.Collections;
using System.Collections.Generic;

namespace Tree
{
    public class BinaryTree<TNode> : IEnumerable<TNode>
        where TNode : BinaryNode<TNode>
    {

        /// <summary>
        ///   Gets the root node of this tree.
        /// </summary>
        /// 
        public TNode Root { get; set; }

        /// <summary>
        ///   Returns an enumerator that iterates through the tree.
        /// </summary>
        /// 
        /// <returns>
        ///   An <see cref="T:System.Collections.IEnumerator"/> object 
        ///   that can be used to iterate through the collection.
        /// </returns>
        /// 
        public virtual IEnumerator<TNode> GetEnumerator()
        {
            if (Root == null)
                yield break;

            var stack = new Stack<TNode>(new[] { Root });

            while (stack.Count != 0)
            {
                TNode current = stack.Pop();

                yield return current;

                if (current.Left != null)
                    stack.Push(current.Left);

                if (current.Right != null)
                    stack.Push(current.Right);
            }
        }

        /// <summary>
        ///   Traverse the tree using a <see cref="TreeTraversal">tree traversal
        ///   method</see>. Can be iterated with a foreach loop.
        /// </summary>
        /// 
        /// <param name="method">The tree traversal method. Common methods are
        /// available in the <see cref="TreeTraversal"/>static class.</param>
        /// 
        /// <returns>An <see cref="IEnumerable{T}"/> object which can be used to
        /// traverse the tree using the chosen traversal method.</returns>
        /// 
        public IEnumerable<TNode> Traverse(BinaryTraversalMethod<TNode> method)
        {
            return new BinaryTreeTraversal(this, method);
        }


        /// <summary>
        ///   Returns an enumerator that iterates through the tree.
        /// </summary>
        /// 
        /// <returns>
        ///   An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// 
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class BinaryTreeTraversal : IEnumerable<TNode>
        {
            private BinaryTree<TNode> tree;
            private BinaryTraversalMethod<TNode> method;

            public BinaryTreeTraversal(BinaryTree<TNode> tree, BinaryTraversalMethod<TNode> method)
            {
                this.tree = tree;
                this.method = method;
            }

            public IEnumerator<TNode> GetEnumerator()
            {
                return method(tree);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return method(tree);
            }
        }
    }
}
