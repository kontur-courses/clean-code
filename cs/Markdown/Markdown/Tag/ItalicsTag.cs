namespace Markdown.Tag
{
    public class ItalicsTag : ITag
    {
        public TagType Type => TagType.Italics;
        public int Start { get; }
        public int End { get; }

        public ItalicsTag(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}