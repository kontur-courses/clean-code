using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown
{
    public static class StringExtensions
    {
        public static IEnumerable<TagToken> FindAll(this string text, IEnumerable<string> tags)
        {
            var foundIndexes = new HashSet<int>();
            foreach (var substring in tags.OrderByDescending(s => s.Length))
            foreach (var i in text.AllIndexesOf(substring))
            {
                if (foundIndexes.Contains(i)) continue;
                for (var j = i; j < i + substring.Length; j++)
                {
                    foundIndexes.Add(j);
                }

                yield return new TagToken(i, substring.Length);
            }
        }

        public static IEnumerable<int> AllIndexesOf(this string str, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"the string to find may not be empty {value}");
            for (var i = 0; i < str.Length; i += value.Length)
            {
                i = str.IndexOf(value, i, StringComparison.Ordinal);
                if (i == -1)
                    yield break;
                yield return i;
            }
        }
    }
}