namespace Markdown;

public abstract record Token(int Index, int Length);

public record DelimiterRunToken(
    int Index,
    int Length,
    bool CanOpen,
    bool CanClose
) : Token(Index: Index, Length: Length);

public record EscapeToken(
    int Index
) : Token(Index: Index, Length: 2);

public readonly record struct TokenPair(
    Token Opening,
    Token Closing
);