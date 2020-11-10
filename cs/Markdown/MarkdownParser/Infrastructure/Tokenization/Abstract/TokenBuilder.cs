using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Infrastructure.Tokenization.Abstract
{
    public abstract class TokenBuilder<TToken> : ITokenBuilder where TToken : Token
    {
        public abstract string TokenSymbol { get; }

        public abstract TToken Create(TokenizationContext context);
        Token ITokenBuilder.Create(TokenizationContext context) => Create(context);

        public abstract bool CanCreateOnPosition(TokenPosition position);
    }
}