namespace Markdown.Domain;

public abstract record Token(int Index, int Length);

public record EscapeToken(
    int Index
) : Token(Index: Index, Length: 2);

public record DelimiterRunToken(
    int Index,
    int Length,
    bool CanOpen,
    bool CanClose
) : Token(Index: Index, Length: Length);

public enum DelimiterType
{
    Emphasis,
    Strong
}

public sealed record DelimiterPair(
    DelimiterRunToken Opening,
    DelimiterRunToken Closing,
    DelimiterType Type);