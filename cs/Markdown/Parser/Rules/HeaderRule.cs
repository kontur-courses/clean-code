using System.Diagnostics;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class HeaderRule : IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var resultRule = new AndRule([
            new PatternRule([TokenType.Octothorpe, TokenType.Space]),
            new ParagraphRule(),
        ]);
        return resultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;
    }

    private static TagNode BuildNode(SpecNode specNode)
    {
        var valueNode = (specNode.Nodes.Second() as TagNode);
        Debug.Assert(valueNode != null, nameof(valueNode) + " != null");
        return new TagNode(NodeType.Header, valueNode.Children, specNode.Start,specNode.Consumed);
    }
}