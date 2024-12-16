using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.SpecialRules;
using Markdown.Parser.Rules.TextRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser.Rules.TagRules;

public class EscapeRule(List<TokenType> escapedTokens) : IParsingRule
{
    public EscapeRule(TokenType escapedTokenType)
        : this([escapedTokenType])
    { }
    
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var resultRule = new AndRule([
            new PatternRule(TokenType.BACK_SLASH),
            new OrRule(escapedTokens)
        ]);
        return resultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;
    }

    private static TagNode BuildNode(SpecNode node) 
        => new(NodeType.ESCAPE, node.Nodes.Second()!, node.Start, node.Consumed);
}