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

        public override bool CanCreateOnPosition(TokenPosition position) =>
            !position.InsideDigit() &&
            !position.OnDigitBorder() &&
            !position.WhitespaceFramed();
    }
}