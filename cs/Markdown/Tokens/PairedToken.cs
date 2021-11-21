using System.Collections.Generic;

namespace Markdown.Tokens
{
    public class PairedToken
    {
        public TokenType Type => From.Type;
        public Token From { get; }
        public Token To { get; }

        public PairedToken(Token from, Token to)
        {
            From = from;
            To = to;
        }

        public static IEnumerable<PairedToken> GetPairedTokens(IEnumerable<Token> tokens,
            HashSet<Token> unpairedTokens)
        {
            var tokensWithoutPair = new Stack<Token>();
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
    }
}
