using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class StringExtentions
    {
        public static List<string> SplitKeepSeparators(this string s, char[] separators,
            bool unionSameSeparators = true, int maxSizeUnion = 2)
        {
            var splittedString = new List<string>();
            var sep = new HashSet<char>(separators);
            var part = new StringBuilder();
            char previous = default;
            var countSeparatorInUnion = 0;
            foreach (var elem in s)
            {
                if (sep.Contains(elem) && part.Length != 0)
                {
                    splittedString.Add(part.ToString());
                    splittedString.Add(elem.ToString());
                    countSeparatorInUnion++;
                    part.Clear();
                }
                else if (sep.Contains(elem))
                {
                    countSeparatorInUnion++;
                    if (previous == elem && unionSameSeparators && countSeparatorInUnion == maxSizeUnion)
                    {
                        UnionSameString(maxSizeUnion - 1, splittedString, previous, elem);
                        countSeparatorInUnion = 0;
                    }
                    else
                        splittedString.Add(elem.ToString());
                    if (countSeparatorInUnion == maxSizeUnion)
                        countSeparatorInUnion--;
                }
                else
                {
                    part.Append(elem);
                    countSeparatorInUnion = 0;
                }
                previous = elem;
            }
            if (splittedString.Count == 0 || part.Length != 0)
                splittedString.Add(part.ToString());
            return splittedString;
        }

        public static void UnionSameString(int sizeUnion, List<string> slpitedString, params char[] elemensToUnion)
        {
            for (int i = 0; i < sizeUnion; i++)
                slpitedString.RemoveAt(slpitedString.Count - 1);
            slpitedString.Add(string.Concat(elemensToUnion));
        }

        public static bool IsDigit(this string text)
        {
            if (text.Length == 0) return false;
            return !text.Any(c => char.IsLetter(c) || char.IsPunctuation(c));
        }
    }
}
