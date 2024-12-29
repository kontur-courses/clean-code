namespace Markdown;

public record Token
{
    private readonly string source;
    private readonly string value;
    public Token(int position, string source, int length)
    {
        Position = position;
        Length = length;
        value = source.Substring(Position, Length);
    }

    public int Position { get; }
    public int Length { get; }

    public string GetValue() => value;
}