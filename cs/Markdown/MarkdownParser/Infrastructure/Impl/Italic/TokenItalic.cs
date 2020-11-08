using MarkdownParser.Infrastructure.Abstract;

namespace MarkdownParser.Infrastructure.Impl.Italic
{
    public class TokenItalic : Token
    {
        public TokenItalic(int startPosition, int rawLength, string rawText)
            : base(startPosition, rawLength, rawText)
        {
        }
    }
}