

namespace Tree.Math.Distances
{
    public interface IDistance<in T, in U>
    {
        double Distance(T x, U y);
    }
    public interface IDistance<in T> : IDistance<T, T>
    {
    }
    public interface IDistance : IDistance<double[]>
    {
    }
}
