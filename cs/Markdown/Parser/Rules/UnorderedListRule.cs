using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class UnorderedListRule : IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var resultRule = new KleeneStarRule(new ListItemRule());
        
        return resultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;
    }
    private static TagNode BuildNode(SpecNode node)
        => new(NodeType.UnorderedList, node.Nodes, node.Start, node.Consumed);
}