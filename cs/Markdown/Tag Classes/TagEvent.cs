namespace Markdown.Tag_Classes
{
    public class TagEvent
    {
        public Side Side;
        public Tag Tag;
        public string TagContent;

        public TagEvent(Side side, Tag tag, string con)
        {
            Side = side;
            Tag = tag;
            TagContent = con;
        }

        public override string ToString()
        {
            return $"Side = {nameof(this.Side)}\n"
                   + $"Tag = {nameof(this.Tag)}\n"
                   + $"TagContent = {TagContent}\n";
        }
    }
}
