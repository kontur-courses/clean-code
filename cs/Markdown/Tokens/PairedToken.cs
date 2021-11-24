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
            var tokensWithoutPair = new Stack<IToken>();
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Content || token.Type == TokenType.Heading)
                    continue;
                if (token.IsOpening)
                    tokensWithoutPair.Push(token);
                else
                {
                    if (tokensWithoutPair.Peek().Type != token.Type)
                    {
                        var wrongToken = tokensWithoutPair.Pop();
                        yield return new PairedToken(tokensWithoutPair.Pop(), token);
                        tokensWithoutPair.Push(wrongToken);
                    }
                    else
                        yield return new PairedToken(tokensWithoutPair.Pop(), token); ;
                }
            }
            while (tokensWithoutPair.Count != 0)
                unpairedTokens.Add(tokensWithoutPair.Pop());
        }

        public bool IsPairsIntersect(PairedToken pt)
        {
            return From.Position < pt.From.Position
                   && pt.From.Position < To.Position
                   && To.Position < pt.To.Position;
        }
    }
}
