using System.Text;
using Markdown.Parser.Nodes;

namespace Markdown.Parser.Rules.Tools;

public static class ListConvertingExtensions
{
    public static string ToText(this Node node) => node switch
    {
        TextNode textNode => textNode.Text,
        TagNode tagNode => StringifyNodes(tagNode.Children).ToString(),
        SpecNode specNode => StringifyNodes(specNode.Children).ToString(),

        _ => throw new ArgumentException("Unknown node type")
    };

    private static StringBuilder StringifyNodes(List<Node> nodes)
        => nodes.Aggregate(new StringBuilder(), (sb, n) => sb.Append(n.ToText()));
}