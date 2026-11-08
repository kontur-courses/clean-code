using Markdown.Tokenization.Tokens.Abstractions;

namespace Markdown.Tokenization.Tokens.Implementations;

public record LinkToken(
    string Source,
    int Start,
    int Length,
    int UrlStart,
    int UrlLength,
    IReadOnlyList<Token> InnerTokens)
    : SourceToken(Source, Start, Length);
