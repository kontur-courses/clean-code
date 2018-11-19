using System.Collections.Generic;
using System.Linq;
using Markdown.Markups;

namespace Markdown
{
    public static class StringExtensions
    {
        public static bool IsEscaped(this string text, int currentPosition)
        {
            return currentPosition > 0 && text.Substring(currentPosition - 1, 1) == @"\";
        }

        public static bool ContainsAt(this string text, string substring, int startPosition)
        {
            if (startPosition + substring.Length > text.Length)
                return false;
            return text.Substring(startPosition, substring.Length) == substring;
        }

        public static bool IsWhiteSpace(this string text, int position)
        {
            return position >= text.Length || string.IsNullOrWhiteSpace(text.Substring(position, 1));
        }

        public static string RemoveEscapes(this string text, List<Markup> markups)
        {
            foreach (var markup in markups)
            {
                var markupChar = markup.Opening.First();
                var subStringToDelete = $@"\{markupChar}";
                text = text.Replace(subStringToDelete, string.Empty);
            }
            return text;
        }
    }
}
