using System.Collections.Generic;
using Markdown.Exporter;

namespace Markdown.Parser
{
    internal class Text : INode
    {
        public string Value { get; }
        public NodeType Type { get; } = NodeType.Text;
        public ICollection<INode> ChildNodes { get; } = null;

        internal Text(string value)
        {
            Value = value;
        }

        public string Export(IExporter exporter) => exporter.TransformText(this);
    }
}