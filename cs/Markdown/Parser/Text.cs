using Markdown.Exporter;

namespace Markdown.Parser
{
    internal class Text : INode
    {
        public NodeType NodeType { get; } = NodeType.Text;
        public INode Parent { get; }
        public string Value { get; }

        internal Text(INode parent, string value)
        {
            Parent = parent;
            Value = value;
        }

        public string Export(IExporter exporter) => exporter.TransformText(this);
    }
}