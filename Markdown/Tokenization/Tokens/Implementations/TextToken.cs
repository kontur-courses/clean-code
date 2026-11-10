using Markdown.Tokenization.Tokens.Abstractions;

namespace Markdown.Tokenization.Tokens.Implementations;

public record TextToken(
    string Source,
    int Start,
    int Length) : SourceToken(Source, Start, Length)
{
    public bool ContainsWordSeparator =>
        Enumerable.Range(Start, Length).Any(i => char.IsWhiteSpace(Source[i]));
}
