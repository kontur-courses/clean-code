using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MixedUnderscoreRestrictionRule : IRule
    {
        private const string Underscore = "_";
        private const string DoubleUnderscore = "__";
        
        public List<Token> Apply(List<Token> symbolsMap)
        {
            return FixMixedUnderscores(symbolsMap).ToList();
        }
        
        private static IEnumerable<Token> FixMixedUnderscores(List<Token> tokens)
        {
            var isInsideUnderscore = false;
            var isInsideDoubleUnderscore = false;
            var isMixed = false;
            var temporaryTokens = new List<Token>();
            
            foreach (var token in tokens.OrderBy(token => token.Position))
            {
                var symbol = token.Data.Symbol;
                switch (symbol)
                {
                    case Underscore:
                        if (token.TokenType == TokenType.Start)
                        {
                            temporaryTokens.Add(token);
                            isInsideUnderscore = true;
                            isMixed = isInsideDoubleUnderscore;
                        }
                        else if (token.TokenType == TokenType.End)
                        {
                            isInsideUnderscore = false;
                            temporaryTokens.Add(token);
                            if (isInsideDoubleUnderscore)
                                continue;

                            if (isMixed)
                            {
                                foreach (var changedToken in ChangeMixedTokens(temporaryTokens))
                                    yield return changedToken;
                            }
                            else
                            {
                                foreach (var temporaryToken in temporaryTokens)
                                    yield return temporaryToken;
                            }
                            temporaryTokens.Clear();
                        }
                        else if (isInsideUnderscore || isInsideDoubleUnderscore)
                            temporaryTokens.Add(token);
                        else
                            yield return token;
                        break;
                        
                    case DoubleUnderscore:
                        if (token.TokenType == TokenType.Start)
                        {
                            temporaryTokens.Add(token);
                            isInsideDoubleUnderscore = true;
                            isMixed = isInsideUnderscore;
                        }
                        else if (token.TokenType == TokenType.End)
                        {
                            isInsideDoubleUnderscore = false;
                            temporaryTokens.Add(token);
                            if (isInsideUnderscore)
                                continue;

                            if (isMixed)
                            {
                                foreach (var changedToken in ChangeMixedTokens(temporaryTokens))
                                    yield return changedToken;
                            }
                            else
                            {
                                foreach (var temporaryToken in temporaryTokens)
                                    yield return temporaryToken;
                            }
                            temporaryTokens.Clear();
                        }
                        else if (isInsideUnderscore || isInsideDoubleUnderscore)
                            temporaryTokens.Add(token);
                        else
                            yield return token;

                        break;
                    
                    default:
                        if (isInsideUnderscore || isInsideDoubleUnderscore)
                            temporaryTokens.Add(token);
                        else
                            yield return token;
                        break;
                }
            }
        }

        private static IEnumerable<Token> ChangeMixedTokens(List<Token> temporaryTokens)
        {
            var lastIndex = temporaryTokens.Count - 1;
            if (temporaryTokens[0].Data.Symbol == temporaryTokens[lastIndex].Data.Symbol)
            {
                foreach (var token in temporaryTokens)
                    yield return token;

                yield break;
            }
            
            var first = temporaryTokens[0].Data;
            var firstData = first.WithTag("em");
            yield return new Token(firstData, temporaryTokens[0].TokenType, temporaryTokens[0].Position);
            
            for (var i = 1; i < temporaryTokens.Count - 1; i++)
            {
                if (temporaryTokens[i].Data.Symbol == Underscore ||
                    temporaryTokens[i].Data.Symbol == DoubleUnderscore)
                    yield return new Token(temporaryTokens[i].Data, TokenType.Escaped, temporaryTokens[i].Position);
                else
                    yield return temporaryTokens[i];
            }
            
            var last = temporaryTokens[lastIndex].Data;
            var lastData = last.WithTag("em");
            yield return new Token(lastData, temporaryTokens[lastIndex].TokenType, temporaryTokens[lastIndex].Position);
        }
    }
}