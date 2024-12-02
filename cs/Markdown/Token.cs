namespace Markdown;

public record Token
{
    public TokenType Type { get; init; }
    public required string Content { get; init; }
    public Token? Pair { get; set; }
}