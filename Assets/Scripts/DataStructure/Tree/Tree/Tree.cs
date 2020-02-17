using System;
using System.Collections;
using System.Collections.Generic;
namespace Tree
{
    public class Tree<TNode> : IEnumerable<TNode>
       where TNode : TreeNode<TNode>
    {

        /// <summary>
        ///   Gets the root node of this tree.
        /// </summary>
        /// 
        public TNode Root { get; protected set; }


        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// 
        /// <returns>
        ///   A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        /// 
        public IEnumerator<TNode> GetEnumerator()
        {
            foreach (var node in Traverse(TreeTraversal.DepthFirst))
                yield return node;
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
        public IEnumerable<TNode> Traverse(TraversalMethod<TNode> method)
        {
            return new InnerTreeTraversal(this, method);
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


        private class InnerTreeTraversal : IEnumerable<TNode>
        {
            private Tree<TNode> tree;
            private TraversalMethod<TNode> method;

            public InnerTreeTraversal(Tree<TNode> tree, TraversalMethod<TNode> method)
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
