using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Default
{
    public class ParagraphToken : Token
    {
        public ParagraphToken(int startPosition, string rawValue) : base(startPosition, rawValue)
        {
        }
    }
}