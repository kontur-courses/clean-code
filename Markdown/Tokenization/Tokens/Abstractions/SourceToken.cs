namespace Markdown.Tokenization.Tokens.Abstractions;

public abstract record SourceToken(
    string Source,
    int Start,
    int Length) : Token;
