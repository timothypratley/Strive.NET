using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using System.Diagnostics.Contracts;


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
            Contract.Requires<ArgumentNullException>(vectors != null);
            Contract.Requires<ArgumentNullException>(vectors.Any());

            return vectors.Sum() / vectors.Count();
        }

        public static Vector3D Average<T>(this IEnumerable<T> vectors, Func<T,Vector3D> selector)
        {
            Contract.Requires<ArgumentNullException>(vectors != null && vectors.Any());
            return vectors.Select(selector).Average();
        }
    }
}
