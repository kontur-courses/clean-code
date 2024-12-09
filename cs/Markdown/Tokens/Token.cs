using Markdown.Tags;

namespace Markdown.Tokens;

public class Token
{
    public Token(TokenType tokenType, string content, int position, bool isCloseTag = false,
        TagType tagType = TagType.UnDefined)
    {
        TokenType = tokenType;
        Content = content;
        Position = position;
        IsCloseTag = isCloseTag;
        TagType = tagType;
    }

    public TokenType TokenType { get; set; }
    public string Content { get; set; }
    public TagType TagType { get; set; }
    public bool IsCloseTag { get; set; }
    public int Position { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Token)obj);
    }

    public bool Equals(Token other)
    {
        if (other is null) return false;
        return TokenType == other.TokenType &&
               Content == other.Content &&
               TagType == other.TagType &&
               IsCloseTag == other.IsCloseTag &&
               Position == other.Position;
    }

    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            var hashCode = (int)TokenType;
            hashCode = (hashCode * 397) ^ (Content != null ? Content.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (int)TagType;
            hashCode = (hashCode * 397) ^ IsCloseTag.GetHashCode();
            hashCode = (hashCode * 397) ^ Position;
            return hashCode;
        }
    }

    public static bool operator ==(Token left, Token right)
    {
        return EqualityComparer<Token>.Default.Equals(left, right);
    }

    public static bool operator !=(Token left, Token right)
    {
        return !(left == right);
    }
}