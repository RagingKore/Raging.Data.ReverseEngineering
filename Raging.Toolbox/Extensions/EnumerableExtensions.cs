using System;
using System.Collections.Generic;
using System.Linq;

namespace Raging.Toolbox.Extensions
{
    public static class EnumerableExtensions
    {
        public static string Join(this IEnumerable<string> value, string separator = ",")
        {
            return string.Join(separator, value);
        }

        public static string Join(this string[] value, string separator = ",")
        {
            return string.Join(separator, value);
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration) action(item);
        }

        public static bool IsAny<T>(this IEnumerable<T> enumerator)
        {
            return enumerator != null && enumerator.Any();
        }
    }
}