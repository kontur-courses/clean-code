namespace Markdown.Tags
{
    public class MdTag
    {
        public MdTag(Tag tag, int index)
        {
            Tag = tag;
            Index = index;
        }

        public Tag Tag { get; }
        public int Index { get; }
    }
}
