using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Reducer
    {
        public IEnumerable<IToken> Reduce(IEnumerable<IToken> source)
        {
            var reducedTokens = new List<IToken>();
            var tokens = source.ToList();
            var previousWasEscape = false;

            foreach (var token in tokens)
            {
                if (token.TokenType == TokenType.Escape && !previousWasEscape)
                {
                    previousWasEscape = true;
                    continue;
                }

                if (token.TokenType == TokenType.Header1 && HeaderNotAtTheLineStart(reducedTokens))
                {
                    reducedTokens.Add(Token.FromText(token.Value));
                    continue;
                }
               
                reducedTokens.Add(DefineTokenToAdd(token, previousWasEscape));
                previousWasEscape = false;
            }
            
            if (OneEscapeSymbolWasProvided(tokens))
                reducedTokens.Add(Token.FromText("\\"));
            return reducedTokens;
        }

        private bool OneEscapeSymbolWasProvided(List<IToken> tokens) =>
            tokens.Count == 1 && tokens[0].TokenType == TokenType.Escape;

        private bool HeaderNotAtTheLineStart(List<IToken> result) => result.Count > 0;

        private IToken DefineTokenToAdd(IToken token, bool previousWasEscape)
        {
            if (token.TokenType == TokenType.Escape && previousWasEscape)
                return Token.FromText("\\");
            if (token.TokenType == TokenType.Text && previousWasEscape)
                return Token.FromText($"\\{token.Value}");
            if (token.TokenType == TokenType.Text && previousWasEscape)
                return token;
            return previousWasEscape ? Token.FromText(token.Value) : token;
        }
    }
}