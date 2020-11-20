using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Workers;

namespace MarkdownParser.Concrete.Italic
{
    public class ItalicTokenBuilder : TokenBuilder<ItalicToken>
    {
        public override string TokenSymbol { get; } = "_";

        public override ItalicToken Create(string raw, int startIndex)
        {
            return new ItalicToken(
                startIndex,
                raw.Substring(startIndex, TokenSymbol.Length),
                TokenPositionHelpers.GetPosition(raw, startIndex, TokenSymbol));
        }

        public override bool CanCreate(string raw, int startIndex)
        {
            var position = TokenPositionHelpers.GetPosition(raw, startIndex, TokenSymbol);
            return !position.InsideDigit() &&
                   !position.OnDigitBorder() &&
                   !position.WhitespaceFramed();
        }
    }
}