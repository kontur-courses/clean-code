using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class MatchesFirst
{
    public static Node? MatchFirst(TokenList tokens, params IParser[] parsers)
    {
        return parsers
            .Select(parser => parser.TryMatch(tokens))
            .FirstOrDefault(node => node != null);
    }
}