namespace Markdown;

public enum TokenType
{
    EmphasisDelimiter,
    StrongDelimiter,
    Escape
}

public record Token(
    TokenType Type,
    int StartIndex,
    int Length,
    bool CanOpen,
    bool CanClose
);

public readonly record struct TokenPair(
    Token Opening,
    Token Closing
);