using MarkdownParser.Infrastructure.Abstract;

namespace MarkdownParser.Infrastructure.Impl.Bold
{
    public class TokenBold : Token
    {
        public TokenBold(int startPosition, int rawLength, string rawText) : base(startPosition, rawLength, rawText)
        {
        }
    }
}