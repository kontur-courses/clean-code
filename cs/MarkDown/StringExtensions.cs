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
            var res = specSymbols.Aggregate(text, (current, s) => current.Replace($"\\{s}", $"{s}"));
            var r1 = res.Replace("\\\\", '\0'.ToString());
            var r2 = r1.Replace("\\", "");
            return r2.Replace('\0'.ToString(), "\\");
        }


        public static bool IsOpeningTag(this string text, int startPosition, string specialSymbol)
        {
            var previousChar = text.SeekFromPosition(startPosition, -1);
            var nextChar = text.SeekFromPosition(startPosition, specialSymbol.Length);
            return (previousChar == '\0' || !char.IsDigit(previousChar))
                   && previousChar != '\\'
                   && nextChar != '\0'
                   && char.IsLetter(nextChar)
                   && text.Substring(startPosition).StartsWith(specialSymbol);
        }

        public static bool IsClosingTag(this string text, int startPosition, string specialSymbol)
        {

            var previousChar = text.SeekFromPosition(startPosition, -1);
            var nextChar = text.SeekFromPosition(startPosition, specialSymbol.Length);
            return (nextChar == '\0' || !char.IsDigit(nextChar)) 
                   && char.IsLetter(previousChar) 
                   && previousChar != '\\' 
                   && nextChar.ToString() != specialSymbol 
                   && text.Substring(startPosition).StartsWith(specialSymbol);
        }
    }
}
