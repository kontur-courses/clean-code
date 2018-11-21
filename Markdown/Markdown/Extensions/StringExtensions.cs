using System.Collections.Generic;
using System.Linq;
using Markdown.Tag;

namespace Markdown.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveEscapedSymbols(this string text, List<string> symbols)
        {
            var position = 0;

            while (position < text.Length - 1)
            {
                var symbol = text.LookAt(position).ToString();
                var nextSymbol = text.LookAt(position + 1).ToString();

                if (symbol.IsEscapedSymbol(nextSymbol, symbols))
                    text = text.Remove(position, 1);

                position++;
            }

            return text;
        }

        private static bool IsEscapedSymbol(this string symbol, string nextSymbol, IEnumerable<string> symbols)
        {
            return symbol == "\\" && symbols.Contains(nextSymbol);
        }

        public static char LookAt(this string text, int index)
        {
            var isIndexInBorders = index <= text.Length - 1 && index >= 0;
            return isIndexInBorders ? text[index] : '\0';
        }
    }
}