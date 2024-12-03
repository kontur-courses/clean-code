namespace Markdown.Tokens;

public class Token(TokenType tokenType, int begin, int length, string sourceText)
{
    public int Begin { get; } = begin;
    public int Length { get; } = length;
    public TokenType TokenType { get; } = tokenType;
    public string Value => value.Value;
    
    private readonly Lazy<string> value = new(() => sourceText[begin..(begin + length)]);

    public override string ToString()
    {
        return $"Token {TokenType}: Value \"{Value}\"";
    }
}