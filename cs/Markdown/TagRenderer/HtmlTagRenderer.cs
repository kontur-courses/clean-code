using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Extensions;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TagRenderer
{
    public class HtmlTagRenderer : ITagRenderer
    {
        public string Render(IEnumerable<TagNode> tokens)
        {
            if (tokens == null) throw new ArgumentNullException(nameof(tokens));
            return StringUtils.Join(tokens.Select(ToString));
        }

        private static string ToString(TagNode node)
        {
            if (node.Tag.Type == TagType.Text)
                return node.Tag.GetText();
            var tag = MapToTagName(node.Tag.Type);
            var sb = new StringBuilder();
            sb.Append(node.Tag.Type == TagType.Link
                ? WrapOpeningTag($"{tag} href=\"{node.Tag.Value}\"")
                : WrapOpeningTag(tag));
            if (node.Children.Length > 0) sb.Append(StringUtils.Join(node.Children.Select(ToString)));
            sb.Append(WrapClosingTag(tag));

            return sb.ToString();
        }

        private static string MapToTagName(TagType type)
        {
            return type switch
            {
                TagType.Header1 => "h1",
                TagType.Cursive => "em",
                TagType.Bold => "strong",
                TagType.Link => "a",
                TagType.Text => throw new Exception($"Unsupported tag: {type}"),
                _ => throw new Exception($"Unsupported tag: {type}")
            };
        }

        private static string WrapOpeningTag(string tag) => $"<{tag}>";

        private static string WrapClosingTag(string tag) => $"</{tag}>";
    }
}