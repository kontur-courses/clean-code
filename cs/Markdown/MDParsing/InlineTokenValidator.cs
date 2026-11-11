using Markdown.MDParsing;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Parsing
{
    public static class InlineTokenValidator
    {
        public static bool IsValid(
            Token? tokenBeforeOpen,
            Token? tokenAfterClose,
            List<Token> innerTokens,
            int delimiterCount)
        {
            if (IsWhitespaceOnly(innerTokens))
                return false;

            if (IsBoundedByDigits(tokenBeforeOpen, tokenAfterClose))
                return false;

            if (!FollowedByNonSpace(innerTokens))
                return false;

            if (!PrecededByNonSpace(innerTokens))
                return false;

            if (delimiterCount == 1 && IsAcrossWords(tokenBeforeOpen, tokenAfterClose, innerTokens))
                return false;

            if (IsCrossing(innerTokens, delimiterCount))
                return false;

            return true;
        }

        private static bool IsWhitespaceOnly(List<Token> tokens)
        {
            var text = string.Join("", tokens.Select(t => t.Text ?? new string('_', t.UnderscoreCount)));
            return string.IsNullOrWhiteSpace(text);
        }

        private static bool IsBoundedByDigits(Token? before, Token? after)
        {
            bool isDigitBefore = before.HasValue && before.Value.UnderscoreCount == 0 &&
                                 before.Value.Text.Length > 0 &&
                                 char.IsDigit(before.Value.Text[^1]);

            bool isDigitAfter = after.HasValue && after.Value.UnderscoreCount == 0 &&
                                after.Value.Text.Length > 0 &&
                                char.IsDigit(after.Value.Text[0]);

            return isDigitBefore && isDigitAfter;
        }

        private static bool FollowedByNonSpace(List<Token> innerTokens)
        {
            var firstTextToken = innerTokens.FirstOrDefault(t => t.UnderscoreCount == 0 && t.Text != null);
            if (firstTextToken.Text == null) return false;
            return firstTextToken.Text.Length > 0 && !char.IsWhiteSpace(firstTextToken.Text[0]);
        }

        private static bool PrecededByNonSpace(List<Token> innerTokens)
        {
            var lastTextToken = innerTokens.LastOrDefault(t => t.UnderscoreCount == 0 && t.Text != null);
            if (lastTextToken.Text == null) return false;
            return lastTextToken.Text.Length > 0 && !char.IsWhiteSpace(lastTextToken.Text[^1]);
        }

        private static bool IsAcrossWords(Token? before, Token? after, List<Token> innerTokens)
        {
            bool isNonSpaceBefore = before.HasValue && before.Value.UnderscoreCount == 0 &&
                                    before.Value.Text.Length > 0 &&
                                    !char.IsWhiteSpace(before.Value.Text[^1]);

            bool isNonSpaceAfter = after.HasValue && after.Value.UnderscoreCount == 0 &&
                                   after.Value.Text.Length > 0 &&
                                   !char.IsWhiteSpace(after.Value.Text[0]);

            bool hasSpaceBetween = innerTokens.Any(t => t.UnderscoreCount == 0 &&
                                                         t.Text != null &&
                                                         t.Text.Contains(" "));

            return isNonSpaceBefore && isNonSpaceAfter && hasSpaceBetween;
        }

        private static bool IsCrossing(List<Token> innerTokens, int currentDelimiterCount)
        {
            if (currentDelimiterCount == 2)
            {

                int singleUnderscores = innerTokens.Sum(t => t.UnderscoreCount % 2);
                return singleUnderscores % 2 != 0;
            }
            else
            {

                int doubleUnderscores = innerTokens.Sum(t => t.UnderscoreCount / 2);
                return doubleUnderscores % 2 != 0;
            }
        }
    }
}