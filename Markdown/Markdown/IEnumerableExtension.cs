using System.Collections.Generic;
using System.Linq;

namespace Markdown.Elements
{
    public static class IEnumerableExtension
    {
        public static Token[] GetInnerTokens(this IEnumerable<Token> tokens)
        {
            var enumerable = tokens as Token[] ?? tokens.ToArray();
            return enumerable.Skip(1)
                .Take(enumerable.Count() - 2)
                .ToArray();
        }

        public static bool TrySeparateSimplePart(this IEnumerable<Token> tokens, out TokenTuple result)
        {
            result = new TokenTuple();
            var enumerable
                = tokens as Token[] ?? tokens.ToArray();
            switch (enumerable.First().Type)
            {
                case TokenType.SimpleWord:
                    result = new TokenTuple(new[] { enumerable.First() }, enumerable.Skip(1));
                    return true;
                case TokenType.EscapingDelimiter:
                    if (enumerable.Count() < 2)
                    {
                        result = new TokenTuple(new[] { enumerable.First() }, enumerable.Skip(1));
                        return true;
                    }
                    else
                    {
                        result = new TokenTuple(enumerable.Skip(1).Take(1), enumerable.Skip(2));
                        return true;
                    }
            }
            result = null;
            return false;
        }


        public static bool TrySeparateStylePart(this IEnumerable<Token> tokens, out TokenTuple tuple)
        {
            tuple = new TokenTuple();
            if (TryCollectTokenToNextDelimiter(tokens, out var collectedTokens))
            {
                tuple = new TokenTuple(collectedTokens, tokens.Skip(collectedTokens.Count()));
                return true;
            }

            return false;
        }

        private static bool TryCollectTokenToNextDelimiter(IEnumerable<Token> tokens, out IEnumerable<Token> result)
        {
            var enumerable = tokens as Token[] ?? tokens.ToArray();
            var del = enumerable.First().Type;
            var colTokens = new List<Token> { enumerable.First() };
            colTokens.AddRange(enumerable.Skip(1).TakeWhile(t => t.Type != del));
            if (colTokens.Count < enumerable.Count())
            {
                colTokens.Add(enumerable.ElementAt(colTokens.Count));
                result = colTokens;
                return true;
            }

            result = null;
            return false;
        }
    }
}
