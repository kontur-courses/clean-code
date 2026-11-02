namespace Markdown.Tokens;

public sealed class Token
{
    public TokenType Type { get; }
    public string Value { get; }

    public Token(TokenType type, string value = "")
    {
        Type = type;
        Value = value;
    }

    public static Token Text(string value) => new(TokenType.Text, value);
    public static Token Newline() => new(TokenType.Newline, "\n");
}
