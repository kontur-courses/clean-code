using System.Collections.Generic;
using Markdown.Exporter;

namespace Markdown.Parser
{
    internal interface INode : IExportable
    {
        string Value { get; }
        NodeType Type { get; }
        ICollection<INode> ChildNodes { get; }
    }
}