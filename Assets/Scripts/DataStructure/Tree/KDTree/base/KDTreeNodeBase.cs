using System;
using System.Text;

namespace Tree.KDTree
{
    public class KDTreeNodeBase<TNode> : BinaryNode<TNode>,
        IComparable<TNode>, IEquatable<TNode> // TODO: Try to remove IEquatable
        where TNode : KDTreeNodeBase<TNode>
    {
        public double[] Position;
        public int Axis { get; set; }

        public int CompareTo(TNode other)
        {
            return this.Position[this.Axis].CompareTo(other.Position[other.Axis]);
        }

        public new bool Equals(TNode other) // TODO: Try to remove IEquatable
        {
            return this == other;
        }

        public override string ToString()
        {
            if (Position == null)
                return "(null)";

            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            for (int i = 0; i < Position.Length; i++)
            {
                sb.Append(Position[i]);
                if (i < Position.Length - 1)
                    sb.Append(",");
            }
            sb.Append(")");

            return sb.ToString();
        }
    }
}
