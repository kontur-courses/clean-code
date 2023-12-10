namespace Markdown;

public class Token
{
    public string Value { get; set; }
    public int Position { get; set; }
    public int Length { get; set; }

    public Token(string value, int position)
    {
        Value = value;
        Position = position;
        Length = value.Length;
    }
}