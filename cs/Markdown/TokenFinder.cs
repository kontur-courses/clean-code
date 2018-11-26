using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            new TokenType(TokenTypeEnum.Star, "*", "li", TokenLocationType.StartingToken),
            new TokenType(TokenTypeEnum.EndLine, "\n", "", TokenLocationType.EndLineToken)
        };

        public IEnumerable<SingleToken> FindTokens(string paragraph)
        {
            var tokens = new List<SingleToken>();

            var startingIsPossible = true;

            for (var index = 0; index < paragraph.Length; index++)
            {
                var openingAndClosingTokens = tokensTypes
                    .Where(t => t.TokenLocationType == TokenLocationType.InlineToken)
                    .GetOpenAndClosingToken(paragraph, index);

                if (openingAndClosingTokens.OpeningToken != null)
                    tokens.Add(new SingleToken(openingAndClosingTokens.OpeningToken, index, LocationType.Opening));
                if (openingAndClosingTokens.ClosingToken != null)
                    tokens.Add(new SingleToken(openingAndClosingTokens.ClosingToken, index, LocationType.Closing));

                if (startingIsPossible)
                {
                    var startingToken = GetStartingToken(paragraph, index);
                    if (startingToken != null)
                        tokens.Add(startingToken);

                    startingIsPossible = false;
                }

                var endLineToken = GetEndLineToken(paragraph, index);
                if (endLineToken != null)
                {
                    tokens.Add(endLineToken);
                    startingIsPossible = true;
                }

            }

            return tokens;
        }

        private SingleToken GetEndLineToken(string paragraph, int index)
        {
            if (paragraph[index] == '\n')
            {
                return new SingleToken(
                    new TokenType(TokenTypeEnum.EndLine, "\n", null, TokenLocationType.EndLineToken), index,
                    LocationType.Single);
            }

            return null;
        }

        private SingleToken GetStartingToken(string paragraph, int index)
        {
            if (!TryGetWordFromThisPosition(paragraph, index, out var word)) return null;

            TryMatchWordToStartingTokens(word, index, new HashSet<TokenType>(),
                tokensTypes.Where(t => t.TokenLocationType == TokenLocationType.StartingToken),
                out var startingToken);

            return startingToken;
        }

        private bool TryGetWordFromThisPosition(string paragraph, int currentIndex, out string word)
        {
            var wordBuilder = new StringBuilder();
            word = "";
            var shift = 0;
            if (currentIndex == 0 || paragraph[currentIndex - 1] == ' ' || paragraph[currentIndex - 1] == '\n')
                while (paragraph.Length > currentIndex + shift && paragraph[currentIndex + shift] != ' ')
                {
                    wordBuilder.Append(paragraph[currentIndex + shift]);
                    shift++;
                }

            word = wordBuilder.ToString();

            return word.Length > 0;
        }

        private bool TryMatchWordToStartingTokens(string word, int currentIndex, HashSet<TokenType> usedTokens,
            IEnumerable<TokenType> startingTokensTypes, out SingleToken token)
        {
            foreach (var startingTokenType in startingTokensTypes)
                if (!usedTokens.Contains(startingTokenType) && startingTokenType.Template == word)
                {
                    token = new SingleToken(startingTokenType, currentIndex, LocationType.Single);
                    return true;
                }

            token = null;
            return false;
        }
    }
}
