using Markdown.Exporter;

namespace Markdown.Parser
{
    internal interface INode : IExportable
    {
        NodeType NodeType { get; }
        INode Parent { get; }
        string Value { get; }
    }
}