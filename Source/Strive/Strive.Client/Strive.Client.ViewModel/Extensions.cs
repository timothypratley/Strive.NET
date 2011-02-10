using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;


namespace Strive.Client.ViewModel
{
    public static class Extensions
    {
        public static Vector3D Sum(this IEnumerable<Vector3D> source)
        {
            return source.Aggregate((u, v) => u + v);
        }

        public static Vector3D Average(this IEnumerable<Vector3D> vectors)
        {
            if (!vectors.Any())
                throw new InvalidOperationException("Cannot compute median for an empty set.");

            return vectors.Sum() / vectors.Count();
        }

        public static Vector3D Average<T>(this IEnumerable<T> vectors, Func<T,Vector3D> selector)
        {
            return vectors.Select(selector).Average();
        }
    }
}
