using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TokenFinder
    {
        private readonly List<TokenType> tokensTypes = new List<TokenType>
        {
            new TokenType(TokenTypeEnum.SingleUnderscore, "_", "em", TokenLocationType.InlineToken),
            new TokenType(TokenTypeEnum.DoubleUnderscore, "__", "strong", TokenLocationType.InlineToken),
            new TokenType(TokenTypeEnum.Lattice, "#", "h1", TokenLocationType.StartingToken),
            new TokenType(TokenTypeEnum.DoubleLattice, "##", "h2", TokenLocationType.StartingToken),
            new TokenType(TokenTypeEnum.TripleLattice, "###", "h3", TokenLocationType.StartingToken),
            new TokenType(TokenTypeEnum.QuadrupleLattice, "####", "h4", TokenLocationType.StartingToken),
            new TokenType(TokenTypeEnum.Star, "*", "li", TokenLocationType.StartingToken)
        };

        public IEnumerable<SingleToken> FindTokens(string paragraph)
        {
            var inlineTokens = new InlineTokenFinder()
                .FindInlineTokens(paragraph, tokensTypes.Where(t => t.TokenLocationType == TokenLocationType.InlineToken));
            var validInlineTokens = new InlineTokensValidator().GetValidTokens(inlineTokens);
            var startingTokens = new StartingTokenFinder()
                .FindStartingTokens(paragraph, tokensTypes.Where(t => t.TokenLocationType == TokenLocationType.StartingToken));

            return validInlineTokens.Union(startingTokens);
        }
    }
}
