namespace Markdown
{
    public class Token
    {
        public readonly int ContentLength;
        public readonly int ContentStart;
        public readonly int TokenLength;
        public readonly int TokenStart;
        public readonly Style TokenStyle;

        public Token(Style style, int tokenStart, int tokenLength, int contentStart, int contentLength)
        {
            TokenStyle = style;
            TokenStart = tokenStart;
            TokenLength = tokenLength;
            ContentStart = contentStart;
            ContentLength = contentLength;
        }

        public static Token Create(Style style, int startTagPosition, int endTagPosition)
        {
            return new Token(
                style,
                startTagPosition,
                endTagPosition + style.EndTag.Length - startTagPosition,
                startTagPosition + style.StartTag.Length,
                endTagPosition - startTagPosition - style.StartTag.Length);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Token))
                return false;
            var other = (Token) obj;
            return TokenStart == other.TokenStart
                   && TokenLength == other.TokenLength
                   && ContentStart == other.ContentStart
                   && ContentLength == other.ContentLength
                   && TokenStyle == other.TokenStyle;
        }

        public override int GetHashCode()
        {
            return 5 * TokenStart.GetHashCode()
                   + 7 * TokenLength.GetHashCode()
                   + 11 * ContentStart.GetHashCode()
                   + 13 * ContentLength.GetHashCode()
                   + TokenStyle.GetHashCode();
        }

        public bool IntersectsWith(Token otherToken)
        {
            return TokenStart > otherToken.TokenStart
                   && TokenStart < otherToken.TokenStart + otherToken.TokenLength
                   && TokenStart + TokenLength > otherToken.TokenStart + otherToken.TokenLength
                   || TokenStart < otherToken.TokenStart
                   && TokenStart + TokenLength > otherToken.TokenStart
                   && TokenStart + TokenLength < otherToken.TokenStart + otherToken.TokenLength;
        }

        public bool Contains(Token otherToken)
        {
            return TokenStart <= otherToken.TokenStart
                   && TokenStart + TokenLength >= otherToken.TokenStart + otherToken.TokenLength;
        }
    }
}