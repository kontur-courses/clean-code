using Markdown.Data;

namespace Markdown.TokenParser.TokenReaders
{
    public class TextTokenReader : ITokenReader
    {
        public TokenReaderResult ReadToken(string text, int startingPosition)
        {
            return new TokenReaderResult(true, 1, new Token(TokenType.Text, text[startingPosition].ToString()));
        }
    }
}