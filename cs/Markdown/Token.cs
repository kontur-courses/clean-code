namespace Markdown;

public class Token
{
    public TokenType Type;
    public string Value;

    public Token(string value, TokenType type)
    {
        Value = value;
        Type = type;
    }
}