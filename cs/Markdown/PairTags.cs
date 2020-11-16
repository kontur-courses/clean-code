namespace Markdown
{
    public class PairTags
    {
        public Tag Start;
        public Tag End;
        public int Length => End.Position - Start.Position;

        public PairTags(Tag start, Tag end)
        {
            Start = start;
            End = end;
        }
    }
}