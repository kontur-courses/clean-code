using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class EscapeRule(List<TokenType> escapedTokens) : IParsingRule
{
    public EscapeRule(TokenType escapedTokenType)
        : this([escapedTokenType])
    { }

    private readonly AndRule resultRule = new([
        new PatternRule(TokenType.Backslash),
        new OrRule(escapedTokens)
    ]);
    
    public Node? Match(List<Token> tokens, int begin = 0) 
        => resultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;

    private static TagNode BuildNode(SpecNode node) 
        => new(NodeType.Escape, node.Nodes.Second() ?? throw new InvalidOperationException(), node.Start, node.Consumed);
}