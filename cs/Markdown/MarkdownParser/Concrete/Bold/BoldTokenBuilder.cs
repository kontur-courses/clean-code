using MarkdownParser.Infrastructure.Tokenization;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Concrete.Bold
{
    public class BoldTokenBuilder : TokenBuilder<BoldToken>
    {
        public override string TokenSymbol { get; } = "__";

        public override BoldToken Create(TokenizationContext context)
        {
            return new BoldToken(
                context.CurrentStartIndex,
                context.Source.Substring(context.CurrentStartIndex, TokenSymbol.Length));
        }

        public override bool CanCreate(TokenizationContext context)
        {
            var position = context.GetPosition(TokenSymbol);
            return !position.InsideDigit() &&
                   !position.OnDigitBorder() &&
                   !position.WhitespaceFramed();
        }
    }
}