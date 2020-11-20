using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Workers;

namespace MarkdownParser.Concrete.Header
{
    public class HeaderTokenBuilder : TokenBuilder<HeaderToken>
    {
        public override string TokenSymbol { get; } = "#";

        public override HeaderToken Create(string raw, int startIndex)
        {
            return new HeaderToken(startIndex, raw.Substring(startIndex, TokenSymbol.Length),
                TokenPositionHelpers.GetPosition(raw, startIndex, TokenSymbol));
        }

        public override bool CanCreate(string raw, int startIndex) =>
            TokenPositionHelpers.GetPosition(raw, startIndex, TokenSymbol).OnParagraphStart();
    }
}