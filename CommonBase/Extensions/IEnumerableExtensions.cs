//@BaseCode
//MdStart
namespace CommonBase.Extensions
{
    public static class IEnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            var idx = 0;
            var result = -1;
            var enumerator = source.GetEnumerator();

            while (result == -1 && enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                {
                    result = idx;
                }
                idx++;
            }
            return result;
        }
        public static IEnumerable<ST> ToEnumerable<T, ST>(this IEnumerable<T> source, Func<T, ST> expandSelector)
        {
            List<ST> expandResult = new();

            if (source != null && expandSelector != null)
            {
                foreach (var item in source)
                {
                    var subItem = expandSelector(item);

                    if (subItem != null)
                    {
                        expandResult.Add(subItem);
                    }
                }
            }
            return expandResult;
        }
        public static IEnumerable<ST> Flatten<T, ST>(this IEnumerable<T> source, Func<T, IEnumerable<ST>> expandSelector)
        {
            List<ST> expandResult = new();

            if (source != null && expandSelector != null)
            {
                foreach (var item in source)
                {
                    var subItems = expandSelector(item);

                    if (subItems != null)
                    {
                        expandResult.AddRange(subItems);
                    }
                }
            }
            return expandResult;
        }
        ///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="predicate">The expression to test the items against.</param>
        ///<returns>The index of the first matching item, or -1 if no items match.</returns>
        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            int retVal = 0;

            foreach (var item in items)
            {
                if (predicate(item))
                    return retVal;

                retVal++;
            }
            return -1;
        }
        ///<summary>Finds the index of the first occurrence of an item in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="item">The item to find.</param>
        ///<returns>The index of the first matching item, or -1 if the item was not found.</returns>
        public static int IndexOf<T>(this IEnumerable<T> items, T item)
        {
            var result = -1;

            if (items != null)
            {
                result = items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i));
            }
            return result;
        }

        public static IEnumerable<T> TakeTo<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            var result = default(IEnumerable<T>);

            if (source != null)
            {
                var end = false;

                result = source.Where(e =>
                {
                    if (end == false && predicate != null)
                    {
                        end = predicate(e);
                    }
                    return end == false;
                });
            }
            return result ?? Array.Empty<T>();
        }
        /// <summary>
        /// Performs the specified action on each element of the IEnumerable<T>.
        /// </summary>
        /// <typeparam name="T">The generic type T.</typeparam>
        /// <param name="source">The collection from type T.</param>
        /// <param name="action">The Action<T> delegate to perform on each element of the IEnumerable<T>.</param>
        /// <returns>The same collection as source.</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source != null && action != null)
            {
                foreach (var item in source)
                {
                    action(item);
                }
            }
            return source ?? Array.Empty<T>();
        }
        /// <summary>
        /// Copy the collection from source and return it.
        /// </summary>
        /// <typeparam name="T">The generic type T.</typeparam>
        /// <param name="source">The collection from type T.</param>
        /// <returns>A copy of source collection.</returns>
        public static IEnumerable<T> Clone<T>(this IEnumerable<T> source)
        {
            return new List<T>(source);
        }
    }
}
//MdEnd