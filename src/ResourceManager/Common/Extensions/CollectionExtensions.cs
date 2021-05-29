using System.Collections;
using System.Collections.Generic;

namespace ResourceManager
{
    public static class CollectionExtensions
    {
        public static void AddRange(this IList list, IEnumerable items)
        {
            foreach (object item in items)
            {
                list.Add(item);
            }
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                list.Add(item);
            }
        }
    }
}
