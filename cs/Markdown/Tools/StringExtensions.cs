using System.Collections.Generic;
using System.Linq;

namespace Markdown.Tools
{
    public static class StringExtensions
    {
        public static bool ContainsAtIndex(this string s, int start, string pattern)
        {
            if (start + pattern.Length - 1 >= s.Length)
                return false;

            for (var i = 0; i < pattern.Length; i++)
            {
                if (s[i + start] != pattern[i])
                    return false;
            }

            return true;
        }

        public static string GetReversed(this string s)
        {
            return new string(s.Reverse().ToArray());
        }
    }
}