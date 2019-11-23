using Markdown.Exporter;

namespace Markdown.Parser
{
    internal class MarkdownBoldElement : MarkdownElement
    {
        internal MarkdownBoldElement(string value) : base(value)
        {
        }

        public override string Export(IExporter exporter) => exporter.TransformBold(this);
    }
}