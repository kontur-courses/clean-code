using System.Text;
using Markdown.Exporter;

namespace Markdown.Parser
{
    internal class MarkdownItalicElement : MarkdownElement
    {
        internal MarkdownItalicElement(INode parent, string value) : base(parent, value)
        {
        }

        internal override bool CanContain(INode node) => node is Text;

        public override string Export(IExporter exporter) => exporter.TransformItalic(this);
    }
}