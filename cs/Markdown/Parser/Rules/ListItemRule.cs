using System.Diagnostics;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class ListItemRule : IParsingRule
{
    private static readonly AndRule ResultRule = new([
        new PatternRule([TokenType.Asterisk, TokenType.Space]),
        new ParagraphRule(),
    ]);
    public Node? Match(List<Token> tokens, int begin = 0) 
        => ResultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;

    private static TagNode BuildNode(SpecNode specNode)
    {
        var valueNode = (specNode.Nodes.Second() as TagNode);
        Debug.Assert(valueNode != null, nameof(valueNode) + " != null");
        return new TagNode(NodeType.ListItem, valueNode.Children, specNode.Start, specNode.Consumed);
    }
}