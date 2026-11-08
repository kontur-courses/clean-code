namespace Markdown.Tokenization.Tokens.Implementations;

public record StrongToken(string Source, int Start)
    : PairToken(Source, Start, 2);
