using Markdown.Tokenization.Tokens.Abstractions;

namespace Markdown.Tokenization.Tokens.Implementations;

public record EscapedToken(string Source, int Start)
    : SourceToken(Source, Start, 2);
