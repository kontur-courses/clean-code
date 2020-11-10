using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Default
{
    public class TextToken : Token
    {
        public TextToken(int startPosition, int rawLength, string rawText) : base(startPosition, rawLength, rawText)
        {
        }
    }
}