using MarkdownParser.Infrastructure.Tokenization;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Concrete.Italic
{
    public class ItalicTokenBuilder : TokenBuilder<ItalicToken>
    {
        public override string TokenSymbol { get; } = "_";

        public override ItalicToken Create(TokenizationContext context)
        {
            return new ItalicToken(
                context.CurrentStartIndex,
                context.Source.Substring(context.CurrentStartIndex, TokenSymbol.Length),
                context.GetPosition(TokenSymbol));
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