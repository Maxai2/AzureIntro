using System.Collections.Generic;
using System.Linq;

namespace VideoConverter.Api.Extensions {
    public static class IEnumerableExtensions {
        public static List<T> AsList<T> (this IEnumerable<T> collection) {
            return collection as List<T> ?? collection.ToList ();
        }
    }
}