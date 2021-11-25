using System;
using System.Collections.Generic;

namespace Markdown.Tokens
{
    public class PairedToken
    {
        public TokenType Type => From.Type;
        public IToken From { get; }
        public IToken To { get; }

        public PairedToken(IToken from, IToken to)
        {
            From = from;
            To = to;
        }

        public static IEnumerable<PairedToken> GetPairedTokens(IEnumerable<IToken> tokens,
            HashSet<IToken> unpairedTokens)
        {
            var openedTokens = new Stack<IToken>();
            var closedTokens = new Queue<IToken>();
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Content || token.Type == TokenType.Heading) 
                {
                    if (token.Value.Contains('\n'))
                        UnionAndClear(openedTokens, closedTokens, unpairedTokens);
                    continue;
                }

                if (token.IsOpening)
                    openedTokens.Push(token);
                else
                    closedTokens.Enqueue(token);
                if (openedTokens.Count != closedTokens.Count) continue;
                if (CompareTokensByType(openedTokens, closedTokens, unpairedTokens,
                    out var openToken, out var closeToken))
                    yield return new PairedToken(openToken, closeToken);
            }

            var minCount = Math.Min(openedTokens.Count, closedTokens.Count);
            while (minCount > 0)
            {
                if (CompareTokensByType(openedTokens, closedTokens, unpairedTokens, 
                    out var openToken, out var closeToken))
                    yield return new PairedToken(openToken, closeToken);
                minCount--;
            }
            UnionAndClear(openedTokens, closedTokens, unpairedTokens);
        }

        private static bool CompareTokensByType(Stack<IToken> openedTokens, 
            Queue<IToken> closedTokens, 
            HashSet<IToken> unpairedTokens, out IToken openToken,
            out IToken closeToken)
        {
            openToken = openedTokens.Pop();
            closeToken = closedTokens.Dequeue();
            if (openToken.Type == closeToken.Type && openToken.Position < closeToken.Position) return true;
            unpairedTokens.UnionWith(new[] {openToken, closeToken});
            return false;
        }

        private static void UnionAndClear(Stack<IToken> openedTokens,
            Queue<IToken> closedTokens,
            HashSet<IToken> unpairedTokens)
        {
            unpairedTokens.UnionWith(openedTokens);
            unpairedTokens.UnionWith(closedTokens);
            openedTokens.Clear();
            closedTokens.Clear();
        }

        public bool IsPairsIntersect(PairedToken pt)
        {
            return From.Position < pt.From.Position
                   && pt.From.Position < To.Position
                   && To.Position < pt.To.Position;
        }
    }
}
