namespace Markdown
{
    internal class Token
    {
        public Syntax.AttributeType Type;

        public int Position;

        public bool IsEnd;

        public Token(Syntax.AttributeType type, int position, bool isEnd)
        {
            Type = type;
            Position = position;
            IsEnd = isEnd;
        }
    }
}