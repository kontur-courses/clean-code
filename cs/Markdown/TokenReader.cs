using System;
using System.Collections.Generic;
using System.Linq;
using static Markdown.ControlSymbols;

namespace Markdown
{
    public  class TokenReader
    {
        public ValueTuple<Token, int> GetToken(string input, int position)
        {
            return IsControlSymbol(input, position)
                ? ReadUntil(input, position)
                : ReadWhile(input, position);
        }

        private static ValueTuple<Token, int> ReadUntil(string input, int startPosition)
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
                            token.CloseToken();
                            token.ClearTags(TagCloseNextTag[controlSymbol], controlSymbol);
                            if (prefix == controlSymbol)
                                return ValueTuple.Create(mainToken, currentPosition + controlSymbol.Length);
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

            return ValueTuple.Create(mainToken, input.Length);
        }


        private static ValueTuple<Token, int> ReadWhile(string input, int startPosition)
        {
            var position = startPosition;
            var token = new Token();
            var stop = false;
            for (; position < input.Length && !stop; position++)
            {
                switch (AnalyzeSymbol(input, position))
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

            token.CloseToken();
            return ValueTuple.Create(token, position);
        }
    }
}