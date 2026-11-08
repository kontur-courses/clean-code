namespace Markdown.Tokenization;

public abstract record Token;

public abstract record SourceToken(
    string Source,
    int Start,
    int Length) : Token;

public enum PairRole
{
    Unresolved,
    Open,
    Close,
    Text
}

public abstract record PairToken(
    string Source,
    int Start,
    int Length) : SourceToken(Source, Start, Length)
{
    public PairRole Role { get; set; } = PairRole.Unresolved;
}

public record EmToken(string Source, int Start)
    : PairToken(Source, Start, 1);

public record StrongToken(string Source, int Start)
    : PairToken(Source, Start, 2);

public record TextToken(
    string Source,
    int Start,
    int Length) : SourceToken(Source, Start, Length);

public record EscapedToken(string Source, int Start)
    : SourceToken(Source, Start, 2);

public record LineBreakToken(string Source, int Start)
    : SourceToken(Source, Start, 2);

public record HeadingToken(bool IsClosing) : Token;

public record LinkToken(
    string Source,
    int Start,
    int Length,
    int UrlStart,
    int UrlLength,
    IReadOnlyList<Token> InnerTokens)
    : SourceToken(Source, Start, Length);