using Markdown.Parser.Nodes;
using Markdown.Parser.Rules;
using Markdown.Tokens;

namespace Markdown.Parser;

public class TokenParser
{
    public Node? Parse(List<Token> tokens)
    {
        return new BodyRule().Match(tokens);
    }
}