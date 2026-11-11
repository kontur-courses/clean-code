using System.Collections.Generic;
using System.Text;
using Markdown.Entity;
using Markdown.Enums;

namespace Markdown.Render
{
    public static class HTMLRender
    {
        public static string Render(IEnumerable<Paragraph> paragraphs)
        {
            var sb = new StringBuilder();

            foreach (var paragraph in paragraphs)
            {
                var tag = paragraph.Type == ParagraphType.Heading ? "h1" : "p";

                sb.Append($"<{tag}>");
                foreach (var node in paragraph.TagsTree)
                {
                    sb.Append(RenderNode(node));
                }
                sb.Append($"</{tag}>\n");
            }

            return sb.ToString().Trim();
        }

        private static string RenderNode(INode node)
        {
            switch (node)
            {
                case TextNode textNode:
                    return textNode.Text;

                case EmNode emNode:
                    return WrapWithTag("em", emNode.Children);

                case StrongNode strongNode:
                    return WrapWithTag("strong", strongNode.Children);

                default:
                    return string.Empty;
            }
        }

        private static string WrapWithTag(string tag, IReadOnlyList<INode> children)
        {
            var sb = new StringBuilder();
            sb.Append($"<{tag}>");
            foreach (var child in children)
            {
                sb.Append(RenderNode(child));
            }
            sb.Append($"</{tag}>");
            return sb.ToString();
        }
    }
}