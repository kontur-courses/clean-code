using Markdown.Tokenization.Tokens.Abstractions;

namespace Markdown.Tokenization.Tokens;

public abstract record PairToken(
    string Source,
    int Start,
    int Length) : SourceToken(Source, Start, Length)
{
    public PairRole Role { get; set; } = PairRole.Unresolved;
}
