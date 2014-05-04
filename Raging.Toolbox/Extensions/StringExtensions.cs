using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Raging.Toolbox.Extensions
{
    public static class StringExtensions
    {
        public static bool Like(this string value, string compare)
        {
            return String.Compare(value, compare, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static bool NotLike(this string value, string compare)
        {
            return !Like(value, compare);
        }

        public static bool LikeAll(this string value, params string[] compare)
        {
            return compare.All(value.Like);
        }

        public static bool NotLikeAll(this string value, params string[] compare)
        {
            return compare.All(value.NotLike);
        }

        public static bool LikeAny(this string value, params string[] compare)
        {
            return compare.Any(value.Like);
        }

        public static bool NotLikeAny(this string value, params string[] compare)
        {
            return compare.Any(value.NotLike);
        }

        public static bool ContainsAny(this string value, params string[] compare)
        {
            return compare.Any(comparableValue => value.ToLowerInvariant().Contains(comparableValue.ToLowerInvariant()));
        }

        public static string FormatWith(this string value, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, value, args);
        }

        public static bool IsBlank(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNotBlank(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static T To<T>(this string value)
        {
            return value.To<T>(CultureInfo.InvariantCulture);
        }

        public static T To<T>(this string value, CultureInfo ci)
        {
            var type = typeof(T);

            if(type.IsPrimitive)
                return (T) Convert.ChangeType(value, type, ci);

            if(type.IsEnum)
                return (T) Enum.Parse(type, value); // Yeah, we're making an assumption

            return (T) TypeDescriptor
                           .GetConverter(typeof(T))
                           .ConvertFrom(value);
        }

        public static bool TryTo<T>(this string value, out T convertedValue)
        {
            return value.TryTo(CultureInfo.InvariantCulture, out convertedValue);
        }

        public static bool TryTo<T>(this string value, CultureInfo ci, out T convertedValue)
        {
            try
            {
                convertedValue = value.To<T>();
                return true;
            }
            catch(Exception)
            {
                convertedValue = default( T );
                return false;
            }
        }

        /// <summary>
        /// A string extension method that converts a value to base 64, using utf8 encoding.
        /// </summary>
        ///
        /// <param name="value">The value to act on.</param>
        ///
        /// <returns>
        /// value as a string.
        /// </returns>
        public static string ToBase64(this string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// A string extension method that converts a value from base 64, using utf8 encoding.
        /// </summary>
        ///
        /// <param name="value">The value to act on.</param>
        ///
        /// <returns>
        /// A string.
        /// </returns>
        public static string FromBase64(this string value)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }
    }
}