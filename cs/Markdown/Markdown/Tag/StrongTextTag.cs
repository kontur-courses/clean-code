namespace Markdown.Tag
{
    public class StrongTextTag : ITag
    {
        public TagType Type => TagType.StrongText;
        public int Start { get; }
        public int End { get; }

        public StrongTextTag(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}