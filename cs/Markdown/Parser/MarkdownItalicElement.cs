using Markdown.Exporter;

namespace Markdown.Parser
{
    internal class MarkdownItalicElement : MarkdownElement
    {
        internal MarkdownItalicElement(string value) : base(value)
        {
        }

        internal override bool CanContain(INode node) => node is Text;

        public override string Export(IExporter exporter) => exporter.TransformItalic(this);
    }
}