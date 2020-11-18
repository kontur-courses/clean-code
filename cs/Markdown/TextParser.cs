using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TextParser : IParser
    {
        private readonly IReadOnlyCollection<ITokenGetter> tokenGetters;

        public TextParser(IReadOnlyCollection<ITokenGetter> tokenGetters)
        {
            this.tokenGetters = tokenGetters;
        }

        public List<TextToken> GetTextTokens(string text)
        {
            var tokens = new List<TextToken>();
            if (text == null)
                throw new NullReferenceException(nameof(text) + " was null");

            if (text.Length == 0)
                return tokens;

            var startPosition = 0;

            for (var index = 0; index < text.Length && startPosition < text.Length; index++)
            {
                var currentToken = TryGetToken(text, index, startPosition);

                if (currentToken == null) continue;

                if (currentToken.Type != TokenType.Text)
                    currentToken.SubTokens = GetTextTokens(currentToken.Text);

                startPosition += currentToken.Length;

                var updatedToken = UpdateLastTextToken(currentToken, tokens);
                if (updatedToken != null) continue;

                tokens.Add(currentToken);
            }

            return tokens;
        }

        private TextToken TryGetToken(string text, int index, int startPosition)
        {
            foreach (var tokenGetter in tokenGetters)
                if (tokenGetter.CanCreateToken(text, index, startPosition))
                    return tokenGetter.GetToken(text, index, startPosition);

            return null;
        }

        private TextToken UpdateLastTextToken(TextToken tokenToAdd, List<TextToken> tokens)
        {
            return tokens.LastOrDefault() != null ? tokens.Last().AddSameToken(tokenToAdd) : null;
        }
    }
}