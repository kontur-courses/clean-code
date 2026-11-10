using Markdown.Tokenization.Tokens.Abstractions;

namespace Markdown.Tokenization.Tokens.Implementations;

public record HeadingToken(bool IsClosing) : Token;
