using System.Collections.Generic;
using System.Linq;

namespace Markdown.Elements
{
    public static class IEnumerableExtension
    {
        public static Token[] GetInnerTokens(this IEnumerable<Token> tokens)
        {
            return tokens.Skip(1)
                .Take(tokens.Count() - 2)
                .ToArray();
        }

        public static bool TrySeparateSimplePart(this IEnumerable<Token> tokens, out TokenTuple result)
        {
            result = new TokenTuple();
            switch (tokens.First().Type)
            {
                case TokenType.SimpleWord:
                    result = new TokenTuple(new[] { tokens.First() }, tokens.Skip(1));
                    return true;
                case TokenType.EscapingDelimiter:
                    if (tokens.Count() < 2)
                    {
                        result = new TokenTuple(new[] { tokens.First() }, tokens.Skip(1));
                        return true;
                    }
                    else
                    {
                        result = new TokenTuple(tokens.Skip(1).Take(1), tokens.Skip(2));
                        return true;
                    }
            }
            result = null;
            return false;
        }


        public static bool TrySeparateStylePart(this IEnumerable<Token> tokens, out TokenTuple tuple)
        {
            tuple = new TokenTuple();
            if (TryCollectTokenToNextDelimiter(tokens, out IEnumerable<Token> collectedTokens))
            {
                tuple = new TokenTuple(collectedTokens, tokens.Skip(collectedTokens.Count()));
                return true;
            }

            return false;
        }

        private static bool TryCollectTokenToNextDelimiter(IEnumerable<Token> tokens, out IEnumerable<Token> result)
        {
            var del = tokens.First().Type;
            var colTokens = new List<Token> { tokens.First() };
            colTokens.AddRange(tokens.Skip(1).TakeWhile(t => t.Type != del));
            if (colTokens.Count < tokens.Count())
            {
                colTokens.Add(tokens.ElementAt(colTokens.Count));
                result = colTokens;
                return true;
            }

            result = null;
            return false;
        }
    }
}
