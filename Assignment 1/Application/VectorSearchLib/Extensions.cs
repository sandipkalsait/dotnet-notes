using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorSearchLib
{
    public static class VectorMath
    {
        public static float Dot(this float[] a, float[] b)
        {
            if (a.Length != b.Length) throw new ArgumentException("Vectors must match in length.");
            float sum = 0;
            for (int i = 0; i < a.Length; i++) sum += a[i] * b[i];
            return sum;
        }

        public static float Norm(this float[] v) => (float)Math.Sqrt(v.Select(x => x * x).Sum());

        public static float[] Normalize(this float[] v)
        {
            var norm = v.Norm();
            if (norm == 0) throw new InvalidOperationException("Zero vector cannot be normalized.");
            return v.Select(x => x / norm).ToArray();
        }

        public static float CosineSimilarity(float[] a, float[] b) => a.Dot(b) / (a.Norm() * b.Norm());
    }
}
