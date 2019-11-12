namespace Markdown
{
    public class Token
    {
        public AttributeType Type { get; }

        public int Position { get; }

        public bool IsEnd { get; }

        public Token(AttributeType type, int position, bool isEnd = false)
        {
            Type = type;
            Position = position;
            IsEnd = isEnd;
        }
    }
}