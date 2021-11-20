namespace Markdown.Tag
{
    public class EmptyTag : ITag
    {
        public TagType Type => TagType.None;
        public int Start { get; }
        public int End { get; }

        public EmptyTag(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}