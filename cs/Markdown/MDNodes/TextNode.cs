using Markdown.Abstractions;
using System.Reflection.Metadata.Ecma335;

namespace Markdown.MDNodes
{
    public sealed class TextNode : InlineNode
    {
        public string Content { get; set; }

        public TextNode(string content)
        {
            Content = content;
        }

        public override string Render() => EscapeHtml(Content);

        private static string EscapeHtml(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return text
                .Replace("&", "&amp;")  
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#39;");
        }
    }
}
