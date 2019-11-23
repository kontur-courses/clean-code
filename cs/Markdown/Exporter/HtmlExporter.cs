using System.Text;
using Markdown.Parser;

namespace Markdown.Exporter
{
    internal class HtmlExporter : IExporter
    {
        public string TransformText(Text text)
        {
            return text.Value;
        }

        public string TransformBold(MarkdownBoldElement element)
        {
            return TransformBlock(element, "strong");
        }

        public string TransformItalic(MarkdownItalicElement element)
        {
            return TransformBlock(element, "em");
        }

        public string TransformBlock(Element element, string htmlTag)
        {
            var builder = new StringBuilder($"<{htmlTag}>");
            foreach (var child in element.ChildNodes)
                builder.Append(child.Export(this));
            builder.Append($"</{htmlTag}>");
            return builder.ToString();
        }
    }
}