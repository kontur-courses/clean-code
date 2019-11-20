using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class StringExtensions
    {
        public static List<int> GetSubstringIndices(this string source, string substring)
        {
            var indices = new List<int>();

            var index = source
                .IndexOf(substring, 0, StringComparison.Ordinal);
            while (index > -1)
            {
                indices.Add(index);
                index = source
                    .IndexOf(substring, index + substring.Length, StringComparison.Ordinal);
            }

            return indices;
        }
    }
}