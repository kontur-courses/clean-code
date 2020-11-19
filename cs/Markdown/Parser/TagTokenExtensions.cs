namespace Markdown
{
    public static class TagTokenExtensions
    {
        public static bool IsInsideOf(this TagToken token, TagToken tag)
        {
            return token.StartPosition > tag.StartPosition && token.EndPosition < tag.EndPosition;
        }

        public static bool IsIntersectedWith(this TagToken firstTag, TagToken secondTag)
        {
            return firstTag.StartPosition < secondTag.EndPosition && firstTag.StartPosition > secondTag.StartPosition
                   || firstTag.EndPosition < secondTag.EndPosition && firstTag.EndPosition > secondTag.StartPosition;
        }
    }
}