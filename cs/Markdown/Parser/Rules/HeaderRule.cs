using System.Diagnostics;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class HeaderRule : IParsingRule
{
    private const uint MaxHeaderSize = 6;

    private static readonly KleeneStarRule OctothorpeRule = new(new PatternRule(TokenType.Octothorpe));
    
    private readonly AndRule resultRule = new([
        OctothorpeRule,
        new PatternRule([TokenType.Space]),
        new ParagraphRule(),
    ]);

    public Node? Match(List<Token> tokens, int begin = 0)
    {
        if (OctothorpeRule.Match(tokens, begin)?.Consumed > MaxHeaderSize)
        {
            return null;
        }
        
        return resultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;   
    }

    private static TagNode BuildNode(SpecNode specNode)
    {
        var headerSize = specNode.Nodes.First() as SpecNode;
        var valueNode = specNode.Nodes.Third() as TagNode;
        
        Debug.Assert(valueNode != null, nameof(valueNode) + " != null");
        Debug.Assert(headerSize != null, nameof(headerSize) + " != null");
        
        return new TagNode(NodeType.Header, valueNode.Children.Prepend(headerSize).ToList(), specNode.Start, specNode.Consumed);
    }
}