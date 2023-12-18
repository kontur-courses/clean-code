using Markdown.Tokens.Types;

namespace Markdown.Tokens;

public class Token
{
    public ITokenType Type { get; }

    public bool IsClosingTag { get; }

    public int StartingIndex { get; }

    public int Length { get; }

    public Token(ITokenType type, bool isClosingTag, int startingIndex, int length)
    {
        Type = type;
        IsClosingTag = isClosingTag;
        StartingIndex = startingIndex;
        Length = length;
    }

    public Token(Token token)
    {
        if (token is null)
            throw new ArgumentException("Provided token is null");

        Type = token.Type;
        IsClosingTag = token.IsClosingTag;
        StartingIndex = token.StartingIndex;
        Length = token.Length;
    }

    public string GetRepresentation()
    {
        return Type.Representation(IsClosingTag);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || GetType() != obj.GetType())
            return false;
        if (this == obj)
            return true;

        var other = (Token) obj;
        return GetRepresentation().Equals(other.GetRepresentation()) &&
               IsClosingTag == other.IsClosingTag &&
               StartingIndex == other.StartingIndex &&
               Length == other.Length;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + GetRepresentation().GetHashCode();
            hash = hash * 23 + IsClosingTag.GetHashCode();
            hash = hash * 23 + StartingIndex.GetHashCode();
            hash = hash * 23 + Length.GetHashCode();
            return hash;
        }
    }
}