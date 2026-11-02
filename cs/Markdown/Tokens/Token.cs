namespace Markdown.Tokens;

public class Token
{
    public Token(TokenType type, string value, int position)
    {
        Type = type;
        Value = value;
        Position = position;
    }

    public TokenType Type { get; set; }
    public string Value { get; }
    public int Position { get; }

    public bool HasSpaceBefore { get; init; }
    public bool HasSpaceAfter { get; init; }
    public bool IsInsideWord { get; init; }
    public bool IsNearDigit { get; init; }

    public TokenRole Role { get; set; } = TokenRole.None;

    public int PairIndex { get; set; } = -1;

    public bool IsTag => Type != TokenType.Text;
}