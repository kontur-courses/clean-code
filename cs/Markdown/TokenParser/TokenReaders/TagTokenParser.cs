using Markdown.Data;

namespace Markdown.TokenParser.TokenReaders
{
    public class TagTokenParser : ITokenReader
    {
        private readonly string tag;

        public TagTokenParser(string tag)
        {
            this.tag = tag;
        }

        public TokenReaderResult ReadToken(string text, int startingPosition)
        {
            return startingPosition + tag.Length > text.Length || text.Substring(startingPosition, tag.Length) != tag
                ? new TokenReaderResult(false)
                : new TokenReaderResult(true, tag.Length, new Token(TokenType.Tag, tag));
        }
    }
}