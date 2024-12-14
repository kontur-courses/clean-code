using System.Diagnostics;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class ItalicRule : IParsingRule
{
    private static readonly AndRule InnerBoldRule = new([
        PatternRuleFactory.DoubleUnderscore(),
        new KleeneStarRule(new OrRule([new PatternRule(TokenType.Backslash), new TextRule()])),
        PatternRuleFactory.DoubleUnderscore()
    ]);
    
    private static readonly List<IParsingRule> PossibleContinues =
    [
        PatternRuleFactory.DoubleUnderscore(),
        new PatternRule(TokenType.Newline),
        new PatternRule(TokenType.Space),
    ];
    
    private static readonly List<IParsingRule> AdditionalTextSymbols =
    [
        new PatternRule(TokenType.Asterisk), new PatternRule(TokenType.Backslash), new PatternRule(TokenType.Octothorpe)
    ];
    
    private static readonly OrRule ValueRule = new([
        new TextRule(), 
        new OrRule(AdditionalTextSymbols), 
        InnerBoldRule]);
        
    private static readonly AndRule Pattern = new([
        new PatternRule(TokenType.Underscore),
        new ConditionalRule(new KleeneStarRule(ValueRule), HasRightBorders),
        new PatternRule(TokenType.Underscore),
    ]);
    
    private static readonly OrRule ContinuesRule = new(PossibleContinues);
        
    private static readonly ContinuesRule ResultRule = new(Pattern, ContinuesRule);

    
    public Node? Match(List<Token> tokens, int begin = 0) 
        => !InWordItalicRule.IsTagInWord(tokens, begin)
            ? MatchItalic(tokens, begin)
            : new InWordItalicRule().Match(tokens, begin);

    private static TagNode? MatchItalic(List<Token> tokens, int begin) 
        => ResultRule.Match(tokens, begin) is SpecNode specNode ? BuildNode(specNode) : null;

    private static TagNode BuildNode(SpecNode node)
    {
        var valueNode = (node.Nodes.Second() as SpecNode);
        
        Debug.Assert(valueNode != null, nameof(valueNode) + " != null");
        return new TagNode(NodeType.Italic, valueNode.Nodes, node.Start, node.Consumed);
    }
    
    private static bool HasRightBorders(Node node, List<Token> tokens)
        => tokens[node.End].TokenType != TokenType.Space && tokens[node.Start].TokenType != TokenType.Space;
}