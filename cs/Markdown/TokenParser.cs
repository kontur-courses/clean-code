using Markdown.Rules;
using Markdown.Tokens;

namespace Markdown;

public class TokenParser
{
    public Node Parse(List<Token> tokens)
    {
        return new BodyRule().Match(tokens);
    }
}