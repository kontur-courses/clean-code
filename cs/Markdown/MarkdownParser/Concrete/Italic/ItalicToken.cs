using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Italic
{
    public class ItalicToken : Token
    {
        public ItalicToken(int startPosition, int rawLength, string rawText)
            : base(startPosition, rawLength, rawText)
        {
        }
    }
}