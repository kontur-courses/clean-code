using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class MarkdownTokenExtensions
    {
        public static bool ValidOpeningPosition(this MarkdownToken markdownToken, string text, int startIndex)
        {
            if (startIndex < 0 || startIndex + markdownToken.Template.Length >= text.Length)
                return false;

            if (markdownToken.HasWhitespaceAfterToken(text, startIndex) || markdownToken.IsValidForSomeTag(text, startIndex))
                return false;

            return markdownToken.Template.Equals(text.Substring(startIndex, markdownToken.Template.Length));
        }

        public static bool ValidClosingPosition(this MarkdownToken markdownToken, string text, int startIndex)
        {
            if (startIndex <= 0 || startIndex + markdownToken.Template.Length > text.Length)
                return false;

            if (HasWhitespaceBeforeToken(text, startIndex) || markdownToken.IsValidForSomeTag(text, startIndex))
                return false;

            return markdownToken.Template.Equals(text.Substring(startIndex, markdownToken.Template.Length));
        }

        public static MarkdownToken GetClosingToken(this List<MarkdownToken> markdownTokens, string text, int startIndex)
        {
            return markdownTokens.FirstOrDefault(markdownToken => ValidClosingPosition(markdownToken, text, startIndex));
        }

        public static MarkdownToken GetOpeningToken(this List<MarkdownToken> markdownTokens, string text, int startIndex)
        {
            return markdownTokens.FirstOrDefault(markdownToken => markdownToken.ValidOpeningPosition(text, startIndex));
        }

        private static bool HasWhitespaceAfterToken(this MarkdownToken markdownToken, string text, int startIndex)
        {
            return text[startIndex + markdownToken.Template.Length] == ' ';
        }

        private static bool HasWhitespaceBeforeToken(string text, int startIndex)
        {
            return text[startIndex - 1] == ' ';
        }

        private static bool HasBackslashBeforeToken(string text, int startIndex)
        {
            return startIndex != 0 && text[startIndex - 1] == '\\';
        }

        private static bool TokenEndsAfterTemplateLength(this MarkdownToken markdownToken, string text, int startIndex)
        {
            return !(startIndex + markdownToken.Template.Length < text.Length &&
                    text[startIndex + markdownToken.Template.Length] == text[startIndex]);
        }

        private static bool TokenStartsInThisPosition(string text, int startIndex)
        {
            return startIndex > 0 && text[startIndex - 1] == text[startIndex];
        }

        private static bool IsValidForSomeTag(this MarkdownToken markdownToken, string text, int startIndex)
        {
            return HasBackslashBeforeToken(text, startIndex) ||
                   TokenStartsInThisPosition(text, startIndex) ||
                   !markdownToken.TokenEndsAfterTemplateLength(text, startIndex);
        }
    }
}
