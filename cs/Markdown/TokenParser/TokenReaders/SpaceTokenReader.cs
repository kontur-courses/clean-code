using System.Text;
using Markdown.Data;

namespace Markdown.TokenParser.TokenReaders
{
    public class SpaceTokenReader : ITokenReader
    {
        public TokenReaderResult ReadToken(string text, int startingPosition)
        {
            var token = new StringBuilder();
            while (startingPosition < text.Length && char.IsWhiteSpace(text[startingPosition]))
            {
                token.Append(text[startingPosition]);
                startingPosition++;
            }
            return token.Length <= 0 
                ? new TokenReaderResult(false)
                : new TokenReaderResult(true, token.Length, new Token(TokenType.Space, token.ToString()));
        }
    }
}