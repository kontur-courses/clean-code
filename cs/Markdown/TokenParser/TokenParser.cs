using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Extensions;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParser
{
    public class TokenParser : ITokenParser
    {
        public IEnumerable<TagNode> Parse(IEnumerable<Token> tokens)
        {
            if (tokens == null) throw new ArgumentNullException(nameof(tokens));
            using var enumerator = tokens.GetEnumerator();
            var iterator = new TokenParsingIterator(enumerator);
            return ReduceTextTokens(iterator.Parse());
        }

        private static IEnumerable<TagNode> ReduceTextTokens(IEnumerable<TagNode> nodes)
        {
            var sb = new StringBuilder();
            foreach (var node in nodes)
                if (node.Tag.Type == TagType.Text)
                {
                    sb.Append(node.Tag.Value);
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        yield return Token.Text(sb.ToString()).ToNode();
                        sb.Clear();
                    }

                    yield return new TagNode(node.Tag, ReduceTextTokens(node.Children).ToArray());
                }

            if (sb.Length > 0) yield return Token.Text(sb.ToString()).ToNode();
        }
    }
}