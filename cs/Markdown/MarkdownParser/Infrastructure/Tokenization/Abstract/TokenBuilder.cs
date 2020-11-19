namespace MarkdownParser.Infrastructure.Tokenization.Abstract
{
    public abstract class TokenBuilder<TToken> : ITokenBuilder where TToken : Token
    {
        public abstract string TokenSymbol { get; }

        public abstract TToken Create(string raw, int startIndex);
        Token ITokenBuilder.Create(string raw, int startIndex) => Create(raw, startIndex);

        public abstract bool CanCreate(string raw, int startIndex);
    }
}