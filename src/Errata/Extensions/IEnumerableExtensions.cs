using System;
using System.Collections.Generic;
using System.Linq;

namespace Errata
{
    internal static class IEnumerableExtensions
    {
        public static IEnumerable<TResult> FilterMap<T, TResult>(this IEnumerable<T> source, Func<T, TResult?> func)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return source.Select(func)
                .Where(x => x != null)
                .Cast<TResult>();
        }

        public static T? MinByKey<T, TKey>(this IEnumerable<T> source, Func<T, TKey> func)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return source.OrderBy(func).FirstOrDefault();
        }

        public static IEnumerable<(int Index, bool First, bool Last, T Item)> Enumerate<T>(this IEnumerable<T> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Enumerate(source.GetEnumerator());
        }

        public static IEnumerable<(int Index, bool First, bool Last, T Item)> Enumerate<T>(this IEnumerator<T> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var first = true;
            var last = !source.MoveNext();
            T current;

            for (var index = 0; !last; index++)
            {
                current = source.Current;
                last = !source.MoveNext();
                yield return (index, first, last, current);
                first = false;
            }
        }

        public static IEnumerable<(int Index, T Item)> EnumerateWithIndex<T>(this IEnumerable<T> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Select((value, index) => (index, value));
        }
    }
}
