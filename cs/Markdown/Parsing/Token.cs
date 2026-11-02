namespace Markdown.Parsing;

public enum TokenType
{
    Text,
    Italic,
    Bold
}

public class Token
{
    public TokenType Type { get; }
    public string Value { get; }
    public int Position { get; } // Индекс в исходной строке

    public Token(TokenType type, string value, int position)
    {
        Type = type;
        Value = value;
        Position = position;
    }
}