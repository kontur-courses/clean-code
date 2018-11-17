using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class TokenTypeExtensions
    {
        public static bool ValidOpeningPosition(this TokenType tokenType, string text, int startIndex)
        {
            if (startIndex < 0 || startIndex + tokenType.Template.Length >= text.Length)
                return false;

            if (tokenType.HasWhitespaceAfterToken(text, startIndex) || tokenType.IsValidForSomeTag(text, startIndex))
                return false;

            return tokenType.Template.Equals(text.Substring(startIndex, tokenType.Template.Length));
        }

        public static bool ValidClosingPosition(this TokenType tokenType, string text, int startIndex)
        {
            if (startIndex <= 0 || startIndex + tokenType.Template.Length > text.Length)
                return false;

            if (HasWhitespaceBeforeToken(text, startIndex) || tokenType.IsValidForSomeTag(text, startIndex))
                return false;

            return tokenType.Template.Equals(text.Substring(startIndex, tokenType.Template.Length));
        }

        public static TokenType GetClosingToken(this List<TokenType> tokens, string text, int startIndex)
        {
            return tokens.FirstOrDefault(token => ValidClosingPosition(token, text, startIndex));
        }

        public static TokenType GetOpeningToken(this List<TokenType> tokens, string text, int startIndex)
        {
            return tokens.FirstOrDefault(token => token.ValidOpeningPosition(text, startIndex));
        }

        private static bool HasWhitespaceAfterToken(this TokenType tokenType, string text, int startIndex)
        {
            return text[startIndex + tokenType.Template.Length] == ' ';
        }

        private static bool HasWhitespaceBeforeToken(string text, int startIndex)
        {
            return text[startIndex - 1] == ' ';
        }

        private static bool HasBackslashBeforeToken(string text, int startIndex)
        {
            return startIndex != 0 && text[startIndex - 1] == '\\';
        }

        private static bool TokenEndsAfterTemplateLength(this TokenType tokenType, string text, int startIndex)
        {
            return !(startIndex + tokenType.Template.Length < text.Length &&
                    text[startIndex + tokenType.Template.Length] == text[startIndex]);
        }

        private static bool TokenStartsInThisPosition(string text, int startIndex)
        {
            return startIndex > 0 && text[startIndex - 1] == text[startIndex];
        }

        private static bool IsValidForSomeTag(this TokenType tokenType, string text, int startIndex)
        {
            return HasBackslashBeforeToken(text, startIndex) ||
                   TokenStartsInThisPosition(text, startIndex) ||
                   !tokenType.TokenEndsAfterTemplateLength(text, startIndex);
        }
    }
}
