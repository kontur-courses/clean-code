using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public interface IParser
{
    public Node? TryMatch(TokenList tokens);
}