using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class InWordItalicRule : IParsingRule
{
    private static readonly List<TokenType> PossibleContinues =
    [
        TokenType.Newline, TokenType.Space, TokenType.Word
    ];

    private static readonly List<IParsingRule> AdditionalTextSymbols =
    [
        new PatternRule(TokenType.Asterisk), new PatternRule(TokenType.Backslash), new PatternRule(TokenType.Octothorpe)
    ];
    
    private static readonly AndRule Pattern = new([
        new PatternRule(TokenType.Underscore), 
        new KleeneStarRule(new OrRule(new PatternRule(TokenType.Word), new OrRule(AdditionalTextSymbols))), 
        new PatternRule(TokenType.Underscore),
    ]);
    
    private static readonly OrRule ContinuesRule = new(PossibleContinues);

    private static readonly ContinuesRule ResultRule = new(Pattern, ContinuesRule);

    private static readonly PatternRule InStartRule = new([
        TokenType.Underscore, TokenType.Word, 
        TokenType.Underscore, TokenType.Word,
    ]);

    public Node? Match(List<Token> tokens, int begin = 0) 
        => ResultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;

    private static TagNode BuildNode(SpecNode node)
        => new(NodeType.Italic, node.Nodes.Second() ?? throw new InvalidOperationException(), node.Start, node.Consumed);

    public static bool IsTagInWord(List<Token> tokens, int begin = 0)
    {
        if (begin != 0 && tokens[begin - 1].TokenType == TokenType.Word) 
            return true;
        
        return InStartRule.Match(tokens, begin) is not null;
    }
}