using Markdown.Parser.Nodes;
using Markdown.Tokens;

namespace Markdown.Parser.Rules.BoolRules;

public class ContinuesRule(IParsingRule rule, IParsingRule continuesRule) : IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        if (rule.Match(tokens, begin) is { } node)
        {
            var newBegin = begin + node.Consumed;
            return HasRightContinues(tokens, newBegin) ? node : null;
        }
        
        return null;
    }

    private bool HasRightContinues(List<Token> tokens, int begin)
    {
        if (tokens.Count == begin)
        {
            return true;
        }

        return continuesRule.Match(tokens, begin) is not null;
    }
}