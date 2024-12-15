using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
#if UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Our.Extensions
{
    public static class RandomProvider
    {
        public static Random Instance { get; } = new();
    }

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
        
#if UNITASK
        public static async UniTask ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, UniTask> action)
        {
            foreach (var element in enumerable)
                await action(element);
        }
#endif

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
            for (var i = 0; i < count; i++)
            {
                var index = RandomProvider.Instance.Next(0, list.Count);
                result.Add(list[index]);
                list.RemoveAt(index);
            }

            return result;
        }
        
        public static T GetRandomItem<T>(this IEnumerable<T> enumerable)
        {
            return enumerable
                .TakeAny(1)
                .FirstOrDefault();
        }
    }
    
    public static class StringExtensions
    {
        public static string ReplaceVariables(this string source, Dictionary<string, string> variables)
        {
            variables.ForEach(variablePair => source = source
                .Replace($"{{{variablePair.Key}}}", variablePair.Value));
            return source;
        }
    }
    
    public static class EnumExtensions
    {
        public static T Next<T>(this T current) where T : struct, Enum
        {
            var values = (T[])Enum.GetValues(typeof(T));
            var currentIndex = Array.IndexOf(values, current);
            var nextIndex = (currentIndex + 1) % values.Length;
            return values[nextIndex];
        }

        public static T Previous<T>(this T current) where T : struct, Enum
        {
            var values = (T[])Enum.GetValues(typeof(T));
            var currentIndex = Array.IndexOf(values, current);
            var previousIndex = (currentIndex - 1 + values.Length) % values.Length;
            return values[previousIndex];
        }
        
        public static T GetRandom<T>() where T : Enum
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            return values[RandomProvider.Instance.Next(values.Length)];
        }
    }

    public static class GenericExtensions
    {
        public static T Clone<T>(this T original)
        {
            var serializeObject = JsonConvert.SerializeObject(original);
            var copy = JsonConvert.DeserializeObject<T>(serializeObject);
            return copy;
        }
    }
}