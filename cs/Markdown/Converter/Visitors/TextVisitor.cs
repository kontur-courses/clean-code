using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class TextVisitor : IVisitor
{
    public string Visit(Node node)
    {
        return node.Value ?? string.Empty;
    }
}