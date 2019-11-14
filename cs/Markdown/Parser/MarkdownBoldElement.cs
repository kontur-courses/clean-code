using Markdown.Exporter;

namespace Markdown.Parser
{
    internal class MarkdownBoldElement : MarkdownElement
    {
        internal MarkdownBoldElement(INode parent, string value) : base(parent, value)
        {
        }

        public override string Export(IExporter exporter) => exporter.TransformBold(this);
    }
}