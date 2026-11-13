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

        //здесь не понял какое разбиение надо сделал покороче с switch expression 
        private static string RenderNode(INode node)
        {
            return node switch
            {
                TextNode textNode => textNode.Text,
                EmNode emNode => RenderPairTag("em", emNode.Children),
                StrongNode strongNode => RenderPairTag("strong", strongNode.Children),
                LinkNode linkNode => RenderLinkTag(linkNode.Url, linkNode.Children),
                _ => string.Empty,
            };
        }

        private static string RenderPairTag(string tag, IReadOnlyList<INode> children)
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

        private static string RenderLinkTag(string url, IReadOnlyList<INode> children)
        {
            var sb = new StringBuilder();
            sb.Append($"<a href=\"{url}\">");
            foreach (var child in children)
            {
                sb.Append(RenderNode(child));
            }
            sb.Append("</a>");
            return sb.ToString();
        }
    }
}