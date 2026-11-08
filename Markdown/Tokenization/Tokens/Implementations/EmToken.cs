namespace Markdown.Tokenization.Tokens.Implementations;

public record EmToken(string Source, int Start)
    : PairToken(Source, Start, 1);
