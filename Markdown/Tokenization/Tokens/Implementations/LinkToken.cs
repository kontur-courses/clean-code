using Markdown.Tokenization.Tokens.Abstractions;

namespace Markdown.Tokenization.Tokens.Implementations;

public sealed record LinkToken(
    string Source,
    int Start,
    int Length,
    int UrlStart,
    int UrlLength)
    : SourceToken(Source, Start, Length)
{
    public int TextStart => Start + 1;
    public int TextLength => UrlStart - TextStart - 2;
}
