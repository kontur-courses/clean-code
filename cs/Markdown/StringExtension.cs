using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class StringExtension
    {
        public static ElementInfo GetOpeningTag(this string markdown, int openingIndex, string indicator)
        {
            var tag = MatchTag(markdown, indicator, openingIndex);
            if (!markdown.IsWhiteSpace(openingIndex + indicator.Length)
                && tag != null)
            {
                return tag;
            }

            return null;
        }

        public static ElementInfo GetClosingTag(this string markdown, int closingIndex, string indicator)
        {
            var tag = MatchTag(markdown, indicator, closingIndex);
            if (!markdown.IsWhiteSpace(closingIndex - 1) && tag != null)
            {
                return tag;
            }

            return null;
        }

        private static ElementInfo MatchTag(string markdown, string substring, int openingIndex)
        {
            var elementInfo = markdown.TryMatchSubstring(substring, openingIndex);
            if (!markdown.IsEscapeChar(openingIndex-1) && elementInfo != null)
            {
                return elementInfo;
            }

            return null;
        }


        public static bool IsSubstring(this string markdown, int currentPosition, string substring)
        {
            if (currentPosition + substring.Length > markdown.Length)
                return false;
            for (var i = 0; i < substring.Length; i++)
                if (markdown.ElementAt(currentPosition + i) != substring.ElementAt(i))
                    return false;
            return true;
        }

        public static bool IsEscapeChar(this string markdown, int currentPosition)
        {
            return currentPosition >= 0 
                   && currentPosition < markdown.Length 
                   && markdown.ElementAt(currentPosition) == '\\';
        }

        public static string RemoveEscapes(this string markdown)
        {
            return markdown.Replace("\\", "");
        }

        public static bool IsWhiteSpace(this string markdown, int position)
        {
            return position >= 0
                   && position < markdown.Length 
                   && markdown.ElementAt(position) == ' ';
        }

        public static ElementInfo TryMatchSubstring(this string markdown, string substring, int openingIndex)
        {
            ElementInfo info = null;
            if (markdown.IsSubstring(openingIndex, substring))
            {
                info = new ElementInfo(openingIndex, openingIndex + substring.Length -1);
            }

            return info;
        }

        public static bool IsSpanElementClosing(this string markdown, int currentPosition, string substring, List<ISpanElement> spanElements)
        {
            return spanElements.Any(e => IsSubstring(markdown, currentPosition, substring));
        }
    }
}
