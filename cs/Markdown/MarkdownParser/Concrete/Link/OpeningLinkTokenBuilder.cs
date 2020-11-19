using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;
using MarkdownParser.Infrastructure.Tokenization.Workers;

namespace MarkdownParser.Concrete.Link
{
    public class OpeningLinkTokenBuilder : TokenBuilder<LinkToken>
    {
        public override string TokenSymbol { get; } = "<";

        public override LinkToken Create(string raw, int startIndex) =>
            new LinkToken(startIndex, TokenSymbol, true);

        public override bool CanCreate(string raw, int startIndex)
        {
            var position = TokenHelpers.GetPosition(raw, startIndex, TokenSymbol);
            return position.HasAnyFlag(TokenPosition.BeforeDigit, TokenPosition.BeforeWord) && 
                   position.HasAnyFlag(TokenPosition.AfterWhitespace, TokenPosition.ParagraphStart);
        }
    }
}