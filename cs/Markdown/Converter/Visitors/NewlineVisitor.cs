using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class NewlineVisitor : IVisitor
{
    public string Visit(Node newlineNode)
    {
        return newlineNode.Value ?? string.Empty;
    }
}