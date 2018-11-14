namespace Markdown
{
    public struct Token
    {
        public readonly int Start;
        public readonly int End;

        public readonly Tag Tag;

        public Token(int start, int end, Tag tag)
        {
            Start = start;
            End = end;
            Tag = tag;
        }
    }
}
