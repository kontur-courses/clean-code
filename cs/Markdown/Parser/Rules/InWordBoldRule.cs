using System.Diagnostics;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class InWordBoldRule : IParsingRule
{
    private static readonly List<TokenType> PossibleContinues =
    [
        TokenType.Newline, TokenType.Space, TokenType.Word
    ];
    
    private static readonly List<IParsingRule> AdditionalTextSymbols =
    [
        new PatternRule(TokenType.Asterisk), new PatternRule(TokenType.Backslash), new PatternRule(TokenType.Octothorpe)
    ];

    private static readonly OrRule ValueRule = new([
        new InWordItalicRule(),
        new PatternRule(TokenType.Word),
        new OrRule(AdditionalTextSymbols)
    ]);
    
    private static readonly AndRule Pattern = new([
        PatternRuleFactory.DoubleUnderscore(),
        new KleeneStarRule(ValueRule),
        PatternRuleFactory.DoubleUnderscore(),
    ]);
    private static readonly OrRule ContinuesRule = new(PossibleContinues);

    private static readonly ContinuesRule ResultRule = new(Pattern, ContinuesRule);

    private static readonly AndRule InStartRule = new([
        PatternRuleFactory.DoubleUnderscore(),
        new KleeneStarRule(ValueRule),
        PatternRuleFactory.DoubleUnderscore(),
        new KleeneStarRule(ValueRule)
    ]);
    
    public Node? Match(List<Token> tokens, int begin = 0) 
        => ResultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;

    private static TagNode BuildNode(SpecNode node)
    {
        var valueNode = node.Nodes.Second() as SpecNode;
        Debug.Assert(valueNode != null, nameof(valueNode) + " != null");
        return new TagNode(NodeType.Bold, valueNode.Nodes, node.Start, node.Consumed);
    }

    public static bool IsTagInWord(List<Token> tokens, int begin = 0)
    {
        if (begin != 0 && tokens[begin - 1].TokenType == TokenType.Word) 
            return true;
        
        return InStartRule.Match(tokens, begin) is not null;
    }
}