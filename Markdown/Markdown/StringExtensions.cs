using System.Collections.Generic;
using System.Linq;
using Markdown.Tag;

namespace Markdown
{
    public static class StringExtensions
    {
        public static string RemoveEscapedSymbols(this string text, List<MdType> types)
        {
            var position = 0;

            while (position < text.Length - 1)
            {
                var symbol = text.LookAt(position).ToString();
                var nextSymbol = text.LookAt(position + 1).ToString();

                if (symbol.IsEscapedSymbol(nextSymbol, types))
                    text = text.Remove(position, 1);

                position++;
            }

            return text;
        }

        private static bool IsEscapedSymbol(this string symbol, string nextSymbol, IEnumerable<MdType> types)
        {
            var symbols = types.Select(MdTagSymbolDetector.Detect).ToList();
            return symbol == "\\" && symbols.Contains(nextSymbol);
        }

        public static int FindCloseTagIndex(this string text, ITag tag)
        {
            for (var i = tag.OpenIndex + 2; i < text.Length - tag.Length + 1; i++)
            {
                var symbolAfterTag = text.LookAt(i + tag.Length);
                var symbolBeforeTag = text.LookAt(i - 1);

                if (text.Substring(i, tag.Length) == tag.Symbol && (char.IsWhiteSpace(symbolAfterTag) ||
                                                                    i == text.Length - tag.Length)
                                                                && char.IsLetter(symbolBeforeTag))
                    return i;
            }

            return -1;
        }

        public static string GetBodyInside(this string text, ITag tag)
        {
            return text.Substring(tag.OpenIndex + tag.Length, tag.CloseIndex - tag.OpenIndex - tag.Length);
        }

        public static char LookAt(this string text, int index)
        {
            var isIndexInBorders = index <= text.Length - 1 && index >= 0;
            return isIndexInBorders ? text[index] : '\0';
        }
    }
}