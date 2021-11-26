namespace Markdown.MdTags
{
    public class Tag
    {
        public virtual TagType Type => TagType.None;
        public int Start { get; }
        public int End { get; }

        public Tag(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}