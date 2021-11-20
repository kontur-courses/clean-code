namespace Markdown.Tag
{
    public class TitleTag : ITag
    {
        public TagType Type => TagType.Title;
        public int Start { get; }
        public int End { get; }

        public TitleTag(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}