using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class TokenReader
    {
        public static Token ReadUntil(Deque<Tuple<string, Token>> controlSymbols, string input)
        {
            var currentPosition = controlSymbols.First.Item2.StartPosition;
            while (currentPosition < input.Length)
            {
                foreach (var element in controlSymbols)
                {
                    var (controlSymbol, hisToken) = element;
                    var countOfNasted = 0;
                    switch (ControlSymbols.ControlSymbolDecisionOnChar[controlSymbol](input, currentPosition))
                    {
                        case StopSymbolDecision.Stop:
                            CloseToken(controlSymbols, controlSymbol, hisToken, currentPosition);
                            if (controlSymbols.First.Item1 == controlSymbol)
                                return hisToken;
                            controlSymbols.RemoveLast();
                            continue;
                        case StopSymbolDecision.NestedToken:
                            countOfNasted++;
                            break;
                    }

                    if (countOfNasted == controlSymbols.Count)
                        NastedToken(controlSymbols, input, currentPosition);
                }

                currentPosition++;
            }
            
            return ClearUnClosedToken(controlSymbols.First.Item2, input.Length);
        }
        
        
        public static Token ReadWhile(Func<string, int, bool> isControlSymbol, string input, int startPosition)
        {
            throw new NotImplementedException();
        }

        public static Token ClearUnClosedToken(Token token, int end)
        {
            var correctToken = new Token{StartPosition = token.ActualStart, Length = end - token.ActualStart};
            if (token.Value.IsEmpty)
            {
                correctToken.Length = end - correctToken.StartPosition;
                return correctToken;
            }
            
            var notNullTagTokens = new Queue<Token>();    
            GetAllNotNullTokens(token, notNullTagTokens);
            while (notNullTagTokens.Any())
            {
                var innerToken = notNullTagTokens.Dequeue();
                if (correctToken.Value.IsEmpty)
                    correctToken.StringBlocks.AddLast(Tuple.Create(correctToken.ActualStart,
                        innerToken.ActualStart - correctToken.ActualStart));
                else
                {
                    var endOfPreviousInnerToken = correctToken.Value.Last.ActualEnd;
                    correctToken.StringBlocks.AddLast(Tuple.Create(endOfPreviousInnerToken,
                        innerToken.ActualStart - endOfPreviousInnerToken));
                }
                correctToken.Value.AddLast(innerToken);
                    
            }
            
            StandardiseToken(correctToken, new HashSet<string>());
            return correctToken;
        }

        public static void GetAllNotNullTokens(Token token, Queue<Token> notNullTagTokens)
        {
            foreach (var tokens in token.Value)
            {
                if(tokens.Tag is null)
                    GetAllNotNullTokens(tokens, notNullTagTokens);
                else
                    notNullTagTokens.Enqueue(tokens);
            }
        }
        private static void NastedToken(Deque<Tuple<string, Token>> controlSymbols, string input, int currentPosition)
        {
            var symbol = ControlSymbols.ResolveControlSymbol(input, currentPosition);
            var newToken = new Token {ActualStart = currentPosition, StartPosition = currentPosition + symbol.Length};
            var prevToken = controlSymbols.Last.Item2;
            if (prevToken.Value.IsEmpty)
                prevToken.StringBlocks.AddLast(Tuple.Create(prevToken.StartPosition,
                    currentPosition - prevToken.StartPosition));
            else
            {
                var lastInnerToken = prevToken.Value.Last;
                prevToken.StringBlocks.AddLast(Tuple.Create(lastInnerToken.ActualEnd,
                    currentPosition - lastInnerToken.ActualEnd - 1));
            }

            prevToken.Value.AddLast(newToken);
            controlSymbols.AddLast(Tuple.Create(symbol, newToken));
        }

        private static void CloseToken(Deque<Tuple<string, Token>> controlSymbols, string controlSymbol, Token hisToken,
            int currentPosition)
        {
            while (controlSymbols.Last.Item1 != controlSymbol)
            {
                controlSymbols.RemoveLast();
            }

            if (!hisToken.Value.IsEmpty && hisToken.Value.Last.Tag == "")
            {
                hisToken.Value.RemoveLast();
                hisToken.StringBlocks.RemoveLast();
            }

            hisToken.Length = currentPosition - hisToken.StartPosition;
            hisToken.ActualEnd = currentPosition + controlSymbol.Length - 1;
            hisToken.Tag = ControlSymbols.ControlSymbolTags[controlSymbol];
            StandardiseToken(hisToken, ControlSymbols.TagCloseNextTag[ControlSymbols.ControlSymbolTags[controlSymbol]]);
        }

        public static void StandardiseToken(Token token, HashSet<string> notAllowedTags)
        {
            if (token.StringBlocks.IsEmpty && token.Value.IsEmpty)
                return;
            
            var newStringBlock = new Deque<Tuple<int, int>>();
            var newValue = new Deque<Token>();
            while (!token.StringBlocks.IsEmpty)
            {
                var block = token.StringBlocks.RemoveFirst();
                if (token.Value.IsEmpty)
                {
                    newStringBlock.AddLast(block);
                    break;
                }
                var innerToken = token.Value.RemoveFirst();
                if (notAllowedTags.Contains(innerToken.Tag))
                {
                    if (token.StringBlocks.IsEmpty)
                        newStringBlock.AddLast(Tuple.Create(block.Item1,
                            token.Length + token.StartPosition - block.Item1));
                    else
                    {
                        var nextBlock = token.StringBlocks.RemoveFirst();
                        token.StringBlocks.AddFirst(Tuple.Create(block.Item1,
                            nextBlock.Item1 - block.Item1 + nextBlock.Item2));
                    }
                }
                else
                {
                    newStringBlock.AddLast(block);
                    newValue.AddFirst(innerToken);
                }
            }

            var tokenEnd = token.StartPosition + token.Length;
            if (tokenEnd != newStringBlock.Last.Item2 + newStringBlock.Last.Item1
                && newValue.Last.ActualEnd < tokenEnd)
                newStringBlock.AddLast(
                    Tuple.Create(newValue.Last.ActualEnd + 1, tokenEnd - newValue.Last.ActualEnd - 1));

            token.StringBlocks = newStringBlock;
            token.Value = newValue;
        }
    }
}