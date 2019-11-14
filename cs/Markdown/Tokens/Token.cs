namespace Markdown.Tokens
{
    public class Token
    {
        public int Position { get; }
        public string Value { get; }

        public Token(int position, string value)
        {
            Position = position;
            Value = value;
        }
    }
}