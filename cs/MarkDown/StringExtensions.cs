using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkDown
{
    public static class StringExtensions
    {
        public static char SeekFromPosition(this string text, int currentPosition, int delta) => 
            currentPosition + delta >= text.Length || currentPosition + delta < 0
            ? '\0'
            : text[currentPosition + delta];

        public static string RemoveScreening(this string text, IEnumerable<string> specSymbols)
        {
            return specSymbols
                .Aggregate(text, (current, s) => current.Replace($"\\{s}", $"{s}"))
                .Replace("\\\\", '\0'.ToString())
                .Replace("\\", "")
                .Replace('\0'.ToString(), "\\");
        }


        public static bool IsOpeningTag(this string text, int startPosition, string specialSymbol)
        {
            var previousChar = text.SeekFromPosition(startPosition, -1);
            var nextChar = text.SeekFromPosition(startPosition, specialSymbol.Length);
            return (previousChar == '\0' || !char.IsDigit(previousChar))
                   && previousChar != '\\'
                   && nextChar != '\0'
                   && char.IsLetterOrDigit(nextChar)
                   && text.Substring(startPosition).StartsWith(specialSymbol);
        }

        public static bool IsClosingTag(this string text, int startPosition, string specialSymbol)
        {

            var previousChar = text.SeekFromPosition(startPosition, -1);
            var nextChar = text.SeekFromPosition(startPosition, specialSymbol.Length);
            return (nextChar == '\0' || !char.IsDigit(nextChar)) 
                   && char.IsLetterOrDigit(previousChar) 
                   && previousChar != '\\' 
                   && nextChar.ToString() != specialSymbol 
                   && text.Substring(startPosition).StartsWith(specialSymbol);
        }
    }
}
