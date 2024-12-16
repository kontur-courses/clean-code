using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.SpecialRules;
using Markdown.Parser.Rules.TextRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser.Rules.TagRules;

public class ItalicRule : IParsingRule
{
    private static readonly List<TokenType> TextSymbols =
    [
        TokenType.WORD, TokenType.SPACE, 
        TokenType.HASH_TAG, TokenType.BACK_SLASH,
        TokenType.OPEN_BRACKET, TokenType.CLOSE_BRACKET,
        TokenType.OPEN_SQUARE_BRACKET, TokenType.CLOSE_SQUARE_BRACKET
    ];
    
    private readonly AndRule innerBoldRule = new([
        PatternRuleFactory.DoubleUnderscore(),
        new TextRule(TextSymbols),
        PatternRuleFactory.DoubleUnderscore()
    ]);

    private readonly List<IParsingRule> possibleContinues =
    [
        PatternRuleFactory.DoubleUnderscore(),
        new PatternRule(TokenType.NEW_LINE),
        new PatternRule(TokenType.SPACE),
    ];
    
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        return !InWordItalicRule.IsTagInWord(tokens, begin)
            ? MatchItalic(tokens, begin)
            : new InWordItalicRule().Match(tokens, begin);
    }

    private TagNode? MatchItalic(List<Token> tokens, int begin)
    {
        var valueRule = new OrRule(new TextRule(TextSymbols), innerBoldRule);
        var pattern = new AndRule([
            new PatternRule(TokenType.UNDERSCORE),
            new ConditionalRule(new KleenStarRule(valueRule), HasRightBorders),
            new PatternRule(TokenType.UNDERSCORE),
        ]);
        var continuesRule = new OrRule(possibleContinues);
        
        var resultRule = new ContinuesRule(pattern, continuesRule);
        return resultRule.Match(tokens, begin) is SpecNode specNode ? BuildNode(specNode) : null;
    }

    private static TagNode BuildNode(SpecNode node)
    {
        var valueNode = (node.Nodes.Second() as SpecNode)!;
        return new TagNode(NodeType.ITALIC, valueNode.Nodes, node.Start, node.Consumed);
    }

    private static bool HasRightBorders(Node node, List<Token> tokens)
        => tokens[node.Start].TokenType != TokenType.SPACE 
           && tokens[node.End].TokenType is not (TokenType.SPACE or TokenType.BACK_SLASH);
}