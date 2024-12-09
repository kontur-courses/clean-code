namespace Markdown.Tokens;

public record Token(TokenType TokenType, string Value)
{
    public Token(TokenType tokenType, int start, int length, string sourceText)
        : this(tokenType, sourceText.Substring(start, length))
    { }

    public int Length { get; } = Value.Length;
}