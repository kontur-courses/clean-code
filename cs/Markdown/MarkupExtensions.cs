using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class MarkupExtensions
    {
        public static bool ValidOpeningPosition(this Markup markup, string text, int startIndex)
        {
            if (startIndex < 0 || startIndex + markup.Template.Length >= text.Length)
                return false;

            if (markup.HasWhitespaceAfterToken(text, startIndex) || markup.IsValidForSomeTag(text, startIndex))
                return false;

            return markup.Template.Equals(text.Substring(startIndex, markup.Template.Length));
        }

        public static bool ValidClosingPosition(this Markup markup, string text, int startIndex)
        {
            if (startIndex <= 0 || startIndex + markup.Template.Length > text.Length)
                return false;

            if (HasWhitespaceBeforeToken(text, startIndex) || markup.IsValidForSomeTag(text, startIndex))
                return false;

            return markup.Template.Equals(text.Substring(startIndex, markup.Template.Length));
        }

        public static Markup GetClosingMarkup(this List<Markup> markups, string text, int startIndex)
        {
            return markups.FirstOrDefault(markup => ValidClosingPosition(markup, text, startIndex));
        }

        public static Markup GetOpeningMarkup(this List<Markup> markups, string text, int startIndex)
        {
            return markups.FirstOrDefault(markup => markup.ValidOpeningPosition(text, startIndex));
        }

        private static bool HasWhitespaceAfterToken(this Markup markup, string text, int startIndex)
        {
            return text[startIndex + markup.Template.Length] == ' ';
        }

        private static bool HasWhitespaceBeforeToken(string text, int startIndex)
        {
            return text[startIndex - 1] == ' ';
        }

        private static bool HasBackslashBeforeToken(string text, int startIndex)
        {
            return startIndex != 0 && text[startIndex - 1] == '\\';
        }

        private static bool TokenEndsAfterTemplateLength(this Markup markup, string text, int startIndex)
        {
            return !(startIndex + markup.Template.Length < text.Length &&
                    text[startIndex + markup.Template.Length] == text[startIndex]);
        }

        private static bool TokenStartsInThisPosition(string text, int startIndex)
        {
            return startIndex > 0 && text[startIndex - 1] == text[startIndex];
        }

        private static bool IsValidForSomeTag(this Markup markup, string text, int startIndex)
        {
            return HasBackslashBeforeToken(text, startIndex) ||
                   TokenStartsInThisPosition(text, startIndex) ||
                   !markup.TokenEndsAfterTemplateLength(text, startIndex);
        }
    }
}
