using Markdown.Tokenization.Tokens.Abstractions;

namespace Markdown.Tokenization.Tokens;

public abstract record PairToken(
    string Source,
    int Start,
    int Length) : SourceToken(Source, Start, Length)
{
    public PairRole Role { get; set; } = PairRole.Unresolved;

    public bool IsBetweenDigits
        => char.IsDigit(PreviousChar ?? '\0') &&
           char.IsDigit(NextChar ?? '\0');

    public bool IsInsideWord
        => char.IsLetterOrDigit(PreviousChar ?? '\0') &&
           char.IsLetterOrDigit(NextChar ?? '\0');

    public bool CanOpen
        => NextChar is char next &&
           !char.IsWhiteSpace(next) &&
           !IsBetweenDigits;

    public bool CanClose
        => PreviousChar is char previous &&
           !char.IsWhiteSpace(previous) &&
           !IsBetweenDigits;

    private char? PreviousChar
        => Start > 0 ? Source[Start - 1] : null;

    private char? NextChar
        => Start + Length < Source.Length
            ? Source[Start + Length]
            : null;
}
