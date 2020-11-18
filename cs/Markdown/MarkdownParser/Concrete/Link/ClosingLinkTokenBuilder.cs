using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;
using MarkdownParser.Infrastructure.Tokenization.Workers;

namespace MarkdownParser.Concrete.Link
{
    public class ClosingLinkTokenBuilder : TokenBuilder<LinkToken>
    {
        public override string TokenSymbol { get; } = ">";

        public override LinkToken Create(TokenizationContext context) =>
            new LinkToken(context.CurrentStartIndex, TokenSymbol, false);

        public override bool CanCreate(TokenizationContext context)
        {
            var position = context.GetPosition(TokenSymbol);
            return !position.AfterWhitespace() &&
                   position.HasAnyFlag(TokenPosition.BeforeWhitespace, TokenPosition.ParagraphEnd);
        }
    }
}