using Markdown.Nodes;

namespace Markdown.Parsing.Parsers;

public abstract class ParserBase : IParser
{
    public List<INode> Parse(List<INode> nodes)
    {
        var result = new List<INode>();
        foreach (var node in nodes)
        {
            if (node is TextNode textNode)
            {
                result.AddRange(ParseText(textNode.Text));
            }
            else if (node is ContainerNode container)
            {
                var newChildren = Parse(container.Children);
                result.Add(new ContainerNode(container.Tag, newChildren));
            }
            else
            {
                result.Add(node);
            }
        }
        return result;
    }

    protected abstract List<INode> ParseText(string text);
}