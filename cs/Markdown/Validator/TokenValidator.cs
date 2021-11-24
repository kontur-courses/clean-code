using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.Validator
{
    public static class TokenValidator
    {
        public static List<IToken> ValidateTokens(List<IToken> tokens, string text)
        {
            var validatedTokens = tokens.Where(token => IsValid(token, text)).ToList();

            validatedTokens = RemoveWrongNestedTokens(validatedTokens);

            return validatedTokens;
        }

        private static List<IToken> RemoveWrongNestedTokens(List<IToken> tokens)
        {
            var validatedTokens = new List<IToken>();

            foreach (var token in tokens)
            {
                if (!token.Tag.IsPairMdTag || token.Tag.MdTag == "_")
                {
                    validatedTokens.Add(token);
                    continue;
                }

                var surroundingTokens = tokens.Where(t => token.IsNestedInAnotherToken(t));
                if (surroundingTokens.All(t => t.Tag.MdTag != "_"))
                    validatedTokens.Add(token);
            }

            return validatedTokens;
        }

        private static bool IsValid(IToken token, string text)
        {
            if (token.Tag.IsPairMdTag)
            {
                return !HasDigitsInside(token) &&
                       !IsEmptyToken(token) &&
                       !IsInsideOfDifferentWords(token, text);
            }

            return true;
        }

        private static bool HasDigitsInside(IToken token)
        {
            return token.Value.Any(char.IsDigit);
        }

        private static bool IsEmptyToken(IToken token)
        {
            return token.Value.All(c => c == token.Tag.MdTag[0]);
        }

        private static bool IsInsideOfDifferentWords(IToken token, string text)
        {
            return token.Value.Contains(' ') &&
                   token.StartPosition - 1 >= 0 &&
                   token.EndPosition + 1 < text.Length &&
                   !char.IsWhiteSpace(text[token.StartPosition - 1]) &&
                   !char.IsWhiteSpace(text[token.EndPosition + 1]);
        }
    }
}