using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace _Game.Scripts.Utility.Extension
{
    public static class EnumerableExtension
    {
        private static readonly Random random = new Random();
        
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.OrderBy(_ => random.Next());
        }

        public static T Random<T>(this IList<T> list)
        {
            var index = random.Next(0, list.Count);
            return list[index];
        }

        public static T RandomOrDefault<T>(this IList<T> list, int minCount = 1)
        {
            return list.Count >= minCount ? list.Random() : default;
        }

        public static IEnumerable<T> TakeAtLeast<T>(this IEnumerable<T> enumerable, int minCount)
        {
            var list = enumerable.ToList();
            return list.Count >= minCount ? list : Enumerable.Empty<T>();
        }
        
        public static bool TryCastValue<TKey, TValue>(this IDictionary<TKey, object> dictionary, TKey key, out TValue result)
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                try
                {
                    result = (TValue) value;
                    return true;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            result = default;
            return false;
        }

        public static bool Check<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue expected)
        {
            return dictionary.TryGetValue(key, out var actual) && Equals(actual, expected);
        }
        
        public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
        {
            foreach (var element in source)
            {
                target.Add(element);
            }
        }

        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> other)
        {
            return dictionary.Concat(other)
                .GroupBy(pair => pair.Key)
                .ToDictionary(pairs => pairs.Key, pairs => pairs.Last().Value);
        }
        
        public static HashSet<R> ToHashSet<T, R>(this IEnumerable<T> enumerable, Func<T, R> valueSelector)
        {
            return new HashSet<R>(enumerable.Select(valueSelector));
        }

        public static bool TryGetFirst<T>(this IEnumerable<T> source, out T result)
        {
            switch (source)
            {
                case null:
                    throw new ArgumentNullException(nameof(source));
                case IList<T> sourceList:
                    if (sourceList.Count > 0)
                    {
                        result = sourceList[0];
                        return true;
                    }
                    break;
                default:
                    using (var enumerator = source.GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            result = enumerator.Current;
                            return true;
                        }
                        break;
                    }
            }
            result = default;
            return false;
        }
        
        public static bool TryGetFirst<T>(this IEnumerable<T> enumerable, Predicate<T> predicate, out T result)
        {
            foreach (var item in enumerable)
            {
                if (predicate(item))
                {
                    result = item;
                    return true;
                }
            }
            
            result = default;
            return false;
        }

        public static bool TryGetLast<T>(this IEnumerable<T> source, out T result)
        {
            switch (source)
            {
                case IList<T> list:
                {
                    var count = list.Count;
                    if (count > 0)
                    {
                        result = list[count - 1];
                        return true;
                    }
                    break;
                }
                default:
                {
                    using var e = source.GetEnumerator();
                    if (e.MoveNext()) {
                        do {
                            result = e.Current;
                        } while (e.MoveNext());
                        return true;
                    }
                    break;
                }
            }

            result = default;
            return false;
        }
        
        public static string ToString<T>(this IEnumerable<T> enumerable, string separator)
        {
            return string.Join(separator, enumerable);
        }

        public static string BuildString<T>(this IDictionary<string, T> dictionary)
        {
            var builder = new StringBuilder();
            foreach (var pair in dictionary)
            {
                var value = pair.Value;
                var valueString = value == null ? "null" : value.ToString();
                builder.Append(pair.Key).Append(": ").AppendLine(valueString);
            }
            return builder.ToString();
        }

        public static IEnumerable<T[]> Chunk<T>(this IEnumerable<T> source, int chunkSize) 
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToArray());
        }
        
        public static T[][] Chunk<T>(this IEnumerable<T> source, int chunkCount, int chunkSize)
        {
            if (chunkCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(chunkCount));

            var chunks = new T[chunkCount][];
            for (var i = 0; i < chunkCount; i++)
            {
                chunks[i] = new T[chunkSize];
            }

            var index = 0;
            var maxIndex = chunkCount * chunkSize - 1;
            foreach (var item in source)
            {
                chunks[index % chunkCount][index / chunkCount] = item;
                if (index == maxIndex) break;
                index++;
            }

            return chunks;
        }

        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            return source.Distinct(new KeyEqualityComparer<T, TKey>(keySelector));
        }

        private class KeyEqualityComparer<T, TKey> : IEqualityComparer<T>
        {
            private readonly EqualityComparer<TKey> keyComparer = EqualityComparer<TKey>.Default;
            private readonly Func<T, TKey> keySelector;

            public KeyEqualityComparer(Func<T, TKey> keySelector)
            {
                this.keySelector = keySelector;
            }

            public bool Equals(T x, T y)
            {
                return keyComparer.Equals(keySelector(x), keySelector(y));
            }

            public int GetHashCode(T obj)
            {
                return keyComparer.GetHashCode(keySelector(obj));
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var index = 0;
            foreach (var item in source)
            {
                action(item, index);
                index++;
            }
        }
        
        public static void CustomForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action, Action<Exception> errorCallback)
        {
            foreach (var item in source)
            {
                try
                {
                    action(item);
                }
                catch (Exception e)
                {
                    errorCallback(e);
                }
            }
        }

        public static int AddAll<T>(this IList<T> list, IEnumerable<T> source)
        {
            var count = 0;
            foreach (var item in source)
            {
                list.Add(item);
                count++;
            }
            return count;
        }
        
        public static IEnumerable<Tuple<TFirst, TSecond>> ZipDefaults<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second)
        {
            return first.ZipDefaults(second, (item1, item2) => new Tuple<TFirst, TSecond>(item1, item2));
        }
        
        public static IEnumerable<TResult> ZipDefaults<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            bool firstMoveNext;
            bool secondMoveNext;
            using var enum1 = first.GetEnumerator();
            using var enum2 = second.GetEnumerator();
            while ((firstMoveNext = enum1.MoveNext()) & (secondMoveNext = enum2.MoveNext()))
            {
                yield return resultSelector(enum1.Current, enum2.Current);
            }
            if (firstMoveNext)
            {
                yield return resultSelector(enum1.Current, default);
                while (enum1.MoveNext())
                {
                    yield return resultSelector(enum1.Current, default);
                }
            }
            else if (secondMoveNext)
            {
                yield return resultSelector(default, enum2.Current);
                while (enum2.MoveNext())
                {
                    yield return resultSelector(default, enum2.Current);
                }
            }
        }

        public static IEnumerable<T> PadRight<T>(this IEnumerable<T> source, int size, T value = default)
        {
            var i = 0;
            foreach (var item in source)
            {
                yield return item;
                i++;
            }

            for (; i < size; i++)
            {
                yield return value;
            }
        }
    }
}
