using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class NewlineParser : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        if (tokens.Peek(TypeOfToken.Newline))
        {
            return new Node(TypeOfNode.Newline, 1, Value: tokens[0].Value);
        }

        return null;
    }
}