using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tree
{
    public interface ITreeNode<TNode>
        where TNode : ITreeNode<TNode>
    {
        /// <summary>
        ///   Gets or sets the collection of child nodes
        ///   under this node.
        /// </summary>
        /// 
        TNode[] Children { get; set; }

        /// <summary>
        ///   Gets whether this node is a leaf (has no children).
        /// </summary>
        /// 
        bool IsLeaf { get; }
    }
}
