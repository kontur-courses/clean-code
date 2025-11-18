namespace Markdown;
public class Token(TokenType type, string value)
{
    public TokenType Type { get; } = type;
    public string Value { get; } = value;
}