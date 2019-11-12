using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    static class StringExtensions
    {
        public static IEnumerable<Token> GetAllSubStrEntries(this string str, string[] substrs)
        {
            if (substrs == null)
                throw new ArgumentNullException();
            var result = new List<Token>();
            foreach (var substr in substrs)
                result.AddRange(
                    str
                    .GetAllSubStrStartIndexes(substr)
                    .Select(i => new Token() { StartIndex = i, Count = substr.Length, Str = str }));
            return result.Distinct();
        }

        private static IEnumerable<int> GetAllSubStrStartIndexes(this string str, string substr)
        {
            var index = -1;
            var startIndex = 0;
            while (startIndex < str.Length && (index = str.IndexOf(substr, startIndex)) != -1)
            {
                startIndex = index + 1;
                yield return index;
            }
        }
    }
}