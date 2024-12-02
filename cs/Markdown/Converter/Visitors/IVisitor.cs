using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public interface IVisitor
{
    public string Visit(Node node);
}