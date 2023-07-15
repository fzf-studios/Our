using System;

namespace Our.Extensions
{
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
    }
}