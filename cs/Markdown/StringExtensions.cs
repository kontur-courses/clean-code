using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class StringExtensions
    {
        public static IEnumerable<(string substring, int index)> FindAll(this string text, IEnumerable<string> substrings)
        {
            return 
                from substring in substrings 
                from i in text.AllIndexesOf(substring) 
                select (substring, i);
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