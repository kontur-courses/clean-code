using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class ItalicRule : IParsingRule
{
    private readonly List<IParsingRule> pattern =
    [
        new PatternRule(TokenType.Underscore),
        new TextRule(),
        new PatternRule(TokenType.Underscore),
    ];

    private readonly OrRule continuesRule = new(
        PatternRule.DoubleUnderscoreRule(),
        new OrRule(TokenType.Newline, TokenType.Space)
    );

    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var innerRule = new InWordItalicRule();
        if (begin != 0 && tokens[begin - 1].TokenType == TokenType.Word)
            return innerRule.Match(tokens, begin);
        return innerRule.Match(tokens, begin) ?? MatchItalic(tokens, begin);
    }

    private TagNode? MatchItalic(List<Token> tokens, int begin)
    {
        var match = tokens.MatchPattern(pattern, begin);

        if (match.Count != pattern.Count) return null;
        if (match.Second() is not TextNode textNode) return null;

        var resultNode = BuildNode(textNode);

        var endWithNonSpace = textNode.Last.TokenType != TokenType.Space;
        var startWithNonSpace = textNode.First.TokenType != TokenType.Space;
        var hasRightContinues = HasRightContinues(tokens, begin + resultNode.Consumed);

        return hasRightContinues && endWithNonSpace && startWithNonSpace ? resultNode : null;
    }

    private bool HasRightContinues(List<Token> tokens, int begin)
    {
        if (tokens.Count == begin) return true;
        return continuesRule.Match(tokens, begin) is not null;
    }

    private static TagNode BuildNode(TextNode textNode) 
        => new(NodeType.Italic, textNode, textNode.Consumed + 2);
}