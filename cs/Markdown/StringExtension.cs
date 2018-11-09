using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class StringExtension
    {

        public static bool IsSpanElementOpening(this string markdown, int currentPosition, List<ISpanElement> spanElements)
        {
            return spanElements.Any(e => IsSubstring(markdown, currentPosition, e.GetOpeningIndicator()));
        }

        public static bool IsSubstring(this string markdown, int currentPosition, string substring)
        {
            return string.Join(string.Empty, markdown.Skip(currentPosition)).StartsWith(substring);
        }

        public static bool IsWrongBoundary(this string markdown, int currentPosition)
        {
            if (currentPosition >= markdown.Length)
                return true;
            if (markdown.ElementAt(currentPosition) == ' ')
                return true;
            return false;
        }

        public static bool IsPreviousCharEscape(this string markdown, int currentPosition)
        {
            return currentPosition - 1 >= 0 && currentPosition < markdown.Length && markdown.ElementAt(currentPosition - 1) == '\\';
        }


        public static bool IsExistingSpanElement(this string markdown, ISpanElement spanElement, int closingIndex)
        {
            return spanElement == null || markdown.IsSubstring(closingIndex + 1, spanElement.GetClosingIndicator());
        }

        public static string RemoveEscapes(this string markdown)
        {
            return markdown.Replace("\\", "");
        }
    }
}
