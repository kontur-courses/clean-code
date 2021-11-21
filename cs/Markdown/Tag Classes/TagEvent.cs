namespace Markdown.Tag_Classes
{
    public class TagEvent
    {
        public readonly Side Side;
        public readonly Tag Tag;
        public readonly string TagContent;

        public TagEvent(Side side, Tag tag, string con)
        {
            Side = side;
            Tag = tag;
            TagContent = con;
        }
    }
}
