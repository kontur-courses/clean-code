using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class TokenExtensions
    {
        public static string GetHTMLTag(this Token token)
        {
            var closeSymbol = token.IsOpen ? "" : "/";

            return $"<{closeSymbol}{token.Tag.HTML}>";
        }

        public static void RemoveStrongInEmphasis(this List<Token> tokens)
        {
            var controversialTokens = tokens
                .Where(token => token.Tag.MD == "__" || token.Tag.MD == "_")
                .OrderBy(token => token.Position)
                .ToList();

            if (controversialTokens.Count == 0)
                return;

            for (var i = 0; i < controversialTokens.Count; i++)
            {
                if (i + 1 < controversialTokens.Count &&
                    IsStrongInEmphasis(controversialTokens[i], controversialTokens[i + 1]))
                {
                    var openStrongTokenIndex = i - 1;
                    while (openStrongTokenIndex >= 0 && controversialTokens[openStrongTokenIndex].Tag.MD != "__")
                        openStrongTokenIndex--;

                    tokens.Remove(controversialTokens[openStrongTokenIndex]);
                    tokens.Remove(controversialTokens[i]);
                }
            }
        }

        public static List<Token> GetValidTokens(this List<Token> possibleTokens)
        {
            var validTokens = new List<Token>();
            var openTokens = new List<Token>();

            foreach (var token in possibleTokens)
            {
                if (token.IsOpen)
                {
                    openTokens.Add(token);
                }
                else
                {
                    var possibleOpenToken = openTokens
                        .FirstOrDefault(openToken => openToken.Tag.MD == token.Tag.MD);
                    if (possibleOpenToken != null)
                    {
                        validTokens.Add(possibleOpenToken);
                        validTokens.Add(token);
                        openTokens.Remove(possibleOpenToken);
                    }
                }
            }

            return validTokens;
        }

        private static bool IsStrongInEmphasis(Token currentToken, Token nextToken)
        {
            return currentToken.Tag.MD == "__" && nextToken.Tag.MD == "_" &&
                   !currentToken.IsOpen && !nextToken.IsOpen;
        }
    }
}
