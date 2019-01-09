using System.Linq;
using System.Collections.Generic;

namespace ASubstitute.Internal {
    public static class EnumerableExtensionMethods {
        public static IEnumerable<(T Value, int Index)> ToValueIndexPairs<T>(
                this IEnumerable<T> source) 
            => source.Select((arg, index) => (arg, index));
    }
}