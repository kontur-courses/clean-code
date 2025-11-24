namespace Markdown.Domains;

public class MdToken(TokenType type, string value)
{
    public TokenType Type { get; } = type;
    public string Value { get; } = value;

    public override bool Equals(object? obj)
    {
        if (obj is not MdToken other)
            return false;

        return Type == other.Type && Value == other.Value;
    }

    public override int GetHashCode() => HashCode.Combine(Type, Value);
}