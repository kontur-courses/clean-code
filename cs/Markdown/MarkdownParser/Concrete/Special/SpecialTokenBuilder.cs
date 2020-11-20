using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Special
{
    public class SpecialTokenBuilder : TokenBuilder<SpecialToken>
    {
        private readonly SpecialTokenType tokenType;
        public override string TokenSymbol { get; }

        public SpecialTokenBuilder(SpecialTokenType tokenType)
        {
            this.tokenType = tokenType;
            TokenSymbol = SpecialToken.GetValue(tokenType);
        }

        public override SpecialToken Create(string raw, int startIndex) => new SpecialToken(startIndex, tokenType);
        public override bool CanCreate(string _, int __) => true;
    }
}