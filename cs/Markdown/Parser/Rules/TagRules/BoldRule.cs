using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.SpecialRules;
using Markdown.Parser.Rules.TextRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser.Rules.TagRules;

public class BoldRule : IParsingRule
{
    private static readonly List<TokenType> TextSymbols =
    [
        TokenType.WORD, TokenType.SPACE, 
        TokenType.HASH_TAG, TokenType.BACK_SLASH,
        TokenType.OPEN_BRACKET, TokenType.CLOSE_BRACKET,
        TokenType.OPEN_SQUARE_BRACKET, TokenType.CLOSE_SQUARE_BRACKET
    ];
    
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        return !InWordBoldRule.IsTagInWord(tokens, begin)
            ? MatchBold(tokens, begin)
            : new InWordBoldRule().Match(tokens, begin);
    }
    
    private static TagNode? MatchBold(List<Token> tokens, int begin = 0)
    {
        var valueRule = new OrRule(new ItalicRule(), new TextRule(TextSymbols));
        var pattern = new AndRule([
            PatternRuleFactory.DoubleUnderscore(),
            new ConditionalRule(new KleenStarRule(valueRule), HasRightBorders),
            PatternRuleFactory.DoubleUnderscore()
        ]);
        var continuesRule = new OrRule(TokenType.NEW_LINE, TokenType.SPACE);
        
        var resultRule = new ContinuesRule(pattern, continuesRule);
        return resultRule.Match(tokens, begin) is SpecNode specNode ? BuildNode(specNode) : null;
    }

    private static TagNode BuildNode(SpecNode node)
    {
        var valueNode = (node.Nodes.Second() as SpecNode)!;
        return new TagNode(NodeType.BOLD, valueNode.Nodes, node.Start, node.Consumed);
    }

    private static bool HasRightBorders(Node node, List<Token> tokens)
        => tokens[node.Start].TokenType != TokenType.SPACE 
           && tokens[node.End].TokenType is not (TokenType.SPACE or TokenType.BACK_SLASH);
}