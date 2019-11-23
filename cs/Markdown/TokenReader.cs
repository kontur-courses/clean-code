using System;
using System.Collections.Generic;
using System.Linq;
using static Markdown.ControlSymbols;

namespace Markdown
{
    public static class TokenReader
    {
        public static Token ReadUntil(string input, int startPosition)
        {
            var prefix = ResolveControlSymbol(input, startPosition);
            var mainToken = new Token(prefix);
            mainToken.CreateInnerToken();
            var innerTokens = new Deque<Token>();
            innerTokens.AddLast(mainToken);
            for (var currentPosition = startPosition + prefix.Length; currentPosition < input.Length; currentPosition++)
            {
                var count = innerTokens.Count;
                foreach (var token in innerTokens)
                {
                    var controlSymbol = token.Prefix;
                    switch (ControlSymbolDecisionOnChar[controlSymbol](input, currentPosition))
                    {
                        case StopSymbolDecision.Stop:
                            token.CloseToken(currentPosition + controlSymbol.Length);
                            token.ClearTags(TagCloseNextTag[controlSymbol], controlSymbol);
                            if (prefix == controlSymbol)
                                return mainToken;
                            while (controlSymbol != innerTokens.Last.Prefix)
                                innerTokens.RemoveLast();
                            innerTokens.RemoveLast();
                            currentPosition += controlSymbol.Length - 1;
                            break;
                        case StopSymbolDecision.NestedToken:
                            var inPrefix = ResolveControlSymbol(input, currentPosition);
                            innerTokens.AddLast(innerTokens.Last.CreateInnerToken(inPrefix));
                            innerTokens.Last.CreateInnerToken("");
                            currentPosition += inPrefix.Length;
                            break;
                        case StopSymbolDecision.AddChar:
                            count--;
                            break;
                    }
                }

                if (count == 0)
                    mainToken.AddChar(input[currentPosition]);
            }

            mainToken.CloseToken(input.Length);
            return mainToken;
        }


        public static Token ReadWhile(Func<string, int, Symbol> analyzeSymbol, string input, int startPosition)
        {
            var position = startPosition;
            var token = new Token();
            var stop = false;
            for (; position < input.Length && !stop; position++)
            {
                switch (analyzeSymbol(input, position))
                {
                    case Symbol.ControlSymbol:
                        stop = true;
                        break;
                    case Symbol.Screen:
                        continue;
                    case Symbol.AnotherSymbol:
                        token.AddChar(input[position]);
                        continue;
                }
            }

            token.CloseToken(position);
            return token;
        }
    }
}