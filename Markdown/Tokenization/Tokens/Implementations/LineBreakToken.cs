using Markdown.Tokenization.Tokens.Abstractions;

namespace Markdown.Tokenization.Tokens.Implementations;

public record LineBreakToken(string Source, int Start)
    : SourceToken(Source, Start, 2);
