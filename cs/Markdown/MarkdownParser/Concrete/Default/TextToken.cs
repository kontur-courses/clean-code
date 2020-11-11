using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Default
{
    public class TextToken : Token
    {
        public TextToken(int startPosition, string rawValue) : base(startPosition, rawValue)
        {
        }
    }
}