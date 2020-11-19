using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.TokenReaders;
using Markdown.Tokens;

namespace Markdown
{
    public class TextParser : IParser
    {
        private readonly IReadOnlyCollection<ITokenReader> tokenReaders;

        public TextParser(IReadOnlyCollection<ITokenReader> tokenReaders)
        {
            this.tokenReaders = tokenReaders;
        }

        public List<IToken> GetTextTokens(string text)
        {
            var tokens = new List<IToken>();
            if (text == null)
                throw new ArgumentNullException(nameof(text) + " was null");

            if (text.Length == 0)
                return tokens;

            for (var start = 0; start < text.Length - 1; start++)
            for (var end = start; end < text.Length; end++)
            {
                var currentToken = TryGetToken(text, start, end);

                if (currentToken == null) continue;

                if (currentToken is ITagToken tokenWithTags)
                    tokenWithTags.SubTokens = GetTextTokens(tokenWithTags.TextWithoutTags);

                tokens.Add(currentToken);
                start += currentToken.Length - 1;
                break;
            }

            return tokens;
        }

        private IToken TryGetToken(string text, int start, int end)
        {
            return tokenReaders
                .Select(tokenGetter => tokenGetter.TyrGetToken(text, start, end))
                .FirstOrDefault(token => token != null);
        }
    }
}