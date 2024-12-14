using System.Diagnostics;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class ParagraphRule: IParsingRule
{
    private static readonly OrRule TagRules = new([
        new EscapeRule([TokenType.Underscore, TokenType.Backslash]),
        new ItalicRule(), new BoldRule(), new TextRule(),
    ]);
    
    private static readonly OrRule TokenRules = new([
        PatternRuleFactory.DoubleUnderscore(),
        new PatternRule(TokenType.Number), 
        new PatternRule(TokenType.Octothorpe),
        new PatternRule(TokenType.Underscore),
        new PatternRule(TokenType.Asterisk),
        new PatternRule(TokenType.Backslash),
    ]);

    private static readonly AndRule ResultRule = new([
        new KleeneStarRule(new OrRule(TagRules, TokenRules)),
        new PatternRule(TokenType.Newline)
    ]);
    
    public Node? Match(List<Token> tokens, int begin = 0) 
        => ResultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;

    private static TagNode BuildNode(SpecNode node)
    {
        var valueNode = (node.Nodes.First() as SpecNode);
        Debug.Assert(valueNode != null, nameof(valueNode) + " != null");
        return new TagNode(NodeType.Paragraph, valueNode.Nodes, node.Start, node.Consumed);
    }
}