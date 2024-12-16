using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.SpecialRules;
using Markdown.Parser.Rules.TextRules;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser.Rules.TagRules;

public class ParagraphRule : IParsingRule
{
    private readonly List<TokenType> escapedTokens =
    [
        TokenType.UNDERSCORE, TokenType.BACK_SLASH,
        TokenType.OPEN_BRACKET, TokenType.CLOSE_BRACKET,
        TokenType.OPEN_SQUARE_BRACKET, TokenType.CLOSE_SQUARE_BRACKET
    ];
    
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var tagRules = new OrRule([
            new EscapeRule(escapedTokens), 
            new HrefRule(), new ItalicRule(), new BoldRule(), new TextRule(),
        ]);
        var tokenRules = new OrRule([
            PatternRuleFactory.DoubleUnderscore(),
            new PatternRule(TokenType.NUMBER), new PatternRule(TokenType.HASH_TAG),
            new PatternRule(TokenType.UNDERSCORE), new PatternRule(TokenType.BACK_SLASH),
            new PatternRule(TokenType.OPEN_BRACKET), new PatternRule(TokenType.CLOSE_BRACKET),
            new PatternRule(TokenType.OPEN_SQUARE_BRACKET), new PatternRule(TokenType.CLOSE_SQUARE_BRACKET),
        ]);
        
        var resultRule = new AndRule([
            new KleenStarRule(new OrRule(tagRules, tokenRules)),
            new PatternRule(TokenType.NEW_LINE)
        ]);
        return resultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;
    }

    private static TagNode BuildNode(SpecNode node)
    {
        var valueNode = (node.Nodes.First() as SpecNode)!;
        return new TagNode(NodeType.PARAGRAPH, valueNode.Nodes, node.Start, node.Consumed); 
    }
}