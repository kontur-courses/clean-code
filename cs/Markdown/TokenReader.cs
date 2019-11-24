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
            var prefix = TryGetControlSymbol(input, startPosition);
            var mainToken = new Token(prefix);
            mainToken.CreateInnerToken();
            var innerTokens = new Deque<Token>();
            innerTokens.AddLast(mainToken);
            for (var i = startPosition + prefix.Length; i < input.Length; i++)
            {
                var count = innerTokens.Count;
                foreach (var token in innerTokens)
                {
                    var controlSymbol = token.Prefix;
                    switch (DecisionByControlSymbol[controlSymbol](input, i))
                    {
                        case StopSymbolDecision.Stop:
                            token.CloseToken();
                            token.ClearTags(TagCloseNextTag[controlSymbol], controlSymbol);
                            if (prefix == controlSymbol)
                                return ValueTuple.Create(mainToken, i + controlSymbol.Length);
                            while (controlSymbol != innerTokens.Last.Prefix)
                                innerTokens.RemoveLast();
                            innerTokens.RemoveLast();
                            i += controlSymbol.Length - 1;
                            break;
                        case StopSymbolDecision.NestedToken:
                            var inPrefix = TryGetControlSymbol(input, i);
                            innerTokens.AddLast(innerTokens.Last.CreateInnerToken(inPrefix));
                            innerTokens.Last.CreateInnerToken("");
                            i += inPrefix.Length;
                            break;
                        case StopSymbolDecision.AddChar:
                            count--;
                            break;
                    }
                }

                if (count == 0)
                    mainToken.AddChar(input[i]);
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
                        break;
                    case Symbol.AnotherSymbol:
                        token.AddChar(input[position]);
                        break;
                }
            }

            token.CloseToken();
            return ValueTuple.Create(token, position);
        }
    }
}