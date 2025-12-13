namespace Markdown.Tokenizer;

public class Token
{
    public readonly TokenType Type;
    public readonly string Value;

    public Token(string value, TokenType type)
    {
        Value = value;
        Type = type;
    }
}