using System;
using System.Collections.Generic;
using System.Linq;

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
                    continue;
                if (token.IsOpening)
                    openedTokens.Push(token);
                else
                    closedTokens.Enqueue(token);
            }

            var minCount = Math.Min(openedTokens.Count, closedTokens.Count);
            while (minCount > 0)
            {
                var openToken = openedTokens.Pop();
                var closeToken = closedTokens.Dequeue();
                if (openToken.Type != closeToken.Type)
                    unpairedTokens.UnionWith(new[] {openToken, closeToken});
                else
                    yield return new PairedToken(openToken, closeToken);
                minCount--;
            }
            unpairedTokens.UnionWith(openedTokens);
            unpairedTokens.UnionWith(closedTokens);
        }

        public bool IsPairsIntersect(PairedToken pt)
        {
            return From.Position < pt.From.Position
                   && pt.From.Position < To.Position
                   && To.Position < pt.To.Position;
        }
    }
}
