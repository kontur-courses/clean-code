using Markdown.Parser.Nodes;
using Markdown.Parser.Rules;
using Markdown.Tokens;

namespace Markdown.Parser;

public static class TokenParser
{
    public static Node? Parse(List<Token> tokens)
    {
        return new BodyRule().Match(tokens);
    }
}