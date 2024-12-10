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
    
    private static TagNode? MatchBold(List<Token> tokens, int begin = 0)
    {
        var valueRule = new OrRule(new ItalicRule(), new TextRule());
        var pattern = new AndRule([
            PatternRuleFactory.DoubleUnderscore(),
            new ConditionalRule(new KleeneStarRule(valueRule), HasRightBorders),
            PatternRuleFactory.DoubleUnderscore()
        ]);
        var continuesRule = new OrRule(TokenType.Newline, TokenType.Space);
        
        var resultRule = new ContinuesRule(pattern, continuesRule);
        return resultRule.Match(tokens, begin) is SpecNode specNode ? BuildNode(specNode) : null;
    }
    private static TagNode BuildNode(SpecNode node)
    {
        var valueNode = (node.Nodes.Second() as SpecNode);
        Debug.Assert(valueNode != null, nameof(valueNode) + " != null");
        return new TagNode(NodeType.Bold, valueNode.Nodes, node.Start, node.Consumed);
    }
    private static bool HasRightBorders(Node node, List<Token> tokens)
        => tokens[node.End].TokenType != TokenType.Space && tokens[node.Start].TokenType != TokenType.Space;
}