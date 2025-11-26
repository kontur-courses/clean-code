namespace Markdown.Core.Lexing;

public readonly struct Token(TokenKind kind, ReadOnlyMemory<char> slice)
{
    public TokenKind Kind { get; init; } = kind;
    public ReadOnlyMemory<char> Slice { get; init; } = slice;
}