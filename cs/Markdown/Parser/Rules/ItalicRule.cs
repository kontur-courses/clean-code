using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class ItalicRule : IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        return !InWordItalicRule.IsTagInWord(tokens, begin)
            ? MatchItalic(tokens, begin)
            : new InWordItalicRule().Match(tokens, begin);
    }

    private static TagNode? MatchItalic(List<Token> tokens, int begin)
    {
        var pattern = new AndRule([
            new PatternRule(TokenType.Underscore),
            new ConditionalRule(new TextRule(), HasRightBorders),
            new PatternRule(TokenType.Underscore),
        ]);
        var continuesRule = new OrRule([
            PatternRule.DoubleUnderscoreRule(),
            new PatternRule(TokenType.Newline),
            new PatternRule(TokenType.Space),
        ]);

        var resultRule = new ContinuesRule(pattern, continuesRule);
        return resultRule.Match(tokens, begin) is SpecNode specNode ? BuildNode(specNode) : null;
    }

    private static TagNode BuildNode(SpecNode node) 
        => new(NodeType.Italic, node.Children.Second()!, node.Consumed);

    private static bool HasRightBorders(Node node)
        => node is TextNode textNode
           && textNode.Last.TokenType != TokenType.Space
           && textNode.First.TokenType != TokenType.Space;
}