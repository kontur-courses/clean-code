using Markdown.Tokens.Types;

namespace Markdown.Tokens;

public class Token
{
    public ITokenType TokenType { get; }

    public bool IsClosingTag { get; }

    public int StartingIndex { get; }

    public int Length { get; }

    public Token(ITokenType tokenType, bool isClosingTag, int startingIndex, int length)
    {
        TokenType = tokenType;
        IsClosingTag = isClosingTag;
        StartingIndex = startingIndex;
        Length = length;
    }

    public string GetValue()
    {
        return TokenType.Representation(IsClosingTag);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is null || GetType() != obj.GetType())
            return false;
        if (this == obj)
            return true;
        
        var other = (Token)obj;
        return GetValue().Equals(other.GetValue()) && 
               IsClosingTag == other.IsClosingTag && 
               StartingIndex == other.StartingIndex && 
               Length == other.Length;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + GetValue().GetHashCode();
            hash = hash * 23 + IsClosingTag.GetHashCode();
            hash = hash * 23 + StartingIndex.GetHashCode();
            hash = hash * 23 + Length.GetHashCode();
            return hash;
        }
    }
}