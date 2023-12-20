namespace Markdown.Tags
{
    public class MdTag
    {
        private readonly TagType type;
        private readonly int index;

        public MdTag(TagType type, int index)
        {
            this.type = type;
            this.index = index;
        }

        public TagType Type => type;
        public int Index => index;
    }
}
