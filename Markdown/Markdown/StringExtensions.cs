using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class StringExtensions
    {
        public static bool ContainsFrom(this string str, string substr, int position)
        {
            return str.Length >= substr.Length + position &&
                   str.Substring(position, substr.Length) == substr;
        }

        public static string Segment(this string str, int startIndex, int endIndex)
        {
            return str.Substring(startIndex, endIndex - startIndex);
        }
    }
}