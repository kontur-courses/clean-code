using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Bold
{
    public class BoldToken : Token
    {
        public BoldToken(int startPosition, int rawLength, string rawText) : base(startPosition, rawLength, rawText)
        {
        }
    }
}