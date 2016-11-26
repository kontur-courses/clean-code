using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Shell;

namespace Markdown
{
    public static class StringExtension
    {
        public static bool IsSubstring(this string substring, string text, int start)
        {
            if (start + substring.Length > text.Length)
            {
                return false;
            }
            return !substring.Where((t, i) => text[start + i] != t).Any();
        }

        public static bool IsIncorrectEndingShell(this string text, int currentPosition)
        {
            return currentPosition >= text.Length || text[currentPosition] == ' ';
        }

        public static bool IsEscapedCharacter(this string text, int currentPosition)
        {
            return currentPosition - 1 >= 0 && currentPosition < text.Length && text[currentPosition - 1] == '\\';
        }
        public static bool IsSurroundedByNumbers(this string text, int startPrefix, int endSuffix)
        {
            int temp;
            return startPrefix > 0 && int.TryParse(text[startPrefix - 1].ToString(), out temp) &&
                   endSuffix + 1 < text.Length && int.TryParse(text[endSuffix + 1].ToString(), out temp);
        }
        public static string RemoveEscapeСharacters(this string text)
        {
            return text.Replace("\\", "");
        }

        public static int GetPositionEndSubstring(this string substring, int startPosition)
        {
            return startPosition + substring.Length - 1;
        }

        public static bool TryMatchSubstring(this string text, string substring, int startPosition, out MatchObject matchObject)
        {
            matchObject = null;
            if (substring.IsSubstring(text, startPosition))
            {
                matchObject = new MatchObject(startPosition, startPosition + substring.Length - 1, new List<Attribute>());
                return true;
            }
            return false;
        }

        public static bool HasSpace(this string text, int position)
        {
            if (position < 0 || position >= text.Length)
            {
                return false;
            }
            return text[position] == ' ';
        }
    }
    
}
