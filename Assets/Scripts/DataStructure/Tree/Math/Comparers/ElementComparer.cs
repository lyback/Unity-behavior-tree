using System;
using System.Collections.Generic;

namespace Tree.Math.Comparerers
{
    public class ElementComparer<T> : IComparer<T[]>, IEqualityComparer<T[]>
       where T : IComparable, IEquatable<T>
    {
        public int Index { get; set; }

        public int Compare(T[] x, T[] y)
        {
            return x[Index].CompareTo(y[Index]);
        }

        public bool Equals(T[] x, T[] y)
        {
            return x[Index].Equals(y[Index]);
        }

        public int GetHashCode(T[] obj)
        {
            return obj[Index].GetHashCode();
        }
    }

    public class ElementComparer : ElementComparer<double>
    {
    }
}