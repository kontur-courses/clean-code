using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Concrete.Header
{
    public class HeaderToken : Token
    {
        public TokenPosition Position { get; }

        public HeaderToken(int startPosition, string rawValue, TokenPosition position) : base(startPosition, rawValue)
        {
            Position = position;
        }
    }
}