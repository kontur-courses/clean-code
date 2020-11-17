namespace Markdown
{
    public class TagToken
    {
        public readonly int StartPosition;
        public readonly string Tag;

        public TagToken(int startPosition, string tag)
        {
            StartPosition = startPosition;
            Tag = tag;
        }
    }
}