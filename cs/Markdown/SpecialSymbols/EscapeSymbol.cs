using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.SpecialSymbols
{
    class EscapeSymbol
    {
        public const char Escape = '\\';

        public static string RemoveAllEscapeSymbolEntries(string text)
        {
            if (text == null)
                throw new ArgumentNullException();
            var builder = new StringBuilder();
            var startIndex = 0;
            foreach (var (indexToRemove, escapedSymbolsIndex) in FindSortedPairsEscapeAndEscapedSymbols(text))
            {
                var endIndex = indexToRemove;
                builder.Append(text, startIndex, endIndex - startIndex);
                startIndex = endIndex + 1;
            }
            builder.Append(text, startIndex, text.Length - startIndex);
            return builder.ToString();
        }

        public static (int escapeSymbolIndex, int escapedSymbolsIndex)[] FindSortedPairsEscapeAndEscapedSymbols(string text)
        {
            if (text == null)
                throw new ArgumentNullException();
            var result = new List<(int escapeSymbolIndex, int escapedSymbolsIndex)>();
            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == Escape)
                {
                    result.Add((i, i + 1));
                    i++;
                }
            }
            if (result.Count > 0 && result.Last().escapedSymbolsIndex >= text.Length)
                throw new FormatException();
            return result.ToArray();
        }
    }
}