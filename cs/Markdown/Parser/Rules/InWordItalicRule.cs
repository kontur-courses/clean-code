using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class InWordItalicRule : IParsingRule
{
    private readonly List<IParsingRule> pattern = 
    [
        new PatternRule(TokenType.Underscore),
        new PatternRule(TokenType.Word),
        new PatternRule(TokenType.Underscore),
    ];
    
    private readonly OrRule continuesRule = new(
        [TokenType.Newline, TokenType.Space, TokenType.Word]
    );

    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var match = tokens.MatchPattern(pattern, begin);

        if (match.Count != pattern.Count) return null;
        if (match.Second() is not TextNode textNode) return null;
        if (!HasRightContinues(tokens, begin + textNode.Consumed + 2)) return null;

        return new TagNode(NodeType.Italic, textNode, textNode.Consumed + 2);
    }

    private bool HasRightContinues(List<Token> tokens, int begin)
    {
        if (tokens.Count == begin) return true;
        return continuesRule.Match(tokens, begin) is not null;
    }
}