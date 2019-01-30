using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tree.KDTree
{
    /// <summary>
    ///   K-dimensional tree node (for <see cref="KDTree"/>).
    /// </summary>
    /// 
    /// <seealso cref="SPTreeNode"/>
    /// <seealso cref="VPTreeNode{TPoint}"/>
    /// 
    public class KDTreeNode : KDTreeNodeBase<KDTreeNode>
    {
    }

    /// <summary>
    ///   K-dimensional tree node (for <see cref="KDTree{T}"/>).
    /// </summary>
    /// 
    /// <seealso cref="SPTreeNode"/>
    /// <seealso cref="VPTreeNode{TPoint}"/>
    /// 
    public class KDTreeNode<T> : KDTreeNodeBase<KDTreeNode<T>>
    {
        /// <summary>
        ///   Gets or sets the value being stored at this node.
        /// </summary>
        /// 
        public T Value { get; set; }
    }
}
