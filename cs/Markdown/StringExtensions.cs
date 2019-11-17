using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    static class StringExtensions
    {
        public static IEnumerable<Token> GetTokensWithSubStrsEntries(this string str, string[] substrs)
        {
            if (substrs == null)
                throw new ArgumentNullException();
            IEnumerable<Token> result = new Token[0];
            foreach (var substr in substrs)
                result = result.Concat(
                    str
                    .GetSubStrStartIndexes(substr)
                    .Select(i => new Token() { StartIndex = i, Length = substr.Length, Str = str }));
            return result.Distinct();
        }

        private static IEnumerable<int> GetSubStrStartIndexes(this string str, string substr)
        {
            if (substr == string.Empty)
                yield break;
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