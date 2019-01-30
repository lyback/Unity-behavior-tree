
namespace Tree.Math.Distances
{
    public interface ISimilarity<T, U>
    {
        double Similarity(T x, U y);
    }
    
    public interface ISimilarity<T> : ISimilarity<T, T>
    {

    }
}
