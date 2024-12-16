using Markdown.Parser.Nodes;
using Markdown.Parser.Nodes.TagNodes;
using Markdown.Parser.Rules.SpecialRules;
using Markdown.Parser.Rules.TextRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser.Rules.TagRules;

public class HrefRule : IParsingRule
{
    private static readonly List<TokenType> TextSymbols =
    [
        TokenType.WORD, TokenType.SPACE, 
        TokenType.HASH_TAG, TokenType.UNDERSCORE, TokenType.NUMBER
    ];
    
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var valueRule = new AndRule([
            new PatternRule(TokenType.OPEN_SQUARE_BRACKET),
            new TextRule(TextSymbols),
            new PatternRule(TokenType.CLOSE_SQUARE_BRACKET),
        ]);
        var hrefRule = new AndRule([
            new PatternRule(TokenType.OPEN_BRACKET),
            new TextRule(TextSymbols),
            new PatternRule(TokenType.CLOSE_BRACKET),
        ]);
        
        var resultRule = new AndRule(valueRule, hrefRule);
        return resultRule.Match(tokens, begin) is SpecNode specNode ? BuildNode(specNode, tokens) : null;
    }
    
    private static HrefNode BuildNode(SpecNode node, List<Token> tokens)
    {
        var valueNode = (node.Nodes.First() as SpecNode)!;
        var hrefNode = (node.Nodes.Second() as SpecNode)!;

        var children = valueNode.Nodes.Second()!;
        var href = hrefNode.Nodes.Second()!.ToText(tokens);

        return new HrefNode(href, [children], node.Start, node.Consumed);
    }
}