using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class InWordBoldRule : IParsingRule
{
    private static readonly OrRule InnerValueRule = 
        new(new InWordItalicRule(), new PatternRule(TokenType.Word));
    private readonly List<IParsingRule> pattern =
    [
        PatternRule.DoubleUnderscoreRule(),
        new KleeneStarRule(InnerValueRule),
        PatternRule.DoubleUnderscoreRule(),
    ];

    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var match = tokens.MatchPattern(pattern, begin);

        if (match.Count != pattern.Count) return null;
        if (match.Second() is not SpecNode specNode) return null;

        return new TagNode(NodeType.Bold, specNode.Children, specNode.Consumed + 4);
    }
}