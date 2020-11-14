using MarkdownParser.Infrastructure.Tokenization;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Concrete.Header
{
    public class HeaderTokenBuilder : TokenBuilder<HeaderToken>
    {
        public override string TokenSymbol { get; } = "#";

        public override HeaderToken Create(TokenizationContext context)
        {
            return new HeaderToken(
                context.CurrentStartIndex,
                context.Source.Substring(context.CurrentStartIndex, TokenSymbol.Length),
                context.GetPosition(TokenSymbol));
        }

        public override bool CanCreate(TokenizationContext context) =>
            context.GetPosition(TokenSymbol).OnParagraphStart();
    }
}