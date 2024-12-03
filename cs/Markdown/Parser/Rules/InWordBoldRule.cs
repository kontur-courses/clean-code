using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class InWordBoldRule : IParsingRule
{
    private readonly List<TokenType> possibleContinues =
    [
        TokenType.Newline, TokenType.Space, TokenType.Word
    ];

    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var valueRule = new OrRule(new InWordItalicRule(), new PatternRule(TokenType.Word));
        var pattern = new AndRule([
            PatternRule.DoubleUnderscoreRule(),
            new KleeneStarRule(valueRule),
            PatternRule.DoubleUnderscoreRule(),
        ]);
        var continuesRule = new OrRule(possibleContinues);

        var resultRule = new ContinuesRule(pattern, continuesRule);
        return resultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;
    }

    private static TagNode BuildNode(SpecNode node)
    {
        var valueNode = (node.Children.Second() as SpecNode)!;
        return new TagNode(NodeType.Bold, valueNode.Children, node.Consumed);
    }

    public static bool IsTagInWord(List<Token> tokens, int begin = 0)
    {
        if (begin != 0 && tokens[begin - 1].TokenType == TokenType.Word) 
            return true;

        var inStartRule = new PatternRule([
            TokenType.Underscore, TokenType.Underscore, TokenType.Word, 
            TokenType.Underscore, TokenType.Underscore, TokenType.Word,
        ]);
        return inStartRule.Match(tokens, begin) is not null;
    }
}