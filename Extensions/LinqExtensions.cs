using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Our.Extensions
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var element in enumerable)
                action(element);
        }
        
        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action)
        {
            foreach (var element in enumerable)
                await action(element);
        }
        
        public static async UniTask ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, UniTask> action)
        {
            foreach (var element in enumerable)
                await action(element);
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, T except)
        {
            return enumerable.Where(element => !element?.Equals(except) ?? false);
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, IEnumerable<T> except)
        {
            return enumerable.Where(element => !except.Contains(element));
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, params T[] except)
        {
            return enumerable.Where(element => !except.Contains(element));
        }

        public static IEnumerable<string> ToStrings<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Select(element => element?.ToString() ?? "");
        }

        public static IEnumerable<string> ToStrings<T>(this IEnumerable<T> enumerable, Func<T, string> selector)
        {
            return enumerable.Select(selector);
        }

        public static string Join(this IEnumerable<string> strings, string separator)
        {
            return string.Join(separator, strings);
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.OrderBy(x => Guid.NewGuid());
        }

        public static IEnumerable<T> TakeAny<T>(this IEnumerable<T> enumerable, int count)
        {
            var list = enumerable.ToList();
            var result = new List<T>();
            var random = new Random();
            for (var i = 0; i < count; i++)
            {
                var index = random.Next(0, list.Count);
                result.Add(list[index]);
                list.RemoveAt(index);
            }

            return result;
        }
    }
}