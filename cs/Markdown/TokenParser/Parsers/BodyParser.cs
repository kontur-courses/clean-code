using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class BodyParser : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        var (nodes, consumed) = MatchesStar.MatchStar(tokens, new ParagraphParser());

        return nodes.Count == 0 ? null : new Node(TypeOfNode.Body, consumed, nodes);
    }
}