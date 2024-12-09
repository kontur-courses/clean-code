using System.Text;
using Markdown.Parser.Nodes;
using Markdown.Tokens;

namespace Markdown.Parser.Rules.Tools;

public static class NodeConvertingExtensions
{
    public static string ToText(this List<Node> nodes, List<Token> tokens)
        => nodes.Aggregate(new StringBuilder(), (sb, n) => sb.Append(n.ToText(tokens))).ToString();
    
    public static string ToText(this Node node, List<Token> tokens) => node switch
    {
        TextNode textNode => textNode.ToText(tokens),
        TagNode tagNode => tagNode.Children.ToText(tokens),
        SpecNode specNode => specNode.Nodes.ToText(tokens),
        
        _ => throw new ArgumentException("Unknown node type")
    };
}