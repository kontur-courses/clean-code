using System.Diagnostics;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class BoldRule : IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        return !InWordBoldRule.IsTagInWord(tokens, begin)
            ? MatchBold(tokens, begin)
            : new InWordBoldRule().Match(tokens, begin);
    }
    
    private static readonly List<IParsingRule> AdditionalTextSymbols =
    [
        new PatternRule(TokenType.Asterisk), new PatternRule(TokenType.Backslash), new PatternRule(TokenType.Octothorpe)
    ];
    
    private static readonly IParsingRule ValueRule = new OrRule([
        new ItalicRule(), 
        new TextRule(),
        new OrRule(AdditionalTextSymbols)
    ]);
    
    
    private static readonly IParsingRule Pattern = new AndRule([
        PatternRuleFactory.DoubleUnderscore(),
        new ConditionalRule(new KleeneStarRule(ValueRule), HasRightBorders),
        PatternRuleFactory.DoubleUnderscore()
    ]);
    
    private static readonly IParsingRule ContinuesRule = new OrRule(TokenType.Newline, TokenType.Space);
        
    private static readonly ContinuesRule ResultRule = new(Pattern, ContinuesRule);
    
    private static TagNode? MatchBold(List<Token> tokens, int begin = 0) 
        => ResultRule.Match(tokens, begin) is SpecNode specNode ? BuildNode(specNode) : null;

    private static TagNode BuildNode(SpecNode node)
    {
        var valueNode = (node.Nodes.Second() as SpecNode);
        Debug.Assert(valueNode != null, nameof(valueNode) + " != null");
        return new TagNode(NodeType.Bold, valueNode.Nodes, node.Start, node.Consumed);
    }
    private static bool HasRightBorders(Node node, List<Token> tokens)
        => tokens[node.End].TokenType != TokenType.Space && tokens[node.Start].TokenType != TokenType.Space;
}