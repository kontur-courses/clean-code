using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.Validator
{
    public class TokenValidator : IValidator
    {
        public List<IToken> ValidateTokens(List<IToken> tokens, string text)
        {
            var validatedTokens = tokens.Where(token => IsValid(token, text)).ToList();

            validatedTokens = RemoveWrongNestedTokens(validatedTokens);

            return validatedTokens;
        }

        private List<IToken> RemoveWrongNestedTokens(List<IToken> tokens)
        {
            var validatedTokens = new List<IToken>();

            foreach (var token in tokens)
            {
                if (!token.TagType.IsPairMdTag || token.TagType.MdTag == "_")
                {
                    validatedTokens.Add(token);
                    continue;
                }

                var surroundingTokens = tokens.Where(t => token.IsNestedInToken(t));
                if (surroundingTokens.All(t => t.TagType.MdTag != "_"))
                    validatedTokens.Add(token);
            }

            return validatedTokens;
        }

        private bool IsValid(IToken token, string text)
        {
            if (token.TagType.IsPairMdTag)
            {
                return !HasDigitsInside(token) &&
                       !IsEmptyToken(token) &&
                       !IsInsideOfDifferentWords(token, text);
            }

            return true;
        }

        private bool HasDigitsInside(IToken token)
        {
            return token.Value.Any(char.IsDigit);
        }

        private bool IsEmptyToken(IToken token)
        {
            return token.Value.All(c => c == token.TagType.MdTag[0]);
        }

        private bool IsInsideOfDifferentWords(IToken token, string text)
        {
            return token.Value.Contains(' ') &&
                   token.StartPosition - 1 >= 0 &&
                   token.EndPosition + 1 < text.Length &&
                   !char.IsWhiteSpace(text[token.StartPosition - 1]) &&
                   !char.IsWhiteSpace(text[token.EndPosition + 1]);
        }
    }
}