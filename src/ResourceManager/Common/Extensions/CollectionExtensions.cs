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

        public static void RemoveRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                list.Remove(item);
            }
        }

        public static bool EquivalentTo<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> dict2)
        {
            if (dictionary.Count == dict2.Count)
            {
                EqualityComparer<TValue> valueComparer = EqualityComparer<TValue>.Default;
                foreach (KeyValuePair<TKey, TValue> pair in dictionary)
                {
                    if (dict2.TryGetValue(pair.Key, out TValue value))
                    {
                        if (!valueComparer.Equals(value, pair.Value))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public static bool EquivalentTo(this ICollection actual, ICollection expected)
        {
            if ((expected == null) != (actual == null))
            {
                return false;
            }

            if (ReferenceEquals(expected, actual) || expected == null)
            {
                return true;
            }

            if (expected.Count != actual.Count)
            {
                return false;
            }

            if (expected.Count == 0)
            {
                return true;
            }
            if (FindMismatchedElement(expected, actual, out int expectedCount, out int actualCount, out object mismatchedElement))
            {
                return false;
            }
            return true;
        }

        private static bool FindMismatchedElement(ICollection expected, ICollection actual, out int expectedCount, out int actualCount, out object mismatchedElement)
        {
            Dictionary<object, int> expectedElements = GetElementCounts(expected, out int expectedNulls);
            Dictionary<object, int> actualElements = GetElementCounts(actual, out int actualNulls);

            if (actualNulls != expectedNulls)
            {
                expectedCount = expectedNulls;
                actualCount = actualNulls;
                mismatchedElement = null;
                return true;
            }

            foreach (object current in expectedElements.Keys)
            {
                expectedElements.TryGetValue(current, out expectedCount);
                actualElements.TryGetValue(current, out actualCount);

                if (expectedCount != actualCount)
                {
                    mismatchedElement = current;
                    return true;
                }
            }

            expectedCount = 0;
            actualCount = 0;
            mismatchedElement = null;
            return false;
        }

        private static Dictionary<object, int> GetElementCounts(ICollection collection, out int nullCount)
        {
            Dictionary<object, int> elementCounts = new Dictionary<object, int>();
            nullCount = 0;

            foreach (object element in collection)
            {
                if (element == null)
                {
                    nullCount++;
                    continue;
                }

                elementCounts.TryGetValue(element, out int value);
                value++;
                elementCounts[element] = value;
            }
            return elementCounts;
        }

    }
}
