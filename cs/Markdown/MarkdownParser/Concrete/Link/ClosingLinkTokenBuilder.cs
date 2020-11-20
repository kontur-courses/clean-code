using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;
using MarkdownParser.Infrastructure.Tokenization.Workers;

namespace MarkdownParser.Concrete.Link
{
    public class ClosingLinkTokenBuilder : TokenBuilder<LinkToken>
    {
        public override string TokenSymbol { get; } = ">";

        public override LinkToken Create(string raw, int startIndex) =>
            new LinkToken(startIndex, TokenSymbol, false);

        public override bool CanCreate(string raw, int startIndex)
        {
            var position = TokenPositionHelpers.GetPosition(raw, startIndex, TokenSymbol);
            return !position.AfterWhitespace() &&
                   position.HasAnyFlag(TokenPosition.BeforeWhitespace, TokenPosition.ParagraphEnd);
        }
    }
}