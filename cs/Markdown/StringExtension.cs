using System;
using System.Collections.Generic;
using System.Linq;
namespace Markdown
{
    public static class StringExtension
    {
        public static bool IsNonWhitespaceAt(this string str, int position)
        {
            return str.IsInBounds(position) && !Char.IsWhiteSpace(str[position]);
        }

        public static bool IsCharAt(this string str, int position, char ch)
        {
            return str.IsInBounds(position) && str[position] == ch;
        }

        private static bool IsInBounds(this string str, int position)
        {
            return position >= 0 && position < str.Length;
        }

        public static int GetIndexOfFirstMatching(this string str, int startPosition, Func<char, bool> matchingFunc)
        {
            int currentPosition = startPosition;
            while (currentPosition < str.Length && !matchingFunc(str[currentPosition]))
                currentPosition++;
            return currentPosition;
        }

        public static bool ContainsMatchingSymbolsBetween(this string str, int leftPosition, int rightPosition,
            Func<char, bool> matchingFunc)
        {
            for(var index = leftPosition; index < rightPosition; index++)
                if (matchingFunc(str[index]))
                    return true;
            return false;
        }
    }
}
