using Markdown.Parser.Nodes;
using Markdown.Tokens;

namespace Markdown.Parser.Rules.BoolRules;

public class ConditionalRule(IParsingRule rule, Func<Node, bool> condition): IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var node = rule.Match(tokens, begin);
        return node is not null && condition(node) ? node : null;
    }
}