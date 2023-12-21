namespace Markdown.Tokens.Types;

public class TokenTypeEqualityComparer : IEqualityComparer<ITokenType>
{
    public bool Equals(ITokenType? x, ITokenType? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;

        return x.Value == y.Value
               && x.SupportsClosingTag == y.SupportsClosingTag
               && x.HasLineBeginningSemantics == y.HasLineBeginningSemantics;
    }

    public int GetHashCode(ITokenType obj)
    {
        return HashCode.Combine(obj.Value, obj.SupportsClosingTag, obj.HasLineBeginningSemantics);
    }
}