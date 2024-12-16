using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.SpecialRules;
using Markdown.Parser.Rules.TextRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser.Rules.TagRules;

public class InWordBoldRule : IParsingRule
{
    private readonly List<TokenType> possibleContinues =
    [
        TokenType.NEW_LINE, TokenType.SPACE, TokenType.WORD
    ];
    
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var valueRule = new OrRule(new InWordItalicRule(), new PatternRule(TokenType.WORD));
        var pattern = new AndRule([
            PatternRuleFactory.DoubleUnderscore(),
            new KleenStarRule(valueRule),
            PatternRuleFactory.DoubleUnderscore(),
        ]);
        var continuesRule = new OrRule(possibleContinues);
        
        var resultRule = new ContinuesRule(pattern, continuesRule);
        return resultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;
    }

    private static TagNode BuildNode(SpecNode node)
    {
        var valueNode = (node.Nodes.Second() as SpecNode)!;
        return new TagNode(NodeType.BOLD, valueNode.Nodes, node.Start, node.Consumed);
    }
    
    public static bool IsTagInWord(List<Token> tokens, int begin = 0)
    {
        if (begin != 0 && tokens[begin - 1].TokenType == TokenType.WORD) 
            return true;
        
        var inStartRule = new PatternRule([
            TokenType.UNDERSCORE, TokenType.UNDERSCORE, TokenType.WORD, 
            TokenType.UNDERSCORE, TokenType.UNDERSCORE, TokenType.WORD,
        ]);
        return inStartRule.Match(tokens, begin) is not null;
    }
}