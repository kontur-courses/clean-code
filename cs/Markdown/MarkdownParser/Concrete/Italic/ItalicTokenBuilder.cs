using MarkdownParser.Infrastructure.Tokenization;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Concrete.Italic
{
    public class ItalicTokenBuilder : TokenBuilder<ItalicToken>
    {
        public override string TokenSymbol { get; } = "_";

        public override ItalicToken Create(TokenizationContext context) =>
            new ItalicToken(context.CurrentStartIndex,
                context.Source.Substring(context.CurrentStartIndex, TokenSymbol.Length));

        public override bool CanCreate(TokenizationContext context)
        {
            var position = TokenHelpers.GetPosition(context.Source, context.CurrentStartIndex, TokenSymbol.Length);
            return !position.InsideDigit() &&
                   !position.OnDigitBorder() &&
                   !position.WhitespaceFramed();
        }
    }
}