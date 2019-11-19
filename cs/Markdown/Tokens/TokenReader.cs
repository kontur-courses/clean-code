using System.Collections.Generic;

namespace Markdown.Tokens
{
    public class TokenReader : ITokenReader
    {
        private readonly ISeparatorHandler separatorHandler;

        public TokenReader(ISeparatorHandler separatorHandler)
        {
            this.separatorHandler = separatorHandler;
        }

        public Token ReadWhileSeparator(string text, int startPosition)
        {
            for (var i = startPosition; i < text.Length; i++)
            {
                if (separatorHandler.IsSeparator(text, i))
                {
                    separatorHandler.GetSeparatorValue(text, i);
                    return new Token(startPosition,
                        text.Substring(startPosition, i - startPosition), false);
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
                yield return nextToken;
                if (currentPosition >= text.Length)
                    break;
                yield return new Token(currentPosition, separatorHandler.GetSeparatorValue(text, currentPosition),
                    true);
                currentPosition += separatorHandler.GetSeparatorLength(text, currentPosition);
            }
        }
    }
}