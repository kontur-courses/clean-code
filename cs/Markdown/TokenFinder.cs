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
            new TokenType(TokenTypeEnum.Star, "\n", "", TokenLocationType.EndLineToken)
        };

        public IEnumerable<SingleToken> FindTokens(string paragraph)
        {
            var tokens = new List<SingleToken>();

            var startingIsPossible = true;

            for (var index = 0; index < paragraph.Length; index++)
            {
                var openingToken = tokensTypes.GetOpeningToken(paragraph, index);
                var closingToken = tokensTypes.GetClosingToken(paragraph, index);

                if (openingToken != null)
                    tokens.Add(new SingleToken(openingToken, index, LocationType.Opening));
                if (closingToken != null)
                    tokens.Add(new SingleToken(closingToken, index, LocationType.Closing));

                if (startingIsPossible)
                {
                    if (!TryGetWordFromThisPosition(paragraph, index, out var word)) continue;

                    startingIsPossible = TryMatchWordToTokens(word, index, new HashSet<TokenType>(),
                        tokensTypes.Where(t => t.TokenLocationType == TokenLocationType.StartingToken),
                        out var startingToken);
                    if (startingIsPossible)
                        tokens.Add(startingToken);
                }

                if (paragraph[index] == '\n')
                {
                    tokens.Add(new SingleToken(
                        new TokenType(TokenTypeEnum.EndLine, "\n", null, TokenLocationType.EndLineToken), index,
                        LocationType.Single));

                    startingIsPossible = true;
                }

            }

            return tokens;
        }

        private bool TryGetWordFromThisPosition(string paragraph, int currentIndex, out string word)
        {
            var wordBuilder = new StringBuilder();
            word = "";
            var shift = 0;
            if (currentIndex == 0 || paragraph[currentIndex - 1] == ' ')
                while (paragraph.Length > currentIndex + shift && paragraph[currentIndex + shift] != ' ')
                {
                    wordBuilder.Append(paragraph[currentIndex + shift]);
                    shift++;
                }

            word = wordBuilder.ToString();

            return word.Length > 0;
        }

        private bool TryMatchWordToTokens(string word, int currentIndex, HashSet<TokenType> usedTokens,
            IEnumerable<TokenType> tokensTypes, out SingleToken token)
        {
            foreach (var tokenType in tokensTypes)
                if (!usedTokens.Contains(tokenType) && tokenType.Template == word)
                {
                    token = new SingleToken(tokenType, currentIndex, LocationType.Single);
                    return true;
                }

            token = null;
            return false;
        }
    }
}
