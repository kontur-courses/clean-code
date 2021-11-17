using System;
using System.Collections;
using System.Collections.Generic;

namespace Markdown
{
    public static class StringExtensions
    {
        public static (string substring, int index)[] FindAll(this string text, string[] substrings)
        {
            var foundSubstrings = new List<(string, int)>();
            for (var i = 0; i < text.Length; i++)
            {
                foreach (var substring in substrings)
                {
                    var index = text.IndexOf(substring, i, substring.Length, StringComparison.Ordinal);
                    if (index != -1)
                        foundSubstrings.Add((substring, index));
                }
            }

            return foundSubstrings.ToArray();
        }
    }
}