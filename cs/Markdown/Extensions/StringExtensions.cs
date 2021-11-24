using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Extensions
{
    public static class StringExtensions
    {
        public static bool InRange(this string str, int position) =>
            position < str.Length && position >= 0;

        public static int LastIndex(this string str) => str.Length - 1;

        public static bool ContainsWhiteSpace(this string str) =>
            str.Any(char.IsWhiteSpace);

        public static bool ContainsDigit(this string str) =>
            str.Any(char.IsDigit);

        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

        public static bool TryGetCharsBehind(this string str, int position, int amount, out char[] chars) => TryGetNextChars(str, position, -amount, out chars);

        public static bool TryGetNextChars(this string str, int position, int amount, out char[] chars)
        {
            if (str.InRange(position + amount))
            {
                chars = str.Substring(position + amount, Math.Abs(amount)).ToCharArray();
                return true;
            }
            chars = default;
            return false;
        }

        public static bool TrySubstring(this string str, int position, int length, out string substring)
        {
            if (str.InRange(position + length - 1))
            {
                substring = str.Substring(position, length);
                return true;
            }
            substring = default;
            return false;
        }

        public static string Substring(this string str, int start, int length, Dictionary<string, string> replaces)
        {
            var substring = str.Substring(start, length);
            foreach (var p in replaces)            
                substring = substring.Replace(p.Key, p.Value);
            return substring;            
        }
    }
}
