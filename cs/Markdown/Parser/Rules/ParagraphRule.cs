using System.Diagnostics;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class ParagraphRule: IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var tagRules = new OrRule([
            new EscapeRule(TokenType.Underscore),
            new ItalicRule(), new BoldRule(), new TextRule(),
        ]);
        var tokenRules = new OrRule([
            PatternRuleFactory.DoubleUnderscore(),
            new PatternRule(TokenType.Number), 
            new PatternRule(TokenType.Octothorpe),
            new PatternRule(TokenType.Underscore),
            new PatternRule(TokenType.Asterisk),
            new PatternRule(TokenType.Backslash),
        ]);

        var resultRule = new AndRule([
            new KleeneStarRule(new OrRule(tagRules, tokenRules)),
            new PatternRule(TokenType.Newline)
        ]);
        
        return resultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;
    }

    private static TagNode BuildNode(SpecNode node)
    {
        var valueNode = (node.Nodes.First() as SpecNode);
        Debug.Assert(valueNode != null, nameof(valueNode) + " != null");
        return new TagNode(NodeType.Paragraph, valueNode.Nodes, node.Start, node.Consumed);
    }
}