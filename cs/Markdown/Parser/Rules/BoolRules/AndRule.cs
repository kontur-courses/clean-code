using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules.BoolRules;

public class AndRule(List<IParsingRule> pattern) : IParsingRule
{
    public AndRule(IParsingRule firstRule, IParsingRule secondRule) :
        this([firstRule, secondRule])
    {
        
    }

    public AndRule(TokenType firstType, TokenType secondType) :
        this([new PatternRule(firstType), new PatternRule(secondType)])
    {
        
    }
    
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var nodes = tokens.MatchPattern(pattern, begin);
        var consumed = nodes.Aggregate(0, (acc, node) => acc + node.Consumed);
        return consumed == 0 ? null : new SpecNode(nodes, begin, consumed);
    }
}