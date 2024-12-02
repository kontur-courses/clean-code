using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class UnorderedListParser : IParser
{
    private readonly int nestedLevel;
    
    public UnorderedListParser(int nestedLevel = 0)
    {
        this.nestedLevel = nestedLevel;
    }
    public Node? TryMatch(TokenList tokens)
    {
        var bullets = new[] {"-", "+", "*" };

        foreach (var bullet in bullets)
        {
            var isUnorderedList = TryGetUnorderedList(tokens, bullet, out var node);

            if (isUnorderedList)
            {
                return node;
            }
        }

        return null;
    }

    public bool TryGetUnorderedList(TokenList tokens, string bullet, out Node? node)
    {
        node = default;
        
        var (nodes, consumed) = MatchesStar.MatchStar(tokens, new UnorderedListItemParser(nestedLevel, bullet));
        if (nodes.Count == 0) return false;
        
        node = new Node(TypeOfNode.UnorderedList, consumed, nodes);
        return true;

    }
}