using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tree
{
    public class TreeNode<TNode> : ITreeNode<TNode>
        where TNode : TreeNode<TNode>
    {
        /// <summary>
        ///   Gets or sets the parent of this node.
        /// </summary>
        /// 
        public TNode Parent { get; set; }

        /// <summary>
        ///   Gets or sets the index of this node in the
        ///   collection of children nodes of its parent.
        /// </summary>
        /// 
        public int Index { get; set; }

        /// <summary>
        ///   Gets or sets the collection of child nodes
        ///   under this node.
        /// </summary>
        /// 
        public TNode[] Children { get; set; }

        /// <summary>
        ///   Gets the next sibling of this node (the node
        ///   immediately next to it in its parent's collection).
        /// </summary>
        /// 
        public TNode Next
        {
            get
            {
                if (Parent == null)
                    return null;
                if (Index + 1 >= Parent.Children.Length)
                    return null;
                return Parent.Children[Index + 1];
            }
        }

        /// <summary>
        ///   Gets the previous sibling of this node.
        /// </summary>
        /// 
        public TNode Previous
        {
            get
            {
                if (Index == 0)
                    return null;
                return Parent.Children[Index - 1];
            }
        }

        /// <summary>
        ///   Gets whether this node is a leaf (has no children).
        /// </summary>
        /// 
        public bool IsLeaf
        {
            get { return Children == null || Children.Length == 0; }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="TreeNode{TNode}"/> class.
        /// </summary>
        /// 
        /// <param name="index">The index of this node in the children collection of its parent node.</param>
        /// 
        public TreeNode(int index)
        {
            this.Index = index;
        }

    }
}
