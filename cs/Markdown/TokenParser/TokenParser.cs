using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenParser
{
    public class TokenParser : ITokenParser
    {
        public IEnumerable<TokenNode> Parse(IEnumerable<Token> tokens)
        {
            if (tokens == null) throw new ArgumentNullException(nameof(tokens));
            using var enumerator = tokens.GetEnumerator();
            var iterator = new TokenParsingIterator(enumerator);
            return ReduceTextTokens(iterator.Parse());
        }

        private static IEnumerable<TokenNode> ReduceTextTokens(IEnumerable<TokenNode> nodes)
        {
            var sb = new StringBuilder();
            foreach (var node in nodes)
                if (node.Token.Type == TokenType.Text)
                {
                    sb.Append(node.Token.Value);
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        yield return Token.Text(sb.ToString()).ToNode();
                        sb.Clear();
                    }

                    yield return new TokenNode(node.Token, ReduceTextTokens(node.Children).ToArray());
                }

            if (sb.Length > 0) yield return Token.Text(sb.ToString()).ToNode();
        }
    }
}