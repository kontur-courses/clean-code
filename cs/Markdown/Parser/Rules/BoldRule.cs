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
            PatternRule.DoubleUnderscoreRule(),
            new ConditionalRule(new KleeneStarRule(valueRule), HasRightBorder),
            PatternRule.DoubleUnderscoreRule()
        ]);
        var continuesRule = new OrRule(TokenType.Newline, TokenType.Space);

        var resultRule = new ContinuesRule(pattern, continuesRule);
        return resultRule.Match(tokens, begin) is SpecNode specNode ? BuildNode(specNode) : null;
    }

    private static TagNode BuildNode(SpecNode node)
    {
        var valueNode = (node.Children.Second() as SpecNode)!;
        return new TagNode(NodeType.Bold, valueNode.Children, node.Consumed);
    }

    private static bool HasRightBorder(Node node) 
        => node is SpecNode specNode 
           && TextDontEndWithSpace(specNode.Children.Last()) 
           && TextDontStartWithSpace(specNode.Children.First());

    private static bool TextDontStartWithSpace(Node node)
    {
        if (node.NodeType != NodeType.Text) return true;
        return node is TextNode textNode && textNode.First.TokenType != TokenType.Space;
    }

    private static bool TextDontEndWithSpace(Node node)
    {
        if (node.NodeType != NodeType.Text) return true;
        return node is TextNode textNode && textNode.Last.TokenType != TokenType.Space;
    }
}
