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
        public string Render(IEnumerable<TagNode> tokens)
        {
            if (tokens == null) throw new ArgumentNullException(nameof(tokens));
            return StringUtils.Join(tokens.Select(ToString));
        }

        private static string ToString(TagNode node)
        {
            if (node.Children.Length == 0)
                return node.Tag.Value;
            var tag = MapToTag(node.Tag.Type);
            return new StringBuilder()
                .Append(WrapOpeningTag(tag))
                .Append(StringUtils.Join(node.Children.Select(ToString)))
                .Append(WrapClosingTag(tag))
                .ToString();
        }

        private static string MapToTag(TagType type)
        {
            return type switch
            {
                TagType.Header1 => "h1",
                TagType.Cursive => "em",
                TagType.Bold => "strong",
                TagType.Text => throw new Exception($"Unsupported tag: {type}"),
                _ => throw new Exception($"Unsupported tag: {type}")
            };
        }

        private static string WrapOpeningTag(string tag) => $"<{tag}>";

        private static string WrapClosingTag(string tag) => $"</{tag}>";
    }
}