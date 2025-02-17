namespace Markdown;

public class Token(string value, TokenType tokenType, int startIndex = 0)
{
    public TokenType Type { get; } = tokenType;
    public string Value { get; } = value;
    public int StartIndex { get; init; } = startIndex;
    public bool IsTag { get; set; }

    public int EndIndex => StartIndex + Value.Length - 1;
    public int Lenght => Value.Length;
}