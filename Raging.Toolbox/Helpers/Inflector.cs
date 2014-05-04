using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Raging.Toolbox.Helpers
{
    public static class Inflector
    {
        /// <summary>
        /// Separates the input words with underscore
        /// </summary>
        /// <param name="input">The string to be underscored</param>
        /// <returns></returns>
        public static string Underscore(this string input)
        {
            return
                Regex.Replace(
                    Regex.Replace(
                        Regex.Replace(input, @"([A-Z]+)([A-Z][a-z])", "$1_$2")
                        , @"([a-z\d])([A-Z])", "$1_$2")
                    , @"[-\s]", "_")
                .ToLower();
        }

        /// <summary>
        /// By default, pascalize converts strings to UpperCamelCase also removing underscores and dashes.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Pascalize(this string input)
        {
            //TODO: Optimize this...

            while (input.StartsWith("_") || input.StartsWith("-"))
            {
                input = input.Remove(0, 1);
            }

            var temp = input
                .Replace("-", "_")
                .Underscore();

            //var tmp = input.Split('_').Select(s => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s));

            return Regex.Replace(temp, "(?:^|_)(.)", match => match.Groups[1].Value.ToUpper());
        }
    }
}