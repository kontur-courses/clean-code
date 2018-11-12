namespace Markdown
{
    public class MarkupPosition
    {
        public MarkupPosition(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int Start { get; }
        public int End { get; }
    }
}
