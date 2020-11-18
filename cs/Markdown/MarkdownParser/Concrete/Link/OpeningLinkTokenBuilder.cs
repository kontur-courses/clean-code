using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;
using MarkdownParser.Infrastructure.Tokenization.Workers;

namespace MarkdownParser.Concrete.Link
{
    public class OpeningLinkTokenBuilder : TokenBuilder<LinkToken>
    {
        public override string TokenSymbol { get; } = "<";

        public override LinkToken Create(TokenizationContext context) =>
            new LinkToken(context.CurrentStartIndex, TokenSymbol, true);

        public override bool CanCreate(TokenizationContext context)
        {
            var position = context.GetPosition(TokenSymbol);
            return position.HasAnyFlag(TokenPosition.BeforeDigit, TokenPosition.BeforeWord) && 
                   position.HasAnyFlag(TokenPosition.AfterWhitespace, TokenPosition.ParagraphStart);
        }
    }
}