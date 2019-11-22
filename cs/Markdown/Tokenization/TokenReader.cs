using System.Collections.Generic;

namespace Markdown.Tokenization
{
    public class TokenReader : ITokenReader
    {
        private readonly ITokenReaderConfiguration tokenReaderConfiguration;

        public TokenReader(ITokenReaderConfiguration tokenReaderConfiguration)
        {
            this.tokenReaderConfiguration = tokenReaderConfiguration;
        }

        public Token ReadWhileSeparator(string text, int startPosition)
        {
            for (var i = startPosition; i < text.Length; i++)
            {
                if (tokenReaderConfiguration.IsSeparator(text, i))
                {
                    tokenReaderConfiguration.GetSeparatorValue(text, i);
                    var tokenValue = text.Substring(startPosition, i - startPosition);
                    return new Token(startPosition, tokenValue, false);
                }
            }

            return new Token(startPosition, text.Substring(startPosition), false);
        }

        public IEnumerable<Token> ReadAllTokens(string text)
        {
            var currentPosition = 0;
            while (true)
            {
                var nextToken = ReadWhileSeparator(text, currentPosition);
                currentPosition = nextToken.Position + nextToken.Value.Length;
                if (nextToken.Value.Length > 0)
                    yield return nextToken;
                if (currentPosition >= text.Length)
                    break;
                var separatorValue = tokenReaderConfiguration.GetSeparatorValue(text, currentPosition);
                yield return new Token(currentPosition, separatorValue, true);
                currentPosition += tokenReaderConfiguration.GetSeparatorLength(text, currentPosition);
            }
        }
    }
}