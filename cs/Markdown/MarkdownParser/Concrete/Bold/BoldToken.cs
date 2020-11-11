using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Bold
{
    public class BoldToken : Token
    {
        public BoldToken(int startPosition, string rawValue) : base(startPosition, rawValue)
        {
        }
    }
}