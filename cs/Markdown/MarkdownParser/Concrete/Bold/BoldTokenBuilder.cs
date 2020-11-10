using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Concrete.Bold
{
    public class BoldTokenBuilder : TokenBuilder<BoldToken>
    {
        public override string TokenSymbol { get; } = "__";

        public override BoldToken Create(TokenizationContext context)
        {
            throw new System.NotImplementedException();
        }

        public override bool CanCreateOnPosition(TokenPosition position)
        {
            throw new System.NotImplementedException();
        }
    }
}