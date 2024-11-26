namespace Markdown.Tokens;

public class Token(TokenType tokenType, int begin, int length, string sourceText)
{
    private string? value;

    public int Begin { get; } = begin;
    public int Length { get; } = length;
    public TokenType Type { get; } = tokenType;

    public string GetValue()
    {
        return value ??= sourceText[Begin..(Begin + Length)];
    }

    public override string ToString()
    {
        return $"Token {Type}: Value \"{GetValue()}\"";
    }
}