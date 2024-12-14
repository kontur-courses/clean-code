using Markdown.Parser.Nodes;
using Markdown.Tokens;

namespace Markdown.Parser.Rules.BoolRules;

public class OrRule(List<IParsingRule> rules) : IParsingRule
{
    public OrRule(IParsingRule firstRule, IParsingRule secondRule)
        : this([firstRule, secondRule]) 
    { }

    public OrRule(TokenType firstToken, TokenType secondToken) 
        : this(new PatternRule(firstToken), new PatternRule(secondToken))
    { }

    public OrRule(List<TokenType> tokenTypes)
        : this(tokenTypes
            .Select(tt => new PatternRule(tt))
            .ToList<IParsingRule>()
        ) 
    { }

    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var match = rules
            .Select(rule => rule.Match(tokens, begin))
            .FirstOrDefault(node => node is not null, null);
        return match;
    }
}