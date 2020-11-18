using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Link
{
    public class LinkToken : PairedToken
    {
        public LinkToken(int startPosition, string rawValue, bool isOpening) : base(startPosition, rawValue)
        {
            Type = isOpening ? TokenType.Opening : TokenType.Closing;
        }

        public override TokenType Type { get; } = TokenType.Opening;
    }
}