namespace Markdown
{
    public class Token
    {
        public readonly Tag Tag;
        public readonly int StartPosition;
        public readonly int EndPosition;

        public Token(Tag tag, int startPosition, int endPosition)
        {
            Tag = tag;
            StartPosition = startPosition;
            EndPosition = endPosition;
        }
    }
}
