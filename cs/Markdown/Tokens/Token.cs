namespace Markdown.Tokens;

public class Token(TokenType tokenType, string value)
{
    public string Value { get; } = value;
    public TokenType Type { get; } = tokenType;
}