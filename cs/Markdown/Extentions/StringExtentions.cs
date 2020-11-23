using Markdown.Extentions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Markdown
{
    public static class StringExtentions
    {
        public static List<string> SplitKeepSeparators(this string str, char[] separators)
        {
            var separatorsSet = new HashSet<char>(separators);
            var splitString = new List<string>();
            var part = new StringBuilder();
            foreach (var chr in str)
                if (separatorsSet.Contains(chr))
                {
                    if (part.Length != 0)
                        splitString.Add(part.ToString());
                    splitString.Add(chr.ToString());
                    part.Clear();
                }
                else
                    part.Append(chr);
            if (part.Length != 0)
                splitString.Add(part.ToString());
            return splitString;
        }

        public static List<string> UnionSameStringByTwo(this List<string> splittedString, params string[] without)
        {
            var combinedString = new List<string>();
            var withoutTheseStrings = new HashSet<string>(without);
            var skip = false;
            string previous = default;
            string current = default;
            foreach (var bigram in splittedString.GetBigrams())
            {
                previous = bigram.Item1;
                current = bigram.Item2;
                if (withoutTheseStrings.Contains(previous))
                    combinedString.Add(previous);
                else if (previous == current && !skip)
                {
                    combinedString.Add(string.Concat(previous, current));
                    skip = true;
                }
                else if (!skip)
                    combinedString.Add(previous);
                else
                    skip = false;
            }
            return combinedString;
        }

        public static bool IsDigit(this string text)
        {
            if (text.Length == 0) return false;
            return !text.Any(c => char.IsLetter(c) || char.IsPunctuation(c));
        }
    }
}
