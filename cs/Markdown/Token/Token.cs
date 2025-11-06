namespace Markdown;

public record Token(int Position, int Length, string Value, TokenType Type);