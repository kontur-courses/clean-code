namespace Markdown
{
    public class Token
    {
        public readonly int StartPosition;
        public int Length;
        public readonly ITag Tag;

        public Token(int startPosition, ITag tag)
        {
            StartPosition = startPosition;
            Tag = tag;
        }
    }
}