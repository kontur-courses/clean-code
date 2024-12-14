using Markdown.Parser.Nodes;
using Markdown.Parser.Rules;
using Markdown.Tokens;

namespace Markdown.Parser;

public static class TokenParser
{
    public static Node? Parse(List<Token> tokens) => new BodyRule().Match(tokens);
}