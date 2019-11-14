using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Exporter;

namespace Markdown.Parser
{
    internal class Element : INode
    {
        public NodeType NodeType { get; } = NodeType.Element;
        public INode Parent { get; }
        public string Value { get; }
        private ICollection<INode> ChildNodes { get; }

        internal Element(INode parent, string value)
        {
            Parent = parent;
            Value = value;
            ChildNodes = new List<INode>();
        }
        
        internal virtual bool CanContain(INode node) => true;

        public virtual string Export(IExporter exporter)
        {
            var builder = new StringBuilder();
            foreach (var value in ChildNodes.Select(child => child.Export(exporter)))
            {
                builder.Append(value);
            }

            return builder.ToString();
        }
    }
}