namespace Markdown
{
    public class Token
    {
        public AttributeType Type { get; }

        public int Position { get; }

        public bool IsСlosing { get; set; }

        public Token(AttributeType type, int position, bool isСlosing = false)
        {
            Type = type;
            Position = position;
            IsСlosing = isСlosing;
        }
    }
}