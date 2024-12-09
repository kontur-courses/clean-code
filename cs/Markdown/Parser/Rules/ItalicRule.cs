using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class ItalicRule : IParsingRule
{
    private readonly AndRule innerBoldRule = new([
        PatternRuleFactory.DoubleUnderscore(),
        new TextRule(),
        PatternRuleFactory.DoubleUnderscore()
    ]);
    private readonly List<IParsingRule> possibleContinues =
    [
        PatternRuleFactory.DoubleUnderscore(),
        new PatternRule(TokenType.Newline),
        new PatternRule(TokenType.Space),
    ];
    
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        return !InWordItalicRule.IsTagInWord(tokens, begin)
            ? MatchItalic(tokens, begin)
            : new InWordItalicRule().Match(tokens, begin);
    }
    private TagNode? MatchItalic(List<Token> tokens, int begin)
    {
        var valueRule = new OrRule(new TextRule(), innerBoldRule);
        var pattern = new AndRule([
            new PatternRule(TokenType.Underscore),
            new ConditionalRule(new KleeneStarRule(valueRule), HasRightBorders),
            new PatternRule(TokenType.Underscore),
        ]);
        var continuesRule = new OrRule(possibleContinues);
        
        var resultRule = new ContinuesRule(pattern, continuesRule);
        return resultRule.Match(tokens, begin) is SpecNode specNode ? BuildNode(specNode) : null;
    }
    private static TagNode BuildNode(SpecNode node)
    {
        var valueNode = (node.Nodes.Second() as SpecNode)!;
        return new TagNode(NodeType.Italic, valueNode.Nodes, node.Start, node.Consumed);
    }
    private static bool HasRightBorders(Node node, List<Token> tokens)
        => tokens[node.End].TokenType != TokenType.Space && tokens[node.Start].TokenType != TokenType.Space;
}