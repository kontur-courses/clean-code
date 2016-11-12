using System.Linq;

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
    }
    
}
