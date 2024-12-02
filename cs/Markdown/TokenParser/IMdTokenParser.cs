using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser;

public interface IMdTokenParser
{
    public Node Parse(TokenList tokens);
}