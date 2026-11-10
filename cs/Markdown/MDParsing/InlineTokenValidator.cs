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


            if (delimiterCount == 1 && IsInsideWord(tokenBeforeOpen, tokenAfterClose))
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

        private static bool IsInsideWord(Token? before, Token? after)
        {
            bool isLetterBefore = before.HasValue && before.Value.UnderscoreCount == 0 &&
                                  before.Value.Text.Length > 0 &&
                                  char.IsLetter(before.Value.Text[^1]);

            bool isLetterAfter = after.HasValue && after.Value.UnderscoreCount == 0 &&
                                 after.Value.Text.Length > 0 &&
                                 char.IsLetter(after.Value.Text[0]);

            return isLetterBefore && isLetterAfter;
        }
    }
}