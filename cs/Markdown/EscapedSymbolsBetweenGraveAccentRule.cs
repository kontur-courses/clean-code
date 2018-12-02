using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class EscapedSymbolsBetweenGraveAccent : IRule
    {
        public List<Token> Apply(List<Token> symbolsMap)
        {
            return MakeTokensOrdinaryBetweenGraveAccent(symbolsMap).ToList();
        }

        private static IEnumerable<Token> MakeTokensOrdinaryBetweenGraveAccent(IEnumerable<Token> tokens)
        {
            var sortedTokens = tokens.OrderBy(s => s.Position);
            var isInside = false;
            foreach (var token in sortedTokens)
            {
                if (token.Data.Symbol == "`" && token.TokenType != TokenType.Ordinary)
                {
                    isInside = token.TokenType == TokenType.Start;
                    yield return token;
                    continue;
                }

                if (isInside)
                    yield return new Token(token.Data, TokenType.Ordinary, token.Position);
                else
                    yield return token;
            }
        }
    }
}