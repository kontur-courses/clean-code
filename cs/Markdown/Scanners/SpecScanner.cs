using Markdown.Tokens;

namespace Markdown.Scanners;

public class SpecScanner : ITokenScanner
{
    public Token? Scan(string text) => text[0] switch
    {
        '_' => new Token(TokenType.Underscore, "_"),
        '\n' => new Token(TokenType.Newline, "\n"),
        '#' => new Token(TokenType.Octothorpe, "#"),
        _ => null
    };
}