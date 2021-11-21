using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenRenderer
{
    public class HtmlTokenRenderer : ITokenRenderer
    {
        public string Render(IEnumerable<TokenNode> tokens)
        {
            if (tokens == null) throw new ArgumentNullException(nameof(tokens));
            return StringUtils.Join(tokens.Select(ToString));
        }

        private static string ToString(TokenNode node)
        {
            if (node.Children.Length == 0)
                return node.Token.Value;
            var tag = MapToTag(node.Token.Type);
            return new StringBuilder()
                .Append(WrapOpeningTag(tag))
                .AppendJoin("", node.Children.Select(ToString))
                .Append(WrapClosingTag(tag))
                .ToString();
        }

        private static string MapToTag(TokenType type)
        {
            return type switch
            {
                TokenType.Header1 => "h1",
                TokenType.Cursive => "em",
                TokenType.Bold => "strong",
                _ => throw new Exception($"Unsupported token: {type}")
            };
        }

        private static string WrapOpeningTag(string tag) => $"<{tag}>";
        
        private static string WrapClosingTag(string tag) => $"</{tag}>";
    }

}