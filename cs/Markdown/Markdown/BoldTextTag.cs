namespace Markdown
{
    public class BoldTextTag : ITag
    {
        public TagType Type => TagType.BoldText;
        public int Start { get; }
        public int End { get; }

        public BoldTextTag(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}