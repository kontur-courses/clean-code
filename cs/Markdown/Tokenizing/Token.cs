namespace Markdown.Tokenizing
{
    public class Token
    {
        public  Tag Tag { get; set; }
        public string Content { get; set; }
        public bool IsOpening { get; set; }

        public Token(Tag tag, bool isOpening, string content = null)
        {
            Tag = tag;
            IsOpening = isOpening;
            Content = content;
        }

        public override string ToString()
        {
            return $"Token({Tag}, {IsOpening}, {Content})";
        }

        #region .Equals()

        protected bool Equals(Token other)
        {
            return Tag == other.Tag && string.Equals(Content, other.Content) && IsOpening == other.IsOpening;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Token) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Tag;
                hashCode = (hashCode * 397) ^ (Content != null ? Content.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsOpening.GetHashCode();
                return hashCode;
            }
        }

        #endregion
    }
}
