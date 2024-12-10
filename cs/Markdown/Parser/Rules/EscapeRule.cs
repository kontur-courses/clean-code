using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class EscapeRule : IParsingRule
{
    private readonly List<TokenType> escapedTokens = 
    [
        TokenType.Underscore, TokenType.Octothorpe
    ];
    
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var resultRule = new AndRule([
            new PatternRule(TokenType.Backslash),
            new OrRule(escapedTokens)
        ]);
        return resultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;
    }
    private static TextNode BuildNode(SpecNode node) => new(node.End, 1);
}