using Markdown.Parser.Nodes;
using Markdown.Tokens;

namespace Markdown.Parser.Rules.BoolRules;

public class AndRule(IParsingRule firstRule, IParsingRule secondRule) : IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var firstMatch = firstRule.Match(tokens, begin);
        return firstMatch is null ? null : secondRule.Match(tokens, begin);
    }
}