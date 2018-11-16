namespace Markdown
{
    public class TokenPosition
    {
        public TokenPosition(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int Start { get; }
        public int End { get; }
    }
}
