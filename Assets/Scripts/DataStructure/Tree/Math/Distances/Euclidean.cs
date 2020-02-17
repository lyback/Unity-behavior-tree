
namespace Tree.Math.Distances
{
    using Math = System.Math;

    public struct Euclidean :
        IMetric<double>, ISimilarity<double>,
        IMetric<double[]>, ISimilarity<double[]>
    {
        
        public double Distance(double x, double y)
        {
            return Math.Abs(x - y);
        }

        public double Distance(double[] x, double[] y)
        {
            double sum = 0.0;
            for (int i = 0; i < x.Length; i++)
            {
                double u = x[i] - y[i];
                sum += u * u;
            }
            return Math.Sqrt(sum);
        }
        
        public double Distance(double vector1x, double vector1y, double vector2x, double vector2y)
        {
            double dx = vector1x - vector2x;
            double dy = vector1y - vector2y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        
        public double Similarity(double x, double y)
        {
            return 1.0 / (1.0 + Math.Abs(x - y));
        }

        public double Similarity(double[] x, double[] y)
        {
            double sum = 0.0;

            for (int i = 0; i < x.Length; i++)
            {
                double u = x[i] - y[i];
                sum += u * u;
            }

            return 1.0 / (1.0 + Math.Sqrt(sum));
        }
    }
}
