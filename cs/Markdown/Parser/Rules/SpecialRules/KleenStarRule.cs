using Markdown.Parser.Nodes;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser.Rules.SpecialRules;

public class KleenStarRule(IParsingRule pattern) : IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var consumed = 0;
        var nodes = new List<Node>();
        
        while (pattern.Match(tokens, begin + consumed) is { } node)
        {
            nodes.Add(node); 
            consumed += node.Consumed;
        }
        return consumed == 0 ? null : new SpecNode(nodes, begin, consumed);
    }
}