using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Markdown
{
    public static class StringExtentions
    {
        //To Refactor
        public static string[] SplitKeppSeparators(this string s, char[] separators,
            bool unionSameSeparators = true, int maxSizeUnion = 2)
        {
            var splitStr = new List<string>();
            var sep = new HashSet<char>(separators);
            var part = new StringBuilder();
            char previous = default;
            var countSeparatorInUnion = maxSizeUnion;
            foreach (var elem in s)
            {
                if (sep.Contains(elem) && part.Length != 0)
                {
                    splitStr.Add(part.ToString());
                    splitStr.Add(elem.ToString());
                    countSeparatorInUnion += 1;
                    part.Clear();
                }
                else if (sep.Contains(elem))
                {
                    if (previous == elem && unionSameSeparators && countSeparatorInUnion > 0)
                    {
                        splitStr.RemoveAt(splitStr.Count - 1);
                        splitStr.Add(string.Concat(new[] { previous, elem }));
                        countSeparatorInUnion -= 2;
                    }
                    else
                    {
                        splitStr.Add(elem.ToString());
                        countSeparatorInUnion += 1;
                    }
                }
                else
                    part.Append(elem);
                previous = elem;
            }
            if (splitStr.Count == 0|| part.Length !=0)
                splitStr.Add(part.ToString());
            return splitStr.ToArray();
        }

    }
}
