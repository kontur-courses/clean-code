using Markdown.Enums;

namespace Markdown.Tokens;

public abstract class Token(int position)
{
    public abstract MarkdownTokenName Name { get; }
    public abstract string Value { get; }
    public int Position => position;
    public int Length => Value.Length;
    public bool Is(MarkdownTokenName type) => type == Name;

    public override bool Equals(object? obj) => obj is Token token && Equals(token);

    public override int GetHashCode() => HashCode.Combine((int)Name, Value);

    private bool Equals(Token token) => Name == token.Name && Position == token.Position && Value == token.Value;
}