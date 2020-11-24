namespace Markdown.TokenSystem
{
    public class Token
    {
        public int Position { get; }
        public int Length { get; }
        public string Value { get; }

        public Token(int position, int length, string value)
        {
            Position = position;
            Length = length;
            Value = value;
        }
    }
}