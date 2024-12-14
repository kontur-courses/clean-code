using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules.BoolRules;

public class KleeneStarRule(IParsingRule pattern) : IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var nodes = tokens.KleeneStarMatch(pattern, begin);
        var consumed = nodes.Aggregate(0, (acc, node) => acc + node.Consumed);
        return consumed == 0 ? null : new SpecNode(nodes, begin, consumed);
    }
}