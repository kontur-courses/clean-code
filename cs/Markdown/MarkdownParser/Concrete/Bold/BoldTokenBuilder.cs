using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Workers;

namespace MarkdownParser.Concrete.Bold
{
    public class BoldTokenBuilder : TokenBuilder<BoldToken>
    {
        public override string TokenSymbol { get; } = "__";

        public override BoldToken Create(string raw, int startIndex)
        {
            return new BoldToken(startIndex, raw.Substring(startIndex, TokenSymbol.Length),
                TokenPositionHelpers.GetPosition(raw, startIndex, TokenSymbol));
        }

        public override bool CanCreate(string raw, int startIndex)
        {
            var position = TokenPositionHelpers.GetPosition(raw, startIndex, TokenSymbol);
            return !position.InsideDigit() &&
                   !position.OnDigitBorder() &&
                   !position.WhitespaceFramed();
        }
    }
}