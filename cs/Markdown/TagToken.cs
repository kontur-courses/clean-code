namespace Markdown
{
    public class TagToken
    {
        public int Position;
        public string Tag;

        public TagToken(int position, string tag)
        {
            Position = position;
            Tag = tag;
        }
    }
}